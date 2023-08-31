using Assets.Scripts.Cards;
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
    [SerializeField] private GameObject BrownPrefab;
    [SerializeField] private GameObject GreenPrefab;
    [SerializeField] private GameObject OrangePrefab;
    [SerializeField] private GameObject PurplePrefab;
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

    private bool DestroyWithSpawner;

    private CardSlotsID slot;

    [SerializeField] private int BlackCardCounter = 0;
    [SerializeField] private int BlueCardCounter = 0;
    [SerializeField] private int BrownCardCounter = 0;
    [SerializeField] private int GreenCardCounter = 0;
    [SerializeField] private int OrangeCardCounter = 0;
    [SerializeField] private int PurpleCardCounter = 0;
    [SerializeField] private int WhiteCardCounter = 0;
    [SerializeField] private int YellowCardCounter = 0;
    [SerializeField] private int RainbowCardCounter = 0;


    #endregion Variables

    private void Awake()
    {
        deck = new List<Card>();
        board = new List<Card>();
        discardPile = new List<Card>();

        foreach(Ticket ticket in tickets.ToList())
        {
            ticket.ownerID = 0;
        }
        foreach (Ticket ticket in specialTickets.ToList())
        {
            ticket.ownerID = 0;
        }
    }

    // This method runs, when the programs starts. \\
    private void Start()
    {
        if (IsServer)
        {
            m_cardPileString.Value = deck.Count.ToString(); 
            m_ticketPileString.Value = tickets.Count.ToString();
            cardPiletxt.text = m_cardPileString.Value.ToString();
            ticketPiletxt.text = m_ticketPileString.Value.ToString();
            m_cardPileString.OnValueChanged += OnTextStringChanged;
            m_ticketPileString.OnValueChanged += OnTextStringChanged;
            AutomaticDrawPile();
        }
        else
        {
            m_cardPileString.OnValueChanged += OnTextStringChanged;
            m_ticketPileString.OnValueChanged += OnTextStringChanged;
        }
    }

    // This method runs every frame & updates the scenes. \\

    private void Update()
    {
        if (IsServer)
        {
            if (m_cardPileString.Value != deck.Count.ToString())
            {
                m_cardPileString.Value = deck.Count.ToString();
            }
            if (m_ticketPileString.Value != tickets.Count.ToString())
            {
                m_ticketPileString.Value = tickets.Count.ToString();
            }
            if(deck.Count == 0)
            {
                isDeckEmpty = true;
                deck = new List<Card>(new List<Card>(discardPile));
                discardPile.Clear();
                AutomaticDrawPile();
            }
        }

        if (!IsOwner) return;

        if (Input.GetKeyUp(KeyCode.N))
        {
            Transform spawnedObjectTransform =  Instantiate(Bruh);
            spawnedObjectTransform.position = new Vector3(0, 5, 0);
            NetworkObject no = spawnedObjectTransform.GetComponent<NetworkObject>();
            no.SpawnWithObservers = false;
            no.Spawn(true);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Card");
            if(obj != null)
            {
                NetworkObject ob = obj.GetComponent<NetworkObject>();
                ob.NetworkShow(1);
            }
        } else if (Input.GetKeyUp(KeyCode.G))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Card");
            if (obj != null)
            {
                NetworkObject ob = obj.GetComponent<NetworkObject>();
                ob.NetworkHide(1);
                Debug.Log("HEY");
                //SetVisibilityClientRpc(false);
            }
        }

    }

    public override void OnNetworkDespawn()
    {
        m_cardPileString.OnValueChanged -= OnTextStringChanged;
        m_ticketPileString.OnValueChanged -= OnTextStringChanged;
    }
    private void OnTextStringChanged(FixedString128Bytes previous, FixedString128Bytes current)
    {
        cardPiletxt.text = m_cardPileString.Value.ToString();
        ticketPiletxt.text = m_ticketPileString.Value.ToString();
    }


    /// <summary>
    /// This part is for all the different Draw functions that are being used in the game. \\
    /// </summary>

    #region DrawFunctions


    // This methode deals 4 card to every player at the start of the game  from the deck
    public List<Card> DealCards(int clientID, List<Card> hand)
    {
        for (int i = 0; i < 4; i++)
        {
            Card cardVariables = deck[UnityEngine.Random.Range(0, deck.Count)];
            if (cardVariables == null) { return null; }

            GameObject randomCardPrefab = CardColorByNumber(0, cardVariables.Color);
            NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
            _card.GetComponent<NetworkObject>().Spawn(true);
            Card randCard = _card.GetComponent<Card>();

            //Card randCard = deck[UnityEngine.Random.Range(0, deck.Count)];
            if (randCard.CardID == 0)
            {
                randCard.CardID = _CardID;
                _CardID++;
            }

            randCard.handIndex = clientID;

            //_card.transform.position = cardSlots[i].position;
            randCard.hasBeenPlayed = false;

            deck.Remove(randCard);
            hand.Add(randCard);

          
            Debug.Log(randCard.Color);
            //return;
        }
        return hand;
    }

    // This methode deals 3 tickets to the player, when firstDeal is true at the start of the game, it deals an aditional special ticket too
    // Then it returns the list with tickets
    public List<Ticket> DealTickets(int clientID, List<Ticket> ticketsInHand = default, bool firstDeal = false) 
    {
        ticketsInHand = new List<Ticket>();
        if (firstDeal)
        {
            // Getting a random ticket from the list that has all the special tickets
            // Then sets ownerId for the ticket, it removes it from the same list, and ads it to the list that the methode will return
            Ticket specialticket = specialTickets[UnityEngine.Random.Range(0, specialTickets.Count)];
            specialticket.ownerID = clientID;
            specialTickets.Remove(specialticket);
            ticketsInHand.Add(specialticket);
        }
        for (int i = 0; i < 3; i++)
        {
            // Getting a random ticket from the list that has all the normal tickets
            // Then sets ownerId for the ticket, it removes it from the same list, and ads it to the list that the methode will return
            Ticket ticket = tickets[UnityEngine.Random.Range(0, tickets.Count)];
            ticket.ownerID = clientID;
            tickets.Remove(ticket);
            ticketsInHand.Add(ticket);
        }
        m_ticketPileString.Value = tickets.Count.ToString();
        Debug.Log("Tickets: "+ticketsInHand.Count);
        return ticketsInHand;
    }

    // This method is for the automaticly fils out the board. \\
    public void AutomaticDrawPile()
    {
        //Card randCard = deck[0];

        if (deck.Count >= 1)
        {
            //CardVariables randCard = deck[UnityEngine.Random.Range(0, deck.Count)];
            for (int i = 0; i < availableCardSlots.Length; i++)
            {
                if (availableCardSlots[i] == true && deck.Count != 0)
                {
                    Card cardVariables = deck[UnityEngine.Random.Range(0, deck.Count)];
                    if(cardVariables == null) return; 
                    GameObject randomCardPrefab = CardColorByNumber(0, cardVariables.Color);
                    Debug.Log(cardVariables.Color);
                    if(randomCardPrefab == null) return;
                    NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
                    if(_card != IsSpawned)
                    {
                        _card.GetComponent<NetworkObject>().Spawn(true);
                    }
                    Card randCard = _card.GetComponent<Card>();
                    if (randCard.CardID == 0)
                    {
                        randCard.CardID = _CardID;
                        _CardID++;
                    }

                    //_card.transform.position = cardSlots[i].position;
                    _card.transform.position = cardSlotButtons[i].position;
                    randCard.hasBeenPlayed = false;

                    availableCardSlots[i] = false;
                    deck.Remove(randCard);
                    board.Add(randCard);

                    CardSlots(randCard, i);

                    // Here we check if a Rainbow cards is drawn onto the board. \\
                    if (randCard.Color == "Rainbow")
                    {
                        RainbowCount++;
                        Debug.Log("Rainbow Count: "+RainbowCount);
                    }
                    if (availableCardSlots[4] == false)
                    {
                        // If more than 3 Rainbow cards are on the field at once, the board is cleared. \\
                        if (RainbowCount >= 3)
                        {
                              CheckCards();
                              AutomaticDrawPile();
                        }
                    }
                    Debug.Log(randCard.Color);
                    //return;
                }
            }
        }
        //SyncListsClientRpc(deck, DestinationTicket, SpecialDestinationTicket, board, discardPile);
    }

    private GameObject CardColorByNumber(int number, string color)
    {
        GameObject prefab = null;

        if(1<=number && number <= 12 && BlackCardCounter < 12 || color == "Black" && BlackCardCounter < 12)
        {
            prefab = BlackPrefab;
            BlackCardCounter++;
        }
        else if (13 <= number && number <= 24 && BlueCardCounter < 12 || color == "Blue" && BlueCardCounter < 12)
        {
            prefab = BluePrefab;
            BlueCardCounter++;
        }
        else if (25 <= number && number <= 36 && BrownCardCounter < 12 || color == "Brown" && BrownCardCounter < 12)
        {
            prefab = BrownPrefab;
            BrownCardCounter++;
        }
        else if (37 <= number && number <= 48 && GreenCardCounter < 12 || color == "Green" && GreenCardCounter < 12)
        {
            prefab = GreenPrefab;
            GreenCardCounter++;
        }
        else if (49 <= number && number <= 60 && OrangeCardCounter < 12 || color == "Orange" && OrangeCardCounter < 12)
        {
            prefab = OrangePrefab;
            OrangeCardCounter++;
        }
        else if (61 <= number && number <= 72 && PurpleCardCounter < 12 || color == "Purple" && PurpleCardCounter < 12)
        {
            prefab = PurplePrefab;
            PurpleCardCounter++;
        }
        else if (73 <= number && number <= 84 && WhiteCardCounter < 12 || color == "White" && WhiteCardCounter < 12)
        {
            prefab = WhitePrefab;
            WhiteCardCounter++;
        }
        else if (85 <= number && number <= 96 && YellowCardCounter < 12 || color == "Yellow" && YellowCardCounter < 12)
        {
            prefab = YellowPrefab;
            YellowCardCounter++;
        }
        else if (97 <= number && number <= 110 && RainbowCardCounter < 14 || color == "Rainbow" && RainbowCardCounter < 14)
        {
            prefab = RainbowPrefab;
            RainbowCardCounter++;
        }

        return prefab;
    }

    public GameObject GetPrefabByColor(string color)
    {
        GameObject prefab = null;

        if (color == "Black")
        {
            prefab = BlackPrefab;
        }
        else if (color == "Blue")
        {
            prefab = BluePrefab;
        }
        else if (color == "Brown")
        {
            prefab = BrownPrefab;
        }
        else if (color == "Green")
        {
            prefab = GreenPrefab;
        }
        else if (color == "Orange")
        {
            prefab = OrangePrefab;
        }
        else if (color == "Purple")
        {
            prefab = PurplePrefab;
        }
        else if (color == "White")
        {
            prefab = WhitePrefab;
        }
        else if (color == "Yellow")
        {
            prefab = YellowPrefab;
        }
        else if (color == "Rainbow")
        {
            prefab = RainbowPrefab;
        }
        return prefab;
    }

    public void CardCounterControll(string color)
    {
        if (color == "Black")
        {
            BlackCardCounter--;
        }
        else if (color == "Blue")
        {
           BlueCardCounter--;
        }
        else if (color == "Brown")
        {
           BrownCardCounter--;
        }
        else if (color == "Green")
        {
            GreenCardCounter--;
        }
        else if (color == "Orange")
        {
            OrangeCardCounter--;
        }
        else if (color == "Purple")
        {
            PurpleCardCounter--;
        }
        else if (color == "White")
        {
            WhiteCardCounter--;
        }
        else if (color == "Yellow")
        {
            YellowCardCounter--;
        }
        else if (color == "Rainbow")
        {
           RainbowCardCounter--;
        }
    }


    #endregion DrawFunctions



    // This method is for checking the cards on the board. \\
    // It checks to make sure that there are not more than 3 Rainbow cards at ones. \\
    public void CheckCards()
    {
        for (int b = 0; b < availableDiscardPileCardSlots.Length; b++)
        {
            availableDiscardPileCardSlots[b] = false;
            availableCardSlots[b] = true;
            bool isEmpty = !board.Any();
            if(board != null && !isEmpty)
            {
                Card delete = board[0];
                delete.transform.position = discardPileDestination.position;
                board.Remove(delete);
                discardPile.Add(delete);
            }
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
        else if(i== 1){
            cardslotsid = cardslot2.GetComponent<CardSlotsID>();
        }
        else if(i == 2)
        {
            cardslotsid = cardslot3.GetComponent<CardSlotsID>();
        }
        else if(i == 3)
        {
            cardslotsid = cardslot4.GetComponent<CardSlotsID>();
        }
        else if (i == 4)
        {
            cardslotsid = cardslot5.GetComponent<CardSlotsID>();
        }
        cardslotsid.cardslotCardID = card.CardID;
    }

















    #region DrawCards

    // This metode is called when the player choose the Draw Card action
    public void DrawCards()
    {
        // This enables the buttons on the board, so the client can draw cards
        CardButtonsEnable_Disable();
        TurnM.Instance.Enable_DisableActionChooser();

        // Sets the pick count and Rainbow count to 0
        PlayerPickCount = 0;
        PickedRainbowCard = 0;
    }


    // When the player clicks on card button, or the pile this metode runs
    // Its get an int which identifies which button the player pushed
    public void DrawCardButtons(int button)
    {
        // It checks that the pick and the rainbow count is not bigger than it should be
        if(PlayerPickCount < 2 && PickedRainbowCard < 1)
        {
            // This sends the button id to a server rpc
            BoardButtonsServerRpc(button);
        }
        else if (PlayerPickCount > 1)
        {
            Debug.Log("Du kan ikke tr�kke flere kort!" +
                " Du m� maks tr�kke 2 kort pr tur!");
        }
    }

    // This metode gets an buttonId when its called, it uses that to set, the correct slot for the button
    // or if the player clicked on the pile then it gets a random card from the deck
    [ServerRpc(RequireOwnership = false)]
    public void BoardButtonsServerRpc(int buttonId, ServerRpcParams serverRpcParams = default)
    {
        bool isRainbow = false;
        Card card = null;
        ulong clientID = serverRpcParams.Receive.SenderClientId;
        // Set the target client
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientID }
            }
        };

        // Gets the slot component, or a random card
        card = GetCard(buttonId);
        // If the player pick card from the board, it goes trough the lists of the cards thats on the board
        // and set the card if the Ids are matching
       
        Debug.Log(card.Color);
        if (card.Color == "Rainbow")
        {
            Debug.Log("Its a rainbow card");
            isRainbow = true;
        }
        

        DrawCardHandlerClientRpc(isRainbow, buttonId, clientID, clientRpcParams);

    }

    // This method is for changeing the playces of the cards. \\
    [ServerRpc(RequireOwnership = false)]
    public void AddCardToHandServerRpc(int buttonId, ulong clientId, ServerRpcParams serverRpcParams = default)
    {
        Card card = GetCard(buttonId);
        PlayerStat stat = null;

        // This finds and sets the client's PlayerStat and index of its playerStat from the player stat list 
        for (int i = 0; i < PlayerManager.Instance.stats.Count; i++)
        {
            if (clientId == PlayerManager.Instance.stats[i].clientId)
            {
                stat = PlayerManager.Instance.stats[i];
            }
        }


        Debug.Log(card.Color);

        // It sets the card position
        card.transform.position = cardsInHand.position;

        // If the card was picked from the board, this make sure the slot where it was is available again and removes the card from the board list
        if (buttonId > 0 && buttonId < 6)
        {
            availableCardSlots[buttonId-1] = true;
            board.Remove(card);
        }
        else if(buttonId == 6)
        {
            deck.Remove(card);
        }

        // This adds the card to the player's hand
        stat.hand.Add(card);
    }

    // Calls AutomaticDrawPile from the server
    [ServerRpc(RequireOwnership = false)]
    public void FillTheBoardServerRpc(ServerRpcParams serverRpcParams = default)
    {
        AutomaticDrawPile();
    }
    
    [ClientRpc]
    public void DrawCardHandlerClientRpc(bool isRainbow, int buttonId, ulong clientID, ClientRpcParams clientRpcParams)
    {
        Debug.Log("Client Rpc");
        
        if (isRainbow && PlayerPickCount >=1 && buttonId != 6)
        {
            Debug.Log("Wrong Pick! Pick a new card");
            return;
        }
        else if (isRainbow && buttonId != 6)
        {
            PickedRainbowCard++;
        }
        AddCardToHandServerRpc(buttonId, clientID);
        PlayerPickCount++;


        if (PlayerPickCount >= 2 || PickedRainbowCard > 0)
        {
            Debug.Log("Hereeee ");
            // This will disable the buttons, and draw new cards, then end the player turn
            CardButtonsEnable_Disable();
            FillTheBoardServerRpc();
            TurnM.Instance.EndTurn();
        }
        Debug.Log("Player pick count = " + PlayerPickCount);
    }


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

        // If the player pick card from the board, it goes trough the lists of the cards thats on the board
        // and set the card if the Ids are matching
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

    public void ResetPickCounter()
    {
        PlayerPickCount = 0;
    }







    // This metode gets the button component from the gameobjects and enable or disable the button interaction depending if its on or off
    public void CardButtonsEnable_Disable()
    {
        Button btn1 = cardslot1.GetComponent<Button>();
        Button btn2 = cardslot2.GetComponent<Button>();
        Button btn3 = cardslot3.GetComponent<Button>();
        Button btn4 = cardslot4.GetComponent<Button>();
        Button btn5 = cardslot5.GetComponent<Button>();

        if (btn1.interactable)
        {
            btn1.interactable = false;
        }
        else{ btn1.interactable = true; }
        if (btn2.interactable)
        {
            btn2.interactable = false;
        }
        else { btn2.interactable = true; }
        if (btn3.interactable)
        {
            btn3.interactable = false;
        }
        else { btn3.interactable = true; }
        if (btn4.interactable)
        {
            btn4.interactable = false;
        }
        else { btn4.interactable = true; }
        if (btn5.interactable)
        {
            btn5.interactable = false;
        }
        else { btn5.interactable = true; }
        if (cardpile.interactable)
        {
            cardpile.interactable = false;
        }
        else { cardpile.interactable = true; }
    }
    
    #endregion 












    /// <summary>
    /// This part is for all the different functions for Tickets that are being used in the game. \\
    /// </summary>
    #region Tickets

    // This metode is called, when the player choose Draw Destination Ticket action
    public void DrawDestinatonTickets()
    {
        // This disables the choosing area
        TurnM.Instance.Enable_DisableActionChooser();
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
        // Sets the ticket choosing area true
        choosingtArea.SetActive(true);
        // Cleares the object list, so we don't use objects from an earlier drawing
        ticketObjects.Clear();

        // This goes trough the ticketIds array
        for (int i = 0; i < ticketIds.Length; i++)
        {
            // This gets the right gameobject by the ticketId and spawns it if it's not null
            GameObject ticketToSpawn = TicketSpawner.Instance.TicketChoser(ticketIds[i]);
            if(ticketToSpawn != null)
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

    // This metode converts the impoortant elements from the choosedTicket list into two array, and sends them to a SeververRPC
    // Also checks and sends an indication if the player doesn't have the required amount of tickets selected and also updates the objects
    public void SendChoosenTicketsToServer()
    {
        int[] ticketIds = new int[choosedTicket.Count];
        bool[] ticketStatus = new bool[choosedTicket.Count];
        int checkcounter = 0;
        int neededCardAmount = 1;

        // This goes trough the choosedTicket list and add 1 to the checkcounter if a ticket is chosen
        for(int i = 0; i < choosedTicket.Count; i++)
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
            Debug.Log("You need to choose minimum "+ neededCardAmount+ " ticket!" );
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
        choosingtArea.SetActive(false);
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
            if(serverRpcParams.Receive.SenderClientId == PlayerManager.Instance.stats[i].clientId)
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




    #region UnityEngine.Random/Extra 

    // This method shuffles the used/discared cards back into the deck. \\ NOT WORKING!
    public void Shuffle()
    {
        if (discardPile.Count >= 1)
        {
            foreach (Card card in discardPile)
            {
                deck.Add(card);
            }
            discardPile.Clear();
        }
    }

    // This method changes the turn to the next player. \\
    public void SwitchTurn()
    {
        Debug.Log("Du har skiftet tur!");
        PlayerPickCount = 0;
    }


    // This method takes the players cards and plays them. \\
    //public void PlayCardHand(string cardcolor)
    //{
    //    // Here we check for the color of the card. \\
    //    if (cardcolor == "Black")
    //    {
    //        if (IBlackPlayerCount >= 1)
    //        {
    //            IBlackPlayerCount--;
    //            TBlackPlayerCount.text = IBlackPlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Blue")
    //    {
    //        if (IBluePlayerCount >= 1)
    //        {
    //            IBluePlayerCount--;
    //            TBluePlayerCount.text = IBluePlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Brown")
    //    {
    //        if (IBrownPlayerCount >= 1)
    //        {
    //            IBrownPlayerCount--;
    //            TBrownPlayerCount.text = IBrownPlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Green")
    //    {
    //        if (IGreenPlayerCount >= 1)
    //        {
    //            IGreenPlayerCount--;
    //            TGreenPlayerCount.text = IGreenPlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Orange")
    //    {
    //        if (IOrangePlayerCount >= 1)
    //        {
    //            IOrangePlayerCount--;
    //            TOrangePlayerCount.text = IOrangePlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Purple")
    //    {
    //        if (IPurplePlayerCount >= 1)
    //        {
    //            IPurplePlayerCount--;
    //            TPurplePlayerCount.text = IPurplePlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "White")
    //    {
    //        if (IWhitePlayerCount >= 1)
    //        {
    //            IWhitePlayerCount--;
    //            TWhitePlayerCount.text = IWhitePlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Yellow")
    //    {
    //        if (IYellowPlayerCount >= 1)
    //        {
    //            IYellowPlayerCount--;
    //            TYellowPlayerCount.text = IYellowPlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    else if (cardcolor == "Rainbow")
    //    {
    //        if (IRainbowPlayerCount >= 1)
    //        {
    //            IRainbowPlayerCount--;
    //            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
    //        }
    //        else
    //        {
    //            Debug.Log("Du har ikke flere kort af den type tilbage!");
    //        }
    //    }
    //    Debug.Log("Du har fjernet 1 " + cardcolor + " kort");
    //}

    #endregion UnityEngine.Random/Extra



    #region BTNS

    /// <summary>
    /// This region "BTNS" are for all the btn funcktions that are in this script. \\
    /// </summary>


    public void Hey()
    {
        Debug.Log("Hey0");
    }


    

    // This methode is for the card play buttons, it gets an int from the button, sets the color
    // then calls PlayCardHand methode with the color
    public void PlayCardBTN(int button)
    {
        string color = "";
        switch (button)
        {
            case 1: 
                color = "Black";
                break;
            case 2:
                color = "Blue";
                break;
            case 3:
                color = "Brown";
                break;
            case 4:
                color = "Green";
                break;
            case 5:
                color = "Orange";
                break;
            case 6:
                color = "Purple";
                break;
            case 7:
                color = "White";
                break;
            case 8:
                color = "Yellow";
                break;
            case 9:
                color = "Rainbow";
                break;
        }
        //PlayCardHand(color);
    }


    #endregion BTNS
}
