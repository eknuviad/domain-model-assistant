using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositionIcon : MonoBehaviour
{
    // Start is called before the first frame update
    public EdgeEnd edgeEnd;

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
            gameObject.transform.position = edgeEnd.Position;
        }
    }

    public void SetEdgeEnd(EdgeEnd aEdgeEnd)
    {
        this.edgeEnd = aEdgeEnd;
    }

    public EdgeEnd GetEdgeEnd()
    {
        return this.edgeEnd;
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
