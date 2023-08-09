using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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



    //// Here we make list for the diffrent kinds of cards piles. \\
    public List<Card> deck;
    public List<Card> DestinationTicket;
    public List<Card> SpecialDestinationTicket;
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


    // This is a int for keeping track of Rainbow Cards. \\
    public int RainbowCount = 0;

    public int PlayerPickCount = 0;

    public int BoardIndex = 0;

    public int CardId = 0;

    public int HandId = 0;

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
    public Text TBlackPlayerCount;

    public int IBluePlayerCount;
    public Text TBluePlayerCount;

    public int IBrownPlayerCount;
    public Text TBrownPlayerCount;

    public int IGreenPlayerCount;
    public Text TGreenPlayerCount;

    public int IOrangePlayerCount;
    public Text TOrangePlayerCount;

    public int IPurplePlayerCount;
    public Text TPurplePlayerCount;

    public int IWhitePlayerCount;
    public Text TWhitePlayerCount;

    public int IYellowPlayerCount;
    public Text TYellowPlayerCount;

    public int IRainbowPlayerCount;
    public Text TRainbowPlayerCount;

    public Text TdiscardPileText;

    private int _CardID = 1;

    public bool DestroyWithSpawner;

    CardSlotsID slot;

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
        DestinationTicket = new List<Card>();
        SpecialDestinationTicket = new List<Card>();
        board = new List<Card>();
        discardPile = new List<Card>();
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
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Card");
            if(obj != null)
            {
                Debug.Log("HEY");
                //SetVisibilityClientRpc(true);
                
            }
        } else if (Input.GetKeyUp(KeyCode.G))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Card");
            if (obj != null)
            {
                Debug.Log("HEY");
                //SetVisibilityClientRpc(false);
            }
        }

        deckSizeText.text = deck.Count.ToString();
        decksSizeText.text = DestinationTicket.Count.ToString();
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


    public List<Card> DealCards(int clientID, List<Card> hand)
    {
        for (int i = 0; i < 4; i++)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];
            //if (cardVariables == null) { return null; }
            //GameObject randomCardPrefab = CardColorByNumber(0, cardVariables.Color);
            //NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
            //_card.GetComponent<NetworkObject>().Spawn(true);
            //Card randCard = _card.GetComponent<Card>();
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

    // This method is for the Train-Destination Drawpile. \\
    public void Drawcard()
    {

        if (deck.Count >= 1)
        {
            GameObject randomCardPrefab = CardColorByNumber(Random.Range(1, 111), "nope");
            NetworkObject _card = NetworkObjectPool.Instance.GetNetworkObject(randomCardPrefab, Vector3.zero, Quaternion.identity);
            _card.GetComponent<NetworkObject>().Spawn(true);
            Card randCard = _card.GetComponent<Card>();
            GameObject rc = transform.parent.GetComponent<GameObject>();
            Debug.Log(rc);
            //Card randCard = deck[Random.Range(0, deck.Count)];
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
        if (DestinationTicket.Count >= 1)
        {
            Card randCard = DestinationTicket[Random.Range(0, DestinationTicket.Count)];

            for (int i = 0; i < availbleDestinationCardSlots.Length; i++)
            {
                if (availbleDestinationCardSlots[i] == true)
                {
                    //randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    //randCard.transform.position = cardSlotsDestination[i].position;
                    randCard.hasBeenPlayed = false;

                    availbleDestinationCardSlots[i] = false;
                    DestinationTicket.Remove(randCard);
                    return;
                }
            }
        }
    }

    // This method is for the SpecialDestination Drawpile. \\
    public void DrawcardSpecialDestination()
    {
        if (SpecialDestinationTicket.Count >= 1)
        {
            Card randCard = SpecialDestinationTicket[Random.Range(0, SpecialDestinationTicket.Count)];

            for (int i = 0; i < availbleSpecialDestinationCardSlots.Length; i++)
            {
                if (availbleSpecialDestinationCardSlots[i] == true)
                {
                    //randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    //randCard.transform.position = cardSlotsSpecialDestination[i].position;
                    randCard.hasBeenPlayed = false;

                    availbleSpecialDestinationCardSlots[i] = false;
                    SpecialDestinationTicket.Remove(randCard);
                    return;
                }
            }
        }
    }

    // This method is for the automaticly fils out the board. \\
    public void AutomaticDrawPile()
    {
        //Card randCard = deck[0];

        if (deck.Count >= 1)
        {
            
            //CardVariables randCard = deck[Random.Range(0, deck.Count)];
            for (int i = 0; i < availbleCardSlots.Length; i++)
            {
                if (availbleCardSlots[i] == true && deck.Count != 0)
                {
                    Card cardVariables = deck[Random.Range(0, deck.Count)];
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



    #region Random/Extra 

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

    #endregion Random/Extra



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
