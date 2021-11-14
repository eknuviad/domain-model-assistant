using System;
using System.Collections;
using System.Collections.Generic;
//using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
//using System.Text.Json; // TODO Use JsonSerializer.Serialize when Unity moves to .NET 6
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;


public class Diagram : MonoBehaviour
{

    public TextAsset jsonFile;
    public float zoomSpeed = 1;
    public CanvasScaler CanvasScaler;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 0.0f;
    public float maxOrtho = 20.0f;
    private Vector3 dragStartPos;
    // private bool dragging = false;
    public GameObject compartmentedRectangle;
    public List<GameObject> compartmentedRectangles;

    public const bool UseWebcore = true; // Change to false to use the wrapper page JSON instead of WebCore

    public const string WebcoreEndpoint = "http://localhost:8080/";

    public const string cdmName = "MULTIPLE_CLASSES"; // TODO Remove this once API is updated

    public const string GetCdmEndpoint = WebcoreEndpoint + "classdiagram/" + cdmName;

    public const string AddClassEndpoint = GetCdmEndpoint + "/class";

    public const string DeleteClassEndpoint = AddClassEndpoint; // + "/:class_id"

    public const string UpdateClassEndpoint = GetCdmEndpoint; //+ {classId}/position

    public const string AddAttributeEndpoint = AddClassEndpoint; // +/{classId}/attribute
    private ClassDiagramDTO classDTO;

    GraphicRaycaster raycaster;

    Dictionary<string, GameObject> _namesToRects = new Dictionary<string, GameObject>();

    Dictionary<string, List<Attribute>> classIdToAttributes = new Dictionary<string, List<Attribute>>();

    Dictionary<string, string> attrIdsToTypes = new Dictionary<string, string>(); //attribute ids to their types

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
    private static extern void SetCursorToAddMode();

    [DllImport("__Internal")]
    private static extern void ResetCursor();

    private bool _updateNeeded = false;

    private bool _namesUpToDate = false;

    // true if app is run in browser, false if run in Unity editor
    private bool _isWebGl = false;

    private UnityWebRequestAsyncOperation _getRequestAsyncOp;

    private UnityWebRequestAsyncOperation _postRequestAsyncOp;

    private UnityWebRequestAsyncOperation _deleteRequestAsyncOp;

    private UnityWebRequestAsyncOperation _putRequestAsyncOp;

    private string _getResult = "";

    public string ID
    { get; set; }

