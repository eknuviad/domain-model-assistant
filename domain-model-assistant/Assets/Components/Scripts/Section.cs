using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Section : MonoBehaviour
{

    public GameObject compRect;
    public const int UpdatePositionConst = -10;
    public GameObject textbox;
    public GameObject LiteralTextbox;
    public List<GameObject> textboxes = new();

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    // ************ UI model Methods for Section ****************//

    public bool SetCompartmentedRectangle(GameObject aCompRect)
    {
        if (aCompRect == null)
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
    public bool AddTextBox(GameObject textbox)
    {
        if (textboxes.Contains(textbox))
        {
            return false;
        }
        textboxes.Add(textbox);
        textbox.GetComponent<AttributeTextBox>().Section = this.gameObject;
        return true;
    }

    public bool AddLiteralTextBox(GameObject textbox)
    {
        if (textboxes.Contains(textbox))
        {
            return false;
        }
        textboxes.Add(textbox);
        textbox.GetComponent<LiteralTextbox>().Section = this.gameObject;
        return true;
    }

    public List<GameObject> GetTextBoxList()
    {
        return textboxes;
    }

    public GameObject GetTextBox(int index)
    {
        if (index >= 0 && index < textboxes.Capacity - 1)
        {
            return this.textboxes[index];
        }
        else
        {
            return null;
        }
    }

    


    // Used when creating attributes from popup menu
    public void AddAttribute()
    {
        // cap (hardcode) the number of attributes that can be added to a class to be 4
        if (textboxes.Count >= 1)
        {
            EnlargeCompRectAndRepositionSections();
            Canvas.ForceUpdateCanvases();
        }
        var TB = GameObject.Instantiate(textbox, this.transform);
        TB.GetComponent<TextBox>().ID = "-1";
        TB.GetComponent<InputField>().text = "Enter Text ...";
        TB.transform.position = this.transform.position + new Vector3(0, UpdatePositionConst, 0) * textboxes.Count;
        // Update size of class depending on number of textboxes(attributes)
        // enlarge the section by 0.1*number of textboxes
        // TB.transform.localScale += new Vector3(0, 0.1F * textboxes.Count, 0);
        // the code commented below can automatically enlarge the section as we create more attributes, 
        // but it would cause the new textboxes created become blured/disappeared as more than 4 attribute are created
        //this.GetCompartmentedRectangle().transform.localScale += new Vector3((float)0.2,(float)0.5, 0);
        //this.GetComponent<Section>().GetCompartmentedRectangle().transform.localScale +=  new Vector3(0,(float)0.5,0);
        this.AddTextBox(TB);

        // close the popup menu
        if (this.compRect != null)
        {
            this.compRect.GetComponent<CompartmentedRectangle>().GetPopUpMenu().GetComponent<PopupMenu>().Close();
        }
    }

    // Used when creating attribute after reading JSON from the WebCORE server
    public void AddAttribute(string _id, string name, string type)
    {
        if (textboxes.Count >= 1)
        {
            EnlargeCompRectAndRepositionSections();
            Canvas.ForceUpdateCanvases();
        }
        var TB = GameObject.Instantiate(textbox, this.transform);
        TB.GetComponent<TextBox>().ID = _id;
        TB.GetComponent<InputField>().text = type + " " + name;
        TB.transform.position = this.transform.position + new Vector3(0, UpdatePositionConst, 0) * textboxes.Count;
        this.AddTextBox(TB);
    }


    // Used when creating attributes from popup menu
    public void AddLiteral()
    {
        // cap (hardcode) the number of attributes that can be added to a class to be 4
        if (textboxes.Count >= 1)
        {
            EnlargeCompRectAndRepositionSections();
            Canvas.ForceUpdateCanvases();
        }
        var TB = GameObject.Instantiate(LiteralTextbox, this.transform);
        TB.GetComponent<TextBox>().ID = "-1";
        TB.GetComponent<InputField>().text = "Enter Text ...";
        TB.transform.position = this.transform.position + new Vector3(0, UpdatePositionConst, 0) * textboxes.Count;
        // Update size of class depending on number of textboxes(attributes)
        // enlarge the section by 0.1*number of textboxes
        TB.transform.localScale += new Vector3(0, 0.1F * textboxes.Count, 0);
        this.AddLiteralTextBox(TB);

        // close the popup menu
        if (this.compRect != null)
        {
            this.compRect.GetComponent<CompartmentedRectangle>().GetPopUpMenuEnum().GetComponent<PopupMenuEnum>().Close();
        }
    }

    // Used when creating attribute after reading JSON from the WebCORE server
    public void AddLiteral(string _id, string name)
    {
        if (textboxes.Count >= 1)
        {
            EnlargeCompRectAndRepositionSections();
            Canvas.ForceUpdateCanvases();
        }
        var TB = GameObject.Instantiate(LiteralTextbox, this.transform);
        TB.GetComponent<TextBox>().ID = _id;
        TB.GetComponent<InputField>().text = name;
        TB.transform.position = this.transform.position + new Vector3(0, UpdatePositionConst, 0) * textboxes.Count;
        this.AddLiteralTextBox(TB);
    }

    private void EnlargeCompRectAndRepositionSections()
    {
        RectTransform rt_sec0 = (RectTransform) gameObject.GetComponent<Section>().transform;
        rt_sec0.sizeDelta = new Vector2 (rt_sec0.sizeDelta.x, rt_sec0.sizeDelta.y+20);
        rt_sec0.anchoredPosition = rt_sec0.anchoredPosition + new Vector2(0, -10);

        if(!compRect.GetComponent<CompartmentedRectangle>().isEnum){
            RectTransform rt_sec1 = (RectTransform) compRect.GetComponent<CompartmentedRectangle>().GetSection(1).GetComponent<Section>().transform;
            rt_sec1.anchoredPosition = rt_sec1.anchoredPosition + new Vector2(0, -20);       
        }

        RectTransform compRect_rt = (RectTransform) compRect.GetComponent<CompartmentedRectangle>().transform;
        compRect_rt.sizeDelta = new Vector2 (compRect_rt.sizeDelta.x, compRect_rt.sizeDelta.y+20);
    }

}



