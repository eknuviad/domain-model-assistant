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

    public GameObject edgeEndTitle;
    public GameObject edgeEndNumber;
    public GameObject textbox;

    public bool isUpper = true;
    public bool isLeft = true;
    public static Vector2 TitleVerticalOffset = new Vector2(0,40);
    public static Vector2 TitleHorizontalOffset = new Vector2(80,0);
    public static Vector2 NumberTitleOffset = new Vector2(0,-20);
    private GameObject edge;
    public GameObject compositionIcon;
    public GameObject aggregationIcon;
    public GameObject generalizationIcon;
    public bool hasActiveIcon = false;
    // public GameObject popupMenu;
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        edgeEndTitle = GameObject.Instantiate(textbox, this.transform);
        edgeEndTitle.GetComponent<InputField>().text = "Enter Text ...";
        edgeEndTitle.transform.position = Position + new Vector2(0,40);
        edgeEndNumber = GameObject.Instantiate(textbox, this.transform);
        edgeEndNumber.GetComponent<InputField>().text = "*";
        edgeEndNumber.transform.position = Position + new Vector2(85,-20);

        generalizationIcon = GameObject.Instantiate(generalizationIcon);
        aggregationIcon = GameObject.Instantiate(aggregationIcon);
        compositionIcon = GameObject.Instantiate(compositionIcon);

        generalizationIcon.SetActive(false);
        aggregationIcon.SetActive(false);
        compositionIcon.SetActive(false);

        generalizationIcon.GetComponent<GeneralizationIcon>().SetEdgeEnd(this);
        aggregationIcon.GetComponent<AggregationIcon>().SetEdgeEnd(this);
        compositionIcon.GetComponent<CompositionIcon>().SetEdgeEnd(this);

        Debug.Log("instatntiate");
    }

    void Update()
    {
        Vector2 titlePos;
        Vector2 numberPos;

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
            if(isLeft)
            {
                titlePos = Position + TitleVerticalOffset;
            }
            else
            {
                titlePos = Position + TitleVerticalOffset - TitleHorizontalOffset;
            }
        }

        numberPos = titlePos + NumberTitleOffset;

        edgeEndTitle.transform.position = titlePos;
        edgeEndNumber.transform.position = numberPos;

        compositionIcon.transform.position = Position;
        aggregationIcon.transform.position = Position;
        generalizationIcon.transform.position = Position;
    }

    void Destroy()
    {
         Destroy(this.gameObject);
    }

    public GameObject GetEdge()
    {
        return edge;
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
        if (edge != null)
        {
            return wasSet;
        }
        edge = aEdge;
        edge.GetComponent<Edge>().AddEdgeEnd(this.gameObject);
        wasSet = true;
        Debug.Log("Edge end: Edge set");
        return wasSet;
    }

    public void SetIconType(int type)
    {
        switch (type)
        {
            case 0:
                //TODO  
               //ASSOCIATION - same as line, 
               //should destroy any existing icon at intended location
                compositionIcon.SetActive(false);
                generalizationIcon.SetActive(false);
                aggregationIcon.SetActive(false);
                hasActiveIcon = false;
                break;
            case 1:
                hasActiveIcon = true;
                if (compositionIcon.activeSelf==true)
                {
                    compositionIcon.SetActive(false);
                }
                if (generalizationIcon.activeSelf==true)
                {
                    generalizationIcon.SetActive(false);
                }
                if (aggregationIcon.activeSelf==false)
                {
                    aggregationIcon.SetActive(true);
                }
                break;
            case 2:
                hasActiveIcon = true;
                if (generalizationIcon.activeSelf==true)
                {
                    generalizationIcon.SetActive(false);
                }
                if (aggregationIcon.activeSelf==true)
                {
                    aggregationIcon.SetActive(false);
                }
                if (compositionIcon.activeSelf==false)
                {
                    compositionIcon.SetActive(true);
                }
                break;
             case 3:
                hasActiveIcon = true;
                if (compositionIcon.activeSelf==true)
                {
                    compositionIcon.SetActive(false);
                }
                if (aggregationIcon.activeSelf==true)
                {
                    aggregationIcon.SetActive(false);
                }
                if (generalizationIcon.activeSelf==false)
                {
                    generalizationIcon.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

}
