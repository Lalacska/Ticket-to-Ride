using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : Singeltone<LobbyScene>
{
    [SerializeField] private TMP_Text joinCode;
    public static Lobby lobby;


    public void StartButton()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Join-Create Game", LoadSceneMode.Single);
        Debug.Log(lobby.Players);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player);
        }
    }
    public void DisplayCode(string code)
    {
        joinCode.text = code;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
