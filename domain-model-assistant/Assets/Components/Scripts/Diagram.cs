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
  public List<GameObject /*CompartmentedRectangle*/> compartmentedRectangles;

  public const string WebcoreEndpoint = "http://localhost:8080/";

  public const string cdmName = "MULTIPLE_CLASSES"; // TODO Remove this once API is updated

  public const string GetCdmEndpoint = WebcoreEndpoint + "classdiagram/" + cdmName;

  public const string AddClassEndpoint = GetCdmEndpoint + "/class";

  private ClassDiagramDTO classDTO;

  GraphicRaycaster raycaster;

  Dictionary<string, GameObject> _namesToRects = new Dictionary<string, GameObject>();

  enum CanvasMode
  {
    Default,
    AddingClass,
    AddingAttribute,
    // ...
  }

  CanvasMode _currentMode = CanvasMode.Default;

  [DllImport("__Internal")]
  private static extern void SetCursorToAddMode();

  [DllImport("__Internal")]
  private static extern void ResetCursor();

  private bool _updateNeeded = false;

  private bool _namesUpToDate = false;

  private bool _isWebGl = false;

  private UnityWebRequestAsyncOperation _getRequestAsyncOp;

  private UnityWebRequestAsyncOperation _postRequestAsyncOp;

  private string _getResult = "";

  public string ID
  { get; set; }

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
  }

  // Update is called once per frame
  void Update()
  {
    if ((_currentMode == CanvasMode.Default && InputExtender.MouseExtender.IsDoubleClick()) ||
        (_currentMode == CanvasMode.AddingClass && InputExtender.MouseExtender.IsSingleClick()))
    {
      AddClass("Class" + compartmentedRectangles.Count, Input.mousePosition);
      ActivateDefaultMode();
    }
    Zoom();
    if (_updateNeeded && _getRequestAsyncOp != null && _getRequestAsyncOp.isDone)
    {
      var req = _getRequestAsyncOp.webRequest;
      if (req.downloadHandler != null && !ReferenceEquals(req.downloadHandler, null))
      {
        var newResult = req.downloadHandler.text;
        Debug.Log(newResult);
        if (newResult != _getResult)
        {
          LoadJson(newResult);
          _getResult = newResult;
          // _updateNeeded = false;
        }
      }
      _updateNeeded = false;
      req.Dispose();
    }
    if (_updateNeeded && _postRequestAsyncOp != null && _postRequestAsyncOp.isDone)
    {
      _postRequestAsyncOp.webRequest.Dispose(); 
    }
  }

  public void LateUpdate()
  {
    if (!_namesUpToDate)
    {
      UpdateNames();
    }
  }

  // ************ Controller Methods for Canvas/Diagram ****************//

  private void LoadData()
  {
    Debug.Log("Loading data ...");
    LoadJson(jsonFile.text);
  }

  /// <summary>
  /// Loads and displays the class diagram encoded by the input JSON string.
  /// </summary>
  public void LoadJson(string cdmJson)
  {
    Debug.Log("Loading JSON:" + cdmJson);
    ResetDiagram();
    var classDiagram = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);
    var idsToClassesAndLayouts = new Dictionary<string, List<object>>();
    classDiagram.classes.ForEach(cls => idsToClassesAndLayouts[cls._id] = new List<object>{cls, null});
    // Debug.Log(String.Join(",", idsToClassesAndLayouts.Keys.ToList()));
    // Debug.Log(String.Join(",", classDiagram.layout.containers[0].value.Select(cv => cv.key).ToList()));
    classDiagram.layout.containers[0].value/*s*/.ForEach(contVal =>
    {
      if (idsToClassesAndLayouts.ContainsKey(contVal.key))
      {
        idsToClassesAndLayouts[contVal.key][1] = contVal;
      }
    });

    _namesToRects.Clear();

    foreach (var clsAndContval in idsToClassesAndLayouts.Values)
    {
      Debug.Log("cls: " + clsAndContval[0] + ", contval: " + clsAndContval[1]);
      var cls = (Class)clsAndContval[0];
      var layoutElement = ((ElementMap)clsAndContval[1]).value;
      _namesToRects[cls.name] = CreateCompartmentedRectangle(cls.name, new Vector2(layoutElement.x, layoutElement.y));
    }

    _namesUpToDate = false;
  }

  public void AddClass(string name, Vector2 position)
  {
    // TODO Replace this ugly string once Unity moves to .NET 6
    var jsonData = $@"{{
      ""className"": ""{name}"",
      ""dataType"": false,
      ""isInterface"": false,
      ""x"": {position.x},
      ""y"": {position.y},
    }}";
    Debug.Log(jsonData);
    PostRequest(AddClassEndpoint, jsonData);
    GetRequest(GetCdmEndpoint);
  }

  public void UpdateNames()
  {
    foreach (var nameRectPair in _namesToRects)
    {
      nameRectPair.Value.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<TextBox>()
          .SetText(nameRectPair.Key);
    }
    _namesUpToDate = true;
  }

  public void ResetDiagram()
  {
    compartmentedRectangles.ForEach(Destroy);
    compartmentedRectangles.Clear();
  }

  public GameObject CreateCompartmentedRectangle(string name, Vector2 position) // should pass in _id
  {
    var compRect = Instantiate(compartmentedRectangle, this.transform);
    compRect.transform.position = position;
    compRect.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<TextBox>().SetText(name);
    AddNode(compRect);
    return compRect;
  }

  /// <summary>
  /// Returns the data from the server as a string.
  /// </summary>
  public void /*string*/ GetRequest(string uri)
  {
    /*using (*/var webRequest = UnityWebRequest.Get(uri);/*)
    {*/
      webRequest.SetRequestHeader("Content-Type", "application/json");
      webRequest.disposeDownloadHandlerOnDispose = false;
      _getRequestAsyncOp = webRequest.SendWebRequest();
      _updateNeeded = true;
      // if (webRequest.result == UnityWebRequest.Result.Success)
      // {
      //   return webRequest.downloadHandler.text;
      // }
      // else
      // {
      //   Debug.Log("HTTP GET error: " + webRequest.error);
      //   return "";
      // }
    // }
  }

  public void PostRequest(string uri, string data)
  {
    /*using (*/var webRequest = UnityWebRequest.Put(uri, data);/*)
    {*/
      webRequest.method = "POST";
      webRequest.disposeDownloadHandlerOnDispose = false;
      webRequest.SetRequestHeader("Content-Type", "application/json");
      _postRequestAsyncOp = webRequest.SendWebRequest();

      // if (webRequest.result != UnityWebRequest.Result.Success)
      // {
      //   Debug.Log("HTTP POST error: " + webRequest.error);
      // }
    // }
  }

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

  public List<GameObject> GetCompartmentedRectangles()
  {
    return compartmentedRectangles;
  }

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
    GetCompartmentedRectangles()[0].GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<TextBox>()
        .SetText("Rabbit");
  }

}
