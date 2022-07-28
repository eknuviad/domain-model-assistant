using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class to represent a web request.
/// </summary>
public class WebRequest : MonoBehaviour
{

    //private GameObject _gameObject = new();
    public static WebRequest instance;

    private string WebResponse { get; set; }

    // void Awake()
    // {
    //     instance = new GameObject().AddComponent<WebRequest>();
    // }

    /// <summary>
    /// Sends a PUT request to the server and returns its response.
    /// </summary>
    public static string PutRequest(string uri, string data = "", string userToken = null, bool usePostMethod = false)
    {
        if (instance == null)
        {
            instance = new GameObject().AddComponent<WebRequest>();
        }
        using var webRequest = WrapRequest(UnityWebRequest.Put(uri, data), userToken);
        // set method to POST here because built-in Post() does not support JSON, eg, AuthCreds
        if (usePostMethod)
        {
            webRequest.method = UnityWebRequest.kHttpVerbPOST;
        }
        //instance.RunCoroutine(webRequest);
        var requestAsyncOp = webRequest.SendWebRequest();
        instance.StartCoroutine(instance.ProcessRequest(requestAsyncOp, response =>
        {
            Debug.Log("lambda: Setting WebResponse to: " + response);
            instance.WebResponse = response;
        }));
        Debug.Log("Returning from PutRequest: " + instance.WebResponse);
        return instance.WebResponse;
    }

    IEnumerator<string> ProcessRequest(UnityWebRequestAsyncOperation requestAsyncOp, Action<string> action)
    {
        if (!requestAsyncOp.isDone)
        {
            yield return null; // wait for the request to complete
        }
        else
        {
            var result = RequestTextOrError(requestAsyncOp);
            Debug.Log("ProcessRequest(): Setting WebResponse to: " + result);
            instance.WebResponse = result;
            action(result);
            yield return result;
        }
    }

    /// <summary>
    /// Wraps a UnityWebRequest with the necessary request headers, including user's authorization header if
    /// the token is given.
    /// </summary>
    private static UnityWebRequest WrapRequest(UnityWebRequest request, string token = null)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        if (token != null)
        {
            request.SetRequestHeader("Authorization", "Bearer " + token);
        }
        request.disposeDownloadHandlerOnDispose = false;
        return request;
    }

    /// <summary>
    /// Returns the request's text if successful, otherwise logs and returns an error message.
    /// </summary>
    private string RequestTextOrError(UnityWebRequestAsyncOperation requestAsyncOp)
    {
        if (requestAsyncOp.webRequest.result != UnityWebRequest.Result.Success)
        {
            var error = "UnityWebRequest Error in User class: " + requestAsyncOp.webRequest.error
                + "\nResult type: " + requestAsyncOp.webRequest.result
                + "\nResponse: " + requestAsyncOp.webRequest.downloadHandler.text;
            Debug.LogError(error);    
            return error;
        }
        return requestAsyncOp.webRequest.downloadHandler.text;
    }
}