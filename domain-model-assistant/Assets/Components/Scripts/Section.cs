using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Section : MonoBehaviour
{

    public GameObject compRect;
    
    public GameObject textB;
    public List<GameObject> textBList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //TODO Update size of class depending on number of textboxes(attributes)
    }

    // ************ UI model Methods for Section ****************//

    public bool SetCompartmentedRectangle(GameObject aCompRect)
    {
        if(aCompRect == null)
        {
            return false;
        }
        compRect = aCompRect;
        return true;
    }

    public GameObject GetCompartmentedRectangle()
    {
        return compRect;
    }

    // Get, Set for TextBox
    public bool AddTextBox(GameObject TB)
    {
        if (textBList.Contains(TB))
        {
            return false;
        }
        textBList.Add(TB);
        TB.GetComponent<TextBox>().SetSection(this.gameObject);
        return true;
    }

    public GameObject GetTextBox(int index){
        if(index >= 0 && index < textBList.Capacity -1){
            return this.textBList[index];
        }else{
            return null;
        }
    }

    public void AddAttribute()
    {
        var TB = GameObject.Instantiate(textB, this.transform);
        TB.GetComponent<InputField>().text = "Enter Text ...";
        TB.transform.position = this.transform.position + new Vector3(0, -10, 0)*textBList.Count;
        this.AddTextBox(TB);
    }

    public void AddAttribute(string _id, string name, string type){
        var TB = GameObject.Instantiate(textB, this.transform);
        TB.GetComponent<TextBox>().ID = _id;
        TB.GetComponent<InputField>().text = type + " " + name;
        TB.transform.position = this.transform.position + new Vector3(0, -10, 0)*textBList.Count; 
        this.AddTextBox(TB);
    }

}
