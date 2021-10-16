using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{

    public GameObject compRect;
    
    public GameObject textB;
    public List<GameObject> textBList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}

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
        TB.GetComponent<TextBox>().SetText("Enter Attribute");
        TB.transform.position = this.transform.position; 
        this.AddTextBox(TB);
    }
}
