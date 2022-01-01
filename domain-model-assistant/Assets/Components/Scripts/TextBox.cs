using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextBox : MonoBehaviour
{
    private Diagram _diagram;
    public Text text;
    public GameObject section;
    public GameObject attributeCross;
    public const int UpdatePositionConst = -100;
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Enter");
            string _id = this.ID;
            if (isValid() && _id.Equals("-1"))
            {
                Debug.Log(this.GetSection().GetComponent<Section>()
                .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>()
                .ID);
                _diagram.AddAttribute(this.gameObject);
            }
        }

        if (this.hold2)
        {
            OnBeginHoldTB();
        }
        text = this.GetComponent<Text>();
        if (isHighlightedtext == true)
        {
            text.color = Color.red;
        }
    }

    public bool isValid()
    {
        bool res = false;
        string text = this.GetComponent<InputField>().text;
        //check that inputfield is of a particular format (Eg. int year)
        string[] values = text.Split(' ');
        Debug.Log(values.Length);
        if (values.Length == 2 && !String.IsNullOrWhiteSpace(values[1]))
        {
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

    public void SetTypeId(string value)
    {
        Dictionary<string, string> tmpDict = _diagram.getAttrTypeIdsToTypes();
        string tmpTypeId = null;
        foreach (var item in tmpDict)
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
        
    public bool SetSection(GameObject section)
    {
        if (section == null)
        {
            return false;
        }
        this.section = section;
        return true;
    }

    public GameObject GetSection()
    {
        return this.section;
    }

    public void OnBeginHoldTB()
    {
        this.hold2 = true;
        holdTimer2 += Time.deltaTime;
    }

    public void OnEndHoldTB()
    {
        // TODO Don't spawn popup if class is being dragged
        if (holdTimer2 > 2f /*&& Vector2.Distance(this.transform.position, _prevPosition) < 0.1f*/ )
        {
            if (this.GetSection() != null)
            {
                SpawnAttributeCross();
            }
        }
    }

    void SpawnAttributeCross()
    {
        if (this.attributeCross.GetComponent<AttributeCross>().GetTextBox() == null)
        {
            this.attributeCross = GameObject.Instantiate(this.attributeCross);
            this.attributeCross.transform.position = this.transform.position + new Vector3(UpdatePositionConst, 0, 0);
            this.attributeCross.GetComponent<AttributeCross>().setTextBox(this);
            this.attributeCross.GetComponent<AttributeCross>().Open();
        }
        else
        {
            this.attributeCross.GetComponent<AttributeCross>().Open();
        }
    }

    public void Destroy()
    {
        _diagram.DeleteAttribute(this.gameObject); //delete attribute from Diagram
        this.attributeCross.GetComponent<AttributeCross>().Close();
        Destroy(this.gameObject);
    }

    public GameObject GetAttributeCross()
    {
        return this.attributeCross;
    }

}
