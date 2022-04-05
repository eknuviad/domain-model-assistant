using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour
{

    GameObject compRect;
    public const int UpdatePositionConst = 138;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    { 
        if (compRect != null)
        {
            this.gameObject.transform.position = compRect.transform.position + new Vector3(UpdatePositionConst, 0, 0);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("close popupmenu");
            this.gameObject.SetActive(false);
        }
    }

    public void SetCompartmentedRectangle(CompartmentedRectangle compRect)
    {
        this.compRect = compRect.gameObject;
        // NB: we'll need to properly implement these two methods based on UI model diagram
        this.transform.GetChild(0).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(compRect.GetSection(0).GetComponent<Section>().AddAttribute);
        // this.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(aCompRect.AddSubclass);
        
        // Adding line
        this.transform.GetChild(1).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(compRect.SetLine);
        this.transform.GetChild(2).GetComponent<UnityEngine.UI.Button>().onClick.AddListener(compRect.Destroy);
    }

    public GameObject GetCompartmentedRectangle()
    {
        return this.compRect;
    }

    public void Close()
    {
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
