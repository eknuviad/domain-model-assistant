using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Section : MonoBehaviour
{

    public GameObject compRect;
    public GameObject textB;
    public List<GameObject> textBList = new List<GameObject>();

    // true if app is run in browser, false if run in Unity editor
    private bool _isWebGl = false;

    private UnityWebRequestAsyncOperation _getRequestAsyncOp;

    private UnityWebRequestAsyncOperation _postRequestAsyncOp;

    private UnityWebRequestAsyncOperation _deleteRequestAsyncOp;

    private UnityWebRequestAsyncOperation _putRequestAsyncOp;

    public const bool UseWebcore = false; // Change to false to use the wrapper page JSON instead of WebCore
    public const string WebcoreEndpoint = "http://localhost:8080/";
    public const string cdmName = "MULTIPLE_CLASSES";

    public const string classAPIEndpoint = WebcoreEndpoint + "classdiagram/" + cdmName + "/class/";
    public const string AddAttributeEndpoint = classAPIEndpoint; // + /{classId}/attribute

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
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
    public bool AddTextBox(GameObject TB)
    {
        if (textBList.Contains(TB))
        {
            return false;
        }
        textBList.Add(TB);
        TB.GetComponent<TextBox>().SetSection(this.gameObject);
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
        if (UseWebcore)
        {
            // TODO Replace this ugly string once Unity moves to .NET 6
            AddAttributeJsonClass info = new AddAttributeJsonClass();
            info.typeId = type;
            info.attributeName = name;
            string jsonData = JsonUtility.ToJson(info);
            PostRequest(AddAttributeEndpoint, jsonData);
            GetRequest(classAPIEndpoint);
        }
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
