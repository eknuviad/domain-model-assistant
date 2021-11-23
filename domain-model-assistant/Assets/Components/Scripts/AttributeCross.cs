using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to create an "X" icon for an attribute. When it is clicked, the attribute is deleted.
/// </summary>
public class AttributeCross : MonoBehaviour
{
    GameObject textbox; 
    public const int UpdatePositionConst = -12;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (textbox !=null)
        {
            this.gameObject.transform.position = textbox.transform.position + new Vector3(UpdatePositionConst, 0, 0);
        }
    }

    public void setTextBox(TextBox attribute)
    {
        this.textbox = attribute.gameObject;
        this.transform.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(attribute.Destroy);
    }

    public GameObject GetTextBox()
    {
        return this.textbox;
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
