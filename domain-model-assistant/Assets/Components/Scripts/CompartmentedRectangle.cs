using System.Drawing;
//using System.Diagnostics;
//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CompartmentedRectangle : Node
{
    public GameObject textbox; //this allows to instantiate textbox prefabs
    public GameObject section;
    public List<GameObject> sections = new List<GameObject>();
    public TextBox text;
    // popup menu variables
    public GameObject popupMenu;
    public RectTransform rectangle;
    
    public Color highlight = new Color(0.61f, 0.81f, 0.973f);
    public Color headerColor; // = this.transform.GetChild(1).gameObject.GetComponent<Image>().color;
    public Color sectionColor;
    float holdTimer = 0;
    bool hold = false;
    public State state
    { get; set; }
    private Diagram _diagram;

    private string id; //should move to node class later
    private Vector2 _prevPosition;
    private float rectHeight;
    private float rectWidth;
    private int headerOffsetX = -31;
    private int headerOffsetY = 70;
    private int sectionOffsetY = -71;
    private int popupMenuOffsetX = 138;

    public bool isHighlighted
    { get; set; }
    public enum State
    {
        Default,
        DraggingClass,
    }

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    // Start is called before the first frame update
    void Start()
    {
        state = State.Default;
        NumOfConnectionPoints = 8; // default value
        for(int i = 0; i < NumOfConnectionPoints; i++)
        {
            connectionPointsAvailable.Add(true);
        }
        RectTransform rectangle = (RectTransform)this.transform;
        rectHeight = rectangle.rect.height;
        rectWidth = rectangle.rect.width;
        CreateHeader();
        CreateSection();
        id = this.GetComponent<CompartmentedRectangle>().ID;
        // Debug.Log(this.transform.GetChild(0));
        // Debug.Log(this.transform.GetChild(1));
        // Debug.Log(this.transform.GetChild(2));
        // Debug.Log(this.transform.GetChild(3));
       
        GameObject childHeader;
        GameObject childSection;
        childHeader = this.transform.GetChild(0).gameObject;
        headerColor = childHeader.GetComponent<Image>().color;
        childSection = this.transform.GetChild(2).gameObject;
        sectionColor = childSection.GetComponent<Image>().color;
            
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hold)
        {
            OnBeginHold();
        }
        //this.transform.GetChild(1).gameObject.GetComponent<Image>().material.color = Color.white;
    }

    // ************ BEGIN Controller Methods for Compartmented Rectangle ****************//

    public void CreateHeader()
    {
        var header = GameObject.Instantiate(textbox, this.transform);
        //vector position will need to be obtained from transform of gameobject in the future
        header.transform.position = this.transform.position + new Vector3(headerOffsetX, headerOffsetY, 0);
        AddHeader(header);
    }

    public void CreateSection()
    {
        Vector3 oldPosition = this.transform.position + new Vector3(0, 18, 0);
        for (int i = 0; i < 2; i++)/*loop for a class with 2 sections*/
        {
            var sect = GameObject.Instantiate(section, this.transform);
            sect.transform.position = oldPosition + new Vector3(0, sectionOffsetY, 0) * sections.Count;
            AddSection(sect);
        }
        _diagram.AddAttributesToSection(GetSection(0));//add atrributes to first section
    }

    public void OnBeginHold()
    {
        this.hold = true;
        holdTimer += Time.deltaTime;
        _prevPosition = this.transform.position;
    }

    public void OnEndHold()
    {
        if (holdTimer > 1f - 5 && state == State.Default)
        {
            SpawnPopupMenu();
        }
        holdTimer = 0;
        this.hold = false;
        toggleHighlight();
        
        //update class position
        _diagram.UpdateClass(this.textbox, this.gameObject);

    }

    void SpawnPopupMenu()
    {
        if (this.popupMenu.GetComponent<PopupMenu>().GetCompartmentedRectangle() == null)
        {
            this.popupMenu = GameObject.Instantiate(this.popupMenu);
            this.popupMenu.transform.position = this.transform.position + new Vector3(popupMenuOffsetX, 0, 0);
            this.popupMenu.GetComponent<PopupMenu>().SetCompartmentedRectangle(this);
            this.popupMenu.GetComponent<PopupMenu>().Open();
        }
        else
        {
            this.popupMenu.GetComponent<PopupMenu>().Open();
        }
    }

    /// <summary>
    /// Destroy class when click on delete class.
    /// </summary>
    public void Destroy()
    {
        _diagram.DeleteClass(this.gameObject);
        this.popupMenu.GetComponent<PopupMenu>().Destroy();
    }

    // ************ END Controller Methods for Compartmented Rectangle ****************//

    // ************ BEGIN UI model Methods for Compartmented Rectangle ****************//
    public bool AddSection(GameObject aSection)
    {
        if (sections.Contains(aSection))
        {
            return false;
        }
        sections.Add(aSection);
        aSection.GetComponent<Section>().SetCompartmentedRectangle(this.gameObject);
        return true;
    }

    public GameObject GetSection(int index)
    {
        if (index >= 0 && index < sections.Count)
        {
            return this.sections[index];
        }
        else
        {
            return null;
        }
    }

    public Vector2 GetPosition()
    {
        return this.transform.position;
    }

    public override List<Vector2> GetConnectionPointsLocations()
    {
        // int debug_count = 0;
        // foreach (bool avail in connectionPointsAvailable)
        // {
        //     Debug.Log("avail " + debug_count++ + " :" + avail);
        // }
        List<Vector2> locations = new List<Vector2>();
        var origin = this.GetPosition() + new Vector2(-rectHeight/2, -rectWidth/2);
        var increment = (rectWidth / (NumOfConnectionPoints / 4));
        // int count = 0;
        for (float x = 0; x <= rectWidth; x += increment)
        {
            for (float y = rectHeight; y >= 0; y -= increment)
            {
                if ((y > 0 && y < rectHeight) && (x > 0 && x < rectWidth))
                {
                    continue;
                } 
                locations.Add(origin + new Vector2(x,y));
                // count++;
            }
        }
        return locations;
    }

    public GameObject GetPopUpMenu()
    {
        return this.popupMenu;
    }
    
    public void SetLine()
    {
        Debug.Log("set line heard");
        _diagram.gameObject.GetComponent<DrawLine>().AddCompartmentedRectangle(this.gameObject);
        this.popupMenu.GetComponent<PopupMenu>().Close(); // close the popup menu
    }

    public void toggleHighlight(){
        if(isHighlighted){
            isHighlighted = false;
            this.transform.GetChild(1).gameObject.GetComponent<Image>().color = headerColor;
            this.transform.GetChild(3).gameObject.GetComponent<Image>().color = sectionColor;
            this.transform.GetChild(4).gameObject.GetComponent<Image>().color = sectionColor;
            //restore color
        }
        else
        {
            isHighlighted = true;
            Color newHeader = headerColor;
            Color newSection = sectionColor;
            newHeader = (newHeader + highlight)/2;
            newSection = (newSection + highlight)/2;
            this.transform.GetChild(1).gameObject.GetComponent<Image>().color = newHeader;
            this.transform.GetChild(3).gameObject.GetComponent<Image>().color = newSection;
            this.transform.GetChild(4).gameObject.GetComponent<Image>().color = newSection;
            

        }
    }
    
    // ************ END UI model Methods for Compartmented Rectangle ****************//

}
