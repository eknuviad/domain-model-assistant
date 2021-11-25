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
    void Start()
    {
        createEdge();
    }

    void Update()
    {
        // if (nodes != null)
        // {
        //     var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(0,-95,0));
        //     pos1.z = 0;
        //     var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(0,95,0));
        //     pos2.z = 0;
        //     line.SetPosition(0, pos1);
        //     line.SetPosition(1, pos2);
        // }
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
        CreateEdgeEnd(nodes[0]);//create edge number and title textboxes for first obj
    

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

    public void CreateEdgeEnd(GameObject obj){
        // var aPos = Camera.main.WorldToScreenPoint(pos);
        // GameObject edgEnd = GameObject.Instantiate(edgeEnd, nodes[0].transform);
        this.edgeTitle = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumber = GameObject.Instantiate(textbox, obj.transform);
        this.edgeTitle.transform.position += new Vector3(60,-170,0);
        this.edgeEndNumber.transform.position += new Vector3(125,-170,0);

        // edgeEnd.transform.position = aPos;
        // Vector3[] postions = this.gameObject.GetComponent<LineRenderer>().GetPositions();
        // Vector3 startPos = this.gameObject.GetComponent<LineRenderer>().GetPosition(0);
        // var tmp = Camera.main.WorldToScreenPoint(startPos);
        // tmp.z = 0;
        // edgeEnd.transform.position = nodes[0].transform.position;
        Debug.Log(nodes[0].transform.position);
        Debug.Log(this.edgeTitle.transform.position);
        // Vector3 endPos = line.GetPosition(line.positionCount - 1);
        Debug.Log("edgeend here");
        // edgEnd.transform.position = pos;
        // AddEdgeEnd(edgEnd);
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
}