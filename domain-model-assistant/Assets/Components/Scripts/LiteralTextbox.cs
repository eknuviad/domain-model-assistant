using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LiteralTextbox : TextBox
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
    //public int TypeId { get; set; }

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    void Start() {
        GetComponent<InputField>().onSubmit.AddListener(e =>
    {
        if (GetComponent<InputField>().isFocused)
        {
            addLiteral();
        }

    });

    }

    void Update()
    {
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
        /*string text = GetComponent<InputField>().text;
        //check that inputfield is of a particular format (Eg. int year)
        string[] values = text.Split(' ');
        Debug.Log(values.Length);
        if (values.Length == 2 && !string.IsNullOrWhiteSpace(values[1]))
        {
            Debug.Log("second element is: " + values[1]);
            //SetTypeId(values[0].ToLower());
            Name = values[1];
            return true;
        }*/
        return false;
    }

    

    

    public void addLiteral(){
            Name =GetComponent<InputField>().text;
        
            //Debug.Log("in literal Textbox, name:"+Name);
            string _id = ID;
            
                Debug.Log(Section.GetComponent<Section>()
                    .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>().ID);
                WebCore.AddLiteral(gameObject);
                _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Info("Literal added");
                //WebCore.AddOperation(gameObject);
            
        
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


