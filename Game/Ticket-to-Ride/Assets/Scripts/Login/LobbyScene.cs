using Assets.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies;
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

    [SerializeField] private Transform spawnObjectPrefab;
    

    // Start is called before the first frame update
    private void Start()
    {
        //Write out lobby code
        joinCode.text = UserData.lobby.LobbyCode;
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyUp(KeyCode.O))
        {
            Transform spawnedObjectTransform = Instantiate(spawnObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        
    }

    //When the host clicks it, it change everyone's  scene to the GameBoard scene
    public void StartButton()
    {
        // Get all networked objects in the scene
        var networkObjects = FindObjectsOfType<NetworkObject>();

        // Iterate through the network objects and unspawn them
        foreach (var networkObject in networkObjects)
        {
            // Unspawn the network object
            networkObject.Despawn(true);
        }

        // Change the scene after destroying network objects
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

    //Gets the client object, Instantiate and spawn a green button owned by the client, then set the player as its parent
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        NetworkObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        NetworkObject GreenButton = Instantiate(Spawnbutton).GetComponent<NetworkObject>();
        GreenButton.SpawnWithOwnership(clientId);
        GreenButton.transform.SetParent(player.transform, false);
    }

}