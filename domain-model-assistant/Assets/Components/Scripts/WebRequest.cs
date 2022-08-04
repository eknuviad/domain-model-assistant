using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class to represent a web request.
/// </summary>
public class WebRequest : MonoBehaviour
{

    private static readonly bool _isWebGl = Application.platform == RuntimePlatform.WebGLPlayer;

    private const int RequestTimeoutSeconds = 1; // s

    public const string OmitContentType = "omit";

    [DllImport("__Internal")]
    private static extern string HttpRequest(string verb, string url, string headers, string data);

    /// <summary>
    /// Sends a GET request to the server and returns the response.
    /// </summary>
    public static string GetRequest(string uri, string userToken = null)
    {
        if (_isWebGl)
        {
            return HttpRequest("GET", uri, WebGlJsHeaders(userToken), "");
        }
        else
        {
            using var webRequest = WrapRequest(UnityWebRequest.Get(uri), userToken);
            webRequest.timeout = RequestTimeoutSeconds;
            var requestAsyncOp = webRequest.SendWebRequest();
            while (!requestAsyncOp.isDone) {} // wait for the request to complete
            return RequestTextOrError(requestAsyncOp);
        }
    }

    /// <summary>
    /// Sends a POST request to the server and returns its response.
    /// </summary>
    public static string PostRequest(string uri, string data = "", string userToken = null,
                                     string contentType = "application/json")
    {
        return PutRequest(uri, data, userToken, usePostMethod: true, contentType);
    }

    /// <summary>
    /// Sends a PUT request to the server and returns its response.
    /// </summary>
    public static string PutRequest(string uri, string data = "", string userToken = null, bool usePostMethod = false,
                                    string contentType = "application/json")
    {
        if (_isWebGl)
        {
            var verb = usePostMethod ? "POST" : "PUT";
            return HttpRequest(verb, uri, WebGlJsHeaders(userToken, contentType), data);
        }
        else
        {
            using var webRequest = WrapRequest(UnityWebRequest.Put(uri, data), userToken, contentType);
            // set method to POST here because built-in Post() does not support JSON, eg, AuthCreds
            if (usePostMethod)
            {
                webRequest.method = UnityWebRequest.kHttpVerbPOST;
            }
            var requestAsyncOp = webRequest.SendWebRequest();
            while (!requestAsyncOp.isDone) {} // wait for the request to complete
            return RequestTextOrError(requestAsyncOp);
        }
    }

    /// <summary>
    /// Sends a DELETE request to the server.
    /// </summary>
    public static string DeleteRequest(string uri, string userToken = null)
    {
        if (_isWebGl)
        {
            return HttpRequest("DELETE", uri, WebGlJsHeaders(userToken), "");
        }
        else
        {
            using var webRequest = WrapRequest(UnityWebRequest.Delete(uri), userToken);
            var requestAsyncOp = webRequest.SendWebRequest();
            while (!requestAsyncOp.isDone) {} // wait for the request to complete
            return RequestTextOrError(requestAsyncOp);
        }
    }

    /// <summary>
    /// Wraps a UnityWebRequest with the necessary request headers, including user's authorization header if
    /// the user token is given.
    /// </summary>
    private static UnityWebRequest WrapRequest(UnityWebRequest request, string userToken = null,
                                               string contentType = "application/json")
    {
        if (contentType.ToLower() != OmitContentType)
        {
            request.SetRequestHeader("Content-Type", contentType);
        }
        if (!string.IsNullOrEmpty(userToken))
        {
            request.SetRequestHeader("Authorization", "Bearer " + userToken);
        }
        request.disposeDownloadHandlerOnDispose = false;
        return request;
    }

    private static string WebGlJsHeaders(string userToken = null, string contentType = "application/json")
    {
        Dictionary<string, string> headers = new();
        if (contentType.ToLower() != OmitContentType)
        {
            headers["Content-Type"] = contentType;
        }
        if (!string.IsNullOrEmpty(userToken))
        {
            headers["Authorization"] = "Bearer " + userToken;
        }
        var json = "{";
        for (int i = 0; i < headers.Count; i++)
        {
            json += '"' + headers.ElementAt(i).Key + "\": \"" + headers.ElementAt(i).Value + '"';
            if (i < headers.Count - 1)
            {
                json += ", "; // JSON does not allow trailing commas
            }
        }
        return json + "}";
    }

    /// <summary>
    /// Returns the request's text if successful, otherwise logs and returns an error message.
    /// </summary>
    private static string RequestTextOrError(UnityWebRequestAsyncOperation requestAsyncOp)
    {
        if (requestAsyncOp.webRequest.result != UnityWebRequest.Result.Success)
        {
            Dictionary<string, string> headers = new();
            if (requestAsyncOp.webRequest.GetResponseHeaders() != null)
            {
                headers = requestAsyncOp.webRequest.GetResponseHeaders();
            }
            var error = "UnityWebRequest Error in WebRequest class: " + requestAsyncOp.webRequest.error
                + "\nResult type: " + requestAsyncOp.webRequest.result
                + "\nRequest URL: " + requestAsyncOp.webRequest.url
                + "\nResponse headers:\n  " + string.Join(Environment.NewLine + "  ", headers)
                + "\nResponse body: " + requestAsyncOp.webRequest.downloadHandler.text;
            Debug.LogError(error);    
            return error;
        }
        return requestAsyncOp.webRequest.downloadHandler.text;
    }

    /// <summary>
    /// Returns true if the response string is valid, ie, if it does not contain an error message.
    /// </summary>
    public static bool ValidResponse(string response)
    {
        return !response.StartsWith("UnityWebRequest Error");
    }

}