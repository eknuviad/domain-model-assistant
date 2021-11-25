using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Edge : MonoBehaviour
{
    private LineRenderer line;
    // private Vector3 mousePos;
    public GameObject popupLineMenu;
    float holdTimer = 0;
    bool hold = false;
    private Diagram _diagram;
    public Material material;
    // private int currLines = 0;//ciunter for lines drawn
    public List<GameObject> nodes = new List<GameObject>();
    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }
    void Start()
    {

    }

    void Update()
    {
        // if (nodes != null)
        // {
        //     var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position);
        //     pos1.z = 0;
        //     var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position);
        //     pos2.z = 0;
        //     line.SetPosition(0, pos1);
        //     line.SetPosition(1, pos2);
        // }
        if(this.hold)
        {
            OnBeginHold();  
        }
    }

    void createEdge()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        line = this.gameObject.GetComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2; //straightline with 2 end points
        line.startWidth = 0.1f; //line width
        line.endWidth = 0.1f; //line width
        line.useWorldSpace = false; //set to true so lines defined in world space
        line.numCapVertices = 50;
        Debug.Log("line created");
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
        return true;
    }

    public void OnBeginHold()
    {
        this.hold = true;
        holdTimer += Time.deltaTime;
        //_prevPosition = this.transform.position;
    }

    public void OnEndHold()
    {
        if (holdTimer > 1f - 5)
        {
            SpawnPopupLineMenu();
        }

    }
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
            this.popupLineMenu.transform.position = this.transform.position + new Vector3(0, 0, 0);
            this.popupLineMenu.GetComponent<PopupLineMenu>().SetLine(this);
            this.popupLineMenu.GetComponent<PopupLineMenu>().Open();
        }
        else
        {
            this.popupLineMenu.GetComponent<PopupLineMenu>().Open();
        }
    }


}