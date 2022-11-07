using UnityEngine;
using UnityEngine.UI;
using System;

public class GetFeedbackAction : MonoBehaviour
{

  [SerializeField]
  private Button GetFeedbackButton; // assigned in the editor

  private Diagram _diagram;

  void Start()
  {
    _diagram = GameObject.Find("Canvas").GetComponent<Diagram>();
  }

  // Update is called once per frame
  void Update()
  {}

  public void GetFeedback()
  {
    _diagram.GetFeedbackButtonPressed();
  }

}
