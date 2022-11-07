using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class TextBox : MonoBehaviour
{
    protected Diagram _diagram;
    public Text text;    
    public const int UpdatePositionConst = -100;

    public string ID { get; set; }

    public bool IsHighlightedtext { get; set; }

    // public bool isChecked;

    protected float holdTimer2 = 0;
    protected bool hold2 = false;

    public abstract bool IsValid();

    public void OnBeginHoldTB()
    {
        hold2 = true;
        holdTimer2 += Time.deltaTime;
    }

    public abstract void OnEndHoldTB();
    
}
