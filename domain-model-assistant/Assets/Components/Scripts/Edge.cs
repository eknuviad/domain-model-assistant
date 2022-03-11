using System.Collections;
using System.Collections.Generic;
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
    public List<GameObject> edgeEnds = new List<GameObject>();

    public GameObject edgeEnd;
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
    public Vector3 mousePos;
    public Vector3 linePosition1;
    public Vector3 linePosition2;
    public Vector3 popupLineMenuOffset;
    public GameObject compositionIcon;
    public GameObject aggregationIcon;
    public GameObject  generalizationIcon;
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
            var diff_y = nodes[0].transform.position.y - nodes[1].transform.position.y;
            var diff_x = nodes[0].transform.position.x - nodes[1].transform.position.x;
            if(diff_x <= diff_y)
            {
                gameObject.transform.position = nodes[0].transform.position + new Vector3(0,-95,0);
                var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(0, -95, 0));
                pos1.z = 0;
                var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(0, 95, 0));
                pos2.z = 0;
                line.SetPosition(0, pos1);
                line.SetPosition(1, pos2);
            }
            else
            {
                gameObject.transform.position = nodes[1].transform.position + new Vector3(95,0,0);
                var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(-95, 0, 0));
                pos1.z = 0;
                var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(95, 0, 0));
                pos2.z = 0;
                line.SetPosition(0, pos1);
                line.SetPosition(1, pos2);
            }
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

        // check closest node edge
        var diff_y = nodes[0].transform.position.y - nodes[1].transform.position.y;
        var diff_x = nodes[0].transform.position.x - nodes[1].transform.position.x;
        if(diff_x <= diff_y)
        {
            gameObject.transform.position = nodes[0].transform.position + new Vector3(0,-95,0);
            CreateEdgeEndUpperObj(nodes[0]);//create edge number and title textboxes for first obj
            CreateEdgeEndLowerObj(nodes[1]);
        }
        else
        {
            gameObject.transform.position = nodes[1].transform.position + new Vector3(95,0,0);
            CreateEdgeEndLeftObject(nodes[1]);
            CreateEdgeEndRightObject(nodes[0]);
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
        
        if(wasAdded)
        {
            Debug.Log("node added to edge");
        }
        return wasAdded;
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

}
