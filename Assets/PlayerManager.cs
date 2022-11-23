using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{

    NetworkList<LobbyPlayer> players = new NetworkList<LobbyPlayer>();
    // Start is called before the first frame update
    void Start()
    {
        if (IsServer)
        {
            // Server subscribes to the NetworkSceneManager.OnSceneEvent event \\
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

            LobbyPlayer player = new LobbyPlayer(NetworkManager.LocalClientId, UserData.userId, UserData.username);
            players.Add(player);
        }
    }

    public void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        // OnSceneEvent is very useful for many things \\
        switch (sceneEvent.SceneEventType)
        {
            // This trigers when the client got syncronized with the server \\
            case SceneEventType.SynchronizeComplete:
                {
                    Debug.Log("Client B");
                    if (sceneEvent.ClientId != NetworkManager.LocalClientId)
                    {
                        Debug.Log("Client A");
                    }
                    break;
                }
        }
    }
    [ClientRpc]
    public void AddPlayerClientRpc(string name, string id, ulong clientId)
    {
        LobbyPlayer player = new LobbyPlayer(clientId, UserData.userId, UserData.username);
        players.Add(player);
    }
}
