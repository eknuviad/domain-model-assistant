using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    private Text _text;

    public static readonly Color DarkBlue = new(0, 0.188f, 0.867f); // #0030DD
    public static readonly Color DarkGreen = new(0, 0.557f, 0.059f); // #008E0F
    public static readonly Color DefaultColor = DarkBlue;

    public string Value
    {
        get => _text.GetComponent<Text>().text;
        set => _text.GetComponent<Text>().text = value;
    }

    public Color TextColor
    {
        get => _text.GetComponent<Text>().color;
        set => _text.GetComponent<Text>().color = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        _text = gameObject.GetComponent<Text>();
        Clear();
    }

    // Update is called once per frame
    void Update()
    {}

    public void Clear()
    {
        Value = "";
        TextColor = DefaultColor;
    }

    public void Red(string text)
    {
        Value = text;
        TextColor = Color.red;
    }

    public void Green(string text)
    {
        Value = text;
        TextColor = DarkGreen;
    }

    public void Blue(string text)
    {
        Value = text;
        TextColor = DarkBlue;
    }

    public void Info(string text)
    {
        Value = text;
        TextColor = DefaultColor;
    }

    public void Warn(string text)
    {
        Red(text);
    }

}
