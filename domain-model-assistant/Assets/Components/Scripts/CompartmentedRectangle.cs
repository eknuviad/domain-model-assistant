using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompartmentedRectangle : MonoBehaviour, BaseComponent
{
    // class attributes
    public string ID{get; set;}
    float hold_timer = 0;
    bool hold = false;
    public GameObject popup_menu;
    public GameObject texbox;
    public GameObject section;
    // List<GameObject> sections = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
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
         if(hold_timer > 1f)
        {
            SpawnPopupMenu();
        }
        hold_timer = 0;
        this.hold = false;
    }
     public void SpawnPopupMenu()
    {
        Debug.Log("Popup menu here");  
        this.popup_menu = GameObject.Instantiate(this.popup_menu);
        this.popup_menu.transform.position = this.transform.position + new Vector3(100, 0, 0);
        // this.popup_menu.GetComponent<PopupMenu>().SetAssociatedObject(this);
    }

    public void setTextbox(){
        // this.texbox = GameObject.Instantiate(this.textbox);
    }

    public void setSection(){
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


}
