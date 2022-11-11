using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayerPrefab : Singeltone<SpawnPlayerPrefab>
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject SpawnTo;

    public override void OnNetworkSpawn()
    {
        SpawnServerRpc();
         
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        Instantiate(playerPrefab, SpawnTo.transform).GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
