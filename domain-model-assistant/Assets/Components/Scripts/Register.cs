using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Register : MonoBehaviour
{
    private Diagram _diagram;
    public TextMeshProUGUI errorlog;
    public TMP_InputField userInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmedPasswordInputField;

    void Start()
    {
    }
    public void RegisterUser()
    {
        if (passwordInputField.text == confirmedPasswordInputField.text)
        {
            Student student = Student.CreateNewStudent(emailInputField.text, passwordInputField.text);
            User user = new User(emailInputField.text, passwordInputField.text);
            User.Users[emailInputField.text] = student;
            errorlog.text = "";
            SceneManager.LoadScene(0);
            // // find user in database
            // Debug.Log(student.Login());
            // Debug.Log("student.LoggedIn: " + student.LoggedIn);
            // if (!student.LoggedIn)
            // {
            //     _diagram.infoBox.Warn("Login failed! See console for exact error. (Hint: Double check that WebCORE is running.)");
            //     return;
            // }

            // var cdmCreated = WebCore.CreateCdm(_diagram.cdmName);
            // Debug.Log("WebCore.CreateCdm(): " + cdmCreated);
            // if (cdmCreated)
            // {
            //     _diagram.infoBox.Info("Class diagram created! You can now add class diagram elements with the diagram editor.");
            // }
            // else
            // {
            //     _diagram.infoBox.Warn("Class diagram creation failed! See console for exact error.");
            // }
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
