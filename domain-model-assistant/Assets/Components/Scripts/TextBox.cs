using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour{
    
    public string text;
    public string ID{
        get{
            return ID;
        }
        set{
            ID = value;
        }
    }

    public string getText(){
        return GetComponent<InputField>().text;
    }

    public bool setText(string text){
        bool wasSet = false;
        GetComponent<InputField>().text = text;
        Debug.Log("Text has been set");
        wasSet = true;
        return wasSet;
    }

}
