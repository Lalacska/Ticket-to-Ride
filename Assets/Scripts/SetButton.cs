using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
    public class SetButton : Singeltone<SetButton>
    {
        [SerializeField] private GameObject button;
        public void Start()
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
            Instantiate(button).GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
            NetworkManager.ConnectedClients[clientId].PlayerObject.TrySetParent(NetworkObject, false);
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
}