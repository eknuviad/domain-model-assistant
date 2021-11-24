using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class DrawLine: MonoBehaviour{

// private List<GameObject> nodes;
public GameObject edge;

public GameObject line;

private Vector3 mousePos;

private GameObject obj1;
private GameObject obj2;
void Start(){
    
}


void Update(){
 
}


public void createLine(){
    // this.line = new GameObject("Line",typeof(Edge));
    this.line = GameObject.Instantiate(edge);
    this.line.AddComponent<LineRenderer>();
    if(obj1 != null && obj2 != null)
    {
        Debug.Log("prevposition1: "+ obj1.transform.position);
        Debug.Log("prevposition2: "+ obj2.transform.position);
        var pos1 = Camera.main.ScreenToWorldPoint(obj1.transform.position);
        pos1.z = 0;
        var pos2 = Camera.main.ScreenToWorldPoint(obj2.transform.position);
        pos2.z = 0;
        Debug.Log("position1: "+ pos1);
        Debug.Log("position2: "+ pos2);
        this.line.GetComponent<LineRenderer>().SetPosition(0, pos1);
        this.line.GetComponent<LineRenderer>().SetPosition(1, pos2); 
        obj1.GetComponent<CompartmentedRectangle>().AddEdge(line); 
        obj2.GetComponent<CompartmentedRectangle>().AddEdge(line);
        obj1 = null;
        obj2 = null;
    }
}


public void AddCompartmentedRectangle(GameObject compRect)
{
    if( obj1 == null){
        obj1 = compRect;
        Debug.Log("obj1 set");
    }
    else if(obj1 != compRect){
        obj2 = compRect;
        Debug.Log("obj2 set");
        createLine();
    }else{
        obj2 = null;
    }
    Debug.Log("Comp rect added: " + compRect.GetComponent<CompartmentedRectangle>().ID);
}






}
