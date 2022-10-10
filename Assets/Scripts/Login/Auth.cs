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
    WWWForm form;

    bool isWorking = false;
    bool registrationCompleted = false;
    bool isLoggedIn = false;
    string errorMessage = "";

    //Logged-in user data
    string userName = "";
    string userEmail = "";


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


    // This method gets the users info & starts the login method. \\
    public async void LoginButtonclick()
    {
        Debug.Log("You clicked the button");
        string name = username.GetComponent<TMP_InputField>().text;
        string pass = password.GetComponent<TMP_InputField>().text;
        Debug.Log(name);
        Debug.Log(pass);

        //await SignInAnonymouslyAsync();
        StartCoroutine(Login());
    }

    async Task SignInWithOpenIdConnectAsync(string idProviderName, string idToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithOpenIdConnectAsync(idProviderName, idToken);
            Debug.Log("SignIn is successful.");
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


    IEnumerator Login()
    {
        errorMessage = "";

        form = new WWWForm();

        form.AddField("Username", username.text);
        form.AddField("Password", password.text);

        
        using (UnityWebRequest www = UnityWebRequest.Post(url + "login.php", form))
        {
            yield return www.SendWebRequest();

            Debug.Log(www);

            Debug.Log(www.downloadHandler.text);
            if (www.result != UnityWebRequest.Result.Success)
            {
                errorMessage = www.error;
            }
            else
            {
                Debug.Log("C");
                string responseText = www.downloadHandler.text;
                Debug.Log(www.downloadHandler);

                if (responseText.StartsWith("Success"))
                {
                    Debug.Log("D");
                    string[] dataChunks = responseText.Split('|');
                    userName = dataChunks[1];
                    userEmail = dataChunks[2];
                    isLoggedIn = true;

                    errorMessage = "";
                    Debug.Log("Logged in");
                }
                else
                {
                    errorMessage = responseText;
                }
            }
        }

        isWorking = false;
    }



    // This method opens the Login Screen. \\
    public static void LoginScene()
    {
        SceneManager.LoadScene("Login");
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
