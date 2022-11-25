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
    private GameObject _header;
    // private List<GameObject> _connections = new List<GameObject>();
    private List<GameObject> _edgeEnds = new List<GameObject>();

    protected int NumOfConnectionPoints
    { get; set; } 

    protected List<bool> connectionPointsAvailable = new List<bool>();
    
    public string ID
    { get; set; }

    public GameObject GetDiagram()
    {
        return canvas;
    }

    public bool SetDiagram(GameObject aDiagram)
    {
        bool wasSet = false;
        GameObject existingDiagram = canvas;
        canvas = aDiagram;
        if (existingDiagram != null && !existingDiagram.Equals(aDiagram))
        {
            existingDiagram.GetComponent<Diagram>().RemoveNode(gameObject);
        }
        if (aDiagram != null)
        {
            aDiagram.GetComponent<Diagram>().AddNode(gameObject);
        }
        wasSet = true;
        return wasSet;
    }

    public GameObject GetHeader()
    {
        return _header;
    }

    public bool AddHeader(GameObject aHeader)
    {
        if (aHeader == null)
        {
            return false;
        }
        _header = aHeader;

        aHeader.GetComponent<ClassHeaderTextBox>().SetCompartmentedRectangle(gameObject);

        return true;
    }

    // public int IndexOfConnection(GameObject aEdge)
    // {
    //     int index = _connections.IndexOf(aEdge);
    //     return index;
    // }

    // public ReadOnlyCollection<GameObject> GetConnections()
    // {
    //     return _connections.AsReadOnly();
    // }

    // public bool AddConnection(GameObject aEdge)
    // {
    //     bool wasAdded =  false;
    //     if (_connections.Contains(aEdge))
    //     {
    //         return wasAdded;
    //     }   

    //     _connections.Add(aEdge);

    //     if (aEdge.GetComponent<Edge>().IndexOfNode(this.gameObject) != -1)
    //     {
    //         var index = aEdge.GetComponent<Edge>().IndexOfNode(this.gameObject);
    //         wasAdded = true;
    //     }
    //     else
    //     {
    //         wasAdded = aEdge.GetComponent<Edge>().AddNode(this.gameObject);
    //         if (!wasAdded)
    //         {
    //             _connections.Remove(aEdge);
    //         }
    //     }
            
    //     // print log for debugging
    //     if (wasAdded)
    //     {
    //         Debug.Log("Edge added");
    //     }
    //     return wasAdded;
    // }

    public abstract List<Vector2> GetConnectionPointsLocations();

    public void SetConnectionPointAvailable(int index, bool isAvailable)
    {
        connectionPointsAvailable[index] = isAvailable;
    }

    public bool GetConnectionPointAvailability(int index)
    {
        return connectionPointsAvailable[index];
    }

    public ReadOnlyCollection<bool> GetConnectionPointsAvailabilities()
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

    public bool AddEdgeEnd(GameObject aEdgeEnd)
    {
        bool wasAdded = false;
        if (_edgeEnds.Contains(aEdgeEnd)) 
        {
            return false; 
        }
        GameObject existingNode = aEdgeEnd.GetComponent<EdgeEnd>().GetNode();
        bool isNewNode = existingNode != null && !gameObject.Equals(existingNode);
        if (isNewNode)
        {
            aEdgeEnd.GetComponent<EdgeEnd>().SetNode(gameObject);
        }
        else
        {
            _edgeEnds.Add(aEdgeEnd);
        }
        if (existingNode == null){
            aEdgeEnd.GetComponent<EdgeEnd>().SetNode(gameObject);
        }
        wasAdded = true;
        return wasAdded;
    }

    public bool RemoveEdgeEnd(GameObject aEdgeEnd)
    {
        bool wasRemoved = false;
        if (!gameObject.Equals(aEdgeEnd.GetComponent<EdgeEnd>().GetNode()))
        {
            _edgeEnds.Remove(aEdgeEnd);
            wasRemoved = true;
        }
        return wasRemoved;
    }

    public abstract void Destroy();

}
