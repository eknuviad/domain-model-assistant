using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node: MonoBehaviour{
    // private string id;
    public GameObject canvas;
    // renamed textbox as header to avoid parent and child
    // from having the same serializable field
    public GameObject header;
    public string ID{
        get{
            return ID;
        }
        set{
            ID = value;
        }
    }

    public GameObject getDiagram(){
        return canvas;
    }

    public bool setDiagram(GameObject aCanvas){
        bool wasSet = false;
        if(aCanvas == null){
            return wasSet;
        }
        canvas = aCanvas;
        Debug.Log("Canvas has been set for Node");
        wasSet = true;
        return wasSet;
    }

    public GameObject getHeader(){
        return header;
    }

    public bool addHeader(GameObject aheader){
        bool wasSet = false;
        if(aheader == null){
            return wasSet;
        }
        header = aheader;
        Debug.Log("Header textbox has been set for Node");
        wasSet = true;
        return wasSet;
    }


}