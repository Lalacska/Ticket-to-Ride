using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Networking;

public class Auth : Singleton<Auth>
{
    [SerializeField] private GameObject _buttons;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;


    [SerializeField] TMP_Text errorMessages;
    [SerializeField] string url;

    // Runs when the program awakes \\
    private async void Awake()
    {
        // Initializing Unity Game Services \\
        await UnityServices.InitializeAsync();
    }

    // This method makes the user sign in anonymously \\
    private static async Task SignInAnonymouslyAsync()
    {
        try
        {
            // Clear Session Token, so we can login as a new player \\
            AuthenticationService.Instance.ClearSessionToken();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows playerID \\
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            UserData.userId = AuthenticationService.Instance.PlayerId;
            SceneManager.LoadScene("Login");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes \\
            // Notify the player with the proper error message \\
            Debug.LogException(ex);
        }
    }


    // This method gets the users info & starts the login method \\
    public async void LoginButtonclick()
    {
        await SignInAnonymouslyAsync();

    }

    // This method switch to the Register scene \\
    public static void RegisterOpenButtonclick()
    {
        SceneManager.LoadScene("Register");
    }

    // This method sends the user back to the Home scene \\
    public static void BackToHome()
    {
        //Sing out the anonymous ticket else the player can't choose login optionagain
        AuthenticationService.Instance.SignOut();
        SceneManager.LoadScene("Login-Register");
    }
}
