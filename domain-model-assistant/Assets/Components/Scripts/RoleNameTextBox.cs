using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoleNameTextBox : TextBox
{
    private GameObject _titleOwner;

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    void Start() {
        GetComponent<InputField>().onSubmit.AddListener(e =>
        {
            if (GetComponent<InputField>().isFocused)
            {
                // addMultiplicity();
            }

        });
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Return))
        // {
        //     Debug.Log("TextBox: Enter button pressed");
        //     string _id = ID;
        //     if (IsValid())
        //     {
        //         // Debug.Log(EdgeEnd.GetComponent<EdgeEnd>().ID);
        //         if(GetTitleOwner().GetComponent<EdgeEnd>() == null)
        //         {
        //             Debug.Log("edgeEnd component is null");
        //         }
        //         WebCore.UpdateRelationshipMultiplicities(gameObject);
        //     }
        // }

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
        bool isValid = false;
        string text = GetComponent<InputField>().text;
        //check that inputfield is of a particular format (Eg. int year)
        string[] values = text.Split("..");

        Debug.Log(values.Length);
        
        if(values.Length == 1)
        {
            // check if the input field is a number or '*'
            isValid = true;
        }

        return isValid; 
    }

    public override void OnEndHoldTB()
    {
        // TODO Don't spawn popup if class is being dragged
        if (holdTimer2 > 2f /*&& Vector2.Distance(transform.position, _prevPosition) < 0.1f*/ )
        {
        }
    }

    public void Destroy()
    {
        //TODO Delete Multiplicity??
        // WebCore.DeleteAttribute(gameObject); //delete attribute from Diagram
        Destroy(gameObject);
    }

    public GameObject GetTitleOwner()
    {
        return _titleOwner;
    }

    public bool SetTitleOwner(GameObject aNewTitleOwner)
    {
        bool wasSet = false;
        if (aNewTitleOwner == null)
        {
            GameObject existingTitleOwner = _titleOwner;
            _titleOwner = null;

            if (existingTitleOwner != null && existingTitleOwner.GetComponent<EdgeEnd>().GetEdgeEndTitle() != null)
            {
                existingTitleOwner.GetComponent<EdgeEnd>().SetEdgeEndTitle(null);
            }
            wasSet = true;
            return wasSet;
        }

        GameObject currentTitleOwner = GetTitleOwner();
        if (currentTitleOwner != null && !currentTitleOwner.Equals(aNewTitleOwner))
        {
            currentTitleOwner.GetComponent<EdgeEnd>().SetEdgeEndTitle(null);
        }

        _titleOwner = aNewTitleOwner;
        GameObject existingEdgeEndTitle = aNewTitleOwner.GetComponent<EdgeEnd>().GetEdgeEndTitle();

        if (!gameObject.Equals(existingEdgeEndTitle))
        {
            aNewTitleOwner.GetComponent<EdgeEnd>().SetEdgeEndTitle(gameObject);
        }
        wasSet = true;
        return wasSet;
    }

}
