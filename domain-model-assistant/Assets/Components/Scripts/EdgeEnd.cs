using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EdgeEnd : MonoBehaviour
{

public string ID
{get;set;}
public GameObject edge;
public GameObject edgeEndTitle;
public GameObject edgeEndNumber;

public GameObject getEdge(){
    return this.edge;
}

public void setEdge(GameObject aEdge){
    this.edge = aEdge;
}

public GameObject getEdgeEndTitle(){
    return this.edgeEndTitle;
}

public void setEdgeEndTitle(GameObject aEdgeEndTitle){
    this.edgeEndTitle = aEdgeEndTitle;
}

public GameObject getEdgeEndNumber(){
    return this.edgeEndNumber;
}

public void setEdgeEndNumber(GameObject aEdgeEndNumber){
    this.edgeEndNumber = aEdgeEndNumber;
}

}