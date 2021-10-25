using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeCross : MonoBehaviour
{
    GameObject textb; //textbox
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setTextBox(TextBox AttriBute){
        this.textb = AttriBute.gameObject;
        this.transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(AttriBute.Destroy);
        //this.transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(AttriBute.Destroy);

    }

    public GameObject getTextBox(){
        return this.textb;
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
