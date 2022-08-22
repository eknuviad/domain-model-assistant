using System.Drawing;
//using System.Diagnostics;
//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

public class CompartmentedRectangle : Node
{
    public GameObject textbox; //this allows to instantiate textbox prefabs
    public GameObject section;
    public List<GameObject> sections = new();
    public TextBox text;
    // popup menu variables
    public GameObject popupMenu;
    public RectTransform rectangle;
    
    public Color highlight = new(0.61f, 0.81f, 0.973f);
    public Color headerColor; // = transform.GetChild(1).gameObject.GetComponent<Image>().color;
    public Color sectionColor;
    float holdTimer = 0;
    bool hold = false;
    public State state { get; set; }

    private Diagram _diagram;

    private string id; //should move to node class later
    private Vector2 _prevPosition;
    private float rectHeight;
    private float rectWidth;
    private int headerOffsetX = -31;
    private int headerOffsetY = 70;
    private int sectionOffsetY = -71;
    private int popupMenuOffsetX = 138;

    public bool IsHighlighted { get; set; }

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
        rectangle = (RectTransform)transform;
        rectHeight = rectangle.rect.height;
        rectWidth = rectangle.rect.width;
        CreateHeader();
        CreateSection();
        id = GetComponent<CompartmentedRectangle>().ID;

        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     var img = transform.GetChild(i).GetComponent<Image>();
        //     string color = img != null ? ("" + img.color) : "null";
        //     Debug.Log("CompRect transform child " + i + ": " + transform.GetChild(i) + " with image " + img
        //         + " and with color " + color);
        // }

        var headerIndex = transform.childCount - 4; // obtained by trial and error - make this more robust later
        var sectionIndex = headerIndex + 2;
        headerColor = transform.GetChild(headerIndex).GetComponent<Image>().color;
        sectionColor = transform.GetChild(sectionIndex).GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (hold)
        {
            OnBeginHold();
        }
        //transform.GetChild(1).gameObject.GetComponent<Image>().material.color = Color.white;
    }

    // ************ BEGIN Controller Methods for Compartmented Rectangle ****************//

    public void CreateHeader()
    {
        var header = GameObject.Instantiate(textbox, transform);
        //vector position will need to be obtained from transform of gameobject in the future
        header.transform.position = transform.position + new Vector3(headerOffsetX, headerOffsetY, 0);
        AddHeader(header);
    }

    public void CreateSection()
    {
        Vector3 oldPosition = transform.position + new Vector3(0, 18, 0);
        for (int i = 0; i < 2; i++) /*loop for a class with 2 sections*/
        {
            var sect = GameObject.Instantiate(section, transform);
            sect.transform.position = oldPosition + new Vector3(0, sectionOffsetY, 0) * sections.Count;
            AddSection(sect);
        }
        _diagram.AddAttributesToSection(GetSection(0));//add atrributes to first section
    }

    public void OnBeginHold()
    {
        hold = true;
        holdTimer += Time.deltaTime;
        _prevPosition = transform.position;
    }

    public void OnEndHold()
    {
        if (holdTimer > 1f - 5 && state == State.Default)
        {
            SpawnPopupMenu();
        }
        holdTimer = 0;
        hold = false;
        ToggleHighlight();
        
        //update class position
        _diagram.UpdateClassPosition(textbox, gameObject);

    }

    void SpawnPopupMenu()
    {
        if (popupMenu.GetComponent<PopupMenu>().GetCompartmentedRectangle() == null)
        {
            popupMenu = GameObject.Instantiate(popupMenu);
            popupMenu.transform.position = transform.position + new Vector3(popupMenuOffsetX, 0, 0);
            popupMenu.GetComponent<PopupMenu>().SetCompartmentedRectangle(this);
            popupMenu.GetComponent<PopupMenu>().Open();
        }
        else
        {
            popupMenu.GetComponent<PopupMenu>().Open();
        }
    }

    /// <summary>
    /// Destroy class when click on delete class.
    /// </summary>
    public void Destroy()
    {
        _diagram.DeleteClass(gameObject);
        popupMenu.GetComponent<PopupMenu>().Destroy();
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
        aSection.GetComponent<Section>().SetCompartmentedRectangle(gameObject);
        return true;
    }

    public GameObject GetSection(int index)
    {
        if (index >= 0 && index < sections.Count)
        {
            return sections[index];
        }
        else
        {
            return null;
        }
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public override List<Vector2> GetConnectionPointsLocations()
    {
        // int debug_count = 0;
        // foreach (bool avail in connectionPointsAvailable)
        // {
        //     Debug.Log("avail " + debug_count++ + " :" + avail);
        // }
        List<Vector2> locations = new();
        var origin = GetPosition() + new Vector2(-rectHeight/2, -rectWidth/2);
        var increment = rectWidth / (NumOfConnectionPoints / 4);
        // int count = 0;
        for (float x = 0; x <= rectWidth; x += increment)
        {
            for (float y = rectHeight; y >= 0; y -= increment)
            {
                if (y > 0 && y < rectHeight && x > 0 && x < rectWidth)
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
        return popupMenu;
    }
    
    public void SetLine()
    {
        Debug.Log("CompartmentedRectangle.SetLine() called");
        _diagram.gameObject.GetComponent<DrawLine>().AddCompartmentedRectangle(gameObject);
        popupMenu.GetComponent<PopupMenu>().Close(); // close the popup menu
    }

    public void ToggleHighlight()
    {
        if (IsHighlighted)
        {
            IsHighlighted = false;
            transform.GetChild(1).gameObject.GetComponent<Image>().color = headerColor;
            transform.GetChild(3).gameObject.GetComponent<Image>().color = sectionColor;
            transform.GetChild(4).gameObject.GetComponent<Image>().color = sectionColor;
            //restore color
        }
        else
        {
            IsHighlighted = true;
            Color newHeader = headerColor;
            Color newSection = sectionColor;
            newHeader = (newHeader + highlight)/2;
            newSection = (newSection + highlight)/2;
            transform.GetChild(1).gameObject.GetComponent<Image>().color = newHeader;
            transform.GetChild(3).gameObject.GetComponent<Image>().color = newSection;
            transform.GetChild(4).gameObject.GetComponent<Image>().color = newSection;
        }
    }
    
    // ************ END UI model Methods for Compartmented Rectangle ****************//

}
