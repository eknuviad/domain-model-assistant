using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class Section : MonoBehaviour
{

    public GameObject compRect;
    public GameObject textB;
    public List<GameObject> textBList = new List<GameObject>();

    // true if app is run in browser, false if run in Unity editor
    private bool _isWebGl = false;
    private bool _updateNeeded = false;

    private Diagram _diagram;
    private UnityWebRequestAsyncOperation _getRequestAsyncOp;

    private UnityWebRequestAsyncOperation _postRequestAsyncOp;

    private UnityWebRequestAsyncOperation _deleteRequestAsyncOp;

    private UnityWebRequestAsyncOperation _putRequestAsyncOp;

    public const bool UseWebcore = true; // Change to false to use the wrapper page JSON instead of WebCore
    public const string WebcoreEndpoint = "http://localhost:8080/";
    public const string cdmName = "MULTIPLE_CLASSES";

    private const string classAPIEndpoint = WebcoreEndpoint + "classdiagram/" + cdmName + "/class/";
    private string class_id;
    private string AddAttributeEndpoint;

     void Awake()
    {
        _diagram = GetComponentInParent<Diagram>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach(var textbox in GetTextBoxList()){
            textbox.GetComponent<TextBox>().CheckKeyEnter();
        }
    }

    public Section (GameObject compRect) 
    {
        this.compRect = compRect;
        class_id = compRect.GetComponent<CompartmentedRectangle>().ID;
        AddAttributeEndpoint = classAPIEndpoint + "/" + class_id + "/attribute"; // + /{classId}/attribute
    }

    // ************ UI model Methods for Section ****************//

    public bool SetCompartmentedRectangle(GameObject aCompRect)
    {
        if (aCompRect == null)
        {
            return false;
        }
        compRect = aCompRect;
        return true;
    }

    public GameObject GetCompartmentedRectangle()
    {
        return compRect;
    }

    // Get, Set for TextBox
    public bool AddTextBox(GameObject aTextBox)
    {
        if (textBList.Contains(aTextBox))
        {
            return false;
        }
        textBList.Add(aTextBox);
        aTextBox.GetComponent<TextBox>().SetSection(this.gameObject);
        // _diagram.addToCreatedAttributes(aTextBox.GetComponent<TextBox>().ID);
        return true;
    }

    public List<GameObject> GetTextBoxList(){
        return textBList;
    }

    public GameObject GetTextBox(int index)
    {
        if (index >= 0 && index < textBList.Capacity - 1)
        {
            return this.textBList[index];
        }
        else
        {
            return null;
        }
    }

    public void AddAttribute()
    {
        // cap (hardcode) the number of attributes that can be added to a class to be 4
        if (textBList.Count == 4) {
            return;
        }
        var TB = GameObject.Instantiate(textB, this.transform);
        TB.GetComponent<TextBox>().ID = "1";
        TB.GetComponent<InputField>().text = "Enter Text ...";
        TB.transform.position = this.transform.position + new Vector3(0, -10, 0) * textBList.Count;
        // Update size of class depending on number of textboxes(attributes)
        // enlarge the section by 0.1*number of textboxes
        TB.transform.localScale += new Vector3(0, 0.1F*textBList.Count, 0);
        // the code commented below can automatically enlarge the section as we create more attributes, 
        // but it would cause the new textboxes created become blured/disappeared as more than 4 attribute are created
        //this.GetCompartmentedRectangle().transform.localScale += new Vector3((float)0.2,(float)0.5, 0);
        //this.GetComponent<Section>().GetCompartmentedRectangle().transform.localScale +=  new Vector3(0,(float)0.5,0);
        this.AddTextBox(TB);
    
    }

    public void AddAttribute(string _id, string name, string type)
    {
        
        var TB = GameObject.Instantiate(textB, this.transform);
        TB.GetComponent<TextBox>().ID = _id;
        TB.GetComponent<InputField>().text = type + " " + name;
        TB.transform.position = this.transform.position + new Vector3(0, -10, 0) * textBList.Count;
        this.AddTextBox(TB); 
    }

    /// <summary>
    /// Sends a GET request to the server. The response is the class diagram JSON and is stored in _getResult.
    /// </summary>
    public void GetRequest(string uri)
    {
        // TODO Check if a `using` block can be used here, to auto-dispose the web request
        var webRequest = UnityWebRequest.Get(uri);
        webRequest.method = "GET";
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.disposeDownloadHandlerOnDispose = false;
        _getRequestAsyncOp = webRequest.SendWebRequest();
        _updateNeeded = true;
    }

    /// <summary>
    /// Sends a POST request to the server to modify the class diagram.
    /// </summary>
    public void PostRequest(string uri, string data)
    {
        var webRequest = UnityWebRequest.Put(uri, data);
        webRequest.method = "POST";
        webRequest.disposeDownloadHandlerOnDispose = false;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        _postRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Sends a DELETE request to the server to remove an item from the class diagram.
    /// </summary>
    public void DeleteRequest(string uri, string _id)
    {
        var webRequest = UnityWebRequest.Delete(uri + "/" + _id);
        webRequest.method = "DELETE";
        webRequest.disposeDownloadHandlerOnDispose = false;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        _deleteRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Sends a PUT request to the server to update an item from the class diagram.
    /// </summary>
    public void PutRequest(string uri, string data, string _id)
    {
        var webRequest = UnityWebRequest.Put(uri + "/" + _id + "/" + "position", data);
        webRequest.method = "PUT";
        webRequest.disposeDownloadHandlerOnDispose = false;
        webRequest.SetRequestHeader("Content-Type", "application/json");
        _putRequestAsyncOp = webRequest.SendWebRequest();
    }

    public class AddAttributeJsonClass
    {
        public int rankIndex;

        public int typeId;

        public string attributeName;

    }

}
