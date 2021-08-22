using UnityEngine;
using UnityEngine.UI;
using System;

public class AddClassAction : MonoBehaviour
{
  bool _isAdding;

  [SerializeField]
  private Button AddClassButton; // assigned in the editor

  private Diagram _diagram;

  void Start()
  {
    _diagram = GameObject.Find("Canvas").GetComponent<Diagram>();
  }

  // Update is called once per frame
  void Update()
  {}

  public void AddClass()
  {
    if (_isAdding)
    {
      _diagram.ActivateDefaultMode();
    }
    else
    {
      _diagram.EnterAddClassMode();
    }
    _isAdding = !_isAdding;
  }

}
