using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : Singeltone<LobbyScene>
{
    public GameObject codeToSpawn;
    [SerializeField] private TMP_Text joinCode;
    public static Lobby lobby;


    public void StartButton()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameBoard", LoadSceneMode.Single);
        //Debug.Log(lobby.Players);
        //foreach (Player player in lobby.Players)
        //{
        //    Debug.Log(player);
        //}
    }

    public void Button()
    {

        //GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        Debug.Log(IsHost);
        Debug.Log(IsClient);
    }

    private void Start()
    {
        if (IsHost)
        {
            joinCode.text = UserData.lobby.LobbyCode;
        }
    }


}
