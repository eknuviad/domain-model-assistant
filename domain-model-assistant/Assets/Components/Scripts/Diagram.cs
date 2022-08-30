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
    public List<GameObject> compartmentedRectangles;
    public InfoBox infoBox;

    private Vector3 dragStartPos;
    // private bool dragging = false;

    public const bool UseWebcore = true; // Change to false to use the wrapper page JSON instead of WebCore

    public const string WebcoreEndpoint = "http://localhost:8080";

    public string cdmName = "FerrySystem";

    public Student student;

    public WebCore WebCore;

    // Temporary text used during application development
    public const string InitialInfoBoxText = "Welcome to the Modeling Assistant! "
        + "Please use the Debug button in the top right corner to login to a new random WebCORE account.";

    GraphicRaycaster raycaster;

    readonly Dictionary<string, GameObject> _namesToRects = new();

    readonly Dictionary<string, List<Attribute>> classIdToAttributes = new();

    readonly Dictionary<string, string> attrTypeIdsToTypes = new();

    List<string> createdAttributeIds = new();

    enum CanvasMode
    {
        Default,
        AddingClass,
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
            WebCore.AddClass("Class" + (compartmentedRectangles.Count + 1), Input.mousePosition);
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

    /// <summary>
    /// Loads and displays the class diagram encoded by the input JSON string.
    /// </summary>
    public void LoadJson(string cdmJson)
    {
        ResetDiagram();
        //Debug.Log(cdmJson);
        var cdmDto = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);

        //store attributes of class in a dictionary
        cdmDto.classDiagram.classes.ForEach(cls => classIdToAttributes[cls._id] = cls.attributes);
        //store attribute types. Map type id to eclass tye
        cdmDto.classDiagram.types.ForEach(type =>
        {
            //cache eClass attr with shortened substring
            //Eg. http://cs.mcgill.ca/sel/cdm/1.0#//CDString -> string
            string res = type.eClass[(type.eClass.LastIndexOf('/') + 1)..].Replace("CD", "").ToLower();
            if (!attrTypeIdsToTypes.ContainsKey(type._id))
            {
                attrTypeIdsToTypes[type._id] = res;
            }
        });
        // maps each _id to its (class object, position) pair 
        var idsToClassesAndLayouts = new Dictionary<string, List<object>>();

        cdmDto.classDiagram.classes.ForEach(cls => idsToClassesAndLayouts[cls._id] = new List<object> { cls, null });
        Debug.Log("cdmJson: " + cdmJson);
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
        });
        _namesToRects.Clear();

        foreach (var keyValuePair in idsToClassesAndLayouts)
        {
            var _id = keyValuePair.Key;
            var clsAndContval = keyValuePair.Value;
            var cls = (Class)clsAndContval[0];
            var layoutElement = ((ElementMap)clsAndContval[1]).value;
            _namesToRects[cls.name] = CreateCompartmentedRectangle(
                _id, cls.name, new Vector2(layoutElement.x, layoutElement.y));
        }
        _namesUpToDate = false;
        _updateNeeded = false;
    }

    public void AddAttributesToSection(GameObject section)
    {
        var compId = section.GetComponent<Section>().GetCompartmentedRectangle()
            .GetComponent<CompartmentedRectangle>().ID;
        foreach (var attr in classIdToAttributes[compId])
        {
            section.GetComponent<Section>().AddAttribute(attr._id, attr.name, attrTypeIdsToTypes[attr.type]);
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
    public void ResetDiagram()
    {
        foreach (var comp in compartmentedRectangles)
        {
            // Pop-up menu is destroyed in comp rect class when delete is called
            // we only need to destroy the attributes.
            // get first section, loop through all attributes, destroy any attribute cross objects
            GameObject section = comp.GetComponent<CompartmentedRectangle>().GetSection(0);
            foreach (var attr in section.GetComponent<Section>().GetTextBoxList())
            {
                if (attr)
                {
                    if (attr.GetComponent<TextBox>())
                    {
                        if (attr.GetComponent<TextBox>().GetAttributeCross() != null)
                        {
                            //TODO: Destroy instance instead
                            attr.GetComponent<TextBox>().GetAttributeCross().GetComponent<AttributeCross>().Close();
                        }
                    }
                }
            }
        }
        compartmentedRectangles.ForEach(Destroy);
        compartmentedRectangles.Clear();
    }

    /// <summary>
    /// Creates a compartmented rectangle with the given name and position.
    /// </summary>
    public GameObject CreateCompartmentedRectangle(string _id, string name, Vector2 position)
    {
        var compRect = Instantiate(compartmentedRectangle, transform);
        compRect.transform.position = position;
        compRect.GetComponent<CompartmentedRectangle>().ID = _id;
        compRect.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<InputField>().text = name;
        AddNode(compRect);
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

    // ************ UI model Methods for Canvas/Diagram ****************//

    /// <summary>
    /// Adds a node to the diagram.
    /// </summary>
    public bool AddNode(GameObject node)
    {
        if (compartmentedRectangles.Contains(node))
        {
            return false;
        }
        compartmentedRectangles.Add(node);
        node.GetComponent<CompartmentedRectangle>().SetDiagram(this.gameObject);
        return true;
    }

    public bool RemoveNode(GameObject node)
    {
        if (compartmentedRectangles.Contains(node))
        {
            compartmentedRectangles.Remove(node);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the list of compartmented rectangles.
    /// </summary>
    public List<GameObject> GetCompartmentedRectangles()
    {
        return compartmentedRectangles;
    }

    /// <summary>
    /// Refreshes the class diagram by sending a GET request to the server.
    /// </summary>
    public bool RefreshCdm()
    {
        if (student != null)
        {
            var result = WebRequest.GetRequest(WebCore.CdmEndpoint(), student.Token);
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

    public Dictionary<string, string> GetAttrTypeIdsToTypes()
    {
        return attrTypeIdsToTypes;
    }

    //When pressing on canvas close popumenu and attributeclosebuttons
    public void CloseMenus()
    {
        // TODO
    }

}
