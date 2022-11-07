using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupLineMenu : MonoBehaviour
{   
    GameObject edge;
    Vector3 updateConstant;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("finding canvas");
        // gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject != null)
        {
            //update position on each frame here
        }
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Close PopupLineMenu");
            gameObject.SetActive(false);
        }
    }

    public void SetLine(Edge edge)
    {
        Debug.Log("PopupLineMenu.SetLine() called");
        this.edge = edge.gameObject;
        var listeners = new List<UnityAction>
        {
            edge.SetAssociation, edge.SetAggregation, edge.SetComposition, edge.SetGeneralization, edge.DeleteEdge
        };
        for (int i = 0; i < listeners.Count; i++)
        {
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(listeners[i]);
        }
    }

    public GameObject GetLine()
    {
        return edge;
    }

    public void SetUpdateConstant(Vector3 posConstant){
        updateConstant = posConstant;
    }
    public void Close()
    {
        Debug.Log("Closing PopupLineMenu");
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

}
