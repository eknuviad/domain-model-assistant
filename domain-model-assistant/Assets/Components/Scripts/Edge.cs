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
    
    public List<string> nodesId = new List<string>();

    public List<GameObject> edgeEnds = new List<GameObject>();

    public GameObject[] classes;
    public GameObject edgeEnd;
    public GameObject popupLineMenu;
    float holdTimer = 0;
    bool hold = false;

    public GameObject diagram;
    private Diagram _diagram;
    public Vector3 mousePos;
    public Vector3 linePosition1;
    public Vector3 linePosition2;
    public Vector3 popupLineMenuOffset;
    // public GameObject compositionIcon;
    // public GameObject aggregationIcon;
    // public GameObject generalizationIcon;

    private int[] prevConnectionPointIndices = new int[] {-1,-1}; // used to keep track of connection point changes for updating availabilities  
    void Awake()
    {
        //_diagram = diagram.GetComponent<Diagram>();
        //Debug.Log(_diagram);
            
    }
    void Start()
    {
        createEdge();
    }

    void Update()
    {
    if (nodes != null)
        {
            if(nodes[0] != null && nodes[1] != null)
            {
                var node1 = nodes[0].GetComponent<Node>();
                var node2 = nodes[1].GetComponent<Node>();
                var node1_locs = node1.GetConnectionPointsLocations();
                var node2_locs = node2.GetConnectionPointsLocations();
                
            

                var edgeEnd1 = edgeEnds[0].GetComponent<EdgeEnd>();
                var edgeEnd2 = edgeEnds[1].GetComponent<EdgeEnd>();
                
                int[] indices = GetIndicesOfMinDist(node1_locs, node2_locs, node1.GetConnectionPointsAvailabilities(), node2.GetConnectionPointsAvailabilities());

                // release previous connection points
                node1.SetConnectionPointAvailable(prevConnectionPointIndices[0], true);
                node2.SetConnectionPointAvailable(prevConnectionPointIndices[1], true);

                // set the optimal connection points
                var edgeEnd1_loc = node1_locs[indices[0]];
                var edgeEnd2_loc = node2_locs[indices[1]];
                edgeEnd1.Position = edgeEnd1_loc;
                edgeEnd2.Position = edgeEnd2_loc;

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

                edgeEnd1.isLeft = edgeEnd1_loc.x < edgeEnd2_loc.x ? true:false;
                edgeEnd2.isLeft = edgeEnd1_loc.x > edgeEnd2_loc.x ? true:false;
                edgeEnd1.isUpper = edgeEnd1_loc.y > edgeEnd2_loc.y ? true:false;
                edgeEnd2.isUpper = edgeEnd1_loc.y < edgeEnd2_loc.y ? true:false;
            }
            else
            {
                RetrieveNodes();
            }
        }
        else if (this.hold && holdTimer > 1f - 5)
        {
            this.hold = false;
            holdTimer = 0;
            SpawnPopupLineMenu();
        }
    }

    void createEdge()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        line = this.gameObject.GetComponent<LineRenderer>();
        // line.material = material;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.material.color = Color.black;
        line.positionCount = 2; //straightline with 2 end points
        line.startWidth = 0.05f; //line width
        line.endWidth = 0.05f; //line width
        line.useWorldSpace = true; //set to true so lines defined in world space
        line.numCapVertices = 50;
        Debug.Log("line created");

        var edgeEnd1 = GameObject.Instantiate(edgeEnd).GetComponent<EdgeEnd>();
        var edgeEnd2 = GameObject.Instantiate(edgeEnd).GetComponent<EdgeEnd>();
        
        edgeEnd1.SetEdge(this.gameObject);
        edgeEnd2.SetEdge(this.gameObject);

        var node1 = nodes[0].GetComponent<Node>();
        var node2 = nodes[1].GetComponent<Node>();
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

        int[] indices = GetIndicesOfMinDist(node1_locs, node2_locs, node1.GetConnectionPointsAvailabilities(), node2.GetConnectionPointsAvailabilities());
        
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

        edgeEnd1.isLeft = edgeEnd1_loc.x < edgeEnd2_loc.x ? true:false;
        edgeEnd2.isLeft = edgeEnd1_loc.x > edgeEnd2_loc.x ? true:false;
        edgeEnd1.isUpper = edgeEnd1_loc.y > edgeEnd2_loc.y ? true:false;
        edgeEnd2.isUpper = edgeEnd1_loc.y < edgeEnd2_loc.y ? true:false;
        // // check closest node edge
         var diff_y = nodes[0].transform.position.y - nodes[1].transform.position.y;
         var diff_x = nodes[0].transform.position.x - nodes[1].transform.position.x;
         if (diff_x <= diff_y)
         {
             gameObject.transform.position = nodes[0].transform.position + new Vector3(0,-95,0);
        // //     CreateEdgeEndUpperObj(nodes[0]);//create edge number and title textboxes for first obj
        // //     CreateEdgeEndLowerObj(nodes[1]);
         }
         else
         {
            gameObject.transform.position = nodes[1].transform.position + new Vector3(95,0,0);
        // //     CreateEdgeEndLeftObject(nodes[1]);
        // //     CreateEdgeEndRightObject(nodes[0]);
         }
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

    public void setColor(int color)
    {
        switch(color){
            case 0:
                line.material.color = Color.black;
                break;
            case 1:
                line.material.color = Color.blue;
                break;
            case 2:
                line.material.color = Color.magenta;
                break;


        }
    }

    void Destroy()
    {
        Destroy(this.gameObject);
    }

    public int IndexOfNode(GameObject aNode)
    {
        int index = nodes.IndexOf(aNode);
        return index;
    }

    public bool AddNode(GameObject aNode)
    {
        bool wasAdded = false;
        if (nodes.Contains(aNode))
        {
            return wasAdded;
        }
        if (nodes.Count >= RequiredNumOfNodes)
        {
            return wasAdded;
        }

        nodes.Add(aNode);
        nodesId.Add(aNode.GetComponent<CompartmentedRectangle>().ID);

        if (aNode.GetComponent<Node>().IndexOfConnection(this.gameObject) != -1)
        {
            wasAdded = true;
        }
        else
        {
            wasAdded = aNode.GetComponent<Node>().AddConnection(this.gameObject);
            if (!wasAdded)
            {
                nodes.Remove(aNode);
            }
        }
        
        if (wasAdded)
        {
            Debug.Log("node added to edge");
        }
        return wasAdded;
    }

    public bool AddEdgeEnd(GameObject aEdgeEnd)
    {
        bool wasAdded = false;
        if (edgeEnds.Contains(aEdgeEnd))
        {
            return wasAdded;
        }
        edgeEnds.Add(aEdgeEnd);
        wasAdded = true;
        Debug.Log("Edge end added to edge");
        return wasAdded;
    }

    public int GetNumberOfEdgeEnds()
    {
        return edgeEnds.Count;
    } 

    public void SetAssociation()
    {
        EdgeEnd edgeEnd = edgeEnds[GetClosestEdgeEndIndex()].GetComponent<EdgeEnd>();
        edgeEnd.SetIconType(0);
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }
    
    public void SetAggregation()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if(otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }
        edgeEnd.SetIconType(1);
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }
    
    public void SetComposition()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if(otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }
        edgeEnd.SetIconType(2);
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }
    
    public void SetGeneralization()
    {
        EdgeEnd edgeEnd;
        EdgeEnd otherEdgeEnd;
        if (GetClosestEdgeEndIndex() == 1)
        {
            edgeEnd = edgeEnds[1].GetComponent<EdgeEnd>();
            otherEdgeEnd = edgeEnds[0].GetComponent<EdgeEnd>();
        }
        else
        {
            edgeEnd = edgeEnds[0].GetComponent<EdgeEnd>();
            otherEdgeEnd = edgeEnds[1].GetComponent<EdgeEnd>();
        }
        if(otherEdgeEnd.hasActiveIcon)
        {
            otherEdgeEnd.SetIconType(0);
        }
        edgeEnd.SetIconType(3);
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }
    
    public void DeleteEdge()
    {
        Destroy(this.gameObject);
        Destroy(edgeEnds[0].gameObject);
        Destroy(edgeEnds[1].gameObject);
        //close the popup menu after clicking Delete
        popupLineMenu.GetComponent<PopupLineMenu>().Close();
    }
    
    public GameObject GetPopUpLineMenu()
    {
        return popupLineMenu;
    }

    public void OnBeginHold()
    {
        Debug.Log("beginholdheard");
        // mousePos = Input.mousePosition;
        this.hold = true;
        holdTimer += Time.deltaTime; 
    }

    public void SpawnPopupLineMenu()
    {
         Debug.Log("beginholdheard");
        popupLineMenuOffset = new Vector3(70, -110, 0);
        if (this.popupLineMenu.GetComponent<PopupLineMenu>().GetLine() == null)
        {
            this.popupLineMenu = GameObject.Instantiate(this.popupLineMenu);
            this.popupLineMenu.transform.SetParent(this.transform);
            //this can be changed so that popupline menu is always instantiated at
            //midpoint of the relationship
            this.popupLineMenu.transform.position = this.mousePos + popupLineMenuOffset;
            this.popupLineMenu.GetComponent<PopupLineMenu>().SetUpdateConstant(this.popupLineMenu.transform.position);
            this.popupLineMenu.GetComponent<PopupLineMenu>().SetLine(this);
        }
        else
        {
            this.popupLineMenu.transform.position = this.mousePos + popupLineMenuOffset;
            this.popupLineMenu.GetComponent<PopupLineMenu>().Open();
        }
    }

    /// <summary> This function takes two lists of connection points Vec2 coordinates 
    /// and returns the indices of each lists as an array of int such that the pairwise 
    /// distance is minimal and the connections indexed are available.
    /// 
    /// Previous connections are regarded as available in order to return the index of 
    /// updated connection points if they are strictly smaller in distance than the previous
    /// connection, otherwise the previous indices are returned.
    /// </summary>
    private int[] GetIndicesOfMinDist(List<Vector2>node1_locs, List<Vector2>node2_locs, ReadOnlyCollection<bool> node1_avails, ReadOnlyCollection<bool> node2_avails)
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
        var node1_dist = Vector3.Distance(mousePos, nodes[0].transform.position);
        var node2_dist = Vector3.Distance(mousePos, nodes[1].transform.position);

        if(node1_dist < node2_dist)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    
    public void RetrieveNodes()
    {
        classes = GameObject.FindGameObjectsWithTag("comprec");
        foreach (var comp in classes)
        {
            if(Equals(comp.GetComponent<CompartmentedRectangle>().ID,nodesId[0]))
            {
                nodes[0] = comp;
            }else if(Equals(comp.GetComponent<CompartmentedRectangle>().ID,nodesId[1]))
            {
                //Debug.Log(comp.GetComponent<CompartmentedRectangle>().ID);
                nodes[1] = comp;

            }
        }
        if(nodes[1]==null||nodes[0]==null){
            Destroy();
        }
    }

}
