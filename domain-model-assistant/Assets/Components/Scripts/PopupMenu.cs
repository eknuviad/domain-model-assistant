using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject associatedObject;

    void Start()
    {
        
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetAssociatedObject(ClassDiagram classDiagram)
    {
        this.associatedObject = classDiagram.gameObject;
        this.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(classDiagram.AddAttribute);
        this.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(classDiagram.AddSubclass);
        this.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(classDiagram.Destroy);

     public void SetCompartmentRectangle(CompartmentedRectangle compRect)
    {
        this.associatedObject = compRect.gameObject;
        Debug.Log("here");
        // this.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(compRect.addTextbox);
        // this.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(compRect.addSection);
    }

   
    public void Destroy()
    {
        //kind of like a reset instance
        // this.gameObject = GameObject.Instantiate(popupobj);
        // Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
