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
    
    public GameObject edgeEndTitle;
    public GameObject edgeEndNumber;
    public GameObject textbox;
    // public GameObject popupMenu;
    void Start()
    {
        // this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        // edgeEndTitle = GameObject.Instantiate(textbox, this.transform);
        edgeEndTitle.GetComponent<InputField>().text = "Enter Text ...";
        // edgeEndNumber = GameObject.Instantiate(textbox, this.transform);
        // edgeEndNumber.transform.position += new Vector3(0,20,0);
        Debug.Log("instatntiate");
    }

    void Update()
    {
     
    }


    void Destroy(){
         Destroy(this.gameObject);
    }

}
