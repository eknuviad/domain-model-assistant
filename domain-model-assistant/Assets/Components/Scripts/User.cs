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
    /// The user's authorization credentials.
    /// </summary>
    protected Dictionary<string, string> AuthCreds => new Dictionary<string, string>
    {
        { "username", Name },
        { "password", _password },
    };

    /// <summary>
    /// The user's authorization header.
    /// </summary>
    protected Dictionary<string, string> AuthHeader => new Dictionary<string, string>
    {
        { "Authorization", "Bearer " + _token },
    };
    
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
        return "TODO";
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
        var username = usernameBytes.Where(c => c >= 'a' && c <= 'z').Aggregate("", (acc, c) => acc + c);
        var password = passwordBytes.Aggregate("", (acc, c) => acc + c);
        return new User(username, password);
    }

    public string ToString()
    {
        return "User" + Description();
    }

    protected string Description()
    {
        return "(name=" + Name + ", token=" + _token + ")";
    } 

}
