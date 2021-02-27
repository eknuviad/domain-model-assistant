using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClassDiagram : MonoBehaviour, BaseComponent
{
    public string ID{get; set;}
    float hold_timer = 0;
    bool hold = false;
    bool selected = false;
    public GameObject popup_menu_prefab;
    private GameObject popup_menu;
    // Start is called before the first frame update
    public GameObject inputTextWithFields;
    List<Association> associations = new List<Association>();
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (this.hold)
        {
            OnBeginHold();
        }
    }

    public void OnBeginHold()
    {
        this.hold = true;
        hold_timer += Time.deltaTime;
    }

    public void OnEndHold()
    {
        // TODO: Should initialize a pop up menu gameobject and destroy on close
        if(hold_timer > 1f)
        {
            SpawnPopupMenu();
        }
        hold_timer = 0;
        this.hold = false;
    }

    public void AddAttribute()
    {
        GameObject newField = (GameObject)GameObject.Instantiate(inputTextWithFields);
        newField.transform.SetParent(this.transform);
    }

    public void AddSubclass()
    {
        // Global manager
        GameObject child = GameObject.Find("Canvas").GetComponent<Canvas>().CreateClassDiagram(Input.mousePosition);
        Association association = new Association(this.gameObject, child);
        this.associations.Add(association);
    }

    void SpawnPopupMenu()
    {
        this.popup_menu = GameObject.Instantiate(this.popup_menu_prefab);
        this.popup_menu.transform.position = this.transform.position + new Vector3(100, 0, 0);
        this.popup_menu.GetComponent<PopupMenu>().SetAssociatedObject(this);
    }


}
