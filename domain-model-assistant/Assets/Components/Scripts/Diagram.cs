using System.ComponentModel;
using System.Reflection.Emit;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using System.Text.Json; // TODO Use JsonSerializer.Serialize when Unity moves to .NET 6
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Diagram : MonoBehaviour
{

    // These public fields are assigned in the editor
    public TextAsset jsonFile;
    public float zoomSpeed = 1;
    public CanvasScaler CanvasScaler;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 0.0f;
    public float maxOrtho = 20.0f;
    public GameObject compartmentedRectangle;
    private List<GameObject> _nodes = new List<GameObject>();
    private List<GameObject> _edges = new List<GameObject>();
    public InfoBox infoBox;

    private Vector3 dragStartPos;
    // private bool dragging = false;

    public const bool UseWebcore = true; // Change to false to use the wrapper page JSON instead of WebCore

    public const string WebcoreEndpoint = "http://localhost:8080";

    public string cdmName = "AirlineSystem"; // For now, use same name as the example in Modeling Assistant backend

    public Student student;

    public WebCore WebCore;

    // Temporary text used during application development
    public const string InitialInfoBoxText = "Welcome to the Modeling Assistant! "
        + "Please use the Debug button in the top right corner to login to a new random WebCORE account.";

    GraphicRaycaster raycaster;

    readonly Dictionary<string, GameObject> _namesToRects = new();

    readonly Dictionary<string, List<Attribute>> classIdToAttributes = new();

    public readonly Dictionary<string, List<AssociationEnd>> classIdToAssociationEndsDTO = new();


    readonly Dictionary<string, List<Literal>> classIdToLiterals = new();


    readonly Dictionary<string, string> attrTypeIdsToTypes = new();

    public Dictionary<string, string> classIdToClassNames = new();

    List<string> createdAttributeIds = new();

    // public Dictionary<List<string>, string>

    enum CanvasMode
    {
        Default,
        AddingClass,
        AddingEnumClass,
        AddingAttribute,
        // ...
    }

    CanvasMode _currentMode = CanvasMode.Default;

    // References to the JavaScript functions defined in Assets/Plugins/cdmeditor.jslib

    [DllImport("__Internal")]
    private static extern string HttpRequest(string verb, string url, string headers, string data);

    [DllImport("__Internal")]
    private static extern string ConvertToUnityString(object o);

    [DllImport("__Internal")]
    private static extern void SetCursorToAddMode();

    [DllImport("__Internal")]
    private static extern void ResetCursor();

    private bool _updateNeeded = false;

    private bool _namesUpToDate = false;

    // true if app is run in browser, false if run in Unity editor
    private static readonly bool _isWebGl = Application.platform == RuntimePlatform.WebGLPlayer;

    private string _currCdmStr = "";

    public string ID { get; set; }

    public bool reGetRequest = false;

    // Awake is called once to initialize this game object
    void Awake()
    {
        // user = User.CreateRandom(); // for now, just create a random user
        // user.Login();
        // user.PutRequest(CdmEndpoint()); // create the CDM
        // Debug.Log("Initialized class diagram with user: " + user);
    }

    // Start is called before the first frame update
    void Start()
    {
        CanvasScaler = gameObject.GetComponent<CanvasScaler>();
        targetOrtho = CanvasScaler.scaleFactor;
        raycaster = GetComponent<GraphicRaycaster>();
        infoBox.Value = InitialInfoBoxText;
        if (UseWebcore)
        {
            WebCore = WebCore.GetInstance(this);
        }
        RefreshCdm();

        /* FOR UNITY FRONTEND DEVELOPMENT ONLY ie NO-BACKEND-SERVER*/
        // LoadData();
        //---------------------------------------//
    }

    // Update is called once per frame
    void Update()
    {
        if ((_currentMode == CanvasMode.Default && InputExtender.MouseExtender.IsDoubleClick()) ||
            (_currentMode == CanvasMode.AddingClass && InputExtender.MouseExtender.IsSingleClick()))
        {
            _updateNeeded = true;
            WebCore.AddClass("Class" + (_nodes.Count + 1), Input.mousePosition);
            ActivateDefaultMode();
        }
        if ((_currentMode == CanvasMode.AddingEnumClass && InputExtender.MouseExtender.IsSingleClick()))
        {
            _updateNeeded = true;
            WebCore.AddEnumClass("Class" + (_nodes.Count + 1), Input.mousePosition);
            ActivateDefaultMode();
        }


        Zoom();

        if (UseWebcore && _updateNeeded)
        {
            // resend get request for frames where json data is not updated in time
            // reGetRequest is true after AddClass
            if (reGetRequest)
            {
                reGetRequest = false;
                RefreshCdm();
            }
        }
    }

    // LateUpdate is called at the end of a frame update, after all Update operations are done
    public void LateUpdate()
    {
        if (!_namesUpToDate)
        {
            UpdateNames();
        }
    }

    // ************ Controller Methods for Canvas/Diagram ****************//

    /// <summary>
    /// Loads and displays the contents of jsonFile defined in Unity editor. 
    /// </summary>
    private void LoadData()
    {
        LoadJson(jsonFile.text);
    }

    public void InitializeDiagram(string cdmJson)
    {
        ClassDiagramDTO cdmDto = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);

        //store attributes of class in a dictionary
        cdmDto.classDiagram.classes.ForEach(cls => classIdToAttributes[cls._id] = cls.attributes);
        //store association ends of a class in a dictionary
        cdmDto.classDiagram.classes.ForEach(cls => classIdToAssociationEndsDTO[cls._id] = cls.associationEnds);        

        CreateClassesFromDTO(cdmDto);

        if (cdmDto.classDiagram.associations != null)
        {
            //TODO
        }

        _updateNeeded = false;

        Debug.Log("Diagram initialized");
    }

    /// <summary>
    /// Loads and displays the class diagram encoded by the input JSON string.
    /// </summary>
    public void LoadJson(string cdmJson)
    {
        Debug.Log("cdmJson: " + cdmJson);
        ClassDiagramDTO cdmDto = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);

        //store attributes of class in a dictionary
        cdmDto.classDiagram.classes.ForEach(cls => classIdToAttributes[cls._id] = cls.attributes);
        //store association ends of a class in a dictionary
        cdmDto.classDiagram.classes.ForEach(cls => classIdToAssociationEndsDTO[cls._id] = cls.associationEnds);

        ClearClasses();
        CreateClassesFromDTO(cdmDto);

        _updateNeeded = false;
    }

    private void CreateClassesFromDTO(ClassDiagramDTO cdmDto)
    {
        var idsToEnumsAndLayouts = new Dictionary<string, List<object>>();
        //store attribute types. Map type id to eclass tye
        cdmDto.classDiagram.types.ForEach(type =>
        {
            //cache eClass attr with shortened substring
            //Eg. http://cs.mcgill.ca/sel/cdm/1.0#//CDString -> string
            string res = type.eClass[(type.eClass.LastIndexOf('/') + 1)..].Replace("CD", "").ToLower();
            
            if(res=="enum")
            {
                idsToEnumsAndLayouts[type._id] = new List<object> { type, null };
                classIdToLiterals[type._id] = type.literals;
                attrTypeIdsToTypes[type._id] = type.name.ToLower();
                
                
            }
            else if(!attrTypeIdsToTypes.ContainsKey(type._id))
            {
                attrTypeIdsToTypes[type._id] = res;
            }
        });

        // maps each _id to its (class object, position) pair 
        var idsToClassesAndLayouts = new Dictionary<string, List<object>>();   

        cdmDto.classDiagram.classes.ForEach(cls => idsToClassesAndLayouts[cls._id] = new List<object> { cls, null });
        // Debug.Log("classDiagram: " + classDiagram);
        // Debug.Log("classDiagram.layout: " + classDiagram.layout);
        // Debug.Log("JsonUtility.ToJson(classDiagram.layout): " + JsonUtility.ToJson(classDiagram.layout));
        // Debug.Log("classDiagram.layout.containers: " + classDiagram.layout.containers);
        if (cdmDto.classDiagram.layout.containers == null)
        {
            Debug.Log("classDiagram.layout.containers is null, so early return");
            return;
        }
        Debug.Log("classDiagram.layout.containers.Count: " + cdmDto.classDiagram.layout.containers.Count); // NullReferenceException
        Debug.Log("classDiagram.layout.containers[0]: " + cdmDto.classDiagram.layout.containers[0]);
        Debug.Log("classDiagram.layout.containers[0].value: " + cdmDto.classDiagram.layout.containers[0].value);
        cdmDto.classDiagram.layout.containers[0].value.ForEach(contVal =>
        {
            if (idsToClassesAndLayouts.ContainsKey(contVal.key))
            {
                idsToClassesAndLayouts[contVal.key][1] = contVal;
            }
            if (idsToEnumsAndLayouts.ContainsKey(contVal.key))
            {
                idsToEnumsAndLayouts[contVal.key][1] = contVal;
            }
        });
        _namesToRects.Clear();

        foreach (var keyValuePair in idsToClassesAndLayouts)
        {
            var _id = keyValuePair.Key;
            var clsAndContval = keyValuePair.Value;
            //Debug.Log("class type: " + type.GetType());
            var cls = (Class)clsAndContval[0];
            var layoutElement = ((ElementMap)clsAndContval[1]).value;

            string className;
            if (!classIdToClassNames.TryGetValue(_id, out className)) 
            {
                className = cls.name;
            }
            _namesToRects[cls.name] = CreateCompartmentedRectangle(
                _id, className, new Vector2(layoutElement.x, layoutElement.y), 2);
        }
        foreach (var keyValuePair in idsToEnumsAndLayouts)
        {
            var _id = keyValuePair.Key;
            var clsAndContval = keyValuePair.Value;
            var cls = (CDType)clsAndContval[0];
            var layoutElement = ((ElementMap)clsAndContval[1]).value;
            _namesToRects[cls.name] = CreateCompartmentedRectangle(
                _id, cls.name, new Vector2(layoutElement.x, layoutElement.y), 1);
        }

        _namesUpToDate = false;
    }

    public void AddAttributesToSection(GameObject section)
    {
        var compId = section.GetComponent<Section>().GetCompartmentedRectangle()
            .GetComponent<CompartmentedRectangle>().ID;
        classIdToAttributes[compId].Reverse();
        foreach (var attr in classIdToAttributes[compId])
        {
            var attr_type_title_case = FirstCharacterUpper(attrTypeIdsToTypes[attr.type]);
            section.GetComponent<Section>().AddAttribute(attr._id, attr.name, attr_type_title_case);
            // Canvas.ForceUpdateCanvases();
        }
    }

     public void AddLiteralsToSection(GameObject section)
    {
        var compId = section.GetComponent<Section>().GetCompartmentedRectangle()
            .GetComponent<CompartmentedRectangle>().ID;
        foreach (var lit in classIdToLiterals[compId])
        {
            //var attr_type_title_case = FirstCharacterUpper(attrTypeIdsToTypes[attr.type]);
            section.GetComponent<Section>().AddLiteral(lit._id, lit.name);
        }
    }

    /// <summary>
    /// Updates the names of the classes (otherwise, they all have the name of the most recently added class).
    /// </summary>
    public void UpdateNames()
    {
        foreach (var nameRectPair in _namesToRects)
        {
            nameRectPair.Value.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<InputField>().text =
                nameRectPair.Key;
        }
        _namesUpToDate = true;
    }

    /// <summary>
    /// Resets the frontend diagram representation. Does NOT reset the representation in the WebCore backend.
    /// </summary>
    public void ClearClasses()
    {
        Debug.Log("Clear Classes called");
        foreach (var node in _nodes)
        {
            Debug.Log("Reset Diagram: Node" + node.GetComponent<CompartmentedRectangle>().ID);
            // Pop-up menu is destroyed in comp rect class when delete is called
            // we only need to destroy the attributes.
            // get first section, loop through all attributes, destroy any attribute cross objects
            // TODO: May need to generalize Compartmented Rectangle to node, keep for now
            if(node.GetComponent<CompartmentedRectangle>() == null)
            {
                Debug.Log("No Compartmented Rectangle");
            }
            GameObject section = node.GetComponent<CompartmentedRectangle>().GetSection(0);
            var li = section.GetComponent<Section>().GetTextBoxList();
            if(section.GetComponent<Section>() == null)
            {
                Debug.Log("No section");
            }
            foreach (var attr in section.GetComponent<Section>().GetTextBoxList())
            {
                if (attr)
                {
                    if (attr.GetComponent<AttributeTextBox>())
                    {
                        if (attr.GetComponent<AttributeTextBox>().GetAttributeCross() != null)
                        {
                            //TODO: Destroy instance instead
                            attr.GetComponent<AttributeTextBox>().GetAttributeCross().GetComponent<AttributeCross>().Close();
                        }
                    }
                }
            }
        }
        _nodes.ForEach(Destroy);
        _nodes.Clear();
    }

    /// <summary>
    /// Creates a compartmented rectangle with the given name and position.
    /// </summary>
    public GameObject CreateCompartmentedRectangle(string _id, string name, Vector2 position, int sectionCount)
    {
        Debug.Log("CreateCompartmentedRectangle");
        var compRect = Instantiate(compartmentedRectangle, transform);
        compRect.transform.position = position;
        compRect.GetComponent<CompartmentedRectangle>().ID = _id;
        compRect.GetComponent<CompartmentedRectangle>().ClassName = name;
        compRect.GetComponent<CompartmentedRectangle>().setSectionCount(sectionCount);
        
        
        if (!AddNode(compRect))
        {
            Debug.Log("Fail to add node");
        }
        return compRect;
    }

    /// <summary>
    /// Zooms the user interface.
    /// </summary>
    void Zoom()
    {
        if (Input.touchSupported)
        {
            if (Input.touchCount == 2)
            {
                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;
                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);
                if ((oldTouchDistance - currentTouchDistance) != 0.0f)
                {
                    targetOrtho += Mathf.Clamp((oldTouchDistance - currentTouchDistance), -1, 1) * zoomSpeed * 0.03f;
                    targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
                }
            }
        }
        else
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                targetOrtho += scroll * zoomSpeed * 0.3f;
                targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
            }
        }
        CanvasScaler.scaleFactor =
            Mathf.MoveTowards(CanvasScaler.scaleFactor, targetOrtho, smoothSpeed * Time.deltaTime);
    }

    //------------------------
    // UI model Methods for Canvas/Diagram
    //------------------------

    /// <summary>
    /// Refreshes the class diagram by sending a GET request to the server.
    /// </summary>
    public bool RefreshCdm()
    {
        if (student != null)
        {
            var result = WebRequest.GetRequest(WebCore.CdmEndpoint(), student.Token);
            if (string.Equals(_currCdmStr, ""))
            {
                // initialize diagram
                InitializeDiagram(result);
                _currCdmStr = result;
                return true;                
            }
            if (result != _currCdmStr)
            {
                LoadJson(result);
                _currCdmStr = result;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Activates the default mode.
    /// </summary>
    public void ActivateDefaultMode()
    {
        if (_isWebGl)
        {
            ResetCursor();
        }
        _currentMode = CanvasMode.Default;
    }

    public void EnterAddClassMode()
    {
        if (_isWebGl)
        {
            SetCursorToAddMode();
        }
        Debug.Log("Entering Add class mode");
        _currentMode = CanvasMode.AddingClass;
    }

    public void EnterAddEnumClassMode()
    {
        if (_isWebGl)
        {
            SetCursorToAddMode();
        }
        Debug.Log("Entering Add enum class mode");
        _currentMode = CanvasMode.AddingEnumClass;
    }

    /// <summary>
    /// Called by AddClassAction when the AddClass button is pressed.
    /// </summary>
    public void AddClassButtonPressed()
    {
        if (_currentMode == CanvasMode.AddingClass)
        {
            ActivateDefaultMode();
        }
        else
        {
            EnterAddClassMode();
        }
    }

    /// <summary>
    /// Called by AddClassAction when the AddClass button is pressed.
    /// </summary>
    public void AddEnumClassButtonPressed()
    {
        if (_currentMode == CanvasMode.AddingEnumClass)
        {
            ActivateDefaultMode();
        }
        else
        {
            EnterAddEnumClassMode();
        }
    }

    public void GetFeedbackButtonPressed()
    {
        Debug.Log("Getting feedback");
        WebCore.GetFeedback();
    }

    /// <summary>
    /// Perform the debug action specified in the body upon the Debug button getting clicked.
    /// </summary>
    public void DebugAction()
    {
        Debug.Log("Debug button clicked!");
        //LoadData();
        // GetCompartmentedRectangles()[0].GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<InputField>()
        //   .text = "Rabbit";

        /*var*/ student = Student.CreateRandom();
        Debug.Log("student: " + student);
        Debug.Log("student.Login(): " + student.Login());
        Debug.Log("student.LoggedIn: " + student.LoggedIn);
        if (!student.LoggedIn)
        {
            infoBox.Warn("Login failed! See console for exact error. (Hint: Double check that WebCORE is running.)");
            return;
        }
        // Logout logic is currently faulty, so uncommenting the lines below will cause the CreateCdm() call to fail
        // Debug.Log("student.Logout(): " + student.Logout());
        // Debug.Log("student.LoggedIn: " + student.LoggedIn);
        // Debug.Log("student.Login() again: " + student.Login());
        // Debug.Log("student.LoggedIn: " + student.LoggedIn);
        var cdmCreated = WebCore.CreateCdm(cdmName);
        Debug.Log("WebCore.CreateCdm(): " + cdmCreated);
        if (cdmCreated)
        {
            infoBox.Info("Class diagram created! You can now add class diagram elements with the diagram editor.");
        }
        else
        {
            infoBox.Warn("Class diagram creation failed! See console for exact error.");
        }
    }

    public void LogIn(Student student)
    {
        Debug.Log("Debug button clicked!");

        Debug.Log("student.Login(): " + student.Login());
        Debug.Log("student.LoggedIn: " + student.LoggedIn);
        if (!student.LoggedIn)
        {
            infoBox.Warn("Login failed! See console for exact error. (Hint: Double check that WebCORE is running.)");
            return;
        }
        var cdmCreated = WebCore.CreateCdm(cdmName);
        Debug.Log("WebCore.CreateCdm(): " + cdmCreated);
        if (cdmCreated)
        {
            infoBox.Info("Class diagram created! You can now add class diagram elements with the diagram editor.");
        }
        else
        {
            infoBox.Warn("Class diagram creation failed! See console for exact error.");
        }
    }

    public Dictionary<string, string> GetAttrTypeIdsToTypes()
    {
        return attrTypeIdsToTypes;
    }

    public InfoBox GetInfoBox()
    {
        return infoBox;
    }


    //When pressing on canvas close popumenu and attributeclosebuttons
    public void CloseMenus()
    {
        // TODO
    }

    //------------------------
    // INTERFACE
    //------------------------

    /// <summary>
    /// Returns the list of nodes as a read-only list.
    /// </summary>
    public IList<GameObject> GetNode()
    {
        IList<GameObject> newNodes = _nodes.AsReadOnly();
        return newNodes;
    }

    /// <summary>
    /// Returns the number of nodes in the diagram.
    /// </summary>
    public int NumberOfNodes()
    {
        int number = _nodes.Count;
        return number;
    }

    /// <summary>
    /// Check if the diagram has at least one node.
    /// </summary>
    public bool HasNodes()
    {
        bool has = _nodes.Count > 0;
        return has;
    }

    /// <summary>
    /// Returns the list of edges as a read-only list.
    /// </summary>
    public IList<GameObject> GetEdges()
    {
        IList<GameObject> newEdges = _edges.AsReadOnly();
        return newEdges;
    }

    /// <summary>
    /// Returns the number of edges in the diagram.
    /// </summary>
    public int NumberOfEdges()
    {
        int number = _edges.Count;
        return number;
    }

    /// <summary>
    /// Check if the diagram has at least one edge.
    /// </summary>
    public bool HasEdges()
    {
        bool has = _edges.Count > 0;
        return has;
    }

    /// <summary>
    /// Adds a node to the diagram.
    /// </summary>
    public bool AddNode(GameObject aNode)
    {
        // Debug.Log("Add Node called");
        bool wasAdded = false;
        if (_nodes.Contains(aNode))
        {
            return false;
        }
        Node node = aNode.GetComponent<Node>();
        GameObject existingDiagram = node.GetDiagram();
        if (existingDiagram == null)
        {
            // Debug.Log("Diagram null");
            node.SetDiagram(gameObject);
        }
        else if (!gameObject.Equals(existingDiagram))
        {
            existingDiagram.GetComponent<Diagram>().RemoveNode(aNode);
            AddNode(aNode);
        }
        else
        {
            _nodes.Add(aNode);
            // Debug.Log("Node added to list");
            aNode.GetComponent<CompartmentedRectangle>().SetDiagram(gameObject);
        }
        wasAdded = true;
        return wasAdded;
    }

    /// <summary>
    /// Remove a node from the diagram.
    /// </summary>
    public bool RemoveNode(GameObject aNode)
    {
        bool wasRemoved = false;
        if (_nodes.Contains(aNode))
        {
            _nodes.Remove(aNode);
            aNode.GetComponent<Node>().SetDiagram(null);
            wasRemoved = true;
        }
        return wasRemoved;
    }

    /// <summary>
    /// Adds an edge to the diagram.
    /// </summary>
    public bool AddEdge(GameObject aEdge)
    {
        bool wasAdded = false;
        if (_edges.Contains(aEdge))
        {
            return false;
        }
        Edge edge = aEdge.GetComponent<Edge>();
        GameObject existingDiagram = edge.GetDiagram();
        if (existingDiagram == null)
        {
            edge.SetDiagram(gameObject);
        }
        else if (!this.Equals(existingDiagram))
        {
            existingDiagram.GetComponent<Diagram>().RemoveEdge(aEdge);
            AddEdge(aEdge);
        }
        else
        {
            _edges.Add(aEdge);
        }
        wasAdded = true;
        return wasAdded;
    }

    /// <summary>
    /// Remove a node from the diagram.
    /// </summary>
    public bool RemoveEdge(GameObject aEdge)
    {
        bool wasRemoved = false;
        if (_edges.Contains(aEdge))
        {
            _edges.Remove(aEdge);
            aEdge.GetComponent<Edge>().SetDiagram(null);
            wasRemoved = true;
        }
        return wasRemoved;
    }

    private string FirstCharacterUpper(string value)
    {
    if (value == null || value.Length == 0)
    {
        return string.Empty;
    }
    if (value.Length == 1)
    {
        return value.ToUpper();
    }
    var firstChar = value.Substring(0, 1).ToUpper();
    return firstChar + value.Substring(1, value.Length - 1);
    }

}
