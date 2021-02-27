using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputTextComponent : MonoBehaviour, BaseComponent
{
    public string ID
    {
        get
        {
            return ID;
        }
        set
        {
            ID = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("Input clicked.");
    }

}
