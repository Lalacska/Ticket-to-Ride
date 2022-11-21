using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScene : Singleton<LobbyScene>
{
    public GameObject codeToSpawn;
    [SerializeField] private TMP_Text joinCode;
    [SerializeField] private Button Spawnbutton;
    public static Lobby lobby;


    // Start is called before the first frame update
    private void Start()
    {
        //Write out lobby code
        joinCode.text = UserData.lobby.LobbyCode;
    }

    //When the host clicks it, it change everyone's  scene to the GameBoard scene
    public void StartButton()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameBoard", LoadSceneMode.Single);
    }

    //Calls leave methode when the player click on it
    public void CloseButton()
    {
        LobbyManager.Instance.LeaveLobby();
    }

    //Calls a ServerRPC which spawns a green button
    public void ReadyButton()
    {
        SpawnServerRpc(NetworkManager.LocalClientId);
    }

    //Gets the client object and spawn a green button owned by the client under the object
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        NetworkObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        NetworkObject GreenButton = Instantiate(Spawnbutton).GetComponent<NetworkObject>();
        GreenButton.SpawnWithOwnership(clientId);
        GreenButton.transform.SetParent(player.transform, false);
    }

}
