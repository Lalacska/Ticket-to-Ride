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
    [SerializeField] private string m_playerName;
    [SerializeField] private bool m_isReady = false;
    //private static ulong m_localClientID;

    public int StatCardID { get { return m_StatCardID; } set { m_StatCardID = value; } }
    public string Color { get { return m_Color; } set { m_Color = value; } }
    public bool myTurn { get { return m_myTurn; } set { m_myTurn = value; } }
    public int ownerID { get { return m_ownerID; } set { m_ownerID = value; } }
    public ulong clientId { get { return m_clientId; } set { m_clientId = value; } }
    public string playerName { get { return m_playerName; } set { m_playerName = value; } }
    public bool isReady { get { return m_isReady; } set { m_isReady = value; } }
    //public ulong localClientID { get { return m_localClientID; } set { m_localClientID = value; } }


    [SerializeField] private List<Card> m_hand;
    [SerializeField] private List<Ticket> m_tickets;
    
    public List<Card> hand { get { return m_hand; } set { m_hand = value; } }
    public List<Ticket> tickets { get { return m_tickets; } set { m_tickets = value; } }
    
    [SerializeField] private GameObject turnIndicator;
    [SerializeField] private TMP_Text Score;
    [SerializeField] private TMP_Text Trains;
    [SerializeField] private TMP_Text Stations;
    [SerializeField] private TMP_Text Cards;
    [SerializeField] private TMP_Text Tickets;


    [SerializeField] private GameObject m_StationObject;
    [SerializeField] private GameObject m_TrainObject;
    public GameObject StationObject { get { return m_StationObject; } set { m_StationObject = value; } }
    public GameObject TrainObject { get { return m_TrainObject; } set { m_TrainObject = value; } }

    private NetworkVariable<FixedString128Bytes> m_ScoreString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_TrainsString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_StationsString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_CardsString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_TicketsString = new NetworkVariable<FixedString128Bytes>();
    [SerializeField] private NetworkVariable<bool> m_isTurn = new NetworkVariable<bool>();

    private Dictionary<FixedString128Bytes, int> m_cardsInHand = new Dictionary<FixedString128Bytes, int>()
        { { "Black", 0 }, { "Blue", 0 }, { "Orange", 0 }, { "Green", 0 }, {"Red", 0 }, {"Pink", 0 }, {"White", 0 },  {"Yellow", 0 }, {"Rainbow", 0 } };

    public Dictionary<FixedString128Bytes, int> cardsInHand { get { return m_cardsInHand; } set { m_cardsInHand = value; } }

    public static Dictionary<FixedString128Bytes, int> m_localCards;

    public Dictionary<FixedString128Bytes, int> localCards { get { return m_localCards; } set { m_localCards = value; } }


    [SerializeField] private NetworkVariable<int> m_stations = new NetworkVariable<int>();
    public NetworkVariable<int> stations { get { return m_stations; } set { m_stations = value; } }

    [SerializeField] private NetworkVariable<int> m_trains = new NetworkVariable<int>();
    public NetworkVariable<int> trains { get { return m_trains; } set { m_trains = value; } }

    private void Start()
    {
        turnIndicator.SetActive(false);
        if (IsServer)
        {
            // Assin the current value based on the current message index value
            stations.Value = 3;
            trains.Value = 50;
            m_ScoreString.Value = "0";
            m_TrainsString.Value = trains.Value.ToString();
            m_StationsString.Value = stations.Value.ToString();
            m_CardsString.Value = "0";
            m_TicketsString.Value = "0";
            m_isTurn.Value = false;
            Score.text = m_ScoreString.Value.ToString();
            Trains.text = m_TrainsString.Value.ToString();
            Stations.text = stations.Value.ToString();
            Cards.text = m_CardsString.Value.ToString();
            Tickets.text = m_TicketsString.Value.ToString();
            m_isTurn.OnValueChanged += ActivateIndicator;
        }
        else
        {
            // Subscribe to the OnValueChanged event
            m_ScoreString.OnValueChanged += OnTextStringChanged;
            m_TrainsString.OnValueChanged += OnTextStringChanged;
            m_StationsString.OnValueChanged += OnTextStringChanged;
            m_CardsString.OnValueChanged += OnTextStringChanged;
            m_TicketsString.OnValueChanged += OnTextStringChanged;
            m_isTurn.OnValueChanged += ActivateIndicator;
            stations.OnValueChanged += OnIntChanged;
            trains.OnValueChanged += OnIntChanged;
            // Log the current value of the text string when the client connected
        }
        
    }

    private void ActivateIndicator(bool previousValue, bool newValue)
    {
        if (newValue)
        {
            turnIndicator.SetActive(true);
        }
        else
        {
            turnIndicator.SetActive(false);
        }
    }

    
    public override void OnNetworkDespawn()
    {
        m_ScoreString.OnValueChanged -= OnTextStringChanged;
        m_TrainsString.OnValueChanged -= OnTextStringChanged;
        m_StationsString.OnValueChanged -= OnTextStringChanged;
        m_CardsString.OnValueChanged -= OnTextStringChanged;
        m_TicketsString.OnValueChanged -= OnTextStringChanged;
        m_isTurn.OnValueChanged -= ActivateIndicator;
        stations.OnValueChanged -= OnIntChanged;
        trains.OnValueChanged -= OnIntChanged;
    }

    private void LateUpdate()
    {
        if (IsServer)
        {
            // It runs when the value and the players card count is not the same
            if (m_CardsString.Value != hand.Count.ToString())
            {
                // First set the value, then runs the CardCheck with the clientId
                m_CardsString.Value = hand.Count.ToString();
                CardCheck(clientId);
                foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
                {
                    //Debug.Log("Cards in hand after: " + kvp.Key + " number: " + kvp.Value);
                }
            }
            if(m_TicketsString.Value != tickets.Count.ToString())
            {
                m_TicketsString.Value = tickets.Count.ToString();
            }
            if(m_StationsString.Value != stations.Value.ToString())
            {
                m_StationsString.Value = stations.Value.ToString();
            }
            if(m_TrainsString.Value != trains.Value.ToString())
            {
                m_TrainsString.Value = trains.Value.ToString();
            }

            if (myTurn)
            {
                m_isTurn.Value = true;
            }
            else
            {
                m_isTurn.Value = false;
            }
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

    private void OnIntChanged(int previousValue, int newValue)
    {
        Stations.text = m_StationsString.Value.ToString();
        Trains.text = m_TrainsString.Value.ToString();
    }



    // This metode chek the cards in the players hand when their list changes and then send the data to the client
    public void CardCheck(ulong clientId)
    {
        if (!IsServer) return;

        // This cleares the keys in the dictionary, so the player won't get extra cards
        foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
        {
          cardsInHand[kvp.Key] = 0;
        }

        // This goes trough the player cards and then the dictionary and when the card has the same color as the key 
        // It adds plus 1 to the value
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

        // Here we make two new strings with the size of the dictionary
        FixedString128Bytes[] cardscolor = new FixedString128Bytes[cardsInHand.Count];
        int[] cardsnumber = new int[cardsInHand.Count];

        // Here we go trough the dictionary and set the elements into the arrays
        for(int i = 0; i < cardsInHand.Count; i++)
        {
            cardscolor[i] = cardsInHand.ElementAt(i).Key;
            cardsnumber[i] = cardsInHand.ElementAt(i).Value;
        }

        // This set the ClientRpc
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        // This sends the two array and the client params to a metode which will run on the clients side
        CardUpdateClientRpc(cardscolor, cardsnumber, clientRpcParams);
    }


    // This ClientRpc runs only the owners client, it puts together the dictionary again and sets it localy
    [ClientRpc]
    public void CardUpdateClientRpc(FixedString128Bytes[] color, int[] number, ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner) return;

        var dictionary = new Dictionary<FixedString128Bytes, int>();

        // This puts the two array together to a dictionary again
        for (int index = 0; index < color.Length; index++)
        {
            dictionary.Add(color[index], number[index]);
        }

        // These clears and sets the dictionary localy to the client
        cardsInHand.Clear();
        cardsInHand = dictionary;

        //localCards.Clear();
        localCards = dictionary;
        //foreach (KeyValuePair<FixedString128Bytes, int> kvp in cardsInHand.ToList())
        //{
        //    Debug.Log("Cards in hand on client: " + kvp.Key + " number: " + kvp.Value);
        //}

        // This calles a metode which sets the card counter localy for the player
        PlayerHand.Instance.setCardsLocaly(cardsInHand);
    }
}

