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
    float holdTimer = 0;
    bool hold = false;
    public State state = State.Default;
    private Diagram _diagram;
    private Vector2 _prevPosition;
    private int headerOffsetX = -31;
    private int headerOffsetY = 70;
    private int sectionOffsetY = -71;
    private int popupMenuOffsetX = 100;
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
        CreateHeader();
        CreateSection();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hold)
        {
            OnBeginHold();
        }
        if (isHighlighted)
        {
            GameObject child = this.transform.GetChild(1).gameObject;
            child.GetComponent<Image>().color = Color.yellow; // or change to light blue
        }
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

        //update class position
        _diagram.UpdateClass(this.textbox, this.gameObject);

    }
    void SpawnPopupMenu()
    {
        if (this.popupMenu.GetComponent<PopupMenu>().getCompartmentedRectangle() == null)
        {
            this.popupMenu = GameObject.Instantiate(this.popupMenu);
            this.popupMenu.transform.position = this.transform.position + new Vector3(popupMenuOffsetX, 0, 0);
            this.popupMenu.GetComponent<PopupMenu>().SetCompartmentedRectangle(this);
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

    public GameObject GetPopUpMenu()
    {
        return this.popupMenu;
    }

    public void SetState(State aState)
    {
        this.state = aState;
    }

    public State GetState()
    {
        return state;
    }
    // ************ END UI model Methods for Compartmented Rectangle ****************//

}
