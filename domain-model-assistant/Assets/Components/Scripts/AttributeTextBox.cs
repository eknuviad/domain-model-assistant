using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AttributeTextBox : TextBox
{
    private GameObject _section;
    public GameObject Section
    {
        get => _section;
        set
        {
            if (value != null)
            {
                _section = value;
            }
        }
    }
    public GameObject attributeCross;
    public string Name { get; set; } //second substring of attribute
    public int TypeId { get; set; }

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    void Start() {}

    void Update()
    {

        /*if(GetComponent<InputField>().isFocused){
                Debug.Log("is focused");
            }
            if(Input.GetKeyDown(KeyCode.Return)){
                Debug.Log("TextBox: Enter button pressed， intextbox");}
        */

        if (Input.GetKey(KeyCode.Return)&&GetComponent<InputField>().isFocused)
        {
            Debug.Log("TextBox: Enter button pressed， intextbox");
            string _id = ID;
            if (IsValid() )//&& "-1".Equals(_id))
            {
                Debug.Log(Section.GetComponent<Section>()
                    .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>().ID);
                WebCore.AddAttribute(gameObject);
                _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Info("Attribute added");
                //WebCore.AddOperation(gameObject);
            }else{
                _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Warn("Attribute format error, example: int age");
                GetComponent<InputField>().text = " ";
            }
        }
            //Debug.Log("TextBox: Enter button pressed， intextbox");
            /*
            string _id = ID;
            if (IsValid() )//&& "-1".Equals(_id))
            {
                Debug.Log(Section.GetComponent<Section>()
                    .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>().ID);
                WebCore.AddAttribute(gameObject);
                _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Info("Attribute added");
                //WebCore.AddOperation(gameObject);
            }else{
                _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Warn("Attribute format error, example: int age");
                GetComponent<InputField>().text = " ";
            }*/
        


        if (hold2)
        {
            OnBeginHoldTB();
        }
        text = GetComponent<Text>();
        if (IsHighlightedtext)
        {
            text.color = Color.red;
        }
    }

    public override bool IsValid()
    {
        string text = GetComponent<InputField>().text;
        //check that inputfield is of a particular format (Eg. int year)
        string[] values = text.Split(' ');
        Debug.Log(values.Length);
        if (values.Length == 2 && !string.IsNullOrWhiteSpace(values[1]))
        {
            Debug.Log("second element is: " + values[1]);
            SetTypeId(values[0]);
            Name = values[1];
            return true;
        }
        return false;
    }

    /// <summary>
    /// Sets the type ID of the attribute. This is a separate method because it has a string input.
    /// </summary>
    public void SetTypeId(string value)
    {
        foreach (var item in _diagram.GetAttrTypeIdsToTypes())
        {
            if (item.Value.Equals(value))
            {
                if (!string.IsNullOrEmpty(item.Key))
                {
                    TypeId = int.Parse(item.Key);
                }
                break;
            }
        }
    }

    public override void OnEndHoldTB()
    {
        // TODO Don't spawn popup if class is being dragged
        if (holdTimer2 > 2f /*&& Vector2.Distance(transform.position, _prevPosition) < 0.1f*/ )
        {
            if (Section != null)
            {
                SpawnAttributeCross();
            }
        }
    }

    void SpawnAttributeCross()
    {
        if (attributeCross.GetComponent<AttributeCross>().GetTextBox() == null)
        {
            attributeCross = GameObject.Instantiate(attributeCross);
            attributeCross.transform.position = transform.position + new Vector3(UpdatePositionConst, 0, 0);
            attributeCross.GetComponent<AttributeCross>().setTextBox(this);
            attributeCross.GetComponent<AttributeCross>().Open();
        }
        else
        {
            attributeCross.GetComponent<AttributeCross>().Open();
        }
    }

    public void Destroy()
    {
        WebCore.DeleteAttribute(gameObject); //delete attribute from Diagram
        attributeCross.GetComponent<AttributeCross>().Close();
        Destroy(gameObject);
    }

    public GameObject GetAttributeCross()
    {
        return attributeCross;
    }

}
