using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attribute1 : MonoBehaviour
{
    public GameObject sect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

}
