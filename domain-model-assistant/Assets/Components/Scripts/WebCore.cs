using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

    // public static API methods

    public static WebCore GetInstance(Diagram diagram)
    {
        if (instance == null)
        {
            instance = new WebCore(diagram);
        }
        return instance;
    }

    public static bool CreateCdm(string name)
    {
        return instance.CreateCdm_(name);
    }

    /// <summary>
    /// Adds a class to the diagram with the given name and position.
    /// </summary>
    public static void AddClass(string name, Vector2 position)
    {
        instance.AddClass_(name, position);
    }

    /// <summary>
    /// Updates the class position.
    /// </summary>
    public static void UpdateClassPosition(GameObject header, GameObject node)
    {
        instance.UpdateClassPosition_(header, node);
    }

    /// <summary>
    /// Deletes the given class.
    /// </summary>
    public static void DeleteClass(GameObject node)
    {
        instance.DeleteClass_(node);
    }

    /// <summary>
    /// Adds an Attribute to the given textbox.
    /// </summary>
    public static void AddAttribute(GameObject textbox)
    {
        instance.AddAttribute_(textbox);
    }

    /// <summary>
    /// Deletes the Attribute.
    /// </summary> 
    public static void DeleteAttribute(GameObject textBox)
    {
        instance.DeleteAttribute_(textBox);
    }

    public static void AddGeneralization()
    {
        instance.AddGeneralization_();
    }

    public static void AddAssociation(GameObject node1, GameObject node2)
    {
        instance.AddAssociation_(node1, node2);
    }

    public static void UpdateRelationshipMultiplicities(GameObject textbox)
    {
        instance.UpdateRelationshipMultiplicities_(textbox);
    }

    public static void UpdateRelationshipRoleNames()
    {
        instance.UpdateRelationshipRoleNames_();
    }

    public static void UpdateRelationshipType()
    {
        instance.UpdateRelationshipType_();
    }

    public static void GetFeedback()
    {
        instance.GetFeedback_();
    }


    // private instance methods that act on the singleton instance

    private bool CreateCdm_(string name)
    {
        var resp = WebRequest.PutRequest(CdmEndpoint(name), userToken: Student.Token,
            contentType: WebRequest.OmitContentType);
        return WebRequest.ValidResponse(resp);
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

    private void UpdateClassPosition_(GameObject header, GameObject node)
    {
        string _id = node.GetComponent<CompartmentedRectangle>().ID;
        string clsName = header.GetComponent<InputField>().text;
        Vector2 newPosition = node.GetComponent<CompartmentedRectangle>().GetPosition();
        // JSON body. Create new serializable JSON object.
        var positionInfo = new Position(newPosition.x, newPosition.y);
        string jsonData = JsonUtility.ToJson(positionInfo);
        // send updated position via PUT request
        WebRequest.PutRequest(UpdateClassPositionEndpoint(_id), jsonData, Student.Token);
    }

    private void DeleteClass_(GameObject node)
    {
        Debug.Log("WebCore.DeleteClass() called");
        string _id = node.GetComponent<CompartmentedRectangle>().ID;
        WebRequest.DeleteRequest(DeleteClassEndpoint(_id), Student.Token);
        _diagram.reGetRequest = true;
        _diagram.RefreshCdm();
        // No need to remove or destroy the node here since entire class diagram is recreated
    }

    private void AddAttribute_(GameObject textbox)
    {
        Section section = textbox.GetComponent<AttributeTextBox>().Section.GetComponent<Section>();
        CompartmentedRectangle compRect = section.GetCompartmentedRectangle()
            .GetComponent<CompartmentedRectangle>();
        List<GameObject> attributes = section.GetTextBoxList();
        string _id = compRect.ID;
        int rankIndex = -1;
        string name = null;
        int typeId = -1;
        for (int i = 0; i < attributes.Count; i++)
        {
            if (attributes[i] == textbox)
            {
                rankIndex = i;
                name = textbox.GetComponent<AttributeTextBox>().Name.ToLower();
                typeId = textbox.GetComponent<AttributeTextBox>().TypeId;
                break;
            }
        }
        var info = new AddAttributeBody(rankIndex, typeId, name);
        string jsonData = JsonUtility.ToJson(info);
        Debug.Log(jsonData);
        // @param body {"rankIndex": Integer, "typeId": Integer, "attributeName": String}
        WebRequest.PostRequest(AddAttributeEndpoint(_id), jsonData, Student.Token);
        _diagram.reGetRequest = true;
        _diagram.RefreshCdm();
    }

    private void DeleteAttribute_(GameObject textBox)
    {
        string _id = textBox.GetComponent<TextBox>().ID;
        WebRequest.DeleteRequest(DeleteAttributeEndpoint(_id), Student.Token);
        _diagram.reGetRequest = true;
        _diagram.RefreshCdm();
        // No need to remove or destroy the attribute here since entire class diagram is recreated
    }

    private void AddGeneralization_()
    {
        Debug.Log("WebCore.AddGeneralization() called");
    }

    private void AddAssociation_(GameObject node1, GameObject node2)
    {
        var id1 = node1.GetComponent<CompartmentedRectangle>().ID;
        var id2 = node2.GetComponent<CompartmentedRectangle>().ID;
        Debug.Log($"WebCore.AddAssociation({id1}, {id2}) called");
        WebRequest.PostRequest(AddAssociationEndpoint(),
            new { fromClassId = id1, toClassId = id2, bidirectional = true }, Student.Token);
        _diagram.reGetRequest = true;
        _diagram.RefreshCdm();
    }

    private void UpdateRelationshipMultiplicities_(GameObject textBox)
    {
        var multiplicityTextBox = textBox.GetComponent<MultiplicityTextBox>();
        string id = multiplicityTextBox.GetNumberOwner().GetComponent<EdgeEnd>().ID;
        
        Debug.Log($"WebCore.UpdateRelationshipMultiplicities({id}, called");

        int uBound = multiplicityTextBox.UpperBound;
        int lBound = multiplicityTextBox.LowerBound;

        WebRequest.PutRequest(AddRelationshipMultiplicitiesEndpoint("1"), 
            new { lowerBound = lBound, upperBound = uBound}, Student.Token);
        _diagram.reGetRequest = true;
        _diagram.RefreshCdm();
    }

    private void UpdateRelationshipRoleNames_()
    {
        // var roleNameTextBox = textBox.GetComponent<RoleNameTextBox>();
        // string id = multiplicityTextBox.EdgeEnd.GetComponent<EdgeEnd>().ID;
        
        // Debug.Log($"WebCore.UpdateRelationshipRoleNames_({id}, called");

        // WebRequest.PutRequest(AddRelationshipMultiplicitiesEndpoint("1"), 
        //     new { lowerBound = lBound, upperBound = uBound}, Student.Token);
        // _diagram.reGetRequest = true;
        // _diagram.RefreshCdm();
    }

    private void UpdateRelationshipType_()
    {
        Debug.Log("WebCore.UpdateRelationshipType() called");
    }

    private void GetFeedback_()
    {
        Debug.Log("WebCore.GetFeedback() called");
        string feedbackJson = WebRequest.GetRequest(GetFeedbackEndpoint(), Student.Token);
        Debug.Log($"Received Feedback JSON: {feedbackJson}");
        var feedback = JsonConvert.DeserializeObject<Dictionary<string, object>>(feedbackJson);
        foreach (var key in feedback.Keys)
        {
            Debug.Log($"Key: {key}, Value: {feedback[key]} of type {feedback[key].GetType()}");
        }
        foreach (var name in new List<string> {"solutionElements", "problemStatementElements"})
        {
            if (feedback.ContainsKey(name))
            {
                var elements = ((JObject)feedback[name]).ToObject<Dictionary<string, List<string>>>();
                foreach (var color in elements.Keys)
                {
                    Color rgb1 = ColorUtility.TryParseHtmlString(color, out Color rgb) ? rgb : Color.red;
                    foreach (var element in elements[color])
                    {
                        Debug.Log($"Highlight element {element} using color {color}");
                        foreach (var rect in _diagram.GetNode())
                        {
                            if (rect.GetComponent<CompartmentedRectangle>().ID == element)
                            {
                                rect.GetComponent<CompartmentedRectangle>().HighlightWith(rgb1);
                            }
                        }
                    }
                }
            }
        }

        var message = "No feedback is currently available from the Modeling Assistant. Please try again later.";
        if (feedback.ContainsKey("writtenFeedback"))
        {
            message = feedback["writtenFeedback"].ToString();
        }
        if (message.StartsWith("No feedback"))
        {
            _diagram.infoBox.Warn(message);
        }
        else
        {
            _diagram.infoBox.Info(message);
        }
    }

    // additional helper methods

    /// <summary>
    /// Returns the class diagram endpoint URL.
    /// </summary>
    public string CdmEndpoint(string name = "")
    {
        if (string.IsNullOrEmpty(name))
        {
            name = _diagram.cdmName;
        }
        return $"{WebcoreEndpoint}/{Student.Name}/classdiagram/{name}";
    }

    /// <summary>
    /// Returns the add class endpoint URL.
    /// </summary>
    public string AddClassEndpoint()
    {
        return $"{CdmEndpoint()}/class";
    }

    /// <summary>
    /// Returns the delete class endpoint URL for the given class _id.
    /// </summary>
    public string DeleteClassEndpoint(string classId)
    {
        return $"{CdmEndpoint()}/class/{classId}";
    }

    /// <summary>
    /// Returns the update class endpoint URL for the given class _id.
    /// </summary>
    public string UpdateClassPositionEndpoint(string classId)
    {
        return $"{CdmEndpoint()}/class/{classId}/position"; // TODO Double check
    }

    /// <summary>
    /// Returns the add attribute endpoint URL for the given class _id.
    /// </summary>
    public string AddAttributeEndpoint(string classId)
    {
        return $"{CdmEndpoint()}/class/{classId}/attribute";
    }

    /// <summary>
    /// Returns the delete attribute endpoint URL for the given attribute _id.
    /// </summary>
    public string DeleteAttributeEndpoint(string attributeId)
    {
        return $"{CdmEndpoint()}/class/attribute/{attributeId}";
    }

    public string AddAssociationEndpoint()
    {
        return $"{CdmEndpoint()}/association";
    }

    public string AddRelationshipMultiplicitiesEndpoint(string associationEndId)
    {
        return $"{CdmEndpoint()}/association/end/{associationEndId}/multiplicity";
    }

    public string GetFeedbackEndpoint()
    {
        return $"{CdmEndpoint()}/feedback";
    }

}
