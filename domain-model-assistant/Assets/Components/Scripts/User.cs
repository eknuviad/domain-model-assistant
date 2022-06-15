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

    public string Name { get; set; }

    private string _password;
    private string _token;

    public bool LoggedIn { get; private set; }

    private static readonly RandomNumberGenerator _random = RandomNumberGenerator.Create();

    private UnityWebRequestAsyncOperation _getRequestAsyncOp;
    private UnityWebRequestAsyncOperation _postRequestAsyncOp;
    private UnityWebRequestAsyncOperation _deleteRequestAsyncOp;
    private UnityWebRequestAsyncOperation _putRequestAsyncOp;

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
        return response.Trim().Replace("User registered. Your authorization token is '", "")
            .Replace("'. Please embed this token in the header as 'Authorization : Bearer <token>' for the "
                + "subsequent requests.", "");
    }

    /// <summary>
    /// Login the user and update their token if needed.
    /// </summary>
    public bool Login()
    {
        var result = false;
        using (var request = WrapRequest(UnityWebRequest.Put(UserLoginEndpoint, AuthCreds)))
        {
            // set method to POST here because built-in Post() does not support JSON, used by AuthCreds
            request.method = UnityWebRequest.kHttpVerbPOST;
            var _postRequestAsyncOp = request.SendWebRequest();
            while (!_postRequestAsyncOp.isDone) {} // Wait for the request to complete.
            if (_postRequestAsyncOp.webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Network error: " + _postRequestAsyncOp.webRequest.error);
                return false;
            }
            var response = _postRequestAsyncOp.webRequest.downloadHandler.text;
            _token = response.Trim().Replace("Logged in. Your authorization token is '", "")
                .Replace("'. Please embed this token in the header as 'Authorization : Bearer <token>' for the "
                    + "subsequent requests.", "");
            LoggedIn = true;
            result = true;
        }
        return result;
    }

    /// <summary>
    /// Logout the user.
    /// </summary>
    public bool Logout()
    {
        var result = false;
        using (var request = WrapRequest(UnityWebRequest.Post(UserLogoutEndpoint, "")))
        {
            var _postRequestAsyncOp = request.SendWebRequest();
            while (!_postRequestAsyncOp.isDone) {} // Wait for the request to complete.
            if (_postRequestAsyncOp.webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError("Network error: " + _postRequestAsyncOp.webRequest.error);
                return false;
            }
            _token = null;
            LoggedIn = false;
            result = true;
        }
        return result;
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
    /// Sends a GET request to the server. The response is stored in _getResult.
    /// </summary>
    protected string GetRequest(string uri)
    {
        using var webRequest = WrapRequest(UnityWebRequest.Get(uri));
        webRequest.timeout = 1;
        _getRequestAsyncOp = webRequest.SendWebRequest();
        //_updateNeeded = true;
        return "";
    }

    /// <summary>
    /// Sends a POST request to the server.
    /// </summary>
    protected void PostRequest(string uri, string data)
    {
        using var webRequest = WrapRequest(UnityWebRequest.Put(uri, data));
        _postRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Sends a DELETE request to the server.
    /// </summary>
    protected void DeleteRequest(string uri)
    {
        using var webRequest = WrapRequest(UnityWebRequest.Delete(uri));
        _deleteRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Sends a PUT request to the server and returns its response.
    /// </summary>
    protected string PutRequest(string uri, string data = "", bool setAuthBearer = true)
    {
        using var webRequest = WrapRequest(UnityWebRequest.Put(uri, data), setAuthBearer);
        _putRequestAsyncOp = webRequest.SendWebRequest();
        while (!_putRequestAsyncOp.isDone) {} // wait for the request to complete
        if (_putRequestAsyncOp.webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + _putRequestAsyncOp.webRequest.error);
            return "";
        }
        return _putRequestAsyncOp.webRequest.downloadHandler.text;
    }

    /// <summary>
    /// Wraps a UnityWebRequest with the user's authorization header.
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

    public void DisposeRequestAsyncOps()
    {
        var ops = new [] { _getRequestAsyncOp, _postRequestAsyncOp, _deleteRequestAsyncOp, _putRequestAsyncOp };
        foreach (var op in ops)
        {
            if (op != null && op.isDone)
            {
                op.webRequest.Dispose();
            }
        }
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
