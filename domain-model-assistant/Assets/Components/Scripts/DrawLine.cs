using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class DrawLine: MonoBehaviour{

private List<GameObject> nodes;
// public GameObject edge;

public GameObject line;


private Vector3 mousePos;

private GameObject obj1;
private GameObject obj2;
void Start(){
    
}


void Update(){
 
    // if (Input.GetMouseButtonDown(0))
    //     {
    //         mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//mouse postiion is Vecot3, mouse cordinates are 2d so convert
    //         mousePos.z = 0; //so lines are drawn on xy plane
    //         if(IsCompRectExist(mousePos) && line == null){
    //            createLine();
    //         }
    //         line.GetComponent<LineRenderer>().SetPosition(0, mousePos);
    //     }
    //     else if (Input.GetMouseButtonUp(0) && line) //check if mouse has been lifted
    //     {
    //         mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         mousePos.z = 0;
    //         if(IsCompRectExist(mousePos)){
    //             line.GetComponent<LineRenderer>().SetPosition(1, mousePos); 
    //             if(obj1 != null && obj2 !=null){
    //                 obj1.AddEdge(line); //not sure if this references actual comprect in diagram. needs testing
    //                 obj2.AddEdge(line);
    //             }
    //         }else{
    //             Destroy(line);
    //         }
    //         // line = null; //if line is not drawn
    //         // currLines++;
    //     }
    //     else if (Input.GetMouseButton(0) && line) //if mouse is pressed, conrinuously set position to cur mouse pos
        // {
        //     mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     mousePos.z = 0;
        //     line.GetComponent<LineRenderer>().SetPosition(1, mousePos);
        // }
}


public void createLine(){
    this.line = new GameObject("Line",typeof(Edge));
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
    else{
        obj2 = compRect;
        Debug.Log("obj2 set");
        createLine();
    }
    Debug.Log("Comp rect added: " + compRect.GetComponent<CompartmentedRectangle>().ID);
}


}
