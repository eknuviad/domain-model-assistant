using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

  private bool _namesUpToDate = false;

  private bool _isWebGl = false;

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
  }

  // Update is called once per frame
  void Update()
  {
    if ((_currentMode == CanvasMode.Default && InputExtender.MouseExtender.IsDoubleClick()) ||
        (_currentMode == CanvasMode.AddingClass && InputExtender.MouseExtender.IsSingleClick()))
    {
      CreateCompartmentedRectangle("Class" + compartmentedRectangles.Count, Input.mousePosition);
      ActivateDefaultMode();
    }
    Zoom();
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
    var classDiagram = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);
    var idsToClassesAndLayouts = new Dictionary<string, List<object>>();
    classDiagram.classes.ForEach(cls => idsToClassesAndLayouts[cls._id] = new List<object>{cls, null});
    classDiagram.layout.containers[0].values.ForEach(contVal => idsToClassesAndLayouts[contVal.key][1] = contVal);

    _namesToRects.Clear();

    foreach (var clsAndContval in idsToClassesAndLayouts.Values)
    {
      var cls = (Class)clsAndContval[0];
      var layoutElement = ((ElementMap)clsAndContval[1]).value;
      _namesToRects[cls.name] = CreateCompartmentedRectangle(cls.name, new Vector2(layoutElement.x, layoutElement.y));
    }

    _namesUpToDate = false;
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
    // added this for debugging
    //StartCoroutine(GetRequest("http://127.0.0.1:8538/helloworld/alice"));

    var compRect = Instantiate(compartmentedRectangle, this.transform);
    compRect.transform.position = position;
    compRect.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<TextBox>().SetText(name);
    AddNode(compRect);
    return compRect;
  }

  /// <summary>
  /// Example from docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Get.html
  /// </summary>
  IEnumerator GetRequest(string uri)
  {
    using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
    {
      // Request and wait for the desired page.
      yield return webRequest.SendWebRequest();

      string[] pages = uri.Split('/');
      int page = pages.Length - 1;

      switch (webRequest.result)
      {
        case UnityWebRequest.Result.ConnectionError:
        case UnityWebRequest.Result.DataProcessingError:
          Debug.LogError(pages[page] + ": Error: " + webRequest.error);
          break;
        case UnityWebRequest.Result.ProtocolError:
          Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
          break;
        case UnityWebRequest.Result.Success:
          Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
          break;
      }
    }
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
