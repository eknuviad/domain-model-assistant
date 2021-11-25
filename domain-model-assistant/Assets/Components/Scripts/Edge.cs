using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Edge : MonoBehaviour
{
     public string ID
    { get; set; }
    private LineRenderer line;
    public List<GameObject> nodes = new List<GameObject>();
    
    public List<GameObject> edgeEnds = new List<GameObject>();
    public GameObject edgeEnd;
    public GameObject edgeTitle;
    public GameObject edgeEndNumber;
    public GameObject textbox;
    // private Vector3 mousePos;
    public GameObject popupLineMenu;
    float holdTimer = 0;
    bool hold = false;
    private Diagram _diagram;
    private Vector3 mousePos;
    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }
    void Start()
    {
        createEdge();
    }

    void Update()
    {
        if (nodes != null)
        {
            var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(0,-95,0));
            pos1.z = 0;
            var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(0,95,0));
            pos2.z = 0;
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
        }
        
        if(Input.GetMouseButtonDown(0)){
            mousePos = Input.mousePosition;
            SpawnPopupLineMenu();
        }
    }

    void createEdge()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        line = this.gameObject.GetComponent<LineRenderer>();
        // line.material = material;
        line.material = new Material (Shader.Find ("Sprites/Default"));
        line.material.color = Color.black; 
        line.positionCount = 2; //straightline with 2 end points
        line.startWidth = 0.1f; //line width
        line.endWidth = 0.1f; //line width
        line.useWorldSpace = true; //set to true so lines defined in world space
        line.numCapVertices = 50;
        Debug.Log("line created");
        CreateEdgeEndUpperObj(nodes[0]);//create edge number and title textboxes for first obj
        CreateEdgeEndLowerObj(nodes[1]);

    }

    void Destroy(){
         Destroy(this.gameObject);
    }

    public bool AddNode(GameObject aNode){
         if(nodes.Contains(aNode)){
            return false;
        }
        nodes.Add(aNode);
        aNode.GetComponent<Node>().AddEdge(this.gameObject);
        Debug.Log("node added to edge");
        return true;
    }
//create edge end for upper object
    public void CreateEdgeEndUpperObj(GameObject obj){
        this.edgeTitle = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumber = GameObject.Instantiate(textbox, obj.transform);
        //will need to get cordinates from edge object in future
        this.edgeTitle.transform.position += new Vector3(40,-170,0);
        this.edgeEndNumber.transform.position += new Vector3(125,-170,0);
        this.edgeTitle.GetComponent<InputField>().text = "enter text";
        this.edgeEndNumber.GetComponent<InputField>().text = "*";
        Debug.Log(nodes[0].transform.position);
        Debug.Log(this.edgeTitle.transform.position);
        Debug.Log("edgeend here");
    }
    public void CreateEdgeEndLowerObj(GameObject obj){
        this.edgeTitle = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumber = GameObject.Instantiate(textbox, obj.transform);
        //will need to get cordinates from edge object in future
        this.edgeTitle.transform.position += new Vector3(40,85,0);
        this.edgeEndNumber.transform.position += new Vector3(125,85,0);
        this.edgeTitle.GetComponent<InputField>().text = "enter text";
        this.edgeEndNumber.GetComponent<InputField>().text = "*";
    }
    public bool AddEdgeEnd(GameObject aEdgeEnd){
         if(edgeEnds.Contains(aEdgeEnd)){
            return false;
        }
        edgeEnds.Add(aEdgeEnd);
        aEdgeEnd.GetComponent<Node>().AddEdge(this.gameObject);
        Debug.Log("edge end added");
        return true;
    }

    // public void OnBeginHold()
    // {
    //     Debug.Log("here1");
    //     this.hold = true;
    //     holdTimer += Time.deltaTime;
    //     //_prevPosition = this.transform.position;
    // }

    // public void OnEndHold()
    // {
    //     if (holdTimer > 1f - 5)
    //     {
    //         Debug.Log("here to spawn edge popup");
    //         SpawnPopupLineMenu();
    //     }
    //     holdTimer = 0;
    //     this.hold = false;

    // }
    public void SetRelationship(int type)
{
    //line = this.GetComponent<DrawLine>();
    if(type == 0)
    {
        //Set relationship to Association

    }
}
    public GameObject GetPopUpLineMenu()
{
    return popupLineMenu;
}

    void SpawnPopupLineMenu()
    {
        if (this.popupLineMenu.GetComponent<PopupLineMenu>().GetLine() == null)
        {
            this.popupLineMenu = GameObject.Instantiate(this.popupLineMenu);
            this.popupLineMenu.transform.SetParent(this.transform);
            //this can be changed so that popupline menu is always instantiated at
            //midpoint of the relationship
            this.popupLineMenu.transform.position = this.mousePos + new Vector3(70,-110,0);
            this.popupLineMenu.GetComponent<PopupLineMenu>().SetLine(this);
        }
        else
        {
            this.popupLineMenu.GetComponent<PopupLineMenu>().Open();
        }
    }


}