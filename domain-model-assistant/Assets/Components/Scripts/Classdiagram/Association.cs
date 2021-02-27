using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Association
{
    GameObject parent;
    GameObject child;

    public Association(GameObject parent, GameObject child)
    {
        this.parent = parent;
        this.child = child;
        GameObject line = new GameObject();
        line.transform.SetParent(GameObject.Find("Canvas").transform);
        line.AddComponent<Image>().color = Color.black;
        
        DrawLine dl = line.AddComponent<DrawLine>();
        dl.SetObjects(parent, child);
        line.name = "Line";
    }
}