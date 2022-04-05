using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupLineMenu : MonoBehaviour
{   
    GameObject edgE;
    Vector3 updateConstant;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("finding canvas");
        // this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject != null){
            //update position on each frame here
        }
    }

    public void SetLine(Edge edgE)
    {
        Debug.Log("set line in popupline menu");
        this.edgE = edgE.gameObject;
        this.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(edgE.SetAssociation);
        this.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(edgE.SetAggregation);
        this.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(edgE.SetComposition);
        this.transform.GetChild(3).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(edgE.SetGeneralization);
        this.transform.GetChild(4).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(edgE.DeleteEdge);
    }
    public GameObject GetLine()
    {
        return this.edgE;
    }

    public void SetUpdateConstant(Vector3 posConstant){
        this.updateConstant = posConstant;
    }
    public void Close()
    {
        Debug.Log("Closing PopuplineMenu");
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
