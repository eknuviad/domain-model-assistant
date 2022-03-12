using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Node: MonoBehaviour
{
    // private string id;
    public GameObject canvas;

    // renamed textbox as header to avoid parent and child
    // from having the same serializable field
    public GameObject header;

    private List<GameObject> connections = new List<GameObject>();

    protected int NumOfConnectionPoints
    { get; set; } 

    protected List<bool> connectionPointsAvailable = new List<bool>();
    
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

    public ReadOnlyCollection<GameObject> GetConnections()
    {
        return connections.AsReadOnly();
    }

    public bool AddConnection(GameObject aEdge)
    {
        bool wasAdded =  false;
        if (connections.Contains(aEdge))
        {
            return wasAdded;
        }   

        connections.Add(aEdge);

        if (aEdge.GetComponent<Edge>().IndexOfNode(this.gameObject) != -1)
        {
            var index = aEdge.GetComponent<Edge>().IndexOfNode(this.gameObject);
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

    public abstract List<Vector2> GetConnectionPointsLocations();

    public void SetConnectionPointAvailable(int index, bool isAvailable)
    {
        // Debug.Log("index: " + index);
        // Debug.Log("Array size: " + connectionPointAvailable.Count);
        connectionPointsAvailable[index] = isAvailable;
    }

    public bool GetConnectionPointAvailable(int index)
    {
        return connectionPointsAvailable[index];
    }

    public ReadOnlyCollection<bool> GetConnectionPointsAvailable()
    {
        return connectionPointsAvailable.AsReadOnly();
    }

    public int GetNumberOfConnectionPointsAvailable()
    {
        int count = 0;
        foreach (bool avail in connectionPointsAvailable)
        {
            if (avail)
            {
                count++;
            }
        }
        return count;
    }

}
