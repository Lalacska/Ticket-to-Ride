using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    [SerializeField] private Transform Bruh;

    [SerializeField] private GameObject BlackPrefab;
    [SerializeField] private GameObject BluePrefab;
    [SerializeField] private GameObject OrangePrefab;
    [SerializeField] private GameObject GreenPrefab;
    [SerializeField] private GameObject RedPrefab;
    [SerializeField] private GameObject PinkPrefab;
    [SerializeField] private GameObject WhitePrefab;
    [SerializeField] private GameObject YellowPrefab;
    [SerializeField] private GameObject RainbowPrefab;
    [SerializeField] private GameObject Cards;



    // Here we make list for the diffrent kinds of cards piles. \\
    [SerializeField] private List<Card> m_deck;
    [SerializeField] private List<Ticket> m_tickets;
    [SerializeField] private List<Ticket> m_specialTickets;
    [SerializeField] private List<Ticket> m_choosedTicket;
    [SerializeField] private List<GameObject> m_ticketObjects;
    [SerializeField] private List<Ticket> m_drawnTickets;
    public List<Card> deck { get { return m_deck; } set { m_deck = value; } }
    public List<Ticket> tickets { get { return m_tickets; } set { m_tickets = value; } }
    public List<Ticket> specialTickets { get { return m_specialTickets; } set { m_specialTickets = value; } }
    public List<Ticket> choosedTicket { get { return m_choosedTicket; } set { m_choosedTicket = value; } }
    public List<GameObject> ticketObjects { get { return m_ticketObjects; } set { m_ticketObjects = value; } }
    public List<Ticket> drawnTickets { get { return m_drawnTickets; } set { m_drawnTickets = value; } }



    public List<Card> m_board;
    public List<Card> m_discardPile;
    public List<Card> board { get { return m_board; } set { m_board = value; } }
    public List<Card> discardPile { get { return m_discardPile; } set { m_discardPile = value; } }

    [SerializeField] private GameObject stationsObject;
    [SerializeField] private GameObject routesObject;
    [SerializeField] private GameObject tunnelsObject;

    [SerializeField] private List<GameObject> m_stations;
    [SerializeField] private List<GameObject> m_routes;
    //[SerializeField] private List<GameObject> m_tunnels;
    public List<GameObject> stations { get { return m_stations; } set { m_stations = value; } }
    public List<GameObject> routes { get { return m_routes; } set { m_routes = value; } }
    //public List<GameObject> tunnels { get { return m_tunnels; } set { m_tunnels = value; } }


    // This part is for the slots/areas where the cards will be shown/displayed. \\
    [SerializeField] private Transform m_cardsInHand;
    [SerializeField] private Transform m_discardPileDestination;

    public Transform cardsInHand { get { return m_cardsInHand; } set { m_cardsInHand = value; } }
    public Transform discardPileDestination { get { return m_discardPileDestination; } set { m_discardPileDestination = value; } }

    private bool m_isDeckEmpty = false;
    public bool isDeckEmpty { get { return m_isDeckEmpty; } set { m_isDeckEmpty = value; } }



    [SerializeField] private RectTransform[] cardSlotButtons;


    // This is bools for available slots, the cards can be playsed in. \\
    [SerializeField] private bool[] availableCardSlots;
    [SerializeField] private bool[] availableDiscardPileCardSlots;

    [SerializeField] private GameObject ticketActionArea;
    [SerializeField] private GameObject choosingtArea;
    [SerializeField] private GameObject ticketArea;

    // This is a int for keeping track of Rainbow Cards. \\
    private int RainbowCount = 0;

    private int PlayerPickCount = 0;

    private int PickedRainbowCard = 0;



    private bool firstChoice = true;


    // These GameObjects are Serialized for later use. \\
    [SerializeField] private GameObject cardslot1;
    [SerializeField] private GameObject cardslot2;
    [SerializeField] private GameObject cardslot3;
    [SerializeField] private GameObject cardslot4;
    [SerializeField] private GameObject cardslot5;
    [SerializeField] private Button cardpile;

    // This is for the Card counter. (Tells how many cards are left) \\ 
    [SerializeField] private TMP_Text cardPiletxt;
    [SerializeField] private TMP_Text ticketPiletxt;

    private NetworkVariable<FixedString128Bytes> m_cardPileString = new NetworkVariable<FixedString128Bytes>();
    private NetworkVariable<FixedString128Bytes> m_ticketPileString = new NetworkVariable<FixedString128Bytes>();


    private int _CardID = 1;

    private CardSlotsID slot;

    [SerializeField] private int BlackCardCounter = 0;
    [SerializeField] private int BlueCardCounter = 0;
    [SerializeField] private int OrangeCardCounter = 0;
    [SerializeField] private int GreenCardCounter = 0;
    [SerializeField] private int RedCardCounter = 0;
    [SerializeField] private int PinkCardCounter = 0;
    [SerializeField] private int WhiteCardCounter = 0;
    [SerializeField] private int YellowCardCounter = 0;
    [SerializeField] private int RainbowCardCounter = 0;


    private GameObject city;
    private PlayerStat player;
    private Route route;

    #endregion Variables

    private void Awake()
    {
        // Initialize lists to store cards, the board, discard pile, and game objects for stations.
        deck = new List<Card>();
        board = new List<Card>();
        discardPile = new List<Card>();
        stations = new List<GameObject>();

        // Initialize ticket ownership for standard tickets.
        foreach (Ticket ticket in tickets.ToList())
        {
            ticket.ownerID = 0;
        }

        // Initialize ticket ownership for special tickets.
        foreach (Ticket ticket in specialTickets.ToList())
        {
            ticket.ownerID = 0;
        }

        // Populate the 'routes' list with objects representing game routes.
        foreach (Transform childRoute in routesObject.transform)
        {
            Route r = childRoute.GetComponent<Route>();

            // If 'Route' component is not found in the child, add its children to 'routes'.
            if (r == null)
            {
                foreach (Transform child in childRoute.transform)
                {
                    routes.Add(child.gameObject);
                }
            }
            else
            {
                routes.Add(childRoute.gameObject);
            }
        }

        // Populate the 'routes' list with objects representing game tunnels and set their type.
        foreach (Transform childTunnel in tunnelsObject.transform)
        {
            Route r = childTunnel.GetComponent<Route>();

            // If 'Route' component is not found in the child, add its children to 'routes'.
            if (r == null)
            {
                foreach (Transform child in childTunnel.transform)
                {
                    r = child.GetComponent<Route>();
                    r.SetType(true);
                    routes.Add(child.gameObject);
                }
            }
            else
            {
                r.SetType(true);
                routes.Add(childTunnel.gameObject);
            }
        }

        // Populate the 'stations' list with game objects representing stations.
        foreach (Transform childStation in stationsObject.transform)
        {
            stations.Add(childStation.gameObject);
        }
    }

    // This method is called when the program starts.It initializes game-related UI and actions based on whether it's the server or a client.
    private void Start()
    {
        if (IsServer)
        {
            // If this instance is the server, update and display card and ticket pile counts.
            m_cardPileString.Value = deck.Count.ToString();
            m_ticketPileString.Value = tickets.Count.ToString();
            cardPiletxt.text = m_cardPileString.Value.ToString();
            ticketPiletxt.text = m_ticketPileString.Value.ToString();

            // Register event handlers for changes in card and ticket pile counts.
            m_cardPileString.OnValueChanged += OnTextStringChanged;
            m_ticketPileString.OnValueChanged += OnTextStringChanged;

            // Perform automatic drawing of cards on the server.
            AutomaticDrawPile();
        }
        else
        {
            // If this is a client, register event handlers for UI updates.
            m_cardPileString.OnValueChanged += OnTextStringChanged;
            m_ticketPileString.OnValueChanged += OnTextStringChanged;
        }
    }

    // This method runs every frame & updates the scenes. \\
    private void Update()
    {
        if (IsServer)
        {
            //This two check if the card or ticket pile count has changed and update the UI if necessary.
            if (m_cardPileString.Value != deck.Count.ToString())
            {
                m_cardPileString.Value = deck.Count.ToString();
            }
            if (m_ticketPileString.Value != tickets.Count.ToString())
            {
                m_ticketPileString.Value = tickets.Count.ToString();
            }

            // If the deck is empty reshuffles discard pile.
            if (deck.Count == 0 && discardPile.Count != 0)
            {

                int rCounter = 0;
                int colorCounter = 0;
                // This checks how many rainbow card is in the discard pile
                foreach (Card card in discardPile.ToList())
                {
                    if (card.Color == "Rainbow")
                    {
                        rCounter++;
                    }
                    else
                    {
                        colorCounter++;
                    }
                }

                // This interrupts the metode if there ar more then 2 rainbowcard and the discardPile has only 5 or less cards
                if (rCounter > 2 && discardPile.Count <= 5 || colorCounter < 3) { return; }

                isDeckEmpty = true;

                // Reset the deck with cards from the discard pile and empty the discard pile.
                deck = new List<Card>(new List<Card>(discardPile));
                EmptyDiscardPile();
                discardPile.Clear();

                // Automatically draw cards from the newly reset deck.
                AutomaticDrawPile();
            }
        }
    }

    //This goes trough the cards in the discardPile and calls the card's ReleaseCard metode
    public void EmptyDiscardPile()
    {
        foreach (Card card in discardPile)
        {
            card.ReleaseCard();
        }
    }

    // When the network despawns this is unsubscribe from the onValueChanged events
    public override void OnNetworkDespawn()
    {
        m_cardPileString.OnValueChanged -= OnTextStringChanged;
        m_ticketPileString.OnValueChanged -= OnTextStringChanged;
    }

    // This sets the text value for the deck counters
    private void OnTextStringChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        cardPiletxt.text = m_cardPileString.Value.ToString();
        ticketPiletxt.text = m_ticketPileString.Value.ToString();
    }

    // This method is responsible for dealing four cards to each player at the start of the game from the deck.
    public List<Card> DealCards(int clientID, List<Card> hand)
    {
        for (int i = 0; i < 4; i++)
        {
            // Select a random card from the deck.
            Card cardVariables = deck[UnityEngine.Random.Range(0, deck.Count)];
            if (cardVariables == null) { return null; }

            // Get the corresponding card prefab based on its color.
            GameObject randomCardPrefab = GetPrefabByColor(cardVariables.Color, true);

            // Create a network object for the card and spawn it.
            NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
            _card.GetComponent<NetworkObject>().Spawn(true);

            // Get the Card component from the spawned card.
            Card randCard = _card.GetComponent<Card>();

            // If the card has no CardID, assign a unique CardID.
            if (randCard.CardID == 0)
            {
                randCard.CardID = _CardID;
                _CardID++;
            }

            // Set the hand index to associate the card with a player.
            randCard.handIndex = clientID;

            // Mark the card as not played and remove it from the deck while adding it to the player's hand.
            randCard.hasBeenPlayed = false;
            deck.Remove(randCard);
            hand.Add(randCard);

            // Debug log to display the card's color.
            Debug.Log(randCard.Color);
        }
        return hand;
    }

    // This methode deals 3 tickets to the player, when firstDeal is true at the start of the game, it deals an aditional special ticket too
    // Then it returns the list with tickets
    public List<Ticket> DealTickets(int clientID, List<Ticket> ticketsInHand = default, bool firstDeal = false)
    {
        // Initialize the list to store the dealt tickets.
        ticketsInHand = new List<Ticket>();

        if (firstDeal)
        {
            // If it's the first deal, select a random special ticket from the list, assign the owner ID,
            // remove it from the special ticket list, and add it to the list that the method will return.
            Ticket specialticket = specialTickets[UnityEngine.Random.Range(0, specialTickets.Count)];
            specialticket.ownerID = clientID;
            specialTickets.Remove(specialticket);
            ticketsInHand.Add(specialticket);
        }

        // Deal 3 normal tickets to the player.
        for (int i = 0; i < 3; i++)
        {
            // Select a random normal ticket from the list, assign the owner ID,
            // remove it from the normal ticket list, and add it to the list that the method will return.

            if(tickets.Count != 0)
            {
                Ticket ticket = tickets[UnityEngine.Random.Range(0, tickets.Count)];
                ticket.ownerID = clientID;
                tickets.Remove(ticket);
                ticketsInHand.Add(ticket);
            }
        }
        // Log the number of tickets in the player's hand.
        Debug.Log("Tickets: " + ticketsInHand.Count);

        return ticketsInHand;
    }

    // This method is responsible for automatically filling out the game board with cards.
    public void AutomaticDrawPile()
    {

        if (deck.Count >= 1)
        {
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true && deck.Count != 0)
                {
                    // Get a random card from the deck and retrieve its associated prefab based on color.
                    Card cardVariables = deck[UnityEngine.Random.Range(0, deck.Count)];
                    if (cardVariables == null) return;
                    GameObject randomCardPrefab = GetPrefabByColor(cardVariables.Color, true);
                    Debug.Log(cardVariables.Color);

                    // Create a network object from the prefab and get the Card component.
                    NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
                    Card randCard = _card.GetComponent<Card>();

                    // Check if the network object is already spawned and spawns if its not
                    if (!_card.IsSpawned)
                    {
                        _card.GetComponent<NetworkObject>().Spawn(true);
                    }

                    // If the card has no CardID, assign a unique CardID
                    if (randCard.CardID == 0)
                    {
                        randCard.CardID = _CardID;
                        _CardID++;
                    }

                    // Set the card's position, mark it as not played, and update the card slot availability.
                    _card.transform.position = cardSlotButtons[i].position;
                    randCard.hasBeenPlayed = false;
                    availableCardSlots[i] = false;

                    // Remove the card from the deck and add it to the game board.
                    deck.Remove(randCard);
                    board.Add(randCard);

                    // Call a method to handle card placement in card slots.
                    CardSlots(randCard, i);

                    // Check if a Rainbow card is drawn onto the board.
                    if (randCard.Color == "Rainbow")
                    {
                        RainbowCount++;
                        Debug.Log("Rainbow Count: " + RainbowCount);
                    }

                    // If there are 5 cards on the board, perform additional checks.
                    if (availableCardSlots[4] == false)
                    {
                        // If more than 3 Rainbow cards are on the board simultaneously, clear the board.
                        if (RainbowCount >= 3)
                        {
                            CheckCards();
                            AutomaticDrawPile();
                        }
                    }
                    Debug.Log(randCard.Color);
                }
            }
        }
    }

    // This metode sets a prefab according to the color, and increaseas or decrases the counter for that color
    public GameObject GetPrefabByColor(string color, bool increase)
    {
        GameObject prefab = null;

        if (color == "Black")
        {
            prefab = BlackPrefab;
            if (increase)
            {
                BlackCardCounter++;
            }
            else
            {
                BlackCardCounter--;
            }
        }
        else if (color == "Blue")
        {
            prefab = BluePrefab;
            if (increase)
            {
                BlueCardCounter++;
            }
            else
            {
                BlueCardCounter--;
            }
        }
        else if (color == "Orange")
        {
            prefab = OrangePrefab;
            if (increase)
            {
                OrangeCardCounter++;
            }
            else
            {
                OrangeCardCounter--;
            }
        }
        else if (color == "Green")
        {
            prefab = GreenPrefab;
            if (increase)
            {
                GreenCardCounter++;
            }
            else
            {
                GreenCardCounter--;
            }
        }
        else if (color == "Red")
        {
            prefab = RedPrefab;
            if (increase)
            {
                RedCardCounter++;
            }
            else
            {
                RedCardCounter--;
            }
        }
        else if (color == "Pink")
        {
            prefab = PinkPrefab;
            if (increase)
            {
                PinkCardCounter++;
            }
            else
            {
                PinkCardCounter--;
            }
        }
        else if (color == "White")
        {
            prefab = WhitePrefab;
            if (increase)
            {
                WhiteCardCounter++;
            }
            else
            {
                WhiteCardCounter--;
            }
        }
        else if (color == "Yellow")
        {
            prefab = YellowPrefab;
            if (increase)
            {
                YellowCardCounter++;
            }
            else
            {
                YellowCardCounter--;
            }
        }
        else if (color == "Rainbow")
        {
            prefab = RainbowPrefab;
            if (increase)
            {
                RainbowCardCounter++;
            }
            else
            {
                RainbowCardCounter--;
            }
        }
        return prefab;
    }

    // This method checks the cards on the board and ensures that there are not more than 3 Rainbow cards at once.
    public void CheckCards()
    {
        for (int b = 0; b < availableDiscardPileCardSlots.Length; b++)
        {
            availableDiscardPileCardSlots[b] = false;
            availableCardSlots[b] = true;

            // Check if the board is not empty.
            bool isEmpty = !board.Any();
            if (board != null && !isEmpty)
            {
                // Get the first card on the board and move it to the discard pile.
                Card delete = board[0];
                delete.transform.position = discardPileDestination.position;
                board.Remove(delete);
                discardPile.Add(delete);
            }

            // Reset the Rainbow card count to zero.
            RainbowCount = 0;
        }
    }

    // This methos finds the cardslot the user presed & passes the id off. \\ 
    public void CardSlots(Card card, int i)
    {
        CardSlotsID cardslotsid = slot;
        if (i == 0)
        {
            cardslotsid = cardslot1.GetComponent<CardSlotsID>();
        }
        else if (i == 1) {
            cardslotsid = cardslot2.GetComponent<CardSlotsID>();
        }
        else if (i == 2)
        {
            cardslotsid = cardslot3.GetComponent<CardSlotsID>();
        }
        else if (i == 3)
        {
            cardslotsid = cardslot4.GetComponent<CardSlotsID>();
        }
        else if (i == 4)
        {
            cardslotsid = cardslot5.GetComponent<CardSlotsID>();
        }
        cardslotsid.cardslotCardID = card.CardID;
    }

    // Draws tunnel cards from the deck then puts them to the discard pile, and send the right color to the client so it can visually spawn it
    [ServerRpc(RequireOwnership = false)]
    public void TunnelDrawServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log(" Tunnel Draw Server Rpc deck: " + deck.Count);

        // Select a random card from the deck and retrieve its associated prefab based on color.
        Card cardVariables = deck[UnityEngine.Random.Range(0, deck.Count)];
        GameObject randomCardPrefab = GetPrefabByColor(cardVariables.Color, true);

        Debug.Log("A");

        // Get a network object from the pool with the help of the prefab
        NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
         
        // Check if the network object is already spawned and spawns if its not
        if (!_card.IsSpawned)
        {
            _card.GetComponent<NetworkObject>().Spawn(true);
        }
        

        Debug.Log("B");

        Card randCard = _card.GetComponent<Card>();

        // If the card has no CardID, assign a unique CardID.
        if (randCard.CardID == 0)
        {
            randCard.CardID = _CardID;
            _CardID++;
        }

        // Set the card's position, mark it as played, and update the deck and discard pile.
        _card.transform.position = discardPileDestination.position;
        randCard.hasBeenPlayed = true;
        deck.Remove(randCard);
        discardPile.Add(randCard);


        // Set the target client for this action.
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { serverRpcParams.Receive.SenderClientId }
            }
        };

        // Determine if the current instance is the host.
        bool host = false;
        if (IsHost) { host = true; }

        // Call the 'DrawTunnelCardClientRpc' method on the client to draw the tunnel card and inform the host status.
        CardSelector.Instance.DrawTunnelCardClientRpc(randCard.Color.ToString(), host, clientRpcParams);
    }

    public int CardSlotCheck()
    {
        int counter = 0;
        foreach (bool avalaible in availableCardSlots)
        {
            if (avalaible)
            {
                counter++;
            }
        }
        return counter;
    }



    /// <summary>
    /// This part is for the different functions for Draw cards that are being used in the game. \\
    /// </summary>

    #region DrawCards

    // This metode is called when the player choose the Draw Card action
    public void DrawCards()
    {
        // Enable the buttons on the board, allowing the client to draw cards.
        CardButtonsEnable_Disable(true);

        // Disable the action chooser for the current turn.
        TurnM.Instance.Enable_DisableActionChooser(false);

        // Reset the pick count and Rainbow card count to 0.
        PlayerPickCount = 0;
        PickedRainbowCard = 0;
    }


    // This method is triggered when the player clicks on a card button or the pile.
    // It receives an int which identifies which button the player pushed
    public void DrawCardButtons(int button)
    {
        // Check that the pick count and Rainbow card count do not exceed their limits.
        if (PlayerPickCount < 2 && PickedRainbowCard < 1)
        {
            // Send the button ID to a server RPC for further processing.
            BoardButtonsServerRpc(button);
        }
        else if (PlayerPickCount > 1)
        {
            Debug.Log("Du kan ikke tr�kke flere kort!" +
                " Du m� maks tr�kke 2 kort pr tur!");
        }
    }

    // This method receives a 'buttonId' when called, and uses it to determine the correct slot for the button or,
    // if the player clicked on the pile, selects a random card from the deck.
    [ServerRpc(RequireOwnership = false)]
    public void BoardButtonsServerRpc(int buttonId, ServerRpcParams serverRpcParams = default)
    {
        bool isRainbow = false;
        Card card = null;
        ulong clientID = serverRpcParams.Receive.SenderClientId;

        // Set the target client for the response.
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID }
            }
        };

        // Get the card based on the button ID. This can either be from a slot on the board or a random card from the deck.
        card = GetCard(buttonId);
        
        // If the player picked a Rainbow card, set the 'isRainbow' flag to true.
        Debug.Log(card.Color);
        if (card.Color == "Rainbow")
        {
            Debug.Log("Its a rainbow card");
            isRainbow = true;
        }

        // Call the 'DrawCardHandlerClientRpc' method to handle the card drawing on the client side.
        DrawCardHandlerClientRpc(isRainbow, buttonId, clientID, clientRpcParams);
    }

    // This method is used for changing the positions of the cards.
    [ServerRpc(RequireOwnership = false)]
    public void AddCardToHandServerRpc(int buttonId, ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        Card card = GetCard(buttonId);
        PlayerStat stat = null;

        // Find and set the client's PlayerStat and its index in the player stat list.
        for (int i = 0; i < PlayerManager.Instance.stats.Count; i++)
        {
            if (clientId == PlayerManager.Instance.stats[i].clientId)
            {
                stat = PlayerManager.Instance.stats[i];
            }
        }


        Debug.Log(card.Color);

        // Set the card's position to be in the player's hand.
        card.transform.position = cardsInHand.position;

        // If the card was picked from the board, this make sure the slot where it was is available again and removes the card from the board list
        if (buttonId > 0 && buttonId < 6)
        {
            availableCardSlots[buttonId - 1] = true;
            board.Remove(card);
        }
        else if (buttonId == 6)
        {
            deck.Remove(card);
        }

        // Adds the card to the player's hand
        stat.hand.Add(card);
    }

    // Calls AutomaticDrawPile from the server
    [ServerRpc(RequireOwnership = false)]
    public void FillTheBoardServerRpc(ServerRpcParams serverRpcParams = default)
    {
        AutomaticDrawPile();
    }




    // Handles clients hand on client
    [ClientRpc]
    public void DrawCardHandlerClientRpc(bool isRainbow, int buttonId, ulong clientID, ClientRpcParams clientRpcParams)
    {
        // If a Rainbow card is picked and the player has already picked a card, show an error message and return.
        if (isRainbow && PlayerPickCount >= 1 && buttonId != 6)
        {
            Debug.Log("Wrong Pick! Pick a new card");
            return;
        }
        else if (isRainbow && buttonId != 6)
        {
            PickedRainbowCard++;
            RainbowCount--;
        }

        // Call the 'AddCardToHandServerRpc' method to handle adding the card to the player's hand.
        AddCardToHandServerRpc(buttonId, clientID);
        PlayerPickCount++;

        // If the player has picked two cards or a Rainbow card, disable the buttons, draw new cards, and end the player's turn.
        if (PlayerPickCount >= 2 || PickedRainbowCard > 0)
        {
            // Disable the buttons, draw new cards, and end the player's turn.
            CardButtonsEnable_Disable(false);
            FillTheBoardServerRpc();
            TurnM.Instance.EndTurn();
        }
        Debug.Log("Player pick count = " + PlayerPickCount);
    }

    // This gets the right card from the right slot and sends back
    public Card GetCard(int buttonId)
    {
        Card card = null;

        // Gets the slot component, or a random card
        switch (buttonId)
        {
            case 1:
                slot = cardslot1.GetComponent<CardSlotsID>();
                break;
            case 2:
                slot = cardslot2.GetComponent<CardSlotsID>();
                break;
            case 3:
                slot = cardslot3.GetComponent<CardSlotsID>();
                break;
            case 4:
                slot = cardslot4.GetComponent<CardSlotsID>();
                break;
            case 5:
                slot = cardslot5.GetComponent<CardSlotsID>();
                break;
            case 6:
                card = deck[UnityEngine.Random.Range(0, deck.Count)];
                break;
        }

        // If the player picked a card from the board, iterate through the cards on the board
        // and set the 'card' if the CardID matches.
        if (buttonId > 0 && buttonId < 6)
        {
            foreach (Card m_card in board.ToList())
            {
                if (m_card.CardID == slot.cardslotCardID)
                {
                    card = m_card;
                }
            }
        }

        return card;
    }

    // This metode gets the button component from the gameobjects and enable or disable the button interaction depending if its on or off
    public void CardButtonsEnable_Disable(bool enable)
    {
        Button btn1 = cardslot1.GetComponent<Button>();
        Button btn2 = cardslot2.GetComponent<Button>();
        Button btn3 = cardslot3.GetComponent<Button>();
        Button btn4 = cardslot4.GetComponent<Button>();
        Button btn5 = cardslot5.GetComponent<Button>();

        if (enable)
        {
            btn1.interactable = true;
            btn2.interactable = true;
            btn3.interactable = true;
            btn4.interactable = true;
            btn5.interactable = true;
            cardpile.interactable = true;
        }
        else
        {
            btn1.interactable = false;
            btn2.interactable = false;
            btn3.interactable = false;
            btn4.interactable = false;
            btn5.interactable = false;
            cardpile.interactable = false;
        }
    }

    #endregion


    /// <summary>
    /// This part is for the different functions for Tickets that are being used in the game. \\
    /// </summary>
    #region Tickets

    // This metode is called, when the player choose Draw Destination Ticket action
    public void DrawDestinatonTickets()
    {
        // This disables the choosing area
        TurnM.Instance.Enable_DisableActionChooser(false);
        // This starts a ServerRpc metode, so the things will run on the server, not on the client
        DrawDestinatonTicketsServerRpc();

    }

    // This metod gets the dealt tickets from another metode, then sets the target client, 
    // Convert the list into an array, so we can send it trough the network and sets thr drawnTickets list
    [ServerRpc(RequireOwnership = false)]
    public void DrawDestinatonTicketsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        // This clear the list, so we don't use cards from an earlier drawing
        drawnTickets.Clear();
        List<Ticket> dealtTickets;
        // Gets, thesender clientID
        ulong clientID = serverRpcParams.Receive.SenderClientId;
        dealtTickets = DealTickets(Convert.ToInt32(clientID));

        //Set the target client
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID }
            }
        };

        // Make a new array, then get all the ticketId in it
        int[] ticketIDs = new int[dealtTickets.Count];
        for (int i = 0; i < dealtTickets.Count; i++)
        {
            ticketIDs[i] = dealtTickets[i].ticketID;
        }

        // Sets the list, with the dealt tickets
        drawnTickets = dealtTickets;
        SpawnTicketsLocalyClientRpc(ticketIDs, clientID, clientRpcParams);

    }

    // This metode gets the ticket ids from the server, and spawns the game objects localy on the client and adds them to a list
    [ClientRpc]
    public void SpawnTicketsLocalyClientRpc(int[] ticketIds, ulong clientId, ClientRpcParams clientRpcParams = default)
    {
        UserData.clientId = clientId;
        // Sets the ticket choosing area true
        ticketActionArea.SetActive(true);
        // Cleares the object list, so we don't use objects from an earlier drawing
        ticketObjects.Clear();

        // This goes trough the ticketIds array
        for (int i = 0; i < ticketIds.Length; i++)
        {
            // This gets the right gameobject by the ticketId and spawns it if it's not null
            GameObject ticketToSpawn = TicketSpawner.Instance.TicketChoser(ticketIds[i]);
            if (ticketToSpawn != null)
            {
                // Spawns the ticket and set parent
                GameObject go_ticket = Instantiate(ticketToSpawn);
                go_ticket.transform.SetParent(choosingtArea.transform, true);

                // Edits the ticket transform, so it shows in the correct place
                RectTransform rt = go_ticket.GetComponent<RectTransform>();
                rt.position = choosingtArea.transform.position;

                // Get the Ticket component, and ads the ticket to the choosed ticket list and the gameobject to the ticketObject list
                Ticket spawendTicket = go_ticket.GetComponent<Ticket>();
                choosedTicket.Add(spawendTicket);
                ticketObjects.Add(go_ticket);
            }
        }
    }

    // This metode converts the impoortant elements from the choosedTicket list into two array, and sends them to a ServerRPC
    // Also checks and sends an indication if the player doesn't have the required amount of tickets selected and also updates the objects
    public void SendChoosenTicketsToServer()
    {
        int[] ticketIds = new int[choosedTicket.Count];
        bool[] ticketStatus = new bool[choosedTicket.Count];
        int checkcounter = 0;
        int neededCardAmount = 1;

        // This goes trough the choosedTicket list and add 1 to the checkcounter if a ticket is chosen
        for (int i = 0; i < choosedTicket.Count; i++)
        {
            if (choosedTicket[i].isChosen)
            {
                checkcounter++;
            }
        }

        // This checks if this is the first time, and if it is sets the required amount to 2
        if (firstChoice)
        {
            neededCardAmount = 2;
        }
        // This checks if the chosed ticket amounr it under the required one, sends a warning and returns if it is. 
        if (checkcounter < neededCardAmount)
        {
            Debug.Log("You need to choose minimum " + neededCardAmount + " ticket!");
            return;
        }

        // This populates the arrays, and goes troug the objects if the object is chosen it get re-parented under another gameobject
        // if it's not selected it gets destroyed
        ticketArea.SetActive(true);
        for (int i = 0; i < choosedTicket.Count; i++)
        {
            ticketIds[i] = choosedTicket[i].ticketID;
            ticketStatus[i] = choosedTicket[i].isChosen;
            foreach (GameObject go_ticket in ticketObjects.ToList())
            {
                Ticket go_ticket_ticket = go_ticket.GetComponent<Ticket>();
                if (go_ticket_ticket.ticketID == ticketIds[i])
                {
                    if (ticketStatus[i])
                    {
                        // If the ticket is chosed this gets the transform and re-parents it
                        RectTransform rt = go_ticket.GetComponent<RectTransform>();
                        go_ticket.transform.SetParent(ticketArea.transform, true);
                        DeactivateInteractionWithTicket(go_ticket);
                    }
                    else
                    {
                        Destroy(go_ticket);
                    }
                }
            }
        }
        // These deacticates the areas, so it won't be visible for the player
        ticketActionArea.SetActive(false);
        ticketArea.SetActive(false);


        CheckChoosenTicketsServerRpc(ticketIds, ticketStatus, firstChoice);

        // If its not the start of the game, this ends the player turn
        if (!firstChoice)
        {
            TurnM.Instance.EndTurn();
        }
        firstChoice = false;
    }

    // This metode, update the client's tickets list in their status object on the server
    [ServerRpc(RequireOwnership = false)]
    public void CheckChoosenTicketsServerRpc(int[] ticketIds, bool[] ticketStatus, bool choice, ServerRpcParams serverRpcParams = default)
    {
        int index = 1000;
        List<Ticket> ticketList = new List<Ticket>();
        PlayerStat stat = null;

        // This finds and sets the client's PlayerStat and index of its playerStat from the player stat list 
        for (int i = 0; i < PlayerManager.Instance.stats.Count; i++)
        {
            if (serverRpcParams.Receive.SenderClientId == PlayerManager.Instance.stats[i].clientId)
            {
                index = i;
                stat = PlayerManager.Instance.stats[i];
            }
        }

        //if the stat is null it returns
        if (stat == null) return;


        // This goes trough the ticketIds array, and where the id is the same as the stats id from the list, check whats the status is for the ticket
        // where the ticketStatus is true it adds the ticket to the ticketList
        for (int i = 0; i < ticketIds.Length; i++)
        {
            // If this is the start if the game, this goes trough the players ticket list, and if the ids are matching
            // and the status is trough it adds the ticket to the list
            if (choice)
            {
                if (stat.tickets[i].ticketID == ticketIds[i])
                {
                    if (ticketStatus[i])
                    {
                        ticketList.Add(stat.tickets[i]);
                    }
                }
            }
            else
            {
                // If this is the player action, it goes trough the drawnTicket list, which we sat earlier and
                // If one of the tickets id is the same as the id from the array and the status is true it adds the ticket to the list
                foreach (Ticket StatTicket in drawnTickets.ToList())
                {
                    if (StatTicket.ticketID == ticketIds[i])
                    {
                        if (ticketStatus[i])
                        {
                            ticketList.Add(StatTicket);
                        }
                    }
                }
            }
        }

        Debug.Log("Blep 7");
        // If this is the start if the game this sets the list to the clients ticket list,
        // and indicates that the player is ready for start the game
        if (choice)
        {
            PlayerManager.Instance.stats[index].tickets = ticketList;
            PlayerManager.Instance.stats[index].isReady = true;
        }
        else
        {
            // If this is a players action, it goes trough the ticketList, we set befire, and adds the tickets individually to the list
            foreach (Ticket ticket in ticketList)
            {
                PlayerManager.Instance.stats[index].tickets.Add(ticket);
            }
        }

    }

    // Tis metode turns off the emission on the ticket, and disables the collider, so the player can's interact with it anymore
    public void DeactivateInteractionWithTicket(GameObject go_ticket)
    {
        Ticket ticket = go_ticket.GetComponent<Ticket>();
        ticket.TurnEmissionOff();
        BoxCollider collider = go_ticket.GetComponent<BoxCollider>();
        collider.enabled = false;

    }

    public void OpenCloseTicketArea()
    {
        if (ticketArea.activeInHierarchy)
        {
            ticketArea.SetActive(false);
        }
        else
        {
            ticketArea.SetActive(true);
        }
    }

    #endregion Tickets


    /// <summary>
    /// This part is for the different functions for Station that are being used in the game. \\
    /// </summary>
    #region Station

    // Activates emission and collider on the available stations
    public void ActiveStation()
    {
        // Disable the action chooser for the current turn.
        TurnM.Instance.Enable_DisableActionChooser(false);

        // This goes trough the stations and turn on theirs emission if they are not taken, and also enabales collider, so the player can interact with it
        foreach (GameObject go in stations)
        {

            Station station = go.GetComponent<Station>();
            if (!station.isTaken.Value)
            {
                CapsuleCollider collider = go.GetComponent<CapsuleCollider>();
                if (collider.enabled == false)
                {
                    collider.enabled = true;
                }
                station.TurnEmissionOn();
            }
        }
    }

    // Spawns Station objects on the server
    [ServerRpc(RequireOwnership = false)]
    public void SpawnStationServerRpc(string _stationName, ServerRpcParams serverRpcParams = default)
    {
        ulong clientID = serverRpcParams.Receive.SenderClientId;

        // Gets the correct city from the stations list
        foreach (GameObject go in stations)
        {
            Station station = go.GetComponent<Station>();
            if (station.stationName == _stationName)
            {
                city = go;
            }
        }

        // Gets the correct PlayerStat by the player's id
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientID)
            {
                player = stat;
            }
        }

        //If the city or the player is null it returns the metode
        if (city == null || player == null)
        {
            Debug.Log("Something is wrong" + city + " " + player.clientId);
            return;
        }

        // Instantiate the player's station object and spawn it as a network object.
        GameObject spawnedObjectTransform = Instantiate(player.StationObject);
        NetworkObject no = spawnedObjectTransform.GetComponent<NetworkObject>();
        no.Spawn(true);

        // Set the spawned station object's parent to the city, adjust its position, and decrement the player's station count.
        spawnedObjectTransform.transform.SetParent(city.transform, true);
        spawnedObjectTransform.transform.position = new Vector3(city.transform.position.x, city.transform.position.y + 1, city.transform.position.z);

        player.stations.Value--;
    }

    // This finds the correct station on tje client and sends the name to a server rpc
    public void ChooseCity(string _stationName)
    {
        foreach (GameObject go in stations)
        {
            Station station = go.GetComponent<Station>();
            if (station.stationName == _stationName)
            {
                // This calls the server rpc that supposed to spawn the station
                SpawnStationServerRpc(_stationName);
            }
        }
    }

    // This turns off the emission and the collider on the stations
    public void TurnOffHighlightStation()
    {
        foreach (GameObject go in stations)
        {
            Station station = go.GetComponent<Station>();
            CapsuleCollider collider = go.GetComponent<CapsuleCollider>();
            collider.enabled = false;
            station.TurnEmissionOff();
        }
    }


    #endregion

    /// <summary>
    /// This part is for the different functions for Train that are being used in the game. \\
    /// </summary>
    #region Train

    // Activates Higlight for the unclaimed routes and tunnels
    public void ActivateRoutes()
    {
        // Disable the action chooser for the current turn.
        TurnM.Instance.Enable_DisableActionChooser(false);

        // Iterate through the routes and tunnels to highlight unclaimed routes for the player to select.
        foreach (Transform childRoute in routesObject.transform)
        {
            Route r = childRoute.GetComponent<Route>();
            if (r != null)
            {
                if (r.isClaimed.Value) return; // Skip if the route is already claimed.
                r.HighlightOn();
            }
            else
            {
                foreach (Transform child in childRoute.transform)
                {
                    r = child.GetComponent<Route>();
                    if (r.isClaimed.Value) return; // Skip if the route is already claimed.
                    r.HighlightOn();
                }
            }
        }
        foreach (Transform childTunnel in tunnelsObject.transform)
        {
            Route r = childTunnel.GetComponent<Route>();
            if (r != null)
            {
                if (r.isClaimed.Value) return; // Skip if the route is already claimed.
                r.HighlightOn();
            }
            else
            {
                foreach (Transform child in childTunnel.transform)
                {
                    r = child.GetComponent<Route>();
                    if (r.isClaimed.Value) return; // Skip if the route is already claimed.
                    r.HighlightOn();
                }
            }
        }
    }

    // This is called when the player clicks on the Claim Route button on the action chooser
    public void ClaimRouteAction()
    {
        // This checks if the player is the host or not
        bool host = false;
        if (IsHost) { host = true; }

        // This calls the server Rpc with the boolean
        CheckTrainsNumberServerRpc(host);
    }

    // Finds the player by ID and sets it's train amount then calls another metode
    [ServerRpc(RequireOwnership = false)]
    public void CheckTrainsNumberServerRpc(bool host, ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        int m_trainNumber = 0;

        // Set the target client
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        // Find and set the player using the client ID.
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientId)
            {
                player = stat;
            }
        }

        // If the player is found and it has trains set the train number
        if (player != null && player.trains.Value > 0)
        {
            m_trainNumber = player.trains.Value;
        }
        else { return; }

        // Call client rpc
        CheckPlayerCardsClientRpc(m_trainNumber, host, clientRpcParams);

    }

    // Makes sure that only the right client could call the next metode
    [ClientRpc]
    public void CheckPlayerCardsClientRpc(int trains, bool host, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner && !host) return;
        CheckPlayerCards(trains);
    }

    // This metode check the cards and only highlights the routes that the player has cards for
    public void CheckPlayerCards(int trains)
     {   
        int highestNumber = 0; 
        TurnM.Instance.Enable_DisableActionChooser(false);
        Dictionary<FixedString128Bytes, int> localcards = PlayerStat.Instance.localCards;
        int rainbowcards = localcards.ElementAt(8).Value;

        // Find the highest number of cards that are not Rainbow cards.
        foreach (KeyValuePair<FixedString128Bytes, int> kvp in localcards.ToList())
        {
            if (kvp.Value > highestNumber && kvp.Key != "Rainbow")
            {
                highestNumber = kvp.Value;
            }
        }

        // This goes trough the routes and enables the ones that the player can afford
        foreach (GameObject go in routes.ToList())
        {
            Route route = go.GetComponent<Route>();

            // This runs if the route color is grey, and the length of the route is less or equal to the max catd amount 
            if (route.routeColor == "Grey" && route.lenght <= highestNumber + rainbowcards && (route.neededLocomotiv == 0 || route.neededLocomotiv <= rainbowcards) 
                && route.lenght <= trains)
            {
                foreach (Transform child in route.transform)
                {
                    GameObject childObject = child.gameObject;
                    Collider collider = childObject.GetComponent<Collider>();
                    collider.enabled = true;
                }
                route.HighlightOn();
            }
            // This runs when the route is not Grey, it goes trough a dictionary that holds the player cards localy
            else
            {
                foreach (KeyValuePair<FixedString128Bytes, int> kvp in localcards.ToList())
                {
                    // If the route color and the key color matches it checks if the player has enough card for the route
                    if (route.routeColor == kvp.Key && route.lenght <= kvp.Value + rainbowcards)
                    {
                        foreach (Transform child in route.transform)
                        {
                            GameObject childObject = child.gameObject;
                            Collider collider = childObject.GetComponent<Collider>();
                            collider.enabled = true;
                        }
                        route.HighlightOn();
                    }
                }
            }
        }
    }

    // This goes trough all the routes and turns off the highlight and the collider
    public void TurnOffHighlightRoutes()
    {
        foreach (GameObject go in routes.ToList())
        {
            Route route = go.GetComponent<Route>();
            route.HighlightOff();
            foreach (Transform child in route.transform)
            {
                GameObject childObject = child.gameObject;
                Collider collider = childObject.GetComponent<Collider>();
                collider.enabled = false;
            }
        }
        
    }

    // This metode checks the route and calls a server RPC to claim it
    public void ChooseRoute(string _routeName, string routeColor, bool isTunnel)
    {
        Debug.Log("Choose Route");
        foreach (GameObject go in routes)
        {
            Route route = go.GetComponent<Route>();
            if (route.routeName == _routeName && route.routeColor == routeColor)
            {
                SpawnRoutesServerRpc(_routeName,routeColor,isTunnel);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnRoutesServerRpc(string _routeName, string routeColor, bool isTunnel, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("SpawnRoutesServerRpc");
        ulong clientID = serverRpcParams.Receive.SenderClientId;

        // Search for the matching route based on its name and color.
        foreach (GameObject go in routes)
        {
            Route _route = go.GetComponent<Route>();
            if (_route.routeName == _routeName && _route.routeColor == routeColor)
            {
                route = _route;
            }
        }

        // Find the player using the client ID.
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientID)
            {
                player = stat;
            }
        }

        // Check for potential issues and return if any.
        if (route == null || player == null)
        {
            Debug.Log("Something is wrong" + city + " " + player.clientId);
            return;
        }

        // Spawn train objects on the route's tiles and adjust their positions and rotations.
        foreach (GameObject tile in route.Tiles.ToList())
        {
            // Instantiate and spawn a train object for the player.
            GameObject spawnedObjectTransform = Instantiate(player.TrainObject);
            NetworkObject no = spawnedObjectTransform.GetComponent<NetworkObject>();
            no.Spawn(true);

            // Set the parent of the spawned object to the route and adjust its position.
            spawnedObjectTransform.transform.SetParent(route.transform, true);
            spawnedObjectTransform.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.6F, tile.transform.position.z);

            // Adjust the rotation of the spawned object based on whether it's a tunnel.
            if (isTunnel)
            {
                spawnedObjectTransform.transform.rotation = tile.transform.rotation;
                spawnedObjectTransform.transform.rotation *= Quaternion.Euler(0, 0, 90);
            }
            else
            {
                spawnedObjectTransform.transform.rotation = tile.transform.rotation;
            }
        }

        // Reduce the player's available train count by the length of the claimed route.
        player.trains.Value = player.trains.Value - route.lenght;
        player.score.Value = player.score.Value + route.points;

    }


    #endregion

}
