using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

  // [Header("The reset event.")]
  // public UnityEvent resetEvent;

  private ClassDiagramDTO classDTO;

  GraphicRaycaster raycaster;

  public string ID
  { get; set; }

  void Awake()
  {
    LoadData();
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
    if (InputExtender.MouseExtender.isDoubleClick(0))
    {
      CreateCompartmentedRectangle("Class" + compartmentedRectangles.Count, Input.mousePosition);
    }
    Zoom();
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

    foreach (var clsAndContval in idsToClassesAndLayouts.Values)
    {
      var cls = (Class)clsAndContval[0];
      Debug.Log("Creating class with name " + cls.name);
      var layoutElement = ((ElementMap)clsAndContval[1]).value;
      var compRect = CreateCompartmentedRectangle(cls.name, new Vector2(layoutElement.x, layoutElement.y));
      //compRect.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<TextBox>().SetText(cls.name);
    }

    // foreach (var clsAndContval in idsToClassesAndLayouts.Values)
    // {
    //   var cls = (Class)clsAndContval[0];
    //   var layoutElement = ((ElementMap)clsAndContval[1]).value;
    //   var compRect = CreateCompartmentedRectangle(cls.name, new Vector2(layoutElement.x, layoutElement.y));
    //   compRect.GetComponent<CompartmentedRectangle>().getHeader().GetComponent<TextBox>().setText(cls.name);
    // }
  }

  public void ResetDiagram()
  {
    compartmentedRectangles.ForEach(Destroy);
    compartmentedRectangles.Clear();
  }

  public GameObject /*CompartmentedRectangle*/ CreateCompartmentedRectangle(string name, Vector2 position) // should pass in _id
  {
    // added this for debugging
    //StartCoroutine(GetRequest("http://127.0.0.1:8538/helloworld/alice"));

    var compRect = Instantiate(compartmentedRectangle, this.transform); // gameObject.AddComponent<CompartmentedRectangle>();
    compRect.transform.position = position;
    var textBox = compRect.GetComponent<CompartmentedRectangle>().GetHeader().GetComponent<TextBox>();
    Debug.Log("Setting text from " + textBox.GetText() + " to " + name);
    textBox.SetText(name);
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

  public bool AddNode(GameObject /*CompartmentedRectangle*/ node)
  {
    if (compartmentedRectangles.Contains(node))
    {
      return false;
    }
    compartmentedRectangles.Add(node);
    node.GetComponent<CompartmentedRectangle>().SetDiagram(this.gameObject);
    Debug.Log("Node added to list of compartmented rectangles");
    Debug.Log(node.GetComponent<CompartmentedRectangle>());
    return true;
  }

  public bool RemoveNode(GameObject /*CompartmentedRectangle*/ node)
  {
    if (compartmentedRectangles.Contains(node))
    {
      compartmentedRectangles.Remove(node);
      return true;
    }
    return false;
  }

  public List<GameObject /*CompartmentedRectangle*/> GetCompartmentedRectangles()
  {
    return compartmentedRectangles;
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
