using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    private Diagram _diagram;
    public Text text;
    public GameObject sect;
    public GameObject attribcross;
    public string ID
    { get; set; }
    public bool isHighlightedtext
    { get; set; }


    float holdTimer2 = 0;
    bool hold2 = false;    
   
    void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }
   
    void Start()
    {

    }

    void Update()
    {
        if (this.hold2)
        {
            OnBeginHold2();
        }
        text = this.GetComponent<Text>();
        if (isHighlightedtext == true)
        {
            text.color = Color.red;
        }
    }

    public bool SetSection(GameObject sSection)
    {
        if (sSection == null)
        {
            return false;
        }
        sect = sSection;
        return true;
    }

    public GameObject GetSection()
    {
        return sect;
    }

    public void OnBeginHold2()
    {
        this.hold2 = true;
        holdTimer2 += Time.deltaTime;
    }

    public void OnEndHold2()
    {
        // TODO Don't spawn popup if class is being dragged
        if (holdTimer2 > 1f - 5 /*&& Vector2.Distance(this.transform.position, _prevPosition) < 0.1f*/ )
        {
            if (this.GetSection() != null)
            {
                SpawnAttributeCross();
            }

        }
    }

    void SpawnAttributeCross()
    {
        if (this.attribcross.GetComponent<AttributeCross>().GetTextBox() == null)
        {
            this.attribcross = GameObject.Instantiate(this.attribcross);
            this.attribcross.transform.position = this.transform.position + new Vector3(-100, 0, 0);
            this.attribcross.GetComponent<AttributeCross>().setTextBox(this);
            this.attribcross.GetComponent<AttributeCross>().Open();
        }
        else
        {
            this.attribcross.GetComponent<AttributeCross>().Open();
        }
    }

    public void Destroy()
    {
        _diagram.DeleteAttribute(this.gameObject); //delete attribute from Diagram
        this.attribcross.GetComponent<AttributeCross>().Close();
        Destroy(this.gameObject);
    }

    public GameObject GetAttributeCross(){
        return this.attribcross;
    }

}

