using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

// This class is set the player objects under a spawend scene network object \\
public class ParentPlayerToInSceneNetworkObject : Singleton<ParentPlayerToInSceneNetworkObject>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TextMeshProUGUI playername;
    public NetworkList<LobbyPlayer> players = new NetworkList<LobbyPlayer>();
    private TextMeshProUGUI setplayername;

    public void Start()
    {
        // Sends the local player data to a ServerRPC to make a player and add it to the list on the server \\ 
        AddPlayerServerRPC(NetworkManager.LocalClientId, UserData.userId, UserData.username);

        if (IsServer)
        {
            // Server subscribes to the NetworkSceneManager.OnSceneEvent event \\
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

            playername.text = UserData.username;
            // Server player is parented under this NetworkObject \\
            SetPlayerParent(NetworkManager.LocalClientId);
        }
        // Spawns player names in serverside with the client id \\
        AddPlayerNameServerRPC(NetworkManager.LocalClientId);

    }

    private void SetPlayerParent(ulong clientId)
    {
        if (IsSpawned && IsServer)
        {
            // As long as the client (player) is in the connected clients list \\
            if (NetworkManager.ConnectedClients.ContainsKey(clientId))
            {
                SpawnServerRpc(clientId);
            }
        }

    }

    // This is a ServerRPC, which means the client send a code to the Server to handle it \\
    // In this case our client has no right to spawn object to the network \\
    // So sends a request to the server to spawn the player object \\
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        // Instantiate and spawn the player object with the client as owner \\
        Instantiate(playerPrefab).GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        // Get the client's player object and set it under a parent object \\
        NetworkManager.ConnectedClients[clientId].PlayerObject.transform.SetParent(NetworkObject.gameObject.transform, false);
    }


    public void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        // OnSceneEvent is very useful for many things \\
        switch (sceneEvent.SceneEventType)
        {
            // This trigers when the client got syncronized with the server \\
            case SceneEventType.SynchronizeComplete:
            {
                if (sceneEvent.ClientId != NetworkManager.LocalClientId)
                {
                        // Set the newly joined and synchronized client-player as a child of this in-scene placed NetworkObject \\
                        SetPlayerParent(sceneEvent.ClientId);
                }
                break;
            }
        }
    }

    // Makes a new lobbyplayer with the data and adds it to the list on the server \\
    [ServerRpc(RequireOwnership = false)]
    public void AddPlayerServerRPC(ulong clientId, string id, string name, ServerRpcParams serverRpcParams = default)
    {
        LobbyPlayer player = new LobbyPlayer(clientId, id, name);
        players.Add(player);
    }

    // Adds player names to the playerobject in serverside \\
    [ServerRpc(RequireOwnership = false)]
    private void AddPlayerNameServerRPC(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        // Get the player object with the clientid \\
        NetworkObject playerobject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        // Finds the player in the list and set the right name to the object \\
        foreach (var a in players)
        {
            if (clientId == a.ClientId)
            {
                playername.text = a.PlayerName.ToString();
            }
        }
        //Instantiate the object, spawn with an owner and set under a parent \\
        NetworkObject nametext = Instantiate(playername).GetComponent<NetworkObject>();
        nametext.SpawnWithOwnership(clientId);
        nametext.transform.SetParent(playerobject.transform, false);
        playername.text = "";

        Debug.Log(players.Count);

        if(clientId == players[0].ClientId) { return; }
        Debug.Log("Don't break pls");
        foreach (var player in players)
        {
            Debug.Log(player);
            NetworkObject playerobject1 = NetworkManager.Singleton.ConnectedClients[player.ClientId].PlayerObject;
            NetworkObject nametext1 = playerobject.GetComponent<NetworkObject>();
            Debug.Log(playername.text);
            setplayername = nametext.GetComponent<TextMeshProUGUI>();
            Debug.Log(setplayername.text);
            SyncNamesClientRPC(player.PlayerName.ToString());
        }
    }
    [ClientRpc]
    public void SyncNamesClientRPC(string name)
    {
        if (IsHost) return;
        Debug.Log(name);
        Debug.Log(setplayername.text);
        setplayername.text = name;
        Debug.Log(setplayername.text);

    }
}