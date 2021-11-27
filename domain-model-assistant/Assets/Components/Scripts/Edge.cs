using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Edge : MonoBehaviour
{
    public string ID
    { get; set; }
    private LineRenderer line;
    public List<GameObject> nodes = new List<GameObject>();

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
    private Vector3 mousePos;
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
            var pos1 = Camera.main.ScreenToWorldPoint(nodes[0].transform.position + new Vector3(0, -95, 0));
            pos1.z = 0;
            var pos2 = Camera.main.ScreenToWorldPoint(nodes[1].transform.position + new Vector3(0, 95, 0));
            pos2.z = 0;
            line.SetPosition(0, pos1);
            line.SetPosition(1, pos2);
        }

        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
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
        CreateEdgeEndUpperObj(nodes[0]);//create edge number and title textboxes for first obj
        CreateEdgeEndLowerObj(nodes[1]);

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
        var a = Vector3.Distance(mousePos, nodes[0].transform.position);
        var b = Vector3.Distance(mousePos, nodes[1].transform.position);
        if (a < b)
        {
            edgeNode = nodes[0];
            x = -95;
        }
        else
        {
            edgeNode = nodes[1];
            x = 95;
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
                aggregationIcon.transform.position = edgeNode.transform.position + new Vector3(0, x, 0);
                aggregationIcon.GetComponent<AggregationIcon>().SetNode(edgeNode, x);
                break;
            case 2:
                compositionIcon = GameObject.Instantiate(compositionIcon);
                compositionIcon.transform.position = edgeNode.transform.position + new Vector3(0, x, 0);
                compositionIcon.GetComponent<CompositionIcon>().SetNode(edgeNode, x);
                break;
             case 3:
                generalizationIcon = GameObject.Instantiate(generalizationIcon);
                generalizationIcon.transform.position = edgeNode.transform.position + new Vector3(0, x, 0);
                generalizationIcon.GetComponent<GeneralizationIcon>().SetNode(edgeNode, x);
                break;
            default:
                break;
        }
    }
    public GameObject GetPopUpLineMenu()
    {
        return popupLineMenu;
    }

    void SpawnPopupLineMenu()
    {
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


}