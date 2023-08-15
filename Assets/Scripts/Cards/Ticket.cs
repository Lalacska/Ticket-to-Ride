using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    [SerializeField] private int m_ticketID;

    [SerializeField] private int m_ownerID;

    [SerializeField] private int m_score;

    [SerializeField] private string m_station1;

    [SerializeField] private string m_station2;

    [SerializeField] private bool m_isSpecial;

    public int ticketID { get { return m_ticketID; } set { m_ticketID = value; } }

    public int ownerID { get { return m_ownerID; } set { m_ownerID = value; } }

    public int score { get { return m_score; } set { m_score = value; } }

    public string station1 { get { return m_station1; } set { m_station1 = value; } }

    public string station2 { get { return m_station2; } set { m_station2 = value; } }

    public bool isSpecial { get { return m_isSpecial; } set { m_isSpecial = value; } }
}

