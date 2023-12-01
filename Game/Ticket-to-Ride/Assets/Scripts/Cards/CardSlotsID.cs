using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Card ID \\ 
public class CardSlotsID : Singleton<CardSlotsID>
{
    [SerializeField] private int m_slotID;
    [SerializeField] private int m_cardslotCardID;

    public int slotID { get { return m_slotID; } set { m_slotID = value; } }
    public int cardslotCardID { get { return m_cardslotCardID; } set { m_cardslotCardID = value; } }
}
