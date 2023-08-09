using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;



namespace Assets.Scripts.Players
{
    public class ChangeStringText : Singleton<ChangeStringText>
    {
        private TMP_Text Txt_String;
        /// Create your 128 byte fixed string NetworkVariable
        private NetworkVariable<FixedString128Bytes> m_TextString = new NetworkVariable<FixedString128Bytes>();

        public override void OnNetworkSpawn()
        {
            Txt_String = GetComponent<TMPro.TMP_Text>();

            if (IsServer)
            {
                // Assin the current value based on the current message index value
                m_TextString.Value = Txt_String.text;
            }
            else
            {
                // Subscribe to the OnValueChanged event
                m_TextString.OnValueChanged += OnTextStringChanged;
                // Log the current value of the text string when the client connected
                Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {m_TextString.Value}");
            }
        }
        public override void OnNetworkDespawn()
        {
            m_TextString.OnValueChanged -= OnTextStringChanged;
        }
        private void OnTextStringChanged(FixedString128Bytes previous, FixedString128Bytes current)
        {
            // Just log a notification when m_TextString changes
            Debug.Log($"Client-{NetworkManager.LocalClientId}'s TextString = {m_TextString.Value}");
            Txt_String.text = m_TextString.Value.ToString();
        }

        private void LateUpdate()
        {
            if (!IsServer)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Txt_String.text = m_TextString.Value.ToString();
            }
        }

    }
}
