using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

//this class should track the different line ends, however
//inconsistencies were found in its transform.position hence
//edge ends were directly included and manipulated in the Edge class.
public class EdgeEnd : MonoBehaviour
{
    
    public string ID
    { get; set; }
    
    public Vector2 Position
    { get; set; }
    private string _nodeId;
    private GameObject _edgeEndTitle;
    private GameObject _edgeEndNumber;
    public GameObject edgeEndTitle;
    public GameObject edgeEndNumber;
    public bool isUpper = true;
    public bool isLeft = true;
    public static Vector2 TitleVerticalOffset = new Vector2(0, 40);
    public static Vector2 TitleHorizontalOffset = new Vector2(80, 0);
    public static Vector2 NumberTitleOffset = new Vector2(0, -20);
    private GameObject[] canvasClasses;
    private GameObject _edge;
    private GameObject _node;
    public GameObject compositionIcon;
    public GameObject aggregationIcon;
    public GameObject generalizationIcon;
    public GameObject arrowHeadIcon;
    public bool hasActiveIcon = false;
    public bool isGeneralization = false;
    // public GameObject popupMenu;
    public float Angle {get; set;}

    void Start()
    {
        gameObject.transform.SetParent(GameObject.Find("Canvas").transform);

        ID = "-1"; // initialize id with -1

        SetEdgeEndTitle(GameObject.Instantiate(edgeEndTitle, transform));
        _edgeEndTitle.GetComponent<RoleNameTextBox>().SetTitleOwner(this.gameObject);
        _edgeEndTitle.GetComponent<InputField>().text = "Enter Text ...";
        _edgeEndTitle.transform.position = Position + new Vector2(0, 40);
        _edgeEndNumber = GameObject.Instantiate(edgeEndNumber, transform);
        
        _edgeEndNumber.GetComponent<MultiplicityTextBox>().SetNumberOwner(this.gameObject);

        _edgeEndNumber.GetComponent<InputField>().text = "1";
        _edgeEndNumber.transform.position = Position + new Vector2(85, -20);

        generalizationIcon = GameObject.Instantiate(generalizationIcon);
        aggregationIcon = GameObject.Instantiate(aggregationIcon);
        compositionIcon = GameObject.Instantiate(compositionIcon);
        arrowHeadIcon = GameObject.Instantiate(arrowHeadIcon);

        arrowHeadIcon.SetActive(false);
        generalizationIcon.SetActive(false);
        aggregationIcon.SetActive(false);
        compositionIcon.SetActive(false);

        generalizationIcon.GetComponent<GeneralizationIcon>().SetEdgeEnd(this);
        aggregationIcon.GetComponent<AggregationIcon>().SetEdgeEnd(this);
        compositionIcon.GetComponent<CompositionIcon>().SetEdgeEnd(this);
        arrowHeadIcon.GetComponent<ArrowHeadIcon>().SetEdgeEnd(this);


        Debug.Log("EdgeEnd instantiated");
    }

    void Update()
    {

        Vector2 titlePos;
        Vector2 numberPos;

        transform.position = Position;

        if (isUpper)
        {
            if (isLeft)
            {
                titlePos = Position - TitleVerticalOffset;
            }
            else
            {
                titlePos = Position - TitleVerticalOffset - TitleHorizontalOffset;
            }
        }
        else
        {
            if (isLeft)
            {
                titlePos = Position + TitleVerticalOffset;
            }
            else
            {
                titlePos = Position + TitleVerticalOffset - TitleHorizontalOffset;
            }
        }

        numberPos = titlePos + NumberTitleOffset;

        _edgeEndTitle.transform.position = titlePos;
        _edgeEndNumber.transform.position = numberPos;

        arrowHeadIcon.transform.position = Position;
        compositionIcon.transform.position = Position;
        aggregationIcon.transform.position = Position;
        generalizationIcon.transform.position = Position;

        // Debug.Log("Angle: " + Angle);
        arrowHeadIcon.transform.eulerAngles = Vector3.forward * -Angle;
    }

    public void DeleteEdgeEnd()
    {
        Destroy(arrowHeadIcon);
        Destroy(compositionIcon);
        Destroy(generalizationIcon);
        Destroy(aggregationIcon);
        Destroy(gameObject);
    }

