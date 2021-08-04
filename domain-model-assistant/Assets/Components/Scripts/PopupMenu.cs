using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour
{
    
    GameObject compRect;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {}

    public void setCompartmentedRectangle(CompartmentedRectangle aCompRect){
        this.compRect = aCompRect.gameObject;
        // NB: we'll need to properly implement these two methods based on UI model diagram
        // this.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(aCompRect.AddAttribute);
        // this.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(aCompRect.AddSubclass);
        this.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(aCompRect.Destroy);
    }
    public GameObject getCompartmentedRectangle(){
        return this.compRect;
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
