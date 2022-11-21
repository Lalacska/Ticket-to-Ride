using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinCreateScene : Singleton<JoinCreateScene>
{
    [SerializeField] TMP_InputField joinCode;
    

    // Makes the player to join a lobby \\
    public void JoinButtonclick()
    {
        string code = joinCode.GetComponent<TMP_InputField>().text;
        Debug.Log("You clicked the button");
        // If the player wrote join code, it will try to make it connect to the chosed lobby \\
        if(code != null && code != string.Empty)
        {
            LobbyManager.Instance.JoinLobby(code);
        }
        // If the player doesn't have join code it will try to connect it to an available lobby \\
        else
        {
            Debug.Log("QUICK JOIN");
            LobbyManager.Instance.QuickJoin();
        }
    }
   
    //Switch to the Game Settings scene \\
    public static void CreateButtonclick()
    {
        SceneManager.LoadScene("Game Settings");
    }

    //Sign the player out, and switch back to the Login-Rgister scene \\
    public void CloseButton()
    {
        AuthenticationService.Instance.SignOut();
        SceneManager.LoadScene("Login-Register");
    }
}
