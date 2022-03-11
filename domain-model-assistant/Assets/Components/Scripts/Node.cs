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
    public GameObject header;

    public List<GameObject> connections;
    
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
        return true;
    }

    public GameObject GetHeader()
    {
        return header;
    }

    public bool AddHeader(GameObject aHeader)
    {
        if (aHeader == null)
        {
            return false;
        }
        header = aHeader;
        return true;
    }

    public int IndexOfConnection(GameObject aEdge)
    {
        int index = connections.IndexOf(aEdge);
        return index;
    }

    public GameObject GetConnections()
    {
        ReadOnlyCollection<GameObject> newConnections = new ReadOnlyCollection<GameObject>(connections);
        return newConnections;
    }

    public bool AddConnection(GameObject aEdge)
    {
        bool wasAdded =  false;
        if (connections.Contains(aEdge))
        {
            return false;
        }
        connections.Add(aEdge);
        if (aEdge.GetComponent<Edge>().indexOfNode(this.gameObject) != -1)
        {
            wasAdded = true;
        }
        else
        {
            wasAdded = aEdge.GetComponent<Edge>().AddNode(this.gameObject);
            if (!wasAdded)
            {
                connections.Remove(aEdge);
            }
        }
        
        // print log for debugging
        if (wasAdded)
        {
            Debug.Log("Edge added");
        }
        return wasAdded;
    }

}
