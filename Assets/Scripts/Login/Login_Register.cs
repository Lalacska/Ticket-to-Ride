using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Http;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Login_Register : MonoBehaviour
{
    // These fields are for the Login part. \\
    [SerializeField] private TMP_InputField _loginEmail;
    [SerializeField] private TMP_InputField _loginPassword;

    // These fields are for the Register part. \\
    [SerializeField] private TMP_InputField _RegisterUserName;
    [SerializeField] private TMP_InputField _RegisterEmail;
    [SerializeField] private TMP_InputField _RegisterPassword1;
    [SerializeField] private TMP_InputField _RegisterPassword2;

    // This part is for identifying what window there is open. \\
    public enum CurrentWindow { Login, Register }
    public CurrentWindow currentWindow = CurrentWindow.Login;

    // Here a lot of variables is being made, for use later on. \\
    string loginEmail = "";
    string loginPassword = "";
    string registerEmail = "";
    string registerPassword1 = "";
    string registerPassword2 = "";
    string registerUsername = "";
    string errorMessage = "";

    bool isWorking = false;
    bool registrationCompleted = false;
    bool isLoggedIn = false;

    // These variables are for the login portion. \\
   
    string userName = "";
    string userEmail = "";
    

    /*string rootURL = "https://bekbekbek.com/Tuhu-Test/"; // Path where php files are located */
    // string rootURL = "https://double-suicide.rehab/Tuhu-Test/"; // Path where php files are located
    string rootURL = "http://localhost/tickettoride/"; 

    #region Login

    /// <summary>
    /// This region is for the login methods in the script. \\
    /// </summary>

    // This method is for when the player clicks the Login button. \\
    public void LoginButtonClick()
        {
            loginEmail = _loginEmail.text;
            loginPassword = _loginPassword.text;

        // This method starts the "Coroutine". \\
        StartCoroutine(LoginEnumerator());
        //StartCoroutine(LoginUser(loginEmail, loginPassword));

    }

    // This method handles the users input and matches them to the database for login. \\

    
    IEnumerator LoginEnumerator()
    {
        Debug.Log("b");
        isWorking = true;
        registrationCompleted = false;
        errorMessage = "";

        WWWForm form = new WWWForm();
        form.AddField("email", loginEmail);
        form.AddField("password", loginPassword);

        using (UnityWebRequest www = UnityWebRequest.Post(rootURL + "Login.php", form))
        {
        Debug.Log("c");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                errorMessage = www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("d" + responseText);

            if (responseText.StartsWith("Success"))
                {
                    
                string[] dataChunks = responseText.Split('|');
                if (dataChunks.Length >= 3)
                {
                    userName = dataChunks[1];
                    userEmail = dataChunks[2];
                    isLoggedIn = true;
                }
                else
                {
                    errorMessage = "Invalid response format";
                }

                    UserData.username = userName;

                    Debug.Log(userName);

                    ResetValues();
                    SceneManager.LoadScene("Join-Create Game");
                }
                else
                {
                    errorMessage = responseText;
                }
            }
        }
        isWorking = false;
    }
    

    
    IEnumerator LoginUser(string email, string password)
    {
        string url = "http://localhost/tickettoride/login.php"; // Replace with your server's URL

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Login Error: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;

                if (responseText.Contains("Success"))
                {
                    // Check if the response contains "Success" indicating a successful login
                    if (responseText.Contains("Success\n"))
                    {
                        int start = responseText.IndexOf("Success\n") + "Success\n".Length;
                        string username = responseText.Substring(start).Trim();

                        Debug.Log("Login Successful. Username: " + username);

                        // Handle the successful login in Unity as needed
                        // For example, you can save the username in a variable, load a new scene, open a URL, etc.
                    }
                    else
                    {
                        Debug.Log("Login Successful");

                        // Handle other cases of success (if any)
                    }
                }
                else
                {
                    Debug.Log("Login Failed");
                    Debug.Log("Server Response: " + responseText);

                    // Handle error messages sent by the PHP script in responseText
                }
            }
        }
    }
    


    #endregion Login

    #region Register

    /// <summary>
    /// This region is for the Register methods of the script. \\
    /// </summary>

    // This method is for when the user clicks "Register". \\
    public void RegisterButtonClick()
    {
        registerEmail = _RegisterEmail.text;
        registerUsername = _RegisterUserName.text;
        registerPassword1 = _RegisterPassword1.text;
        registerPassword2 = _RegisterPassword2.text;

        StartCoroutine(RegisterEnumerator());
    }


    // This method handles the User's inputs and inserts them into the database. \\
    IEnumerator RegisterEnumerator()
    {
        isWorking = true;
        registrationCompleted = false;
        errorMessage = "";

        WWWForm form = new WWWForm();
        form.AddField("email", registerEmail);
        form.AddField("username", registerUsername);
        form.AddField("password1", registerPassword1);
        form.AddField("password2", registerPassword2);

        using (UnityWebRequest www = UnityWebRequest.Post(rootURL + "register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                errorMessage = www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;

                if (responseText.StartsWith("Success"))
                {
                    ResetValues();
                    registrationCompleted = true;
                    currentWindow = CurrentWindow.Login;
                    SceneManager.LoadScene("Login-Register");
                }
                else
                {
                    errorMessage = responseText;
                }
            }
        }
        isWorking = false;
    }

    #endregion Register

    #region Extra
    /// <summary>
    /// This region is for the extra methods in the script. \\
    /// </summary>

    void ResetValues()
    {
        errorMessage = "";
        loginEmail = "";
        loginPassword = "";
        registerEmail = "";
        registerPassword1 = "";
        registerPassword2 = "";
        registerUsername = "";
    }

    #endregion Extra
}