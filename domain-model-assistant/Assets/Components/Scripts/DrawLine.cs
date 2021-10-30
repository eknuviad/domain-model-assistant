using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class DrawLine: MonoBehaviour{

private List<GameObject> nodes;

public GameObject line;

private Vector3 mousePos;

private CompartmentedRectangle obj1;
private CompartmentedRectangle obj2;
void Start(){
    
}


void Update(){
    if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);//mouse postiion is Vecot3, mouse cordinates are 2d so convert
            mousePos.z = 0; //so lines are drawn on xy plane
            if(IsCompRectExist(mousePos) && line == null){
               createLine();
            }
            line.GetComponent<LineRenderer>().SetPosition(0, mousePos);
        }
        else if (Input.GetMouseButtonUp(0) && line) //check if mouse has been lifted
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if(IsCompRectExist(mousePos)){
                line.GetComponent<LineRenderer>().SetPosition(1, mousePos); 
                obj1.AddEdge(line); //not sure if this references actual comprect in diagram. needs testing
                obj2.AddEdge(line);
            }else{
                Destroy(line);
            }
            // line = null; //if line is not drawn
            // currLines++;
        }
        else if (Input.GetMouseButton(0) && line) //if mouse is pressed, conrinuously set position to cur mouse pos
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            line.GetComponent<LineRenderer>().SetPosition(1, mousePos);
        }
}


public void createLine(){
    this.line = new GameObject("Line",typeof(Edge));
    this.line.AddComponent<LineRenderer>();
}

public bool IsCompRectExist(Vector3 aMousePos){
    bool res = true; //false;
    if(nodes == null){
        nodes = this.gameObject.GetComponent<Diagram>().GetCompartmentedRectangles();
    }
    if(nodes.Count == 0){
        res = false;
    }else{
        foreach(var obj in nodes){
            var distance = Vector3.Distance(mousePos, obj.transform.position);
            if(distance < 0.1f /*threshold btw lineend and comprect*/){
                var ob = obj.GetComponent<CompartmentedRectangle>();
                if(obj1 == null){
                    obj1 = ob;
                }
                else if(Vector3.Distance(obj1.transform.position,ob.transform.position) > 0.1f /*threshold between comprects*/){
                    obj2 = ob;
                }
                res = true;
                break;
            }
        }
    }
    return res;
}






}