using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddEnumAction : MonoBehaviour
{
     [SerializeField]
    private Button AddEnumButton; // assigned in the editor

    private Diagram _diagram;
    // Start is called before the first frame update
    void Start()
    {
        _diagram = GameObject.Find("Canvas").GetComponent<Diagram>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnumClass()
  {
    //_diagram.AddClassButtonPressed();
    _diagram.AddEnumClassButtonPressed();
    Debug.Log("add enum class pressed ");
  }
}

