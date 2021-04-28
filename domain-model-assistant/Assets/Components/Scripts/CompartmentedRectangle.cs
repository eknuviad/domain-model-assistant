using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompartmentedRectangle : Node
{
    
    public GameObject textbox;
    public GameObject section;
    public List<GameObject> sections;
    
    // popup menu variables
    public GameObject popupMenu;
    float hold_timer = 0;
    bool hold = false;

    void Awake(){
        
    }
    
    // Start is called before the first frame update
    void Start(){
        CreateHeader();
        CreateSection();
    }

    // Update is called once per frame
    void Update(){
        if (this.hold){
            OnBeginHold();
        }
    }



// ************ BEGIN Controller Methods for Compartmented Rectangle ****************//
    public void CreateHeader(){
        GameObject header = GameObject.Instantiate(textbox, this.transform);
        //vector position will need to be obtained from transform of gameobject in the 
        //future
        header.transform.position = this.transform.position + new Vector3(0,45,0);
        addHeader(header);
    }
    
    public void CreateSection(){
        Vector3 oldPosition = this.transform.position;
        for (int i = 0; i<2; i++){
            Debug.Log(oldPosition);
            GameObject sect = GameObject.Instantiate(section, this.transform);
            sect.transform.position = oldPosition;
            // at the moment vector positions are hardcoded but will need to be obtained
            //from the transform of the gameobject
            oldPosition += new Vector3(0, - 46, 0);
            Debug.Log(oldPosition);
            addSection(sect);
        }
    }

    public void OnBeginHold(){
        this.hold = true;
        hold_timer += Time.deltaTime;
    }

    public void OnEndHold(){
        if(hold_timer > 1f - 5){
            SpawnPopupMenu();
        }
        hold_timer = 0;
        this.hold = false;
    }

     void SpawnPopupMenu(){
        if(this.popupMenu.GetComponent<PopupMenu>().getCompartmentedRectangle() == null){
            this.popupMenu = GameObject.Instantiate(this.popupMenu);
            this.popupMenu.transform.position = this.transform.position + new Vector3(100, 0, 0);
            this.popupMenu.GetComponent<PopupMenu>().setCompartmentedRectangle(this);
        }else{
            this.popupMenu.GetComponent<PopupMenu>().Open(); 
        } 
    }
    public void Destroy() //destroy class diagram when click on delete class
    {
        this.popupMenu.GetComponent<PopupMenu>().Destroy(); 
        Destroy(this.gameObject);
    }

// ************ END Controller Methods for Compartmented Rectangle ****************//

// ************ BEGIN UI model Methods for Compartmented Rectangle ****************//
    public bool addSection(GameObject aSection){
        bool wasSet = false;
        if(sections.Contains(aSection)){
            return false;
        }
        sections.Add(aSection);
        aSection.GetComponent<Section>().setCompartmentedRectangle(this.gameObject);
        Debug.Log("Section added to list of sections for this compartmented rectangles");
        wasSet = true;
        return wasSet;
    }

// ************ END UI model Methods for Compartmented Rectangle ****************//

}
