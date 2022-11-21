using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Login : MonoBehaviour
{
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public void LoginUser()
    {
        // find user in database
        User user = User.Users[emailInputField.text];
        Debug.Log(user.Login());
        Debug.Log(emailInputField.text);
        Debug.Log(passwordInputField.text);

    }
}
