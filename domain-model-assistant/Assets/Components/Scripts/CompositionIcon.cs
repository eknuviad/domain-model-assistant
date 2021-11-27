using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositionIcon : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject node;
    public float updateConstant;
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != null && updateConstant != -1)
        {
            //update position with respect to associated node
            gameObject.transform.position = node.transform.position + new Vector3(0, updateConstant, 0);
        }
    }

    public void SetNode(GameObject aNode, float posConstant)
    {
        this.node = aNode;
        this.updateConstant = posConstant;
    }

    public GameObject GetNode()
    {
        return this.node;
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
