using Unity.Netcode;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using Unity.Collections;

public class SyncNames : Singleton<SyncNames>
{
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
            // Subscribe to the OnValueChanged event
            PlayerName.OnValueChanged += OnTextStringChanged;
            // Log the current value of the text string when the client connected
            Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {PlayerName.Value}");
        }
    }
    public override void OnNetworkDespawn()
    {
        PlayerName.OnValueChanged -= OnTextStringChanged;
    }
    private void OnTextStringChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        // Just log a notification when m_TextString changes
        Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {PlayerName.Value}");
        playername.text = PlayerName.Value.ToString();
    }

    private void LateUpdate()
    {
        if (!IsServer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playername.text = PlayerName.Value.ToString();
        }
    }

}


