using System;
using System.Collections;
using TMPro;
using UnityEngine;
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
    string rootURL = "https://double-suicide.rehab/Tuhu-Test/"; // Path where php files are located

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
        }

    // This method handles the users input and matches them to the database for login. \\
    IEnumerator LoginEnumerator()
        {
            isWorking = true;
            registrationCompleted = false;
            errorMessage = "";

            WWWForm form = new WWWForm();
            form.AddField("email", loginEmail);
            form.AddField("password", loginPassword);

            using (UnityWebRequest www = UnityWebRequest.Post(rootURL + "login.php", form))
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
                        string[] dataChunks = responseText.Split('|');
                        userName = dataChunks[1];
                        userEmail = dataChunks[2];
                        isLoggedIn = true;
                    
                        UserData.username = userName;
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