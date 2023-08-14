using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private ulong m_clientId;

    public int StatCardID { get { return m_StatCardID; } set { m_StatCardID = value; } }
    public string Color { get { return m_Color; } set { m_Color = value; } }
    public bool myTurn { get { return m_myTurn; } set { m_myTurn = value; } }
    public int ownerID { get { return m_ownerID; } set { m_ownerID = value; } }
    public ulong clientId { get { return m_clientId; } set { m_clientId = value; } }


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

    private Dictionary<FixedString128Bytes, int> cardsInHand = new Dictionary<FixedString128Bytes, int>()
        { { "Black", 0 }, { "Blue", 0 }, { "Brown", 0 }, { "Green", 0 }, {"Orange", 0 }, {"Purple", 0 }, {"Yellow", 0 }, {"White", 0 }, {"Rainbow", 0 } };


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
    public override void OnNetworkSpawn()
    {

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
                foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
                {
                    Debug.Log("Cards in hand before: " + kvp.Key +" number: " + kvp.Value);
                }
                CardCheck(clientId);
                foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
                {
                    Debug.Log("Cards in hand after: " + kvp.Key + " number: " + kvp.Value);
                }
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


   public void CardCheck(ulong clientId)
    {
        if (!IsServer) return;


        foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
        {
          cardsInHand[kvp.Key] = 0;
        }
        foreach (Card card in hand)
        {
            foreach(KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
            {
                if(card.Color == kvp.Key)
                {
                    cardsInHand[kvp.Key] +=1;
                }
            }
        }

        FixedString128Bytes[] cardscolor = new FixedString128Bytes[hand.Count];
        int[] cardsnumber = new int[hand.Count];

        for(int i = 0; i < hand.Count; i++)
        {
            cardscolor[i] = cardsInHand.ElementAt(i).Key;
            cardsnumber[i] = cardsInHand.ElementAt(i).Value;
        }
        //foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
        //{
        //    cardscolor.Add(kvp.Key.ToString());
        //    cardsnumber.Add(kvp.Value.ToString());
        //}

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        Debug.Log("Ello");
        CardUpdateClientRpc(cardscolor, cardsnumber, clientRpcParams);
    }

    [ClientRpc]
    public void CardUpdateClientRpc(FixedString128Bytes[] color, int[] number, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Ello 2");
        if (!IsOwner) return;

        Debug.Log("Ello 3");
        var dictionary = new Dictionary<FixedString128Bytes, int>();

        for (int index = 0; index < color.Length; index++)
        {
            dictionary.Add(color[index], number[index]);
        }

        Debug.Log("Ello 4");
        cardsInHand.Clear();
        cardsInHand = dictionary;
        foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
        {
            Debug.Log("Cards in hand on client: " + kvp.Key + " number: " + kvp.Value);
        }
    }
}

