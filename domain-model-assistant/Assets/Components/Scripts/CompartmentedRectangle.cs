using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompartmentedRectangle : Node
{
    
    public GameObject textbox;
    public GameObject section;
    public List<GameObject> sections = new List<GameObject>();
    
    // popup menu variables
    public GameObject popupMenu;
    float holdTimer = 0;
    bool hold = false;

    private Diagram _diagram;

    private Vector2 _prevPosition;

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
            _diagram.UpdateClass(this.textbox, this.gameObject);
        }
        
    }

    // ************ BEGIN Controller Methods for Compartmented Rectangle ****************//

    public void CreateHeader()
    {
        var header = GameObject.Instantiate(textbox, this.transform);
        //vector position will need to be obtained from transform of gameobject in the future
        header.transform.position = this.transform.position + new Vector3(0, 45, 0);
        AddHeader(header);
    }
    
    public void CreateSection()
    {
        Vector3 oldPosition = this.transform.position;
        for (int i = 0; i < 2; i++)
        {
            var sect = GameObject.Instantiate(section, this.transform);
            sect.transform.position = oldPosition;
            // At the moment vector positions are hardcoded but will need to be obtained
            // from the transform of the gameobject
            oldPosition += new Vector3(0, -46, 0);
            AddSection(sect);
        }
    }

    public void OnBeginHold()
    {
        this.hold = true;
        holdTimer += Time.deltaTime;
        _prevPosition = this.transform.position;
    }

    public void OnEndHold()
    {
        // TODO Don't spawn popup if class is being dragged
        if(holdTimer > 1f - 5 /*&& Vector2.Distance(this.transform.position, _prevPosition) < 0.1f*/)
        {
            SpawnPopupMenu();
        }
        holdTimer = 0;
        this.hold = false;
    }

    void SpawnPopupMenu()
    {
        if (this.popupMenu.GetComponent<PopupMenu>().getCompartmentedRectangle() == null)
        {
            this.popupMenu = GameObject.Instantiate(this.popupMenu);
            this.popupMenu.transform.position = this.transform.position + new Vector3(100, 0, 0);
            this.popupMenu.GetComponent<PopupMenu>().setCompartmentedRectangle(this);
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

    public Vector3 GetPosition(){
        return this.transform.position;
    }
    // ************ END UI model Methods for Compartmented Rectangle ****************//

}
