using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Auth : MonoBehaviour
{
    [SerializeField] private GameObject _buttons;

    

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    private static async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // Shows how to get the playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

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

     public static async void LoginButtonclick()
     {
        Debug.Log("You clicked the button");
        await SignInAnonymouslyAsync();
        SceneManager.LoadScene("Join-Make Game");
     }

    public static async void RegisterOpenButtonclick()
    {
        System.Diagnostics.Process.Start("https://www.bekbekbek.com/signup.php");
    }

}
