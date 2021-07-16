using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

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
  public List<GameObject> compRectList;
  private ClassDiagramDTO classDTO;

  public string ID
  { get; set; }

  void Awake()
  {
    LoadData();
  }

  // Start is called before the first frame update
  GraphicRaycaster raycaster;

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
      Vector2 tempFingerPos = (Input.mousePosition);
      CreateCompartmentedRectangle(tempFingerPos);
    }
    Zoom();
  }

  // ************ Controller Methods for Canvas/Diagram ****************//

  private void LoadData()
  {
    Debug.Log("Loading data ...");
    // obtain class DTO from json string format
    classDTO = JsonUtility.FromJson<ClassDiagramDTO>(jsonFile.text);
    // convert float positions to Vector2
    var value = classDTO.layout.containers[0].values[0].value;
    Vector2 position = new Vector2(value.x, value.y);
    // create comp rectangle with header and sections
    GameObject newCompRect = CreateCompartmentedRectangle(position);
    // set the header value of the created class
    newCompRect.GetComponent<CompartmentedRectangle>().getHeader().
                GetComponent<TextBox>().setText(classDTO.classes[0].name);
  }

  /// <summary>
  /// Loads and displays the class diagram encoded by the input JSON string.
  /// </summary>
  private void LoadData(string cdmJson)
  {
    var classDiagram = JsonUtility.FromJson<ClassDiagramDTO>(cdmJson);
    var idsToClassesAndLayouts = new Dictionary<string, List<object>>();
    classDiagram.classes.ForEach(cls => idsToClassesAndLayouts[cls._id] = new List<object>{cls, null});
    classDiagram.layout.containers[0].values.ForEach(contVal => idsToClassesAndLayouts[contVal._id][1] = contVal);

    foreach (var clsAndContval in idsToClassesAndLayouts.Values)
    {
      var cls = (Class)clsAndContval[0];
      var coord = ((Value)clsAndContval[1]).value;
      var compRect = CreateCompartmentedRectangle(new Vector2(coord.x, coord.y));
      compRect.GetComponent<CompartmentedRectangle>().getHeader().GetComponent<TextBox>().setText(cls.name);
    }
  }

  public GameObject CreateCompartmentedRectangle(Vector2 position)
  {
    // added this for debugging
    StartCoroutine(GetRequest("http://127.0.0.1:8538/helloworld"));

    GameObject compRect = Instantiate(compartmentedRectangle, this.transform);
    compRect.transform.position = position;
    addNode(compRect);
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

  public bool addNode(GameObject aNode)
  {
    bool wasSet = false;
    if (compRectList.Contains(aNode))
    {
      return false;
    }
    compRectList.Add(aNode);
    aNode.GetComponent<CompartmentedRectangle>().setDiagram(this.gameObject);
    Debug.Log("Node added to list of compartmented rectangles");
    wasSet = true;
    return wasSet;
  }

  public List<GameObject> getCompartmentedRectangles()
  {
    return compRectList;
  }

}
