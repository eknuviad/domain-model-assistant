using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Create the InputTextComponent prefab for this to work. I had deleted it.
// public class TextInputWithFieldsComponent : InputTextComponent, BaseComponent
public class TextInputWithFieldsComponent : TextBox, BaseComponent
{
    // Equivalent to 'Attribute'
    static string[] field_types = {"string", "int", "double", "char"};    // TODO: Add types
    public string field_type;
    public string field_value;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void parseField(string text)  
    {
        // TODO: restore to the previous text if not accepted
        string[] splitted = text.Split(' ');
        if (CheckType(splitted[0]) && splitted.Length == 2)
        {
            this.field_type = splitted[0];
            this.field_value = splitted[1];
        }
        else
        {
            this.gameObject.transform.GetChild(1).GetComponent<InputField>().text = "";
            this.field_type = "";
            this.field_value = "";
        }
        
        
    }

    private bool CheckType(string type)
    {
        foreach(string t in field_types)
        {
            if (type.Equals(t))
            {
                return true;
            }
        }
        return false;
    }



}
