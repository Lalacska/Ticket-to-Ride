using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStat : Singleton<PlayerStat>
{
    [SerializeField] private int m_StatCardID;
    [SerializeField] private string m_Color;
    [SerializeField] private bool m_myTurn = false;
    [SerializeField] private int m_ownerID;

    public int StatCardID { get { return m_StatCardID; } set { m_StatCardID = value; } }
    public string Color { get { return m_Color; } set { m_Color = value; } }
    public bool myTurn { get { return m_myTurn; } set { m_myTurn = value; } }
    public int ownerID { get { return m_ownerID; } set { m_ownerID = value; } }


    [SerializeField] private List<Card> m_hand;
    [SerializeField] private List<Ticket> m_tickets;
    public List<Card> hand { get { return m_hand; } set { m_hand = value; } }
    public List<Ticket> tickets { get { return m_tickets; } set { m_tickets = value; } }

    [SerializeField] private TMP_Text Score;
    [SerializeField] private TMP_Text Trains;
    [SerializeField] private TMP_Text Stations;
    [SerializeField] private TMP_Text Cards;
    [SerializeField] private TMP_Text Tickets;

    private NetworkVariable<FixedString128Bytes> m_ScoreString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_TrainsString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_StationsString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_CardsString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_TicketsString = new NetworkVariable<FixedString128Bytes>();


    private void Start()
    {
        if (IsServer)
        {
            // Assin the current value based on the current message index value
            m_ScoreString.Value = "0";
            m_TrainsString.Value = "50";
            m_StationsString.Value = "3";
            m_CardsString.Value = "0" /*hand.Count.ToString()*/;
            m_TicketsString.Value = "0";
            Score.text = m_ScoreString.Value.ToString();
            Trains.text = m_TrainsString.Value.ToString();
            Stations.text = m_StationsString.Value.ToString();
            Cards.text = m_CardsString.Value.ToString();
            Tickets.text = m_TicketsString.Value.ToString();

        }
        else
        {
            // Subscribe to the OnValueChanged event
            m_ScoreString.OnValueChanged += OnTextStringChanged;
            m_TrainsString.OnValueChanged += OnTextStringChanged;
            m_StationsString.OnValueChanged += OnTextStringChanged;
            m_CardsString.OnValueChanged += OnTextStringChanged;
            m_TicketsString.OnValueChanged += OnTextStringChanged;
            // Log the current value of the text string when the client connected
        }
    }

    public override void OnNetworkDespawn()
    {
        m_ScoreString.OnValueChanged -= OnTextStringChanged;
        m_TrainsString.OnValueChanged -= OnTextStringChanged;
        m_StationsString.OnValueChanged -= OnTextStringChanged;
        m_CardsString.OnValueChanged -= OnTextStringChanged;
        m_TicketsString.OnValueChanged -= OnTextStringChanged;
    }

    private void LateUpdate()
    {
        if (IsServer)
        {
            if (m_CardsString.Value != hand.Count.ToString())
            {
                m_CardsString.Value = hand.Count.ToString();
            }
            if(m_TicketsString.Value != tickets.Count.ToString())
            {
                m_TicketsString.Value = tickets.Count.ToString();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
        }
        else if(Input.GetKeyDown(KeyCode.I))
        {

        }

        Score.text = m_ScoreString.Value.ToString();
        Trains.text = m_TrainsString.Value.ToString();
        Stations.text = m_StationsString.Value.ToString();
        Cards.text = m_CardsString.Value.ToString();
        Tickets.text = m_TicketsString.Value.ToString();
    }

    private void OnTextStringChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        Score.text = m_ScoreString.Value.ToString();
        Trains.text = m_TrainsString.Value.ToString();
        Stations.text = m_StationsString.Value.ToString();
        Cards.text = m_CardsString.Value.ToString();
        Tickets.text = m_TicketsString.Value.ToString();
    }
}

