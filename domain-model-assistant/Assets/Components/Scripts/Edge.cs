using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;

//this is an unstable implementation of edge, but to demonstrate
//proof of concept
public class Edge : MonoBehaviour
{
    public string ID
    { get; set; }
    private LineRenderer line;
    public List<GameObject> nodes = new List<GameObject>();
    public const int RequiredNumOfNodes = 2;
    public const int RequiredNumOfEdgeEnds = 2;
    private List<GameObject> _edgeEnds = new List<GameObject>();
    public GameObject edgeEnd;
    public GameObject popupLineMenu;
    float holdTimer = 0;
    bool hold = false;
    private GameObject _diagram;
    public Vector3 mousePos;
    public Vector3 linePosition1;
    public Vector3 linePosition2;
    public Vector3 popupLineMenuOffset;
    private int[] prevConnectionPointIndices = new int[] { -1, -1 }; // used to keep track of connection point changes for updating availabilities  
    
    
    void Awake()
    {
        _diagram = GameObject.Find("Canvas");
    }

    void Start()
    {
        CreateEdge();
    }

    void Update()
    {
        if (_edgeEnds[0] != null && _edgeEnds[1] != null)
        {

            EdgeEnd edgeEnd1 = _edgeEnds[0].GetComponent<EdgeEnd>();
            EdgeEnd edgeEnd2 = _edgeEnds[1].GetComponent<EdgeEnd>();

            if (edgeEnd1.GetNode() == null)
            {
                edgeEnd1.RetrieveNode();
            }

            if (edgeEnd2.GetNode() == null)
            {
                edgeEnd2.RetrieveNode();
            }

            if (edgeEnd1.GetNode() == null || edgeEnd2.GetNode() == null)
            {
                // no call to WebCore as the backend edge is already deleted for referential integrity
                Destroy(gameObject);
                Destroy(_edgeEnds[0].gameObject);
                Destroy(_edgeEnds[1].gameObject);
                return;
            }

            var node1 = edgeEnd1.GetNode().GetComponent<Node>();
            var node2 = edgeEnd2.GetNode().GetComponent<Node>();
            var node1_locs = node1.GetConnectionPointsLocations();
            var node2_locs = node2.GetConnectionPointsLocations();

            if (node1.isGeneralizationCntPointTaken)
            {
                node1.ReserveGeneralizationPt();
            }
            if (node2.isGeneralizationCntPointTaken)
            {
                node2.ReserveGeneralizationPt();
            }

            int[] indices = GetIndicesOfMinDist(node1_locs, node2_locs, node1.GetConnectionPointsAvailabilities(),
                node2.GetConnectionPointsAvailabilities());

            if (edgeEnd1.isGeneralization)
            {
                indices[0] = 4;
            }
            if (edgeEnd2.isGeneralization)
            {
                indices[1] = 4;
            }

            // release previous connection points
            node1.SetConnectionPointAvailable(prevConnectionPointIndices[0], true);
            node2.SetConnectionPointAvailable(prevConnectionPointIndices[1], true);

            // set the optimal connection points
            var edgeEnd1_loc = node1_locs[indices[0]];
            var edgeEnd2_loc = node2_locs[indices[1]];
            edgeEnd1.Position = edgeEnd1_loc;
            edgeEnd2.Position = edgeEnd2_loc;

            // if(edgeEnd1.isGeneralization)
            // {
            //     edgeEnd1.Position = node1.generalizationPointLoc;
            // }

            // if(edgeEnd2.isGeneralization)
            // {
            //     edgeEnd1.Position = node2.generalizationPointLoc;
            // }

            // set the connection points as taken
            node1.SetConnectionPointAvailable(indices[0], false);
            node2.SetConnectionPointAvailable(indices[1], false);

            // record the connection for the next update
            prevConnectionPointIndices[0] = indices[0];
            prevConnectionPointIndices[1] = indices[1];

            // update the line end positions
            var pos1 = Camera.main.ScreenToWorldPoint(edgeEnd1.Position);
            pos1.z = 0;
            var pos2 = Camera.main.ScreenToWorldPoint(edgeEnd2.Position);
            pos2.z = 0;
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);

            float angle = Vector3.Angle(pos2 - pos1, new Vector3(0, 1, 0));
            edgeEnd2.Angle = angle;

            edgeEnd1.isLeft = edgeEnd1_loc.x < edgeEnd2_loc.x;
            edgeEnd2.isLeft = edgeEnd1_loc.x > edgeEnd2_loc.x;
            edgeEnd1.isUpper = edgeEnd1_loc.y > edgeEnd2_loc.y;
            edgeEnd2.isUpper = edgeEnd1_loc.y < edgeEnd2_loc.y;
            //Debug.Log("association id is"+ID);
            if(string.Equals(ID,"-1")){
                _diagram.GetComponent<Diagram>().SetAssociationID(gameObject);
                Debug.Log("set association id to "+ID);
            }
        }
    }

    public void CreateEdge()
    {
        gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        line = gameObject.GetComponent<LineRenderer>();
        // line.material = material;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = Color.black;
        line.positionCount = 2; //straightline with 2 end points
        line.startWidth = 0.05f; //line width
        line.endWidth = 0.05f; //line width
        line.useWorldSpace = true; //set to true so lines defined in world space
        line.numCapVertices = 50;
        Debug.Log("line created");

        var edgeEnd1 = _edgeEnds[0].GetComponent<EdgeEnd>();
        var edgeEnd2 = _edgeEnds[1].GetComponent<EdgeEnd>();

        edgeEnd1.SetEdge(gameObject);
        edgeEnd2.SetEdge(gameObject);

        var node1 = edgeEnd1.GetNode().GetComponent<Node>();
        var node2 = edgeEnd2.GetNode().GetComponent<Node>();
        var node1_locs = node1.GetConnectionPointsLocations();
        var node2_locs = node2.GetConnectionPointsLocations();

        if (node1.GetNumberOfConnectionPointsAvailable() == 0)
        {
            //TODO: Implement Node Connection Points Expansion
            Debug.Log("Critical: Need more connection points.");
        }
        if (node2.GetNumberOfConnectionPointsAvailable() == 0)
        {
            //TODO: Implement Node Connection Points Expansion 
            Debug.Log("Critical: Need more connection points.");
        }

        if (node1.isGeneralizationCntPointTaken)
        {
            node1.ReserveGeneralizationPt();
        }
        if (node2.isGeneralizationCntPointTaken)
        {
            node2.ReserveGeneralizationPt();
        }

        int[] indices = GetIndicesOfMinDist(node1_locs, node2_locs, node1.GetConnectionPointsAvailabilities(),
            node2.GetConnectionPointsAvailabilities());

        if (edgeEnd1.isGeneralization)
        {
            indices[0] = 4;
        }
        if (edgeEnd2.isGeneralization)
        {
            indices[1] = 4;
        }

        // set the optimal connection points
        var edgeEnd1_loc = node1_locs[indices[0]];
        var edgeEnd2_loc = node2_locs[indices[1]];
        edgeEnd1.Position = edgeEnd1_loc;
        edgeEnd2.Position = edgeEnd2_loc;

        // set the connection points as taken 
        node1.SetConnectionPointAvailable(indices[0], false);
        node2.SetConnectionPointAvailable(indices[1], false);

        // record the new indices for next update
        prevConnectionPointIndices[0] = indices[0];
        prevConnectionPointIndices[1] = indices[1];

        // update the line end positions
        var pos1 = Camera.main.ScreenToWorldPoint(edgeEnd1.Position);
        pos1.z = 0;
        var pos2 = Camera.main.ScreenToWorldPoint(edgeEnd2.Position);
        pos2.z = 0;
        line.SetPosition(0, pos1);
        line.SetPosition(1, pos2);

        edgeEnd1.isLeft = edgeEnd1_loc.x < edgeEnd2_loc.x;
        edgeEnd2.isLeft = edgeEnd1_loc.x > edgeEnd2_loc.x;
        edgeEnd1.isUpper = edgeEnd1_loc.y > edgeEnd2_loc.y;
        edgeEnd2.isUpper = edgeEnd1_loc.y < edgeEnd2_loc.y;
        // check closest node edge
        var diff_y = node1.transform.position.y - node2.transform.position.y;
        var diff_x = node1.transform.position.x - node2.transform.position.x;
        if (diff_x <= diff_y)
        {
            gameObject.transform.position = node1.transform.position + new Vector3(0, -95, 0);
            // CreateEdgeEndUpperObj(nodes[0]);//create edge number and title textboxes for first obj
            // CreateEdgeEndLowerObj(nodes[1]);
        }
        else
        {
            gameObject.transform.position = node2.transform.position + new Vector3(95, 0, 0);
            // CreateEdgeEndLeftObject(nodes[1]);
            // CreateEdgeEndRightObject(nodes[0]);
        }
        ID = "-1";
    }

    public Vector3 GetPosition1()
    {
        linePosition1 = line.GetPosition(0);
        return linePosition1;
    }

    public Vector3 GetPosition2()
    {
        linePosition2 = line.GetPosition(1);
        return linePosition2;
    }


    public float GetWidth()
    {
        return line.startWidth;
    }

    public void SetColor(Color color)
    {
        line.material.color = color;
    }

    // void Destroy()
    // {
    //     Destroy(gameObject);
    // }

    // public int IndexOfNode(GameObject aNode)
    // {
    //     int index = nodes.IndexOf(aNode);
    //     return index;
    // }

    // public bool AddNode(GameObject aNode)
    // {
    //     bool wasAdded = false;
    //     if (nodes.Contains(aNode))
    //     {
    //         return wasAdded;
    //     }
    //     if (nodes.Count >= RequiredNumOfNodes)
    //     {
    //         return wasAdded;
    //     }

    //     nodes.Add(aNode);
    //     nodesId.Add(aNode.GetComponent<CompartmentedRectangle>().ID);

    //     if (aNode.GetComponent<Node>().IndexOfConnection(gameObject) != -1)
    //     {
    //         wasAdded = true;
    //     }
    //     else
    //     {
    //         wasAdded = aNode.GetComponent<Node>().AddConnection(gameObject);
    //         if (!wasAdded)
    //         {
    //             nodes.Remove(aNode);
    //         }
    //     }

    //     if (wasAdded)
    //     {
    //         Debug.Log("node added to edge");
    //     }
    //     return wasAdded;
    // }

    public bool AddEdgeEnd(GameObject aEdgeEnd)
    {
        bool wasAdded = false;
        if (_edgeEnds.Contains(aEdgeEnd))
        {
            return wasAdded;
        }
        _edgeEnds.Add(aEdgeEnd);
        wasAdded = true;
        Debug.Log("Edge end added to edge");
        return wasAdded;
    }

    public int GetNumberOfEdgeEnds()
    {
        return _edgeEnds.Count;
    }

    public void SetAssociation()
    {
        EdgeEnd edgeEnd_1 = _edgeEnds[0].GetComponent<EdgeEnd>();
        EdgeEnd edgeEnd_2 = _edgeEnds[1].GetComponent<EdgeEnd>();
        edgeEnd_1.SetIconType(0);
        edgeEnd_2.SetIconType(0);

        edgeEnd_1.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;
        edgeEnd_2.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;

        edgeEnd_1.isGeneralization = false;
        edgeEnd_2.isGeneralization = false;
        edgeEnd_1.GetEdgeEndTitle().SetActive(true);
        edgeEnd_1.GetEdgeEndNumber().SetActive(true);
        edgeEnd_2.GetEdgeEndTitle().SetActive(true);
        edgeEnd_2.GetEdgeEndNumber().SetActive(true);

        WebCore.SetReferenceType(edgeEnd_1.gameObject, "Regular");
        WebCore.SetReferenceType(edgeEnd_2.gameObject, "Regular");
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }

    public void SetUniNavigability()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if (otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }
        edgeEnd.SetIconType(1);

        edgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;
        otherEdgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;

        edgeEnd.isGeneralization = false;
        otherEdgeEnd.isGeneralization = false;
        edgeEnd.GetEdgeEndTitle().SetActive(true);
        edgeEnd.GetEdgeEndNumber().SetActive(true);
        otherEdgeEnd.GetEdgeEndTitle().SetActive(true);
        otherEdgeEnd.GetEdgeEndNumber().SetActive(true);

        float angle = Vector3.Angle(otherEdgeEnd.transform.position - edgeEnd.transform.position, new Vector3(0, 1, 0));
        edgeEnd.Angle = angle;
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }

    public void SetAggregation()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if (otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }

        edgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;
        otherEdgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;

        edgeEnd.isGeneralization = false;
        otherEdgeEnd.isGeneralization = false;
        edgeEnd.GetEdgeEndTitle().SetActive(true);
        edgeEnd.GetEdgeEndNumber().SetActive(true);
        otherEdgeEnd.GetEdgeEndTitle().SetActive(true);
        otherEdgeEnd.GetEdgeEndNumber().SetActive(true);

        edgeEnd.SetIconType(2);
        WebCore.SetReferenceType(edgeEnd.gameObject, "Aggregation");
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }

    public void SetComposition()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if (otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }

        edgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;
        otherEdgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;

        edgeEnd.isGeneralization = false;
        otherEdgeEnd.isGeneralization = false;
        edgeEnd.GetEdgeEndTitle().SetActive(true);
        edgeEnd.GetEdgeEndNumber().SetActive(true);
        otherEdgeEnd.GetEdgeEndTitle().SetActive(true);
        otherEdgeEnd.GetEdgeEndNumber().SetActive(true);

        edgeEnd.SetIconType(3);
        WebCore.SetReferenceType(edgeEnd.gameObject, "Composition");
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }

    public void SetGeneralization()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = _edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = _edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if (otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }
        edgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = true;
        otherEdgeEnd.GetNode().GetComponent<CompartmentedRectangle>().isGeneralizationCntPointTaken = false;

        edgeEnd.isGeneralization = true;

        edgeEnd.GetEdgeEndTitle().SetActive(false);
        edgeEnd.GetEdgeEndNumber().SetActive(false);
        otherEdgeEnd.GetEdgeEndTitle().SetActive(false);
        otherEdgeEnd.GetEdgeEndNumber().SetActive(false);

        edgeEnd.SetIconType(4);
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }

    public void DeleteEdge()
    {
        WebCore.DeleteAssociation(gameObject);
        Destroy(gameObject);
        Destroy(_edgeEnds[0].gameObject);
        Destroy(_edgeEnds[1].gameObject);
        
        //close the popup menu after clicking Delete
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
        Debug.Log("Edge deleted");
    }

    public GameObject GetPopUpLineMenu()
    {
        return popupLineMenu;
    }

    public void OnBeginHold()
    {
        Debug.Log("beginholdheard");
        // mousePos = Input.mousePosition;
        hold = true;
        holdTimer += Time.deltaTime;
    }

    public void SpawnPopupLineMenu()
    {
        Debug.Log("beginholdheard");
        popupLineMenuOffset = new Vector3(70, -110, 0);
        if (popupLineMenu.GetComponent<PopupLineMenu>().GetLine() == null)
        {
            popupLineMenu = GameObject.Instantiate(popupLineMenu);
            popupLineMenu.transform.SetParent(transform);
            //this can be changed so that popupline menu is always instantiated at
            //midpoint of the relationship
            popupLineMenu.transform.position = mousePos + popupLineMenuOffset;
            popupLineMenu.GetComponent<PopupLineMenu>().SetUpdateConstant(popupLineMenu.transform.position);
            popupLineMenu.GetComponent<PopupLineMenu>().SetLine(this);
        }
        else
        {
            popupLineMenu.transform.position = mousePos + popupLineMenuOffset;
            popupLineMenu.GetComponent<PopupLineMenu>().Open();
        }
    }

    //------------------------
    // INTERFACE
    //------------------------

    public GameObject GetDiagram()
    {
        return _diagram;
    }

    public bool HasDiagram()
    {
        bool has = _diagram != null;
        return has;
    }

    public static int MinimumNumberOfEdgeEnd()
    {
        return 2;
    }

    public GameObject GetEdgeEnd(int index)
    {
        GameObject aEdgeEnd = _edgeEnds[index];
        return aEdgeEnd;
    }

    public IList<GameObject> GetEdgeEnds()
    {
        IList<GameObject> newEdgeEnds = _edgeEnds.AsReadOnly();
        return newEdgeEnds;
    }

    public int NumberOfEdgeEnds()
    {
        int number = _edgeEnds.Count;
        return number;
    }

    public bool HasEdgeEnds()
    {
        bool has = _edgeEnds.Count > 0;
        return has;
    }

    public bool RemoveEdgeEnd(GameObject aEdgeEnd)
    {
        bool wasRemoved = false;
        if (gameObject.Equals(aEdgeEnd.GetComponent<EdgeEnd>().GetEdge()))
        {
            return wasRemoved;
        }

        if (NumberOfEdgeEnds() <= MinimumNumberOfEdgeEnd())
        {
            return wasRemoved;
        }
        _edgeEnds.Remove(aEdgeEnd);
        wasRemoved = true;
        return wasRemoved;
    }

    public bool SetDiagram(GameObject aDiagram)
    {
        bool wasSet = false;
        GameObject existingDiagram = _diagram;
        _diagram = aDiagram;
        if (existingDiagram != null && !existingDiagram.Equals(aDiagram))
        {
            existingDiagram.GetComponent<Diagram>().RemoveEdge(gameObject);
        }
        if (aDiagram != null)
        {
            aDiagram.GetComponent<Diagram>().AddEdge(gameObject);
        }
        wasSet = true;
        return wasSet;
    }


    //------------------------
    // Helper Methods
    //------------------------

    /// <summary> This function takes two lists of connection points Vec2 coordinates 
    /// and returns the indices of each lists as an array of int such that the pairwise 
    /// distance is minimal and the connections indexed are available.
    /// 
    /// Previous connections are regarded as available in order to return the index of 
    /// updated connection points if they are strictly smaller in distance than the previous
    /// connection, otherwise the previous indices are returned.
    /// </summary>
    private int[] GetIndicesOfMinDist(List<Vector2> node1_locs, List<Vector2> node2_locs,
        ReadOnlyCollection<bool> node1_avails, ReadOnlyCollection<bool> node2_avails)
    {
        int[] indices = new int[2];
        float minDist = float.MaxValue;
        for (int i = 0; i < node1_locs.Count; i++)
        {
            if (!node1_avails[i] && i != prevConnectionPointIndices[0])
            {
                continue;
            }
            for (int j = 0; j < node2_locs.Count; j++)
            {
                if (!node2_avails[j] && j != prevConnectionPointIndices[1])
                {
                    continue;
                }
                float dist = Vector2.Distance(node1_locs[i], node2_locs[j]);
                if (dist < minDist)
                {
                    minDist = dist;
                    indices[0] = i;
                    indices[1] = j;
                }
            }
        }
        return indices;
    }

    private int GetClosestEdgeEndIndex()
    {
        var edgeEnd1_dist = Vector3.Distance(mousePos, _edgeEnds[0].transform.position);
        var edgeEnd2_dist = Vector3.Distance(mousePos, _edgeEnds[1].transform.position);
        Debug.Log("edgeEnd1_dist: " + edgeEnd1_dist);
        Debug.Log("edgeEnd2_dist: " + edgeEnd2_dist);

        if (edgeEnd1_dist < edgeEnd2_dist)
        {
            Debug.Log("GetClosestEdgeEndIndex: 0");
            return 0;
        }
        else
        {
            Debug.Log("GetClosestEdgeEndIndex: 1");
            return 1;
        }
    }
}
