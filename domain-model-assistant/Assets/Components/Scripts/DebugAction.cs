using UnityEngine;
using UnityEngine.UI;
using System;

public class DebugAction : MonoBehaviour
{

  [SerializeField]
  private Button DebugButton; // assigned in the editor

  private Diagram _diagram;

  void Start()
  {
    _diagram = GameObject.Find("Canvas").GetComponent<Diagram>();
  }

  // Update is called once per frame
  void Update()
  {}

  public void Debug()
  {
    _diagram.DebugAction();
  }

}
