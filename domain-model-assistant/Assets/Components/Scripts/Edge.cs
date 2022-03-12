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
    public GameObject edgeEnd;
    public List<GameObject> edgeEnds = new List<GameObject>();
    public GameObject edgeTitleUpper;
    public GameObject edgeEndNumberUpper;
    public GameObject edgeTitleLower;
    public GameObject edgeEndNumberLower;
    public GameObject textbox;
    // private Vector3 mousePos;
    public GameObject popupLineMenu;
    float holdTimer = 0;
    bool hold = false;
    private Diagram _diagram;
    private Vector3 mousePos;
    public GameObject compositionIcon;
    public GameObject aggregationIcon;
    public GameObject generalizationIcon;
    private int[] prevConnectionPointIndices = new int[] {-1,-1}; // used to keep track of connection point changes for updating availabilities  
    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }
    void Start()
    {
        createEdge();
    }

    void Update()
    {
        if (nodes != null)
        {
            // Testing 
            // var node = nodes[0].GetComponent<Node>();
            // List<Vector2> locations = node.GetConnectionPointsLocations();
            // foreach(var loc in locations) 
            // {
            //     Debug.Log(loc.ToString());
            // }

            // Debug.Log("Mouse: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));

            var node1 = nodes[0].GetComponent<Node>();
            var node2 = nodes[1].GetComponent<Node>();
            var edgeEnd1 = edgeEnds[0].GetComponent<EdgeEnd>();
            var edgeEnd2 = edgeEnds[1].GetComponent<EdgeEnd>();

            var node1_locs = node1.GetConnectionPointsLocations();
            var node2_locs = node2.GetConnectionPointsLocations();

            Debug.Log("available node1_locs size: " + node1.GetNumberOfConnectionPointsAvailable());
            Debug.Log("available node2_locs size: " + node2.GetNumberOfConnectionPointsAvailable());

            Vector2 edgeEnd1_loc = nodes[0].transform.position;
            Vector2 edgeEnd2_loc = nodes[1].transform.position;

            if (node1.GetNumberOfConnectionPointsAvailable() == 0 || node2.GetNumberOfConnectionPointsAvailable() == 0)
            {
                //TODO: Implement Node Connection Points Expansion 
                Debug.Log("Entered critical area");
            } 
            else
            {
                int[] indices = GetIndicesOfMinDist(node1_locs, node2_locs, node1.GetConnectionPointsAvailable(), node2.GetConnectionPointsAvailable());
                bool nodeOneUpdated = prevConnectionPointIndices[0] != indices[0];
                bool nodeTwoUpdated = prevConnectionPointIndices[1] != indices[1];
                Debug.Log("prevConnectionPointIndex 1: " + prevConnectionPointIndices[0]);
                Debug.Log("prevConnectionPointIndex 2: " + prevConnectionPointIndices[1]);
                // Debug.Log("currIndex 1: " + indices[0]);
                // Debug.Log("currIndex 2: " + indices[1]);
                if (nodeOneUpdated || nodeTwoUpdated)
                {
                    if(prevConnectionPointIndices[0] >= 0 && prevConnectionPointIndices[1] >= 0)
                    {
                        if(prevConnectionPointIndices[0] < node1_locs.Count && prevConnectionPointIndices[1] < node2_locs.Count)
                        {
                            Debug.Log("prevConnectionPointIndices 1: " + prevConnectionPointIndices[0]);
                            Debug.Log("prevConnectionPointIndices 2: " + prevConnectionPointIndices[1]);
                            float prevDist = Vector2.Distance(edgeEnd1.Position, edgeEnd2.Position);
                            float currentDist = Vector2.Distance(node1_locs[indices[0]], node2_locs[indices[1]]);
                            Debug.Log("difference: " + Math.Abs(prevDist - currentDist));
                            if (Math.Abs(prevDist - currentDist) < 250)
                            {
                                Debug.Log("Do not change");
                                indices[0] = prevConnectionPointIndices[0];
                                indices[1] = prevConnectionPointIndices[1];
                            } else {
                                Debug.Log("Change but why?");
                            }
                        }
                    } 

                    if (prevConnectionPointIndices[0] >= 0 && nodeOneUpdated)
                    {
                        node1.SetConnectionPointAvailable(prevConnectionPointIndices[0], true);
                    }
                    if (prevConnectionPointIndices[1] >= 0 && nodeTwoUpdated)
                    {
                        node2.SetConnectionPointAvailable(prevConnectionPointIndices[1], true);
                    }
                }

                if(node1.GetConnectionPointAvailable(indices[0]) || node2.GetConnectionPointAvailable(indices[1]))
                {
                    edgeEnd1_loc = node1_locs[indices[0]];
                    edgeEnd2_loc = node2_locs[indices[1]];
                } 
                else
                {
                    Debug.Log("Errorrrrrrrrr !!!!!!!!!!!!!!!!!!!!!!!!!!");
                }

                edgeEnd1.Position = edgeEnd1_loc;
                edgeEnd2.Position = edgeEnd2_loc;

                // Set connection points as unavailable
                node1.SetConnectionPointAvailable(indices[0], false);
                node2.SetConnectionPointAvailable(indices[1], false);
  
                prevConnectionPointIndices[0] = indices[0];
                prevConnectionPointIndices[1] = indices[1];
            }

            var pos1 = Camera.main.ScreenToWorldPoint(edgeEnd1.Position);
            pos1.z = 0;
            var pos2 = Camera.main.ScreenToWorldPoint(edgeEnd2.Position);
            pos2.z = 0;
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
        //     var diff_y = nodes[0].transform.position.y - nodes[1].transform.position.y;
        //     var diff_x = nodes[0].transform.position.x - nodes[1].transform.position.x;
        //     if (diff_x <= diff_y)
        //     {
        //         gameObject.transform.position = nodes[0].transform.position + new Vector3(0,-95,0);
        //         var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(0, -95, 0));
        //         pos1.z = 0;
        //         var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(0, 95, 0));
        //         pos2.z = 0;
        //         line.SetPosition(0, pos1);
        //         line.SetPosition(1, pos2);
        //     }
        //     else
        //     {
        //         gameObject.transform.position = nodes[1].transform.position + new Vector3(95,0,0);
        //         var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(-95, 0, 0));
        //         pos1.z = 0;
        //         var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(95, 0, 0));
        //         pos2.z = 0;
        //         line.SetPosition(0, pos1);
        //         line.SetPosition(1, pos2);
        //     }
        }

        // if (Input.GetMouseButtonDown(1))//right click
        // {
        //     //check if type of edge. check if within radius
        //     Debug.Log("edgePos: " + gameObject.transform.position);
        //     mousePos = Input.mousePosition;
        //     Debug.Log("mousePos: " + mousePos);
        //     //NB gameobject.transform.position is the location of upper edgeend
        //     //or left edgeend
        //     var radius = Vector3.Distance(mousePos, gameObject.transform.position);
        //     Debug.Log("radius: " + radius);
        //     if (radius < 20){
        //         SpawnPopupLineMenu();
        //     }
        // }
        // else if (this.hold && holdTimer > 1f - 5)
        // {
        //     this.hold = false;
        //     holdTimer = 0;
        //     SpawnPopupLineMenu();
        // }
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

        var edgeEnd1 = GameObject.Instantiate(edgeEnd);
        var edgeEnd2 = GameObject.Instantiate(edgeEnd);
        edgeEnd1.GetComponent<EdgeEnd>().SetEdge(this.gameObject);
        edgeEnd2.GetComponent<EdgeEnd>().SetEdge(this.gameObject);

        // var node1 = nodes[0].GetComponent<Node>();
        // var node2 = nodes[1].GetComponent<Node>();

        // var node1_locs = node1.GetAvailableConnectionPointsLocations();
        // var node2_locs = node2.GetAvailableConnectionPointsLocations();

        // if (node1_locs.Count == 0 || node2_locs.Count == 0)
        // {
        //     //TODO: Implement Node Connection Points Expansion 
        // } 
        // else
        // {
        //     int[] indices = GetIndicesOfMinDist(node1_locs, node2_locs);
        //     var edgeEnd1_loc = node1_locs[indices[0]];
        //     var edgeEnd2_loc = node2_locs[indices[1]];

        //     edgeEnd1.GetComponent<EdgeEnd>().Position = edgeEnd1_loc;
        //     node1.SetConnectionPointAvailability(indices[0], false);

        //     edgeEnd2.GetComponent<EdgeEnd>().Position = edgeEnd2_loc;
        //     node2.SetConnectionPointAvailability(indices[1], false);
        // }

        // var pos1 = Camera.main.ScreenToWorldPoint(edgeEnd1.GetComponent<EdgeEnd>().Position);
        // pos1.z = 0;
        // var pos2 = Camera.main.ScreenToWorldPoint(edgeEnd2.GetComponent<EdgeEnd>().Position);
        // pos2.z = 0;
        // line.SetPosition(0, pos1);
        // line.SetPosition(1, pos2);

        // // check closest node edge
        var diff_y = nodes[0].transform.position.y - nodes[1].transform.position.y;
        var diff_x = nodes[0].transform.position.x - nodes[1].transform.position.x;
        if (diff_x <= diff_y)
        {
            gameObject.transform.position = nodes[0].transform.position + new Vector3(0,-95,0);
            // Debug.Log("hello hello hello how low");
        //     CreateEdgeEndUpperObj(nodes[0]);//create edge number and title textboxes for first obj
        //     CreateEdgeEndLowerObj(nodes[1]);
        }
        else
        {
            gameObject.transform.position = nodes[1].transform.position + new Vector3(95,0,0);
            // Debug.Log("hello hello hello how low");
        //     CreateEdgeEndLeftObject(nodes[1]);
        //     CreateEdgeEndRightObject(nodes[0]);
        }
    }

    public void CreateEdgeEndLeftObject(GameObject obj)
    {
        //replace edge title upper by edge title right
        this.edgeTitleUpper = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumberUpper = GameObject.Instantiate(textbox, obj.transform);
        //will need to get cordinates from edge object in future
        this.edgeTitleUpper.transform.position += new Vector3(220, -15, 0);
        this.edgeEndNumberUpper.transform.position += new Vector3(210, -60, 0);
        //Left Object
        this.edgeTitleUpper.GetComponent<InputField>().text = "enter text";
        this.edgeEndNumberUpper.GetComponent<InputField>().text = "*";
        // Debug.Log(nodes[0].transform.position);
        // Debug.Log(this.edgeTitleUpper.transform.position);
        Debug.Log("edgeend here");
    }

    public void CreateEdgeEndRightObject(GameObject obj)
    {
        //replace edge title upper by edge title left
        this.edgeTitleUpper = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumberUpper = GameObject.Instantiate(textbox, obj.transform);
        //will need to get cordinates from edge object in future
        this.edgeTitleUpper.transform.position += new Vector3(-90, -10, 0);
        this.edgeEndNumberUpper.transform.position += new Vector3(-20, -50, 0);
        //Right Object
        this.edgeTitleUpper.GetComponent<InputField>().text = "enter text";
        this.edgeEndNumberUpper.GetComponent<InputField>().text = "*";
        // Debug.Log(nodes[0].transform.position);
        // Debug.Log(this.edgeTitleUpper.transform.position);
        Debug.Log("edgeend here");
    }

    //create edge end for upper object
    public void CreateEdgeEndUpperObj(GameObject obj)
    {
        this.edgeTitleUpper = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumberUpper = GameObject.Instantiate(textbox, obj.transform);
        //will need to get cordinates from edge object in future
        this.edgeTitleUpper.transform.position += new Vector3(40, -170, 0);
        this.edgeEndNumberUpper.transform.position += new Vector3(125, -170, 0);
        this.edgeTitleUpper.GetComponent<InputField>().text = "enter text";
        this.edgeEndNumberUpper.GetComponent<InputField>().text = "*";
        // Debug.Log(nodes[0].transform.position);
        // Debug.Log(this.edgeTitleUpper.transform.position);
        Debug.Log("edgeend here");
    }
    public void CreateEdgeEndLowerObj(GameObject obj)
    {
        this.edgeTitleLower = GameObject.Instantiate(textbox, obj.transform);
        this.edgeEndNumberLower = GameObject.Instantiate(textbox, obj.transform);
        //will need to get cordinates from edge object in future
        this.edgeTitleLower.transform.position += new Vector3(40, 85, 0);
        this.edgeEndNumberLower.transform.position += new Vector3(125, 85, 0);
        this.edgeTitleLower.GetComponent<InputField>().text = "enter text";
        this.edgeEndNumberLower.GetComponent<InputField>().text = "*";
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
        SetIconType(0);

    }
    public void SetAggregation()
    {
        SetIconType(1);

    }
    public void SetComposition()
    {
        SetIconType(2);

    }
    public void SetGeneralization()
    {
        SetIconType(3);

    }

    public void SetIconType(int type)
    {
        GameObject edgeNode;
        float x;
        float y;
        // find closest edge
        var a = Vector3.Distance(mousePos, nodes[0].transform.position);
        var b = Vector3.Distance(mousePos, nodes[1].transform.position);
        // find if connection is horizontal or vertical
        var diff_y = nodes[0].transform.position.y - nodes[1].transform.position.y;
        var diff_x = nodes[0].transform.position.x - nodes[1].transform.position.x;
        if (a < b && diff_x <= diff_y)
        {
            edgeNode = nodes[0];
            y = -95;
            x = 0;

        }
        else if (diff_x <= diff_y)
        {
            edgeNode = nodes[1];
            y = 95;
            x = 0;

        }
        else if (a<b)
        {
            edgeNode = nodes[0];
            x = -95;
            y = 0;

        }
        else{
            edgeNode = nodes[1];
            x = 95;
            y = 0;

        }
        switch (type)
        {
            case 0:
                //TODO  
               //ASSOCIATION - same as line, 
               //should destroy any existing icon at intended location
                break;
            case 1:
                //TODO  
                aggregationIcon = GameObject.Instantiate(aggregationIcon);
                aggregationIcon.transform.position = edgeNode.transform.position + new Vector3(x, y, 0);
                aggregationIcon.GetComponent<AggregationIcon>().SetNode(edgeNode, x, y);
                break;
            case 2:
                compositionIcon = GameObject.Instantiate(compositionIcon);
                compositionIcon.transform.position = edgeNode.transform.position + new Vector3(x, y, 0);
                compositionIcon.GetComponent<CompositionIcon>().SetNode(edgeNode, x, y);
                break;
             case 3:
                generalizationIcon = GameObject.Instantiate(generalizationIcon);
                generalizationIcon.transform.position = edgeNode.transform.position + new Vector3(x, y, 0);
                generalizationIcon.GetComponent<GeneralizationIcon>().SetNode(edgeNode, x, y);
                break;
            default:
                break;
        }
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

    // public void OnEndHold()
    // {
    //     if (holdTimer > 1f - 5)
    //     {
    //         SpawnPopupLineMenu();
    //     }
    //     holdTimer = 0;
    //     this.hold = false;

    // }
    public void SpawnPopupLineMenu()
    {
         Debug.Log("beginholdheard");
        if (this.popupLineMenu.GetComponent<PopupLineMenu>().GetLine() == null)
        {
            this.popupLineMenu = GameObject.Instantiate(this.popupLineMenu);
            this.popupLineMenu.transform.SetParent(this.transform);
            //this can be changed so that popupline menu is always instantiated at
            //midpoint of the relationship
            this.popupLineMenu.transform.position = this.mousePos + new Vector3(70, -110, 0);
            this.popupLineMenu.GetComponent<PopupLineMenu>().SetUpdateConstant(this.popupLineMenu.transform.position);
            this.popupLineMenu.GetComponent<PopupLineMenu>().SetLine(this);
        }
        else
        {
            this.popupLineMenu.GetComponent<PopupLineMenu>().Open();
        }
    }

    /// <summary> This function takes two lists of connection points Vec2 coordinates 
    /// and returns the indices of each lists as an array of int such that the pairwise 
    /// distance is minimal and the connections indexed are available.   
    /// </summary>
    private int[] GetIndicesOfMinDist(List<Vector2>node1_locs, List<Vector2>node2_locs, ReadOnlyCollection<bool> node1_avails, ReadOnlyCollection<bool> node2_avails)
    {
        int[] indices = new int[2];
        float minDist = float.MaxValue;
        for (int i = 0; i < node1_locs.Count; i++)
        {
            if (!node1_avails[i])
            {
                continue;
            }
            for (int j = 0; j < node2_locs.Count; j++)
            {
                if (!node2_avails[j])
                {
                    continue;
                }
                float dist = Vector2.Distance(node1_locs[i], node2_locs[j])*Vector2.Distance(node1_locs[i], node2_locs[j]);
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

}
