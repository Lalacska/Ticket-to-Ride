using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class TextSyncTest : Singleton<TextSyncTest>
{
    [SerializeField] private TMP_Text text;
    /// Create your 128 byte fixed string NetworkVariable
    private NetworkVariable<FixedString128Bytes> m_TextString = new NetworkVariable<FixedString128Bytes>();

    private string[] m_Messages ={ "50",
            "8+",
            "10",
            "125"
        };

    private int m_MessageIndex = 0;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Assin the current value based on the current message index value
            m_TextString.Value = m_Messages[m_MessageIndex];
        }
        else
        {
            // Subscribe to the OnValueChanged event
            m_TextString.OnValueChanged += OnTextStringChangedScores;
            // Log the current value of the text string when the client connected
            Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {m_TextString.Value}");
        }
    }

    public override void OnNetworkDespawn()
    {
        m_TextString.OnValueChanged -= OnTextStringChangedScores;
    }
    private void OnTextStringChangedScores(FixedString128Bytes previous, FixedString128Bytes current)
    {
        // Just log a notification when m_TextString changes
        Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {m_TextString.Value}");
        text.text = m_TextString.Value.ToString();
    }

    private void LateUpdate()
    {
        if (!IsServer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_MessageIndex++;
            m_MessageIndex %= m_Messages.Length;
            m_TextString.Value = m_Messages[m_MessageIndex];
            Debug.Log($"Server-{NetworkManager.LocalClientId}'s TextString = {m_TextString.Value}");
            text.text = m_TextString.Value.ToString();
        }
    }

}
