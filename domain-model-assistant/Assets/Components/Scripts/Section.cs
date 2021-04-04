using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public GameObject compRect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }




// ************ UI model Methods for Section ****************//
    public bool setCompartmentedRectangle(GameObject aCompRect){
        bool wasSet = false;
        if(aCompRect == null){
            return wasSet;
        }
        compRect = aCompRect;
        Debug.Log("Compartmented rectangle has been set for this section");
        wasSet = true;
        return wasSet;
    }

    public GameObject getCompartmentedRectangle(){
        return compRect;
    }
}
