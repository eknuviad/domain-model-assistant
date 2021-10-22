using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{

    public GameObject sect;
    public string ID
    { get; set; }

    void Start()
    {
        // this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        // this.gameObject.transform.SetParent(this.sect.transform);

    }

    public string GetText()
    {
        return GetComponent<InputField>().text;
    }

    public bool SetText(string text)
    {
        var inputField = GetComponent<InputField>();
        if (inputField == null)
        {
            inputField = gameObject.AddComponent<InputField>();
        }
        inputField.text = text;
        return true;
    }

    public bool SetSection(GameObject sSection)
    {
        if(sSection == null)
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


}
