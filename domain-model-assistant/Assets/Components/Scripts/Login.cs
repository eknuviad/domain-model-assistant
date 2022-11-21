using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Login : MonoBehaviour
{
    private Diagram _diagram;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    void Start()
    {
        _diagram = GameObject.Find("Canvas").GetComponent<Diagram>();
    }
    public void LoginUser()
    {
        // find user in database
        User user = User.Users[emailInputField.text];
        Debug.Log(user.Login());
        Debug.Log(emailInputField.text);
        Debug.Log(passwordInputField.text);
    }

    public void LoginUser(Student student)
    {
        // find user in database
        Debug.Log(student.Login());
        Debug.Log("student.LoggedIn: " + student.LoggedIn);
        if (!student.LoggedIn)
        {
            _diagram.infoBox.Warn("Login failed! See console for exact error. (Hint: Double check that WebCORE is running.)");
            return;
        }

        var cdmCreated = WebCore.CreateCdm(_diagram.cdmName);
        Debug.Log("WebCore.CreateCdm(): " + cdmCreated);
        if (cdmCreated)
        {
            _diagram.infoBox.Info("Class diagram created! You can now add class diagram elements with the diagram editor.");
        }
        else
        {
            _diagram.infoBox.Warn("Class diagram creation failed! See console for exact error.");
        }
    }
}
