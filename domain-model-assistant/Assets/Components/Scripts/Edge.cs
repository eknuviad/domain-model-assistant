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
    public List<GameObject> nodes;
    void Start()
    {
        createEdge();
    }

    void Update()
    {
  
    }

    void createEdge()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2; //straightline with 2 end points
        line.startWidth = 0.15f; //line width
        line.endWidth = 0.15f; //line width
        line.useWorldSpace = false; //set to true so lines defined in world space
        line.numCapVertices = 50;
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
