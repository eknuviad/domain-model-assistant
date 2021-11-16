using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextBox : MonoBehaviour
{
    private Diagram _diagram;
    public Text text;
    public GameObject sect;
    public GameObject attribcross;
    public string ID
    { get; set; }
    public bool isHighlightedtext
    { get; set; }
    public string name; //second substring of attribute

    public int typeId;
    
    // public bool isChecked;
    
    float holdTimer2 = 0;
    bool hold2 = false;

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("Enter");
            string _id = this.ID;
             if(isValid() && _id.Equals("-1")){
            // _diagram.AddAttribute via post request
                Debug.Log(this.GetSection().GetComponent<Section>()
                .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>()
                .ID);
                // GameObject compRect = this.GetSection().GetComponent<Section>()
                // .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>();
                _diagram.AddAttribute(this.gameObject);
            }
        }
        
        if (this.hold2)
        {
            OnBeginHold2();
        }
        text = this.GetComponent<Text>();
        if (isHighlightedtext == true)
        {
            text.color = Color.red;
        }
    }

    public bool isValid(){
        bool res = false;
        string text = this.GetComponent<InputField>().text;
        //check that id is not in diagram.getcreatedattributes
        // if(_diagram.getCreatedAttributes().Contains(this.ID)){
        //     return res = false;
        // }
        //check that inputfield in not null
        // if( text == null ){
        //     return res = false;
        // }
        //check that inputfield is of a particular format (int year true)
        string[] values = text.Split(' ');
        Debug.Log(values.Length);
        if (values.Length == 2 && !String.IsNullOrWhiteSpace(values[1]))
        {
            // if(!values[1].Equals(' ')
            Debug.Log("second element is: " + values[1]);
            this.SetTypeId(values[0]);
            this.SetName(values[1]);
            return res = true;
        } 
        else 
        {
            return res = false;
        }
    }

    public void SetName(string aName)
    {
        this.name = aName;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetTypeId (string value)
    {
        Dictionary<string, string> tmpDict = _diagram.getAttrTypeIdsToTypes();
        string tmpTypeId = null;
        foreach(var item in tmpDict)
        {
            if (item.Value.Equals(value))
            {
                tmpTypeId = item.Key;
                break;
            }
        }
        if (!String.IsNullOrEmpty(tmpTypeId))
        {
            this.typeId = Int16.Parse(tmpTypeId);
        } 
    }

    public int GetTypeId()
    {
        return this.typeId;
    }
        
    public bool SetSection(GameObject sSection)
    {
        if (sSection == null)
        {
            return false;
        }
        sect = sSection;
        return true;
    }

    public GameObject GetSection()
    {
        return sect;
    }

    public void OnBeginHold2()
    {
        this.hold2 = true;
        holdTimer2 += Time.deltaTime;
    }

    public void OnEndHold2()
    {
        // TODO Don't spawn popup if class is being dragged
        if (holdTimer2 > 1f - 5 /*&& Vector2.Distance(this.transform.position, _prevPosition) < 0.1f*/ )
        {
            if (this.GetSection() != null)
            {
                SpawnAttributeCross();
            }

        }
    }

    void SpawnAttributeCross()
    {
        if (this.attribcross.GetComponent<AttributeCross>().GetTextBox() == null)
        {
            this.attribcross = GameObject.Instantiate(this.attribcross);
            this.attribcross.transform.position = this.transform.position + new Vector3(-100, 0, 0);
            this.attribcross.GetComponent<AttributeCross>().setTextBox(this);
        }
        else
        {
            this.attribcross.GetComponent<AttributeCross>().Open();
        }
    }

    public void Destroy()
    {
        this.attribcross.GetComponent<AttributeCross>().Close();
        Destroy(this.gameObject);
    }

    public GameObject GetAttributeCross(){
        return this.attribcross;
    }

}