    //------------------------
    // INTERFACE
    //------------------------

    public GameObject GetEdge()
    {
        return _edge;
    }

    public GameObject GetNode()
    {
        return _node;
    }

    public GameObject GetEdgeEndTitle()
    {
        return _edgeEndTitle;
    }

    public GameObject GetEdgeEndNumber()
    {
        return _edgeEndNumber;
    }

    public bool SetEdgeEndTitle(GameObject aNewEdgeEndTitle)
    {
        bool wasSet = false;
        if (aNewEdgeEndTitle == null)
        {
            GameObject existingEdgeEndTitle = _edgeEndTitle;
            _edgeEndTitle = null;

            if (existingEdgeEndTitle != null && existingEdgeEndTitle.GetComponent<RoleNameTextBox>().GetTitleOwner() != null)
            {
                existingEdgeEndTitle.GetComponent<RoleNameTextBox>().SetTitleOwner(null);
            }
            wasSet = true;
            return wasSet;
        }

        GameObject currentEdgeEndTitle = GetEdgeEndTitle();
        if (currentEdgeEndTitle != null && !currentEdgeEndTitle.Equals(aNewEdgeEndTitle))
        {
            currentEdgeEndTitle.GetComponent<RoleNameTextBox>().SetTitleOwner(null);
        }

        _edgeEndTitle = aNewEdgeEndTitle;
        GameObject existingTitleOwner = aNewEdgeEndTitle.GetComponent<RoleNameTextBox>().GetTitleOwner();

        if (!Equals(existingTitleOwner))
        {
            aNewEdgeEndTitle.GetComponent<RoleNameTextBox>().SetTitleOwner(gameObject);
        }
        wasSet = true;
        return wasSet;
    }

    public bool SetEdgeEndNumber(GameObject aNewEdgeEndNumber)
    {
        bool wasSet = false;
        if (aNewEdgeEndNumber == null)
        {
            GameObject existingEdgeEndNumber = _edgeEndNumber;
            _edgeEndNumber = null;

            if (existingEdgeEndNumber != null && existingEdgeEndNumber.GetComponent<MultiplicityTextBox>().GetNumberOwner() != null)
            {
                existingEdgeEndNumber.GetComponent<MultiplicityTextBox>().SetNumberOwner(null);
            }
            wasSet = true;
            return wasSet;
        }

        GameObject currentEdgeEndNumber = GetEdgeEndNumber();
        if (currentEdgeEndNumber != null && !currentEdgeEndNumber.Equals(aNewEdgeEndNumber))
        {
            currentEdgeEndNumber.GetComponent<MultiplicityTextBox>().SetNumberOwner(null);
        }

        _edgeEndNumber = aNewEdgeEndNumber;
        GameObject existingNumberOwner = aNewEdgeEndNumber.GetComponent<MultiplicityTextBox>().GetNumberOwner();

        if (!Equals(existingNumberOwner))
        {
            aNewEdgeEndNumber.GetComponent<MultiplicityTextBox>().SetNumberOwner(gameObject);
        }
        wasSet = true;
        return wasSet;
    }

    public bool SetNode(GameObject aNode)
    {
        bool wasSet = false;
        if (aNode == null)
        {
            return wasSet;
        }

        GameObject existingNode = _node;
        _node = aNode;
        _nodeId = aNode.GetComponent<CompartmentedRectangle>().ID;
        if (existingNode != null && !existingNode.Equals(aNode))
        {
            existingNode.GetComponent<Node>().RemoveEdgeEnd(gameObject);
        }
        _node.GetComponent<Node>().AddEdgeEnd(gameObject);
        wasSet = true;
        return wasSet;
    }

