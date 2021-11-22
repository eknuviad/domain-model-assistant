using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public Text text;
    public GameObject section;
    public GameObject attributeCross;
    public const int UpdatePositionConst = -100;
    public string ID
    { get; set; }
    public bool isHighlightedtext
    { get; set; }

    float holdTimer2 = 0;
    bool hold2 = false;
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

    public bool SetSection(GameObject section)
    {
        if (section == null)
        {
            return false;
        }
        this.section = section;
        return true;
    }

    public GameObject GetSection()
    {
        return section;
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
        if (this.attributeCross.GetComponent<AttributeCross>().GetTextBox() == null)
        {
            this.attributeCross = GameObject.Instantiate(this.attributeCross);
            this.attributeCross.transform.position = this.transform.position + new Vector3(UpdatePositionConst, 0, 0);
            this.attributeCross.GetComponent<AttributeCross>().setTextBox(this);
            this.attributeCross.GetComponent<AttributeCross>().Open();
        }
        else
        {
            this.attributeCross.GetComponent<AttributeCross>().Open();
        }
    }

    public void Destroy()
    {
        this.attributeCross.GetComponent<AttributeCross>().Close();
        Destroy(this.gameObject);
    }

    public GameObject GetAttributeCross(){
        return this.attributeCross;
    }

}
