using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ParentPlayerToInSceneNetworkObject : Singeltone<ParentPlayerToInSceneNetworkObject>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TextMeshProUGUI playername;
    public List<PlayerInGame> players = new List<PlayerInGame>();
    public void Start()
    {
        if (IsServer)
        {
            // Server subscribes to the NetworkSceneManager.OnSceneEvent event
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

            // Server player is parented under this NetworkObject

            playername.text = UserData.username;
            SetPlayerParent(NetworkManager.LocalClientId);
        }
    }

    private void SetPlayerParent(ulong clientId)
    {
        if (IsSpawned && IsServer)
        {
            // As long as the client (player) is in the connected clients list
            if (NetworkManager.ConnectedClients.ContainsKey(clientId))
            {
                playername.text = UserData.username;
                SpawnServerRpc(clientId);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        Instantiate(playerPrefab).GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        NetworkManager.ConnectedClients[clientId].PlayerObject.transform.SetParent(NetworkObject.gameObject.transform, false);
        NetworkObject player = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
        NetworkObject name = Instantiate(playername).GetComponent<NetworkObject>();
        name.SpawnWithOwnership(clientId);
        name.transform.SetParent(player.transform, false);
    }


    public void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        // OnSceneEvent is very useful for many things
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.SynchronizeComplete:
            {
                if (sceneEvent.ClientId != NetworkManager.LocalClientId)
                {
                    // Set the newly joined and synchronized client-player as a child of this in-scene placed NetworkObject
                    SetPlayerParent(sceneEvent.ClientId);
                }
                break;
            }
            case SceneEventType.LoadComplete:
                {
                    break;
                }
        }
    }
}