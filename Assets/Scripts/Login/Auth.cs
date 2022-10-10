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

public class Auth : MonoBehaviour
{
    [SerializeField] private GameObject _buttons;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;


    [SerializeField] TMP_Text errorMessages;
    [SerializeField] string url;


    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    // This method makes the user signin anonymously. \\
    private static async Task SignInAnonymouslyAsync()
    {
        try
        {
            AuthenticationService.Instance.ClearSessionToken();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            SceneManager.LoadScene("Join-Create Game");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }


    // This method opens the Login Scene. \\
    public async void LoginButtonclick()
    {
        Debug.Log("You clicked the button");
        /*string name = username.GetComponent<TMP_InputField>().text;
        string pass = password.GetComponent<TMP_InputField>().text;
        Debug.Log(name);
        Debug.Log(pass);*/

        await SignInAnonymouslyAsync();

    }

    // This method opens the Register scene. \\
    public static void RegisterOpenButtonclick()
    {
        System.Diagnostics.Process.Start("https://www.bekbekbek.com/signup.php");
    }

    // This method sends the user back to the Home scene. \\
    public static void BackToHome()
    {
        SceneManager.LoadScene("Login-Register");
    }
}
