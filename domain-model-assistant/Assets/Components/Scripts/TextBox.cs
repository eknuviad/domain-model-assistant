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

