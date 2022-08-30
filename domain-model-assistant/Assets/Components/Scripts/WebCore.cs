using System;
using UnityEngine;

public class WebCore
{

    public const string WebcoreEndpoint = "http://localhost:8080";

    private readonly Diagram _diagram;

    private static WebCore instance;

    public static Student Student { get; set; }

    private WebCore(Diagram diagram)
    {
        _diagram = diagram;
    }

    public static WebCore GetInstance(Diagram diagram)
    {
        if (instance == null)
        {
            instance = new WebCore(diagram);
        }
        return instance;
    }

    /// <summary>
    /// Adds a class to the diagram with the given name and position.
    /// </summary>
    public static void AddClass(string name, Vector2 position)
    {
        instance.AddClass_(name, position);
    }

    private void AddClass_(string name, Vector2 position)
    {
        string jsonData = JsonUtility.ToJson(new AddClassDTO()
        {
            x = position.x,
            y = position.y,
            className = name
        });
        WebRequest.PostRequest(AddClassEndpoint(), jsonData, Student.Token);
        _diagram.reGetRequest = true;
        _diagram.RefreshCdm();
    }

    /// <summary>
    /// Returns the class diagram endpoint URL.
    /// </summary>
    public string CdmEndpoint()
    {
        return WebcoreEndpoint + "/" + Student.Name + "/classdiagram/" + _diagram.cdmName;
    }

    /// <summary>
    /// Returns the add class endpoint URL.
    /// </summary>
    public string AddClassEndpoint()
    {
        return CdmEndpoint() + "/class";
    }

}
