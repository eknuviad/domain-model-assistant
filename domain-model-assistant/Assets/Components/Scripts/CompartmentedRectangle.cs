using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompartmentedRectangle : Node
{
   
    // private GameObject popup_menu;
    public GameObject popupMenu;
    public GameObject textbox;
    public GameObject section;
    public List<GameObject> sections;
    public string ID{
        get{
            return ID;
        }
        set{
            ID = value;
        }
    }

    // Start is called before the first frame update
    void Start(){
        CreateHeader();
        CreateSection();
    }

    // Update is called once per frame
    void Update(){
     
    }



// ************ Controller Methods for Compartmented Rectangle ****************//

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


// ************ UI model Methods for Compartmented Rectangle ****************//

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


}
