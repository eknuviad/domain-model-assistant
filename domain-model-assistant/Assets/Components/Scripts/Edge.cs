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

    public List<string> nodesId = new List<string>();

    public List<GameObject> edgeEnds = new List<GameObject>();

    public GameObject[] classes;
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

    public GameObject diagram;
    private Diagram _diagram;
    private Vector3 mousePos;
    public GameObject compositionIcon;
    public GameObject aggregationIcon;
    public GameObject  generalizationIcon;
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
        }else{
            RetrieveNodes();
            
        }

        if (Input.GetMouseButtonDown(1))//right click
        {
            //check if type of edge. check if within radius
            Debug.Log("edgePos: " + gameObject.transform.position);
            mousePos = Input.mousePosition;
            Debug.Log("mousePos: " + mousePos);
            //NB gameobject.transform.position is the location of upper edgeend
            //or left edgeend
            var radius = Vector3.Distance(mousePos, gameObject.transform.position);
            Debug.Log("radius: " + radius);
            if(radius < 20){
                SpawnPopupLineMenu();
            }
        }
        // else if (this.hold && holdTimer > 1f - 5)
        // {
        //     this.hold = false;
        //     holdTimer = 0;
        //     SpawnPopupLineMenu();
        // }
    }}

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

    public bool AddNode(GameObject aNode)
    {
        if (nodes.Contains(aNode))
        {
            return false;
        }
        nodes.Add(aNode);
        nodesId.Add(aNode.GetComponent<CompartmentedRectangle>().ID);
        aNode.GetComponent<Node>().AddEdge(this.gameObject);
        Debug.Log("node added to edge");
        return true;
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
