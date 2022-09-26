using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JoinCreateScene : Singeltone<JoinCreateScene>
{
    [SerializeField] TMP_InputField joinCode;
    
    public void JoinButtonclick()
    {
        string code = joinCode.GetComponent<TMP_InputField>().text;
        Debug.Log("You clicked the button");
        if(code != null)
        {
            LobbyManager.Instance.JoinLobby(code);
        }
    }
    public static void CreateButtonclick()
    {
        Debug.Log("You clicked the button");
        SceneManager.LoadScene("Game Settings");
    }
}
