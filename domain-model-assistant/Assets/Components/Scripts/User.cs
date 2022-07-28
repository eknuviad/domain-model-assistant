using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class to represent a user.
/// </summary>
public class User
{

    public const string UserRegisterEndpoint = Constants.WebcoreEndpoint + "/user/public/register";
    public const string UserLoginEndpoint = Constants.WebcoreEndpoint + "/user/public/login";
    public const string UserLogoutEndpoint = Constants.WebcoreEndpoint + "/user/logout";

    private const int UsernamePrefixLength = 8;

    private const int RequestTimeoutSeconds = 1; // s

    public string Name { get; set; }

    private string _password;
    
    private string _token;

    private string WebResponse { get; set; }

    public bool LoggedIn { get; private set; }

    private static readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

    /// <summary>
    /// The user's authorization credentials, as a JSON string.
    /// </summary>
    protected string AuthCreds => "{\"username\": \"" + Name + "\", \"password\": \"" + _password + "\"}";
    
    public User(string name, string password)
    {
        Name = name;
        _password = password;
        _token = GetToken();
        LoggedIn = false;
    }

    /// <summary>
    /// Gets the user's token.
    /// </summary>
    private string GetToken()
    {
        var response = PutRequest(UserRegisterEndpoint, AuthCreds, setAuthBearer: false);
        if (ValidResponse(response))
        {
            return TrimTokenResponse(response);
        }
        return ""; // should not happen
    }

    /// <summary>
    /// Login the user and update their token if needed.
    /// </summary>
    public bool Login()
    {
        var response = PostRequest(UserLoginEndpoint, AuthCreds, setAuthBearer: true);
        if (ValidResponse(response))
        {
            _token = TrimTokenResponse(response);
            LoggedIn = true;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Logout the user.
    /// </summary>
    public bool Logout()
    {
        var response = PostRequest(UserLogoutEndpoint);
        if (ValidResponse(response))
        {
            _token = null;
            LoggedIn = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Create a user with a random name and password, useful for testing.
    /// </summary>
    public static User CreateRandom()
    {
        var usernameBytes = new byte[UsernamePrefixLength];
        var passwordBytes = new byte[2 * UsernamePrefixLength];
        _random.GetBytes(usernameBytes);
        _random.GetBytes(passwordBytes);
        var username = usernameBytes.Select(b => b % 26 + 'a').Aggregate("", (s, c) => s + (char)c);
        var password = passwordBytes.Aggregate("", (acc, c) => acc + c);
        return new User(username, password);
    }

    /// <summary>
    /// Sends a GET request to the server and returns the response.
    /// </summary>
    public string GetRequest(string uri)
    {
        using var webRequest = WrapRequest(UnityWebRequest.Get(uri));
        webRequest.timeout = RequestTimeoutSeconds;
        var requestAsyncOp = webRequest.SendWebRequest();
        while (!requestAsyncOp.isDone) {} // wait for the request to complete
        return RequestTextOrError(requestAsyncOp);
    }

    /// <summary>
    /// Sends a POST request to the server and returns its response.
    /// </summary>
    public string PostRequest(string uri, string data = "", bool setAuthBearer = true)
    {
        return PutRequest(uri, data, setAuthBearer, usePostMethod: true);
    }

    /// <summary>
    /// Sends a DELETE request to the server.
    /// </summary>
    public string DeleteRequest(string uri)
    {
        using var webRequest = WrapRequest(UnityWebRequest.Delete(uri));
        var requestAsyncOp = webRequest.SendWebRequest();
        while (!requestAsyncOp.isDone) {} // wait for the request to complete
        return RequestTextOrError(requestAsyncOp);
    }

    /// <summary>
    /// Sends a PUT request to the server and returns its response.
    /// </summary>
    public string PutRequest(string uri, string data = "", bool setAuthBearer = true, bool usePostMethod = false)
    {
        // using var webRequest = WrapRequest(UnityWebRequest.Put(uri, data), setAuthBearer);
        // // set method to POST here because built-in Post() does not support JSON, eg, AuthCreds
        // if (usePostMethod)
        // {
        //     webRequest.method = UnityWebRequest.kHttpVerbPOST;
        // }
        // StartCoroutine(ProcessRequest(webRequest));
        // Debug.Log("Returning from PutRequest: " + WebResponse);
        // return WebResponse;
        return WebRequest.PutRequest(uri, data, _token, usePostMethod);
    }

    IEnumerator<string> ProcessRequest(UnityWebRequest webRequest/*, Action<User> action = null*/)
    {
        var requestAsyncOp = webRequest.SendWebRequest();
        while (!requestAsyncOp.isDone) yield return null; // wait for the request to complete
        var result = RequestTextOrError(requestAsyncOp);
        WebResponse = result;
        yield return result;
    }

    /// <summary>
    /// Wraps a UnityWebRequest with the necessary request headers, including user's authorization header if
    /// setAuthBearer is set to true.
    /// </summary>
    private UnityWebRequest WrapRequest(UnityWebRequest request, bool setAuthBearer = true)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        if (setAuthBearer)
        {
            request.SetRequestHeader("Authorization", "Bearer " + _token);
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

    /// <summary>
    /// Extracts the token from the given response string.
    /// </summary>
    private string TrimTokenResponse(string tokenResponse)
    {
        return tokenResponse.Trim()
            .Replace("User registered.", "")
            .Replace("Logged in.", "")
            .Replace(" Your authorization token is '", "")
            .Replace("'. Please embed this token in the header as 'Authorization : Bearer <token>' for the "
                + "subsequent requests.", "");
    }

    /// <summary>
    /// Returns true if the response string is valid, ie, if it does not contain an error message.
    /// </summary>
    protected bool ValidResponse(string response)
    {
        return !response.StartsWith("UnityWebRequest Error");
    }

    public override string ToString()
    {
        return "User" + Description();
    }

    protected string Description()
    {
        // TODO token returned for debugging only, remove before release
        return "(name=" + Name + ", token=" + _token + ")";
    }

}


/// <summary>
/// A student who interacts with the application via the frontend.
/// </summary>
class Student : User
{

    public Student(string name, string password) : base(name, password) {}

    public bool CreateCdm(string name)
    {
        PutRequest(CdmEndpoint(name));
        return true;
    }

    private string CdmEndpoint(string cdmName)
    {
        return Constants.WebcoreEndpoint + "/" + Name + "/classdiagram/" + cdmName;
    }

}
