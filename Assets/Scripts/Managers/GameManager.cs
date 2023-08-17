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
    public List<Card> deck { get { return m_deck; } set { m_deck = value; } }
    public List<Ticket> tickets { get { return m_tickets; } set { m_tickets = value; } }
    public List<Ticket> specialTickets { get { return m_specialTickets; } set { m_specialTickets = value; } }
    public List<Ticket> choosedTicket { get { return m_choosedTicket; } set { m_choosedTicket = value; } }
    public List<GameObject> ticketObjects { get { return m_ticketObjects; } set { m_ticketObjects = value; } }


    public List<Card> board;
    public List<Card> discardPile;
    


    // This part is for the slots/areas where the cards will be shown/displayed. \\
    public Transform[] cardSlots;
    public Transform[] cardSlotsDestination;
    public Transform[] cardSlotsSpecialDestination;
    public Transform[] discardPileDestination;


    // This is bools for availble slots, the cards can be playsed in. \\
    public bool[] availbleCardSlots;
    public bool[] availbleDestinationCardSlots;
    public bool[] availbleSpecialDestinationCardSlots;
    public bool[] availbleDiscardPileCardSlots;

    [SerializeField] private GameObject choosingtArea;
    [SerializeField] private GameObject ticketArea;

    // This is a int for keeping track of Rainbow Cards. \\
    private int RainbowCount = 0;

    private int PlayerPickCount = 0;

    private int BoardIndex = 0;

    private int CardId = 0;

    private int HandId = 0;

    private FixedString128Bytes lastcard = "";


    // These GameObjects are Serialized for later use. \\
    [SerializeField] private GameObject cardslot1;
    [SerializeField] private GameObject cardslot2;
    [SerializeField] private GameObject cardslot3;
    [SerializeField] private GameObject cardslot4;
    [SerializeField] private GameObject cardslot5;

    // This is for the Card counter. (Tells how many cards are left) \\ 
    public Text deckSizeText;
    public Text decksSizeText;
    public Text deckssSizeText;

    // This part is for the counters of the players cards. \\
    public int IBlackPlayerCount;
    public TMP_Text TBlackPlayerCount;

    public int IBluePlayerCount;
    public TMP_Text TBluePlayerCount;

    public int IBrownPlayerCount;
    public TMP_Text TBrownPlayerCount;

    public int IGreenPlayerCount;
    public TMP_Text TGreenPlayerCount;

    public int IOrangePlayerCount;
    public TMP_Text TOrangePlayerCount;

    public int IPurplePlayerCount;
    public TMP_Text TPurplePlayerCount;

    public int IWhitePlayerCount;
    public TMP_Text TWhitePlayerCount;

    public int IYellowPlayerCount;
    public TMP_Text TYellowPlayerCount;

    public int IRainbowPlayerCount;
    public TMP_Text TRainbowPlayerCount;

    public Text TdiscardPileText;

    private int _CardID = 1;

    private bool DestroyWithSpawner;

    private CardSlotsID slot;

    private int BlackCardCounter = 0;
    private int BlueCardCounter = 0;
    private int BrownCardCounter = 0;
    private int GreenCardCounter = 0;
    private int OrangeCardCounter = 0;
    private int PurpleCardCounter = 0;
    private int WhiteCardCounter = 0;
    private int YellowCardCounter = 0;
    private int RainbowCardCounter = 0;


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
            AutomaticDrawPile();
        }
    }

    // This method runs every frame & updates the scenes. \\
    private void Update()
    {
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

        deckSizeText.text = deck.Count.ToString();
        decksSizeText.text = tickets.Count.ToString();
        //deckssSizeText.text = SpecialDestinationTicket.Count.ToString();
        //discardPileText.text = discardPile.Count.ToString();

        // This if statment runs the automatic board filler until there are no more cards left in the deck. \\
        if(deck.Count >= 1)
        {
            //AutomaticDrawPile();
        }
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

    public List<Ticket> DealTickets(int clientID, List<Ticket> ticketsInHand = default, bool firstDeal = false) 
    {
        ticketsInHand = new List<Ticket>();
        if (firstDeal)
        {
            Ticket specialticket = specialTickets[UnityEngine.Random.Range(0, specialTickets.Count)];
            specialticket.ownerID = clientID;
            specialTickets.Remove(specialticket);
            ticketsInHand.Add(specialticket);
        }
        for (int i = 0; i < 3; i++)
        {
            Ticket ticket = tickets[UnityEngine.Random.Range(0, tickets.Count)];
            ticket.ownerID = clientID;
            tickets.Remove(ticket);
            ticketsInHand.Add(ticket);
        }
        Debug.Log("Tickets: "+ticketsInHand.Count);
        return ticketsInHand;
    }

    // This method is for the Train-Destination Drawpile. \\
    public void Drawcard()
    {

        if (deck.Count >= 1)
        {
            GameObject randomCardPrefab = CardColorByNumber(UnityEngine.Random.Range(1, 111), "nope");
            NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
            _card.GetComponent<NetworkObject>().Spawn(true);
            Card randCard = _card.GetComponent<Card>();
            GameObject rc = transform.parent.GetComponent<GameObject>();
            Debug.Log(rc);
            //Card randCard = deck[UnityEngine.Random.Range(0, deck.Count)];
            for (int i = 0; i < availbleCardSlots.Length; i++)
            {
                if (availbleCardSlots[i] == true)
                {
                    //randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    //NetworkObject c = randCard.transform.GetComponent<NetworkObject>();
                    //c.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = true;

                    availbleCardSlots[i] = false;
                    deck.Remove(randCard);
                    board.Add(randCard);

                    //CardSlots(randCard, i);

                    // Here we check if a Rainbow cards is drawn onto the board. \\
                    if (randCard.Color == "Rainbow")
                    {
                        RainbowCount++;
                        Debug.Log(RainbowCount);
                    }
                    if (availbleCardSlots[4] == false)
                    {
                        // If more than 3 Rainbow cards are on the field at once, the board is cleared. \\
                        if (RainbowCount == 3)
                        {
                            //await CheckCards();
                        }
                    }
                    Debug.Log(randCard.Color);
                    return;
                }
                Debug.Log(cardSlots);
            }
        }
    }

    // This method is for the Destination Drawpile. \\
    public void DrawcardDestination()
    {
        //if (DestinationTicket.Count >= 1)
        //{
        //    Card randCard = DestinationTicket[UnityEngine.Random.Range(0, DestinationTicket.Count)];

        //    for (int i = 0; i < availbleDestinationCardSlots.Length; i++)
        //    {
        //        if (availbleDestinationCardSlots[i] == true)
        //        {
        //            //randCard.gameObject.SetActive(true);
        //            randCard.handIndex = i;

        //            //randCard.transform.position = cardSlotsDestination[i].position;
        //            randCard.hasBeenPlayed = false;

        //            availbleDestinationCardSlots[i] = false;
        //            DestinationTicket.Remove(randCard);
        //            return;
        //        }
        //    }
        //}
    }

    // This method is for the SpecialDestination Drawpile. \\
    public void DrawcardSpecialDestination()
    {
        //if (SpecialDestinationTicket.Count >= 1)
        //{
        //    Card randCard = SpecialDestinationTicket[UnityEngine.Random.Range(0, SpecialDestinationTicket.Count)];

        //    for (int i = 0; i < availbleSpecialDestinationCardSlots.Length; i++)
        //    {
        //        if (availbleSpecialDestinationCardSlots[i] == true)
        //        {
        //            //randCard.gameObject.SetActive(true);
        //            randCard.handIndex = i;

        //            //randCard.transform.position = cardSlotsSpecialDestination[i].position;
        //            randCard.hasBeenPlayed = false;

        //            availbleSpecialDestinationCardSlots[i] = false;
        //            SpecialDestinationTicket.Remove(randCard);
        //            return;
        //        }
        //    }
        //}
    }

    // This method is for the automaticly fils out the board. \\
    public void AutomaticDrawPile()
    {
        //Card randCard = deck[0];

        if (deck.Count >= 1)
        {
            
            //CardVariables randCard = deck[UnityEngine.Random.Range(0, deck.Count)];
            for (int i = 0; i < availbleCardSlots.Length; i++)
            {
                if (availbleCardSlots[i] == true && deck.Count != 0)
                {
                    Card cardVariables = deck[UnityEngine.Random.Range(0, deck.Count)];
                    if(cardVariables == null) { return; }
                    GameObject randomCardPrefab = CardColorByNumber(0, cardVariables.Color);
                    NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
                    _card.GetComponent<NetworkObject>().Spawn(true);
                    Card randCard = _card.GetComponent<Card>();
                    if (randCard.CardID == 0)
                    {
                        randCard.CardID = _CardID;
                        _CardID++;
                    }

                    _card.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = false;

                    availbleCardSlots[i] = false;
                    deck.Remove(randCard);
                    board.Add(randCard);

                    CardSlots(randCard, i);

                    // Here we check if a Rainbow cards is drawn onto the board. \\
                    if (randCard.Color == "Rainbow")
                    {
                        RainbowCount++;
                        Debug.Log(RainbowCount);
                    }
                    if (availbleCardSlots[4] == false)
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
                Debug.Log(cardSlots);
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


    #endregion DrawFunctions



    // This method is for checking the cards on the board. \\
    // It checks to make sure that there are not more than 3 Rainbow cards at ones. \\
    public void /*Task*/ CheckCards()
    {
        //await Task.Delay(1000);
        for (int b = 0; b < availbleDiscardPileCardSlots.Length; b++)
        {
            availbleDiscardPileCardSlots[b] = false;
            availbleCardSlots[b] = true;
            bool isEmpty = !board.Any();
            if(board != null && !isEmpty)
            {
                Card delete = board[0];
                delete.transform.position = discardPileDestination[b].position;
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



    #region Tickets

    public void DrawDestinatonTickets()
    {
        DrawDestinatonTicketsServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void DrawDestinatonTicketsServerRpc(ServerRpcParams serverRpcParams = default)
    {
        PlayerStat player;
        List<Ticket> choosedTickets;
        ulong clientID = serverRpcParams.Receive.SenderClientId;
        for(int i = 0; i < PlayerManager.Instance.stats.Count; i++)
        {
            if(PlayerManager.Instance.stats[i].clientId == clientID)
            {
                player = PlayerManager.Instance.stats[i];
            }
        }
        choosedTickets = DealTickets(Convert.ToInt32(clientID));

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { serverRpcParams.Receive.SenderClientId }
            }
        };

        int[] ticketIDs = new int[choosedTickets.Count];
        for (int i = 0; i < choosedTickets.Count; i++)
        {
            ticketIDs[i] = choosedTickets[i].ticketID;
        }

        GameManager.Instance.SpawnTicketsLocalyClientRpc(ticketIDs, clientID, clientRpcParams);


    }


    // This metode gets the ticket ids from the server, and spawns the game objects localy on the client and adds them to a list
    [ClientRpc]
    public void SpawnTicketsLocalyClientRpc(int[] ticketIds, ulong clientId, ClientRpcParams clientRpcParams = default)
    {
        choosingtArea.SetActive(true);
        Debug.Log("Hehe");
        //if (!IsOwner) return;

        Debug.Log("Hehe 1 ");
        for (int i = 0; i < ticketIds.Length; i++)
        {
            Debug.Log("Hehe 2");
            GameObject ticketToSpawn = TicketSpawner.Instance.TicketChoser(ticketIds[i]);
            if(ticketToSpawn != null)
            {
                Debug.Log("Hehe 3");
                GameObject go_ticket = Instantiate(ticketToSpawn);
                go_ticket.transform.SetParent(choosingtArea.transform, true);
                RectTransform rt = go_ticket.GetComponent<RectTransform>();
                rt.position = choosingtArea.transform.position;
                Ticket spawendTicket = go_ticket.GetComponent<Ticket>();
                choosedTicket.Add(spawendTicket);
                ticketObjects.Add(go_ticket);
                Debug.Log("Hehe 4");
            }
        }

        Debug.Log("Hehe 5");
    }

    // This metode converts the impoortant elements from the choosedTicket list into two array, and sends them to a SeververRPC
    // Also checks and sends an indication if the player doesn't have at least 2 tickets selected and also updates the objects
    public void SendChoosenTicketsToServer()
    {
        int[] ticketIds = new int[choosedTicket.Count];
        bool[] ticketStatus = new bool[choosedTicket.Count];
        int checkcounter = 0;

        // This checks that how many tickets is selected, returns the metode and sends a warning if its under two
        for(int i = 0; i < choosedTicket.Count; i++)
        {
            if (choosedTicket[i].isChosen)
            {
                checkcounter++;
            }
        }

        if (checkcounter < 2)
        {
            Debug.Log("You need to choose minimum 2 tickets!");
            return;
        }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Here


        // This for populates the arrays, and goes troug the objects if the object is chosen it get re-parented under another gameobject
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
        choosingtArea.SetActive(false);
        ticketArea.SetActive(false);

        Debug.Log("Blep 2");
        CheckChoosenTicketsServerRpc(ticketIds, ticketStatus);
    }

    // This metode, update the client's tickets list in their status object on the server
    [ServerRpc(RequireOwnership = false)]
    public void CheckChoosenTicketsServerRpc(int[] ticketIds, bool[] ticketStatus, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Blep 3");
        int index = 1000;
        List<Ticket> ticketList = new List<Ticket>();
        PlayerStat stat = null;

        // This finds the client's PlayerStat and index of its playerStat 
        for (int i = 0; i < PlayerManager.Instance.stats.Count; i++)
        {
            if(serverRpcParams.Receive.SenderClientId == PlayerManager.Instance.stats[i].clientId)
            {
                index = i;
                stat = PlayerManager.Instance.stats[i];
                Debug.Log("Blep 4: " + stat);
            }
        }

        if (stat == null) return;

        // This goes trough the ticketIds array, and where the id is the same as the stats id from the list, check whats the status is for the ticket
        // where the ticketStatus is true it adds the ticket to the ticketList
        Debug.Log("Blep 5");
        for (int i = 0; i < ticketIds.Length; i++)
        {
            if (stat.tickets[i].ticketID == ticketIds[i])
            {
                if (ticketStatus[i])
                {
                    Debug.Log("Blep 6");
                    ticketList.Add(stat.tickets[i]);
                }
            }
        }

        Debug.Log("Blep 7");
        // This finds the client's stat again, and set the new list for the tickets
        PlayerManager.Instance.stats[index].tickets = ticketList;
        PlayerManager.Instance.stats[index].isReady = true;

        Debug.Log("Blep 8");
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
        lastcard = "";
        AutomaticDrawPile();
    }

    // This method is for changeing the playces of the cards. \\
    public void CardColorPick(Card card, int slotnumber)
    {
        if (PlayerPickCount <= 1)
        {

            //Card card = board[0];
            Debug.Log(card.Color);

            // Here we check for the color of the card. \\
            if (card.Color == "Black")
            {
                IBlackPlayerCount++;
                TBlackPlayerCount.text = IBlackPlayerCount.ToString();
            }
            else if (card.Color == "Blue")
            {
                IBluePlayerCount++;
                TBluePlayerCount.text = IBluePlayerCount.ToString();
            }
            else if (card.Color == "Brown")
            {
                IBrownPlayerCount++;
                TBrownPlayerCount.text = IBrownPlayerCount.ToString();
            }
            else if (card.Color == "Green")
            {
                IGreenPlayerCount++;
                TGreenPlayerCount.text = IGreenPlayerCount.ToString();
            }
            else if (card.Color == "Orange")
            {
                IOrangePlayerCount++;
                TOrangePlayerCount.text = IOrangePlayerCount.ToString();
            }
            else if (card.Color == "Purple")
            {
                IPurplePlayerCount++;
                TPurplePlayerCount.text = IPurplePlayerCount.ToString();
            }
            else if (card.Color == "White")
            {
                IWhitePlayerCount++;
                TWhitePlayerCount.text = IWhitePlayerCount.ToString();
            }
            else if (card.Color == "Yellow")
            {
                IYellowPlayerCount++;
                TYellowPlayerCount.text = IYellowPlayerCount.ToString();
            }
            else if (card.Color == "Rainbow")
            {
                if(lastcard == "")
                {
                    IRainbowPlayerCount++;
                    TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
                    PlayerPickCount++;

                    if (RainbowCount > 0)
                    {
                        RainbowCount--;
                    }
                }
                else
                {
                    return;
                }
            }
            lastcard = card.Color;
            // When the color has been found, it will be added to the playerhand.\\
            //Card delete = board[0];
            Card delete = card;
            delete.transform.position = discardPileDestination[0].position;
            availbleDiscardPileCardSlots[0] = false;
            availbleCardSlots[slotnumber] = true;
            //delete.gameObject.SetActive(false);
            board.Remove(delete);
            discardPile.Add(delete);
            discardPile.Clear();
            availbleDiscardPileCardSlots[0] = true;
            PlayerPickCount++;
        }
        else if (PlayerPickCount > 1)
        {
            Debug.Log("Du kan ikke trække flere kort!" +
                " Du må maks trække 2 kort pr tur!");
        }
        Debug.Log("Player pick count = " + PlayerPickCount);
    }

    // This method takes the players cards and plays them. \\
    public void PlayCardHand(string cardcolor)
    {
        // Here we check for the color of the card. \\
        if (cardcolor == "Black")
        {
            if (IBlackPlayerCount >= 1)
            {
                IBlackPlayerCount--;
                TBlackPlayerCount.text = IBlackPlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Blue")
        {
            if (IBluePlayerCount >= 1)
            {
                IBluePlayerCount--;
                TBluePlayerCount.text = IBluePlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Brown")
        {
            if (IBrownPlayerCount >= 1)
            {
                IBrownPlayerCount--;
                TBrownPlayerCount.text = IBrownPlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Green")
        {
            if (IGreenPlayerCount >= 1)
            {
                IGreenPlayerCount--;
                TGreenPlayerCount.text = IGreenPlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Orange")
        {
            if (IOrangePlayerCount >= 1)
            {
                IOrangePlayerCount--;
                TOrangePlayerCount.text = IOrangePlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Purple")
        {
            if (IPurplePlayerCount >= 1)
            {
                IPurplePlayerCount--;
                TPurplePlayerCount.text = IPurplePlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "White")
        {
            if (IWhitePlayerCount >= 1)
            {
                IWhitePlayerCount--;
                TWhitePlayerCount.text = IWhitePlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Yellow")
        {
            if (IYellowPlayerCount >= 1)
            {
                IYellowPlayerCount--;
                TYellowPlayerCount.text = IYellowPlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        else if (cardcolor == "Rainbow")
        {
            if (IRainbowPlayerCount >= 1)
            {
                IRainbowPlayerCount--;
                TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
            }
            else
            {
                Debug.Log("Du har ikke flere kort af den type tilbage!");
            }
        }
        Debug.Log("Du har fjernet 1 " + cardcolor + " kort");
    }

    #endregion UnityEngine.Random/Extra



    #region BTNS

    /// <summary>
    /// This region "BTNS" are for all the btn funcktions that are in this script. \\
    /// </summary>

    // This methode is for the buttons on the board, these moves the cards to the players hand.
    // When the player clicks one of the buttons, it sends it's slot id and set the right slot
    // then calls CardColorPick metode, with the card that is in the slot and with the slotnu,ber
    public void BoardButtons(int slotnumber)
    {
        switch (slotnumber)
        {
            case 0:
                slot = cardslot1.GetComponent<CardSlotsID>();
                break;
            case 1:
                slot = cardslot2.GetComponent<CardSlotsID>();
                break;
            case 2:
                slot = cardslot3.GetComponent<CardSlotsID>();
                break;
            case 3:
                slot = cardslot4.GetComponent<CardSlotsID>();
                break;
            case 4:
                slot = cardslot5.GetComponent<CardSlotsID>();
                break;
        }
        foreach (Card card in board.ToList())
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card, slotnumber);
            }
        }
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
        PlayCardHand(color);
    }


    #endregion BTNS
}
