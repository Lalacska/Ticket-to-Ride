using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScene : Singeltone<LobbyScene>
{
    public GameObject codeToSpawn;
    [SerializeField] private TMP_Text joinCode;
    [SerializeField] private Button Spawnbutton;
    public static Lobby lobby;

    private void Start()
    {
        joinCode.text = UserData.lobby.LobbyCode;
    }
    public void StartButton()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameBoard", LoadSceneMode.Single);
        //Debug.Log(lobby.Players);
        //foreach (Player player in lobby.Players)
        //{
        //    Debug.Log(player);
        //}
    }

    public void CloseButton()
    {
        LobbyManager.Instance.LeaveLobby();
    }

    public void Button()
    {
        Debug.Log("a");
        SpawnServerRpc(NetworkManager.LocalClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        NetworkObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        NetworkObject GreenButton = Instantiate(Spawnbutton).GetComponent<NetworkObject>();
        GreenButton.SpawnWithOwnership(clientId);
        GreenButton.transform.SetParent(player.transform, false);
        //GreenButton.TrySetParent(player, false);
    }

}