    public bool SetEdge(GameObject aEdge)
    {
        bool wasSet = false;
        if (aEdge == null)
        {
            return wasSet;
        }
        Edge edgeComp = aEdge.GetComponent<Edge>();
        // Check if edge already have 2 or more edge ends 
        if (edgeComp.GetNumberOfEdgeEnds() >= Edge.RequiredNumOfEdgeEnds)
        {
            return wasSet;
        }
        // Check if edge has already been set
        GameObject existingEdge = _edge;
        _edge = aEdge;
        if (existingEdge != null && !existingEdge.Equals(aEdge)) 
        {
            bool didRemove = existingEdge.GetComponent<Edge>().RemoveEdgeEnd(gameObject);
            if (!didRemove)
            {
                _edge = existingEdge;
                return wasSet;
            } 
            return wasSet;
        }
        _edge = aEdge;
        _edge.GetComponent<Edge>().AddEdgeEnd(gameObject);
        wasSet = true;
        Debug.Log("Edge end: Edge set");
        return wasSet;
    }

    public void SetIconType(int type)
    {
        switch (type)
        {
            case 0:
                // TODO  
                // ASSOCIATION - same as line, 
                // should destroy any existing icon at intended location
                arrowHeadIcon.SetActive(false);
                compositionIcon.SetActive(false);
                generalizationIcon.SetActive(false);
                aggregationIcon.SetActive(false);
                hasActiveIcon = false;
                break;
            case 1:
                hasActiveIcon = true;
                if (compositionIcon.activeSelf)
                {
                    compositionIcon.SetActive(false);
                }
                if (generalizationIcon.activeSelf)
                {
                    generalizationIcon.SetActive(false);
                }
                if (aggregationIcon.activeSelf)
                {
                    aggregationIcon.SetActive(false);
                }
                if (!arrowHeadIcon.activeSelf)
                {
                    arrowHeadIcon.SetActive(true);
                }
                break;
            case 2:
                hasActiveIcon = true;
                if (arrowHeadIcon.activeSelf)
                {
                    compositionIcon.SetActive(false);
                }
                if (compositionIcon.activeSelf)
                {
                    compositionIcon.SetActive(false);
                }
                if (generalizationIcon.activeSelf)
                {
                    generalizationIcon.SetActive(false);
                }
                if (!aggregationIcon.activeSelf)
                {
                    aggregationIcon.SetActive(true);
                }
                break;
            case 3:
                hasActiveIcon = true;
                if (arrowHeadIcon.activeSelf)
                {
                    compositionIcon.SetActive(false);
                }
                if (generalizationIcon.activeSelf)
                {
                    generalizationIcon.SetActive(false);
                }
                if (aggregationIcon.activeSelf)
                {
                    aggregationIcon.SetActive(false);
                }
                if (!compositionIcon.activeSelf)
                {
                    compositionIcon.SetActive(true);
                }
                break;
             case 4:
                hasActiveIcon = true;
                if (arrowHeadIcon.activeSelf)
                {
                    compositionIcon.SetActive(false);
                }
                if (compositionIcon.activeSelf)
                {
                    compositionIcon.SetActive(false);
                }
                if (aggregationIcon.activeSelf)
                {
                    aggregationIcon.SetActive(false);
                }
                if (!generalizationIcon.activeSelf)
                {
                    generalizationIcon.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    public void RetrieveNode()
    {
        canvasClasses = GameObject.FindGameObjectsWithTag("comprec");
        foreach (var comp in canvasClasses)
        {
            if (Equals(comp.GetComponent<CompartmentedRectangle>().ID,_nodeId))
            {
                SetNode(comp);
                
                if (string.Equals(ID, "-1")) // id not initialized
                {
                    InitIDFromJSON(); // get the ID from backend
                }
            }
        }
        if (_node == null){
            DeleteEdgeEnd();
        }
    }

    private void InitIDFromJSON()
    {
        // Assign the edge ends the highest id (newest association end)
        var diagram = _node.GetComponent<CompartmentedRectangle>().GetDiagram();
        var endsDTO = diagram.GetComponent<Diagram>().classIdToAssociationEndsDTO[_nodeId];
        int maxID;
        int.TryParse(endsDTO[0]._id, out maxID); // Intialize max ID with the ID of the first element
        foreach (var ends in endsDTO)
        {
            int id;
            int.TryParse(ends._id, out id);
            if (id > maxID)
            {
                maxID = id;
            }
        }
        ID = maxID.ToString();
        Debug.Log("EdgeEnd assigned id: " + ID);
    }

}
