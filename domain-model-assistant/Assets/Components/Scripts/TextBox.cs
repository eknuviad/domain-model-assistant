﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TextBox : MonoBehaviour
{
    private Diagram _diagram;
    public Text text;
    
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
    
    public const int UpdatePositionConst = -100;

    public string ID { get; set; }

    public bool IsHighlightedtext { get; set; }

    public string Name { get; set; } //second substring of attribute

    public int TypeId { get; set; }

    // public bool isChecked;

    float holdTimer2 = 0;
    bool hold2 = false;

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    void Start() {}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("TextBox: Enter button pressed");
            string _id = ID;
            if (IsValid() && "-1".Equals(_id))
            {
                Debug.Log(Section.GetComponent<Section>()
                    .GetCompartmentedRectangle().GetComponent<CompartmentedRectangle>().ID);
                _diagram.AddAttribute(this.gameObject);
            }
        }

        if (this.hold2)
        {
            OnBeginHoldTB();
        }
        text = this.GetComponent<Text>();
        if (IsHighlightedtext)
        {
            text.color = Color.red;
        }
    }

    public bool IsValid()
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
            if (Section != null)
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
