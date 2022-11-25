using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.Collections;

public class SyncNames : Singleton<SyncNames>
{
    [SerializeField] private TextMeshProUGUI playername;
    private NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>();

    public void Start()
    {
        if (!IsServer) { return; }
        
        
        if (IsOwner)
        {
            SearchServerRpc(NetworkManager.LocalClientId);
            if(name != null)
            {
                PlayerName.Value = name;
                playername.text = PlayerName.Value.ToString();
            }
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void SearchServerRpc(ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        foreach (var player in ParentPlayerToInSceneNetworkObject.Instance.players)
        {
            if(player.ClientId == clientId)
            {
                
            }
        }
    }
}

