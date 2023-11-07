using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.Collections;

public class SyncNames : Singleton<SyncNames>
{
    private Transform _transform;
    private GameObject child;


    [SerializeField] private TMP_Text playername;
    private NetworkVariable<FixedString128Bytes> PlayerName = new NetworkVariable<FixedString128Bytes>();

    public override void OnNetworkSpawn()
    {
        playername = GetComponent<TMPro.TMP_Text>();
        if (IsServer)
        {
            // Assin the current value based on the current message index value
            PlayerName.Value = playername.text;
        }
        else
        {
            // Log the current value of the text string when the client connected
            Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {PlayerName.Value}");
            playername.text = PlayerName.Value.ToString();
        }
    }


}


