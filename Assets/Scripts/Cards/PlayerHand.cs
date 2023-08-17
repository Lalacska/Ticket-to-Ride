using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHand : Singleton<PlayerHand>
{
    private int m_blackCardCount;
    [SerializeField] private TMP_Text m_blackCardText;

    private int m_blueCardCount;
    [SerializeField] private TMP_Text m_blueCardText;

    private int m_brownCardCount;
    [SerializeField] private TMP_Text m_brownCardText;

    private int m_greenCardCount;
    [SerializeField] private TMP_Text m_greenCardText;

    private int m_orangeCardCount;
    [SerializeField] private TMP_Text m_orangeCardText;

    private int m_purpleCardCount;
    [SerializeField] private TMP_Text m_purpleCardText;

    private int m_whiteCardCount;
    [SerializeField] private TMP_Text m_whiteCardText;

    private int m_yellowCardCount;
    [SerializeField] private TMP_Text m_yellowCardText;

    private int m_rainbowCardCount;
    [SerializeField] private TMP_Text m_rainbowCardText;




    // Here we check for the color of the card. \\
    public void setCardsLocaly(Dictionary<FixedString128Bytes, int> dictionary)
    {
        ResetCounters();
        foreach (KeyValuePair<FixedString128Bytes, int> kvp in dictionary.ToList())
        {
            if (kvp.Key == "Black")
            {
                m_blackCardCount = kvp.Value;
                m_blackCardText.text = m_blackCardCount.ToString();
            }
            else if (kvp.Key == "Blue")
            {
                m_blueCardCount = kvp.Value;
                m_blueCardText.text = m_blueCardCount.ToString();
            }
            else if (kvp.Key == "Brown")
            {
                m_brownCardCount = kvp.Value;
                m_brownCardText.text = m_brownCardCount.ToString();
            }
            else if (kvp.Key == "Green")
            {
                m_greenCardCount = kvp.Value;
                m_greenCardText.text = m_greenCardCount.ToString();
            }
            else if (kvp.Key == "Orange")
            {
                m_orangeCardCount = kvp.Value;
                m_orangeCardText.text = m_orangeCardCount.ToString();
            }
            else if (kvp.Key == "Purple")
            {
                m_purpleCardCount = kvp.Value;
                m_purpleCardText.text = m_purpleCardCount.ToString();
            }
            else if (kvp.Key == "White")
            {
                m_whiteCardCount = kvp.Value;
                m_whiteCardText.text = m_whiteCardCount.ToString();
            }
            else if (kvp.Key == "Yellow")
            {
                m_yellowCardCount = kvp.Value;
                m_yellowCardText.text = m_yellowCardCount.ToString();
            }
            else if (kvp.Key == "Rainbow")
            {
                m_rainbowCardCount = kvp.Value;
                m_rainbowCardText.text = m_rainbowCardCount.ToString();

            }
        }

    }

    public void ResetCounters()
    {
        m_blackCardCount = 0;
        m_blueCardCount = 0;
        m_brownCardCount = 0;
        m_greenCardCount = 0;
        m_orangeCardCount = 0;
        m_purpleCardCount = 0;
        m_yellowCardCount = 0;
        m_whiteCardCount = 0;
        m_rainbowCardCount = 0;
    }


}
