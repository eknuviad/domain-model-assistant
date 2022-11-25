using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    
    public string Token { get; set; }
    public static Dictionary<string, User> Users = new();

    private readonly bool _isWebGl = Application.platform == RuntimePlatform.WebGLPlayer;

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
        Token = GetToken();
        LoggedIn = false;
        Users[name] = this;
    }

    /// <summary>
    /// Gets the user's token.
    /// </summary>
    private string GetToken()
    {
        var response = WebRequest.PutRequest(UserRegisterEndpoint, AuthCreds);
        if (WebRequest.ValidResponse(response))
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
        var response = WebRequest.PostRequest(UserLoginEndpoint, AuthCreds, Token);
        if (WebRequest.ValidResponse(response))
        {
            Token = TrimTokenResponse(response);
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
        var response = WebRequest.PostRequest(UserLogoutEndpoint, userToken: Token, contentType: WebRequest.OmitContentType);
        if (WebRequest.ValidResponse(response))
        {
            Token = null;
            LoggedIn = false;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns a random (username, password) pair.
    /// </summary>
    public static (string, string) GetRandomCreds()
    {
        var usernameBytes = new byte[UsernamePrefixLength];
        var passwordBytes = new byte[2 * UsernamePrefixLength];
        _random.GetBytes(usernameBytes);
        _random.GetBytes(passwordBytes);
        var username = usernameBytes.Select(b => b % 26 + 'a').Aggregate("", (s, c) => s + (char)c);
        var password = passwordBytes.Aggregate("", (acc, c) => acc + c);
        return (username, password);
    }

    /// <summary>
    /// Create a user with a random name and password, useful for testing.
    /// </summary>
    public static User CreateRandom()
    {
        var (username, password) = GetRandomCreds();
        return new User(username, password);
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

    public override string ToString()
    {
        return "User" + Description();
    }

    protected string Description()
    {
        // TODO token returned for debugging only, remove before release
        return "(name=" + Name + ", token=" + Token + ")";
    }

}


/// <summary>
/// A student who interacts with the application via the frontend.
/// </summary>
public class Student : User
{

    public Student(string name, string password) : base(name, password) {}
    
    // Returns a new random student. The `new` keyword below indicates that this method hides User.CreateRandom().
    public static new Student CreateRandom()
    {
        var (username, password) = GetRandomCreds();
        Student student = new(username, password);
        WebCore.Student = student;
        return student;
    }
    public static new Student CreateNewStudent(string username, string password)
    {
        Student student = new(username, password);
        WebCore.Student = student;
        return student;
    }

}
