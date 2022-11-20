using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MultiplicityTextBox : TextBox
{
    private GameObject _numberOwner;
    private string prevText;
    public int UpperBound { get; set; }
    public int LowerBound { get; set; }

    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    void Start() 
    {
        GetComponent<InputField>().onSubmit.AddListener(e =>
        {
            if (GetComponent<InputField>().isFocused)
            {
                SetMultiplicity();
            }
        });

        // if (IsValid())
        // {
        //     WebCore.UpdateRelationshipMultiplicities(gameObject);
        // }
        prevText = GetComponent<InputField>().text;
    }

    void Update()
    {
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
        
        if(values.Length == 1 || values.Length == 2)
        {
            // check if the input field is a number or '*'
            int value1;
            if (values[0] == "*")
            {
                LowerBound = -1;
                UpperBound = -1;
                isValid = true;
            } 
            else if (int.TryParse(values[0], out value1))
            {
                LowerBound = value1;
                UpperBound = value1;
                isValid = true;
            }
            Debug.Log("first element is: " + values[0]);

            if (values.Length == 2 && isValid)
            {
                int value2;
                if (values[1] == "*")
                {
                    Debug.Log("UpperBound set");
                    UpperBound = -1;
                    isValid = true;
                } 
                else if (int.TryParse(values[1], out value2))
                {
                    Debug.Log("UpperBound set");
                    UpperBound = value2;
                    isValid = true;
                }
                else
                {
                    isValid = false;
                }
                Debug.Log("second element is: " + values[1]);
            }
        }

        return isValid; 
    }

    public void SetMultiplicity()
    {
        if (IsValid())
        {
            // Debug.Log(EdgeEnd.GetComponent<EdgeEnd>().ID);
            if (_numberOwner.GetComponent<EdgeEnd>() == null)
            {
                Debug.Log("edgeEnd component is null");
            }
            WebCore.SetMultiplicity(gameObject);
            prevText = GetComponent<InputField>().text;
            _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Info("Multiplicity updated");
        } else {
            _diagram.GetComponent<Diagram>().GetInfoBox().GetComponent<InfoBox>().Warn("Multiplicity format error, examples: *, 1, 1..*");
            GetComponent<InputField>().text = prevText;
        }
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

    public GameObject GetNumberOwner()
    {
        return _numberOwner;
    }

    public bool SetNumberOwner(GameObject aNewNumberOwner)
    {
        bool wasSet = false;
        if (aNewNumberOwner == null)
        {
            GameObject existingNumberOwner = _numberOwner;
            _numberOwner = null;

            if (existingNumberOwner != null && existingNumberOwner.GetComponent<EdgeEnd>().GetEdgeEndNumber() != null)
            {
                existingNumberOwner.GetComponent<EdgeEnd>().SetEdgeEndNumber(null);
            }
            wasSet = true;
            return wasSet;
        }

        GameObject currentNumberOwner = GetNumberOwner();
        if (currentNumberOwner != null && !currentNumberOwner.Equals(aNewNumberOwner))
        {
            currentNumberOwner.GetComponent<EdgeEnd>().SetEdgeEndNumber(null);
        }

        _numberOwner = aNewNumberOwner;
        GameObject existingEdgeEndNumber = aNewNumberOwner.GetComponent<EdgeEnd>().GetEdgeEndNumber();

        if (!gameObject.Equals(existingEdgeEndNumber))
        {
            aNewNumberOwner.GetComponent<EdgeEnd>().SetEdgeEndNumber(gameObject);
        }
        wasSet = true;
        return wasSet;
    }

}
