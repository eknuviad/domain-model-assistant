using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassHeaderTextBox : TextBox
{
    private GameObject _headerOwner;

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
            if (IsValid())
            {
                // Debug.Log(EdgeEnd.GetComponent<EdgeEnd>().ID);
                // TODO API call
                // WebCore.UpdateRelationshipMultiplicities(gameObject);
            }
        }

        if (hold2)
        {
            OnBeginHoldTB();
        }

        // text = GetComponent<Text>();
        // if (IsHighlightedtext)
        // {
        //     text.color = Color.red;
        // }
    }

    public override bool IsValid()
    {
        //TODO Implement 
        return true;
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

    public GameObject GetHeaderOwner()
    {
        return _headerOwner;
    }

    public bool SetHeaderOwner(GameObject aNewHeaderOwner)
    {
        bool wasSet = false;
        if (aNewHeaderOwner == null)
        {
            return wasSet;
        }

        GameObject currentHeaderOwner = GetHeaderOwner();
        if (currentHeaderOwner != null && !currentHeaderOwner.Equals(aNewHeaderOwner))
        {
            currentHeaderOwner.GetComponent<Node>().AddHeader(null);
        }

        _headerOwner = aNewHeaderOwner;
        GameObject existingHeader = aNewHeaderOwner.GetComponent<Node>().GetHeader();

        if (!gameObject.Equals(existingHeader))
        {
            aNewHeaderOwner.GetComponent<Node>().AddHeader(gameObject);
        }
        wasSet = true;
        return wasSet;
    }
}
