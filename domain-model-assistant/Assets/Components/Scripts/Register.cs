using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Register : MonoBehaviour
{
    public TextMeshProUGUI errorlog;
    public TMP_InputField userInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmedPasswordInputField;
    public void RegisterUser()
    {
        if (passwordInputField.text == confirmedPasswordInputField.text)
        {
            User user = new User(emailInputField.text, passwordInputField.text);
        }
        else
        {
            errorlog.text = "Passwords do not match";
        }
        Debug.Log(userInputField.text);
        Debug.Log(emailInputField.text);
        Debug.Log(passwordInputField.text);
        Debug.Log(confirmedPasswordInputField.text);
    }
}
