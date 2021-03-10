using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompartmentedRectangle : MonoBehaviour, BaseComponent
{
    // class attributes
    static Vector3 savedPosition = Vector3.zero;
    static GameObject compRect;
    public string ID{get; set;}
    float hold_timer = 0;
    bool hold = false;
    private GameObject popup_menu;
    public GameObject popup_menu_prefab;
    public GameObject textbox_prefab;
    private GameObject textbox;
    // public GameObject section;
    // List<GameObject> sections = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        savedPosition = getPosition();
        compRect = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.hold)
        {
            OnBeginHold();
        }
    }



// ************ Support Methods for Compartmented Rectangle ****************//
    public void OnBeginHold(){
        this.hold = true;
        hold_timer += Time.deltaTime;
    }

    public void OnEndHold(){
         if(hold_timer > 1f - 5)
        {
            SpawnPopupMenu();
        }
        hold_timer = 0;
        this.hold = false;
    }
     public void SpawnPopupMenu()
    {
        this.popup_menu = GameObject.Instantiate(this.popup_menu_prefab);
        this.popup_menu.transform.position = savedPosition + new Vector3(100,0,0);
        this.popup_menu.GetComponent<PopupMenu>().SetCompartmentRectangle(this);
        Debug.Log("Displaying popup menu");
    }

    public void addTextbox(){
        textbox = GameObject.Instantiate(textbox_prefab, compRect.transform);
        Debug.Log("Add Textbox");
    }

    public void addSection(){
        // if(sections.Count() == 2){
        //   Debug.Log("No more sections can be added");  
        // }else{
        //     this.section = GameObject.Instantiate(this.section);
        //     this.sections.Add(this.section);
        // }
    }
    // public GameObject getSection(int index){
    //     // return this.sections.ElementAt(index);
    //     return 0;
    // }

    public Vector3 getPosition(){
        return this.transform.position;
    }

}
