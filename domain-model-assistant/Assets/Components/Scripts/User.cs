using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

    private readonly string _password;
    private readonly string _token;

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
        var request = UnityWebRequest.Put(UserRegisterEndpoint, AuthCreds);
        request.SetRequestHeader("Content-Type", "application/json");
        var _putRequestAsyncOp = request.SendWebRequest();
        while (!_putRequestAsyncOp.isDone) {} // Wait for the request to complete.
        var response = _putRequestAsyncOp.webRequest.downloadHandler.text;
        return response.Trim().Replace("User registered. Your authorization token is '", "")
            .Replace("'. Please embed this token in the header as 'Authorization : Bearer <token>' for the "
                + "subsequent requests.", "");
    }

    /// <summary>
    /// Login the user and update their token if needed.
    /// </summary>
    public bool Login()
    {
        return true;
    }

    /// <summary>
    /// Logout the user.
    /// </summary>
    public bool Logout()
    {
        return true;
    }

    /// <summary>
    /// Create a user with a random name and password, useful for testing.
    /// </summary>
    public static User CreateRandom()
    {
        var usernameBytes = new Byte[UsernamePrefixLength];
        var passwordBytes = new Byte[2 * UsernamePrefixLength];
        _random.GetBytes(usernameBytes);
        _random.GetBytes(passwordBytes);
        var username = usernameBytes.Select(b => b % 26 + (int)'a').Aggregate("", (s, c) => s + (char)c);
        var password = passwordBytes.Aggregate("", (acc, c) => acc + c);
        return new User(username, password);
    }

    /// <summary>
    /// Sends a GET request to the server. The response is stored in _getResult.
    /// </summary>
    protected void GetRequest(string uri)
    {
        // TODO Check if a `using` block can be used here, to auto-dispose the web request
        var webRequest = WrapRequest(UnityWebRequest.Get(uri));
        webRequest.timeout = 1;
        _getRequestAsyncOp = webRequest.SendWebRequest();
        //_updateNeeded = true;
    }

    /// <summary>
    /// Sends a POST request to the server.
    /// </summary>
    protected void PostRequest(string uri, string data)
    {
        var webRequest = WrapRequest(UnityWebRequest.Put(uri, data));
        _postRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Sends a DELETE request to the server.
    /// </summary>
    protected void DeleteRequest(string uri)
    {
        var webRequest = WrapRequest(UnityWebRequest.Delete(uri));
        _deleteRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Sends a PUT request to the server.
    /// </summary>
    protected void PutRequest(string uri, string data)
    {
        var webRequest = WrapRequest(UnityWebRequest.Put(uri, data));
        _putRequestAsyncOp = webRequest.SendWebRequest();
    }

    /// <summary>
    /// Wraps a UnityWebRequest with the user's authorization header.
    /// </summary>
    private UnityWebRequest WrapRequest(UnityWebRequest request)
    {
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + _token);
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
        return "(name=" + Name + ", token=" + _token + ")";
    }

}