    // Awake is called once to initialize this game object
    void Awake()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _isWebGl = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CanvasScaler = this.gameObject.GetComponent<CanvasScaler>();
        targetOrtho = CanvasScaler.scaleFactor;
        this.raycaster = GetComponent<GraphicRaycaster>();
        GetRequest(GetCdmEndpoint);


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
            AddClass("Class" + (compartmentedRectangles.Count + 1), Input.mousePosition);
            ActivateDefaultMode();
        }

        Zoom();

        if (UseWebcore && _updateNeeded)
        {
            if (_getRequestAsyncOp != null && _getRequestAsyncOp.isDone)
            {
                var req = _getRequestAsyncOp.webRequest;
                if (req.downloadHandler != null && !ReferenceEquals(req.downloadHandler, null))
                {
                    var newResult = req.downloadHandler.text;
                    if (newResult != _getResult)
                    {
                        LoadJson(newResult);
                        _getResult = newResult;
                    }
                }
                _updateNeeded = false;
                req.Dispose();
            }
        }
        if (_postRequestAsyncOp != null && _postRequestAsyncOp.isDone)
        {
            _postRequestAsyncOp.webRequest.Dispose();
        }
        if (_deleteRequestAsyncOp != null && _deleteRequestAsyncOp.isDone)
        {
            _deleteRequestAsyncOp.webRequest.Dispose();
        }
        if (_putRequestAsyncOp != null && _putRequestAsyncOp.isDone)
        {
            _putRequestAsyncOp.webRequest.Dispose();
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
        var classDiagram = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);

        //store attributes of class in a dictionary
        classDiagram.classes.ForEach(cls => this.classIdToAttributes[cls._id] = cls.attributes);
        //store attribute types. Map type id to eclass tye
        classDiagram.types.ForEach(type =>
        {
            //cache eClass attr with shortened substring
            //Eg. http://cs.mcgill.ca/sel/cdm/1.0#//CDString -> string
            string res = type.eClass.Substring(type.eClass.LastIndexOf('/') + 1).Replace("CD", "").ToLower();
            if (!attrIdsToTypes.ContainsKey(type._id))
            {
                this.attrIdsToTypes[type._id] = res;
            }
        });
        // maps each _id to its (class object, position) pair 
        var idsToClassesAndLayouts = new Dictionary<string, List<object>>();

        classDiagram.classes.ForEach(cls => idsToClassesAndLayouts[cls._id] = new List<object> { cls, null });
        classDiagram.layout.containers[0].value/*s*/.ForEach(contVal =>
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
    }

    public void AddAttributesToSection(GameObject section)
    {
        var compId = section.GetComponent<Section>().GetCompartmentedRectangle()
                        .GetComponent<CompartmentedRectangle>().ID;
        foreach (var attr in classIdToAttributes[compId])
        {
            section.GetComponent<Section>().AddAttribute(attr._id, attr.name, attrIdsToTypes[attr.type]);
        }

    }

    /// <summary>
    /// Adds a class to the diagram with the given name and position.
    /// </summary>
    public void AddClass(string name, Vector2 position)
    {
        if (UseWebcore)
        {
            // TODO Replace this ugly string once Unity moves to .NET 6
            AddJsonClass info = new AddJsonClass();
            info.x = position.x;
            info.y = position.y;
            info.className = name;
            string jsonData = JsonUtility.ToJson(info);
            PostRequest(AddClassEndpoint, jsonData);
            GetRequest(GetCdmEndpoint);
        }
        else
        {
            CreateCompartmentedRectangle((compartmentedRectangles.Capacity + 1).ToString(), name, position);
        }
    }

    public void DeleteClass(GameObject node)
    {
        if (UseWebcore)
        {
            string _id = node.GetComponent<CompartmentedRectangle>().ID;
            DeleteRequest(DeleteClassEndpoint, _id);
            GetRequest(GetCdmEndpoint);
            // No need to remove or destroy the node here since entire class diagram is recreated
        }
        else
        {
            RemoveNode(node);
            Destroy(node);
        }
    }

    public void UpdateClass(GameObject header, GameObject node)
    {
        if (UseWebcore)
        {
            string _id = node.GetComponent<CompartmentedRectangle>().ID;
            string clsName = header.GetComponent<InputField>().text;
            Vector2 newPosition = node.GetComponent<CompartmentedRectangle>().GetPosition();
            //JSON body. Create new serializable JSON object.
            Position pInfo = new Position(newPosition.x, newPosition.y);
            string jsonData = JsonUtility.ToJson(pInfo);
            //send updated position via PUT request
            PutRequest(UpdateClassEndpoint, jsonData, _id);
        }
    }

    /// <summary>
    /// Updates the names of the classes (otherwise, they all have the name of the most recently added class).
    /// </summary>
    public void UpdateNames()
    {
        foreach (var nameRectPair in _namesToRects)
        {
            nameRectPair.Value.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<InputField>().text = nameRectPair.Key;
        }
        _namesUpToDate = true;
    }

    /// <summary>
    /// Add Attribute
    /// </summary> 
    public void AddAttribute(GameObject textBox, string name)
    {
        if (UseWebcore)
        {
            // TODO Replace this ugly string once Unity moves to .NET 6
            AddAttributeJsonClass info = new AddAttributeJsonClass();
            // AddAttribute(attr._id, attr.name, type)
            // @param body {"rankIndex": Integer, "typeId": Integer, "attributeName": String}
            info.rankIndex = 0;
            string _id = textBox.GetComponent<TextBox>().ID;
            info.typeId = Int16.Parse(_id);
            info.attributeName = name;
            string jsonData = JsonUtility.ToJson(info);
            PostRequest(AddAttributeEndpoint, jsonData);
            GetRequest(GetCdmEndpoint);
        }
    }

    /// <summary>
    /// Resets the frontend diagram representation. Does NOT reset the representation in the WebCore backend.
    /// </summary>
    public void ResetDiagram()
    {
        foreach (var comp in compartmentedRectangles)
        {
            //destroy any exisiting popup menu objects
            GameObject popupMenu = comp.GetComponent<CompartmentedRectangle>().GetPopUpMenu();
            if (popupMenu != null)
            {
                //TODO: Destroy instance instead 
                // Destroying gameobject might destoy the asset. Closing the menu for now.
                popupMenu.GetComponent<PopupMenu>().Close();
            }
            //get first section, loop through all attributes, destroy any attribute cross objects
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
        var compRect = Instantiate(compartmentedRectangle, this.transform);
        compRect.transform.position = position;
        compRect.GetComponent<CompartmentedRectangle>().ID = _id;
        compRect.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<InputField>().text = name;
        AddNode(compRect);
        return compRect;
    }

    /// <summary>
    /// Sends a GET request to the server. The response is the class diagram JSON and is stored in _getResult.
    /// </summary>
    public void GetRequest(string uri)
    {
        // TODO Check if a `using` block can be used here, to auto-dispose the web request
        var webRequest = UnityWebRequest.Get(uri);
        webRequest.method = "GET";
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.disposeDownloadHandlerOnDispose = false;
        _getRequestAsyncOp = webRequest.SendWebRequest();
        _updateNeeded = true;
    }

    /// <summary>
    /// Sends a POST request to the server to modify the class diagram.
    /// </summary>
    public void PostRequest(string uri, string data)
    {
        var webRequest = UnityWebRequest.Put(uri, data);
        webRequest.method = "POST";
        webRequest.disposeDownloadHandlerOnDispose = false;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        _postRequestAsyncOp = webRequest.SendWebRequest();
    }


    /// <summary>
    /// Sends a DELETE request to the server to remove an item from the class diagram.
    /// </summary>
    public void DeleteRequest(string uri, string _id)
    {
        var webRequest = UnityWebRequest.Delete(uri + "/" + _id);
        webRequest.method = "DELETE";
        webRequest.disposeDownloadHandlerOnDispose = false;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        _deleteRequestAsyncOp = webRequest.SendWebRequest();
    }

    public void PutRequest(string uri, string data, string _id)
    {
        var webRequest = UnityWebRequest.Put(uri + "/" + _id + "/" + "position", data);
        webRequest.method = "PUT";
        webRequest.disposeDownloadHandlerOnDispose = false;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        _putRequestAsyncOp = webRequest.SendWebRequest();
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
        CanvasScaler.scaleFactor = Mathf.MoveTowards(CanvasScaler.scaleFactor, targetOrtho, smoothSpeed * Time.deltaTime);
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

    public class AddAttributeJsonClass
    {
        public int rankIndex;

        public int typeId;

        public string attributeName;

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
    /// Activates the default mode.
    /// </summary>
    public void ActivateDefaultMode()
    {
        if (_isWebGl)
        {
            ResetCursor();
        }
        Debug.Log("Activating default mode");
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
        GetCompartmentedRectangles()[0].GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<InputField>()
          .text = "Rabbit";
    }

    //When pressing on canvas close popumenu and attributeclosebuttons
    public void CloseMenus()
    {
        // TODO
    }

}
