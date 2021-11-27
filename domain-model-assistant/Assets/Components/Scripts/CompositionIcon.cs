using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositionIcon : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject node;
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject != null){
            //update position with respect to associated node
        }
    }

    public void SetNode(GameObject aNode){
        this.node = aNode;
    }

    public GameObject GetNode(){
        return this.node;
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
