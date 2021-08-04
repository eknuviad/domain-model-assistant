using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node: MonoBehaviour
{
    // private string id;
    public GameObject canvas;

    // renamed textbox as header to avoid parent and child
    // from having the same serializable field
    public GameObject /*TextBox*/ header;
    
    public string ID
    { get; set; }

    public GameObject GetDiagram()
    {
        return canvas;
    }

    public bool SetDiagram(GameObject aCanvas)
    {
        if (aCanvas == null)
        {
            return false;
        }
        canvas = aCanvas;
        Debug.Log("Canvas has been set for Node");
        return true;
    }

    public GameObject /*TextBox*/ GetHeader()
    {
        return header;
    }

    public bool AddHeader(GameObject /*TextBox*/ aHeader)
    {
        if(aHeader == null)
        {
            return false;
        }
        header = aHeader;
        Debug.Log("Header textbox has been set for Node with text " + aHeader.GetComponent<TextBox>().text);
        return true;
    }

}
