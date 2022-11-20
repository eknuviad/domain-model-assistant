using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class DrawLine: MonoBehaviour
{

    // private List<GameObject> nodes;
    public GameObject edge;
    public GameObject line;
    public GameObject edgeEnd;
    private GameObject end1;
    private GameObject end2;
    private Vector3 mousePos;

    private GameObject compRec1;
    private GameObject compRec2;
    
    void Start() {}

    void Update() {}

    public void CreateLine()
    {
        // line = new GameObject("Line",typeof(Edge));
        line = Instantiate(edge);
        end1 = Instantiate(edgeEnd);
        end2 = Instantiate(edgeEnd);

        end1.GetComponent<EdgeEnd>().SetEdge(line);
        end2.GetComponent<EdgeEnd>().SetEdge(line);

        var pos1 = Camera.main.ScreenToWorldPoint(compRec1.transform.position + new Vector3(0,-95,0));
        pos1.z = 0;
        var pos2 = Camera.main.ScreenToWorldPoint(compRec2.transform.position + new Vector3(0,95,0));
        pos2.z = 0;
            
        line.GetComponent<LineRenderer>().SetPosition(0, pos1);
        line.GetComponent<LineRenderer>().SetPosition(1, pos2); 
        compRec1.GetComponent<CompartmentedRectangle>().AddEdgeEnd(end1); 
        compRec2.GetComponent<CompartmentedRectangle>().AddEdgeEnd(end2);

        // if (end1.GetComponent<EdgeEnd>().GetNode() == null) {
        //     Debug.Log("testing testing: ");
        // }
            // obj1.GetComponent<CompartmentedRectangle>().AddConnection(line); 
            // obj2.GetComponent<CompartmentedRectangle>().AddConnection(line);
        compRec1 = null;
        compRec2 = null;
    }

    public void AddCompartmentedRectangle(GameObject compRect)
    {
        if (compRec1 == null)
        {
            compRec1 = compRect;
            Debug.Log("obj1 set");
        }
        else if (compRec1 != compRect)
        {
            compRec2 = compRect;
            Debug.Log("obj2 set");
            WebCore.AddAssociation(compRec1, compRec2);
            CreateLine();
        }
        else
        {
            compRec2 = null;
        }
        Debug.Log("Comp rect added: " + compRect.GetComponent<CompartmentedRectangle>().ID);
    }

}
