using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggregationIcon : MonoBehaviour
{
       // Start is called before the first frame update
    public GameObject node;
    public float updateConstantX;
    public float updateConstantY;

    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != null)
        {
            //update position with respect to associated node
            gameObject.transform.position = node.transform.position + new Vector3(updateConstantX, updateConstantY, 0);
        }
    }

    public void SetNode(GameObject aNode, float posConstantX,float posConstantY)
    {
        this.node = aNode;
        this.updateConstantX = posConstantX;
        this.updateConstantY = posConstantY;

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
