using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{

    public GameObject compRect;
    public GameObject textB;

    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}

    // ************ UI model Methods for Section ****************//

    public bool SetCompartmentedRectangle(GameObject aCompRect)
    {
        if(aCompRect == null)
        {
            return false;
        }
        compRect = aCompRect;
        return true;
    }

    public GameObject GetCompartmentedRectangle()
    {
        return compRect;
    }

    // Get, Set for TextBox
    public bool SetTextBox(GameObject aTextBox)
    {
        if(aTextBox == null)
        {
            return false;
        }
        textB = aTextBox;
        return true;
    }

    public GameObject GetTextBox()
    {
        return textB;
    }

    
}
