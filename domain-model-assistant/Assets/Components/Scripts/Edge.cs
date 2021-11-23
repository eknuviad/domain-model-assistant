using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Edge : MonoBehaviour
{
    private LineRenderer line;
    // private Vector3 mousePos;
    public Material material;
    // private int currLines = 0;//ciunter for lines drawn
    public List<GameObject> nodes = new List<GameObject>();
    void Start()
    {
        createEdge();
    }

    void Update()
    {
        if (nodes != null)
        {
            var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position);
            pos1.z = 0;
            var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position);
            pos2.z = 0;
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
        }
    }

    void createEdge()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        line = this.gameObject.GetComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2; //straightline with 2 end points
        line.startWidth = 0.15f; //line width
        line.endWidth = 0.15f; //line width
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
}