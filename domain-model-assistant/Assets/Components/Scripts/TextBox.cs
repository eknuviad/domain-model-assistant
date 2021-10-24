using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Text text;
    public GameObject sect;
    public string ID
    { get; set; }
    public bool isHighlightedtext
    { get; set; }

    void Start()
    {

    }

    void Update()
    {
        text = this.GetComponent<Text>();
        if (isHighlightedtext == true)
        {
            text.color = Color.red;
        }
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

