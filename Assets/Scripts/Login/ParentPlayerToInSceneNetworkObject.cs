using Unity.Netcode;
using UnityEngine;

public class ParentPlayerToInSceneNetworkObject : Singeltone<ParentPlayerToInSceneNetworkObject>
{
    [SerializeField] private GameObject playerPrefab;
    public  void Start()
    {
        if (IsServer)
        {
            // Server subscribes to the NetworkSceneManager.OnSceneEvent event
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;

            // Server player is parented under this NetworkObject
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
                SpawnServerRpc(clientId);
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        Instantiate(playerPrefab).GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        NetworkManager.ConnectedClients[clientId].PlayerObject.TrySetParent(NetworkObject, false);
    }

    public void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        // OnSceneEvent is very useful for many things
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadEventCompleted:
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
            
            //// The C2S_SyncComplete event tells the server that a client-player has:
            //// 1.) Connected and Spawned
            //// 2.) Loaded all scenes that were loaded on the server at the time of connecting
            //// 3.) Synchronized (instantiated and spawned) all NetworkObjects in the network session
            //case SceneEventData.SceneEventTypes.C2S_SyncComplete:
            //    {
            //        // As long as we are not the server-player
            //        if (sceneEvent.ClientId != NetworkManager.LocalClientId)
            //        {
            //            // Set the newly joined and synchronized client-player as a child of this in-scene placed NetworkObject
            //            SetPlayerParent(sceneEvent.ClientId);
            //        }
            //        break;
            //    }
        }
    }
}