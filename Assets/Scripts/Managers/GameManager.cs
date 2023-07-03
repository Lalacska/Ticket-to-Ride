using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    [SerializeField] private Transform Bruh;

    [SerializeField] private Transform BlackPrefab;
    [SerializeField] private Transform BluePrefab;
    [SerializeField] private Transform BrownPrefab;
    [SerializeField] private Transform GreenPrefab;
    [SerializeField] private Transform OrangePrefab;
    [SerializeField] private Transform PurplePrefab;
    [SerializeField] private Transform WhitePrefab;
    [SerializeField] private Transform YellowPrefab;
    [SerializeField] private Transform RainbowPrefab;
    [SerializeField] private GameObject Cards;


    // Here we make list for the diffrent kinds of cards piles. \\
    public NetworkList<Card> deck;
    public NetworkList<Card> DestinationTicket;
    public NetworkList<Card> SpecialDestinationTicket;
    public NetworkList<Card> board;
    public NetworkList<Card> discardPile;


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

    private string lastcard = "";


    // These GameObjects are Serialized for later use. \\
    [SerializeField] private GameObject cardslot1;
    [SerializeField] private GameObject cardslot2;
    [SerializeField] private GameObject cardslot3;
    [SerializeField] private GameObject cardslot4;
    [SerializeField] private GameObject cardslot5;


    [SerializeField] private GameObject Handslot1;
    [SerializeField] private GameObject Handslot2;
    [SerializeField] private GameObject Handslot3;
    [SerializeField] private GameObject Handslot4;
    [SerializeField] private GameObject Handslot5;
    [SerializeField] private GameObject Handslot6;
    [SerializeField] private GameObject Handslot7;
    [SerializeField] private GameObject Handslot8;
    [SerializeField] private GameObject Handslot9;

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

    private NetworkObject m;

    #endregion Variables

    private void Awake()
    {
        deck = new NetworkList<Card>();
        DestinationTicket = new NetworkList<Card>();
        SpecialDestinationTicket = new NetworkList<Card>();
        board = new NetworkList<Card>();
        discardPile = new NetworkList<Card>();
    }

    // This method runs, when the programs starts. \\
    private void Start()
    {
        DrawcardSpecialDestination();
    }

    public override void OnNetworkSpawn()
    {
        //Something();
        CreateCards();
    }


    private void CreateCards()
    {
        GameObject cards = Instantiate(Cards, Vector3.up, Quaternion.identity);
        cards.GetComponent<NetworkObject>().Spawn(true);
        int Idcounter = 1;
        Idcounter = CardLoop(false, BlackPrefab, Idcounter, cards);
        Idcounter = CardLoop(false, BluePrefab, Idcounter, cards);
        Idcounter = CardLoop(false, BrownPrefab, Idcounter, cards);
        Idcounter = CardLoop(false, GreenPrefab, Idcounter, cards);
        Idcounter = CardLoop(false, OrangePrefab, Idcounter, cards);
        Idcounter = CardLoop(false, PurplePrefab, Idcounter, cards);
        Idcounter = CardLoop(false, WhitePrefab, Idcounter, cards);
        Idcounter = CardLoop(false, YellowPrefab, Idcounter, cards);
        Idcounter = CardLoop(true, RainbowPrefab, Idcounter, cards);
    }

    // Spawn every card
    private int CardLoop(bool rainbow, Transform prefab, int ID, GameObject Deck)
    {
        int counter = 12;
        if (rainbow)
        {
            counter = 14;
        }
        for (int i = 0; i < counter; i++)
        {
            Transform go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            go.transform.eulerAngles = new Vector3(0, 180, 0);
            Card card = go.GetComponent<Card>();
            card.CardID = ID;
            deck.Add(card);
            go.GetComponent<NetworkObject>().Spawn(true);
            go.transform.SetParent(Deck.transform, true);
            ID++;
        }

        return ID;
    }

    private void Something()
    {
        Transform go = Instantiate(BlackPrefab, Vector3.zero, Quaternion.identity);
        Card card = go.GetComponent<Card>();
        card.CardID = 16;
        deck.Add(card);
        go.GetComponent<NetworkObject>().Spawn(true);

    }

    //[ServerRpc]
    //void SetVisibilityServerRpc(bool status)
    //{
    //    GameObject obj = GameObject.FindGameObjectWithTag("Card");
    //    GameObject cardbase = obj.gameObject.transform.GetChild(0).gameObject;
    //    MeshRenderer basemesh = cardbase.gameObject.GetComponent<MeshRenderer>();
    //    GameObject picture = cardbase.gameObject.transform.GetChild(0).gameObject;
    //    MeshRenderer picturemesh = picture.gameObject.GetComponent<MeshRenderer>();
    //    picturemesh.enabled = status;
    //    basemesh.enabled = status;
    //}


    [ClientRpc]
    void SetVisibilityClientRpc(bool status)
    {
        Debug.Log("HEYO");
        GameObject obj = GameObject.FindGameObjectWithTag("Card");
        GameObject cardbase = obj.gameObject.transform.GetChild(0).gameObject;
        MeshRenderer basemesh = cardbase.gameObject.GetComponent<MeshRenderer>();
        GameObject picture = cardbase.gameObject.transform.GetChild(0).gameObject;
        MeshRenderer picturemesh = picture.gameObject.GetComponent<MeshRenderer>();
        picturemesh.enabled = status;
        basemesh.enabled = status;
        Debug.Log("HEYO 2");
        //cardbase.SetActive(status);
    }

    // This method runs every frame & updates the scenes. \\
    private void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyUp(KeyCode.O))
        {
            Transform spawnedObjectTransform = Instantiate(Bruh);
            spawnedObjectTransform.position = new Vector3(0, 5, 0);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Card");
            if(obj != null)
            {
                Debug.Log("HEY");
                SetVisibilityClientRpc(true);
                
            }
        } else if (Input.GetKeyUp(KeyCode.G))
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Card");
            if (obj != null)
            {
                Debug.Log("HEY");
                SetVisibilityClientRpc(false);
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
            AutomaticDrawPileClientRpc();
        }
    }



    /// <summary>
    /// This part is for all the different Draw functions that are being used in the game. \\
    /// </summary>

    #region DrawFunctions

    // This method is for the Train-Destination Drawpile. \\
    public async void Drawcard()
    {
        Card randCard = deck[0];
        GameObject rc = transform.parent.GetComponent<GameObject>();
        Debug.Log(rc);

        if (deck.Count >= 1)
        {
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
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    randCard.transform.position = cardSlotsDestination[i].position;
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
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    randCard.transform.position = cardSlotsSpecialDestination[i].position;
                    randCard.hasBeenPlayed = false;

                    availbleSpecialDestinationCardSlots[i] = false;
                    SpecialDestinationTicket.Remove(randCard);
                    return;
                }
            }
        }
    }

    [ClientRpc]
    public void AutomaticDrawPileClientRpc()
    {
        //Card randCard = deck[0];

        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availbleCardSlots.Length; i++)
            {
                if (availbleCardSlots[i] == true)
                {
                    //randCard.gameObject.SetActive(true);

                    randCard.handIndex = i;

                    randCard.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = true;

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
                        if (RainbowCount == 3)
                        {
                            CheckCards();
                        }
                    }
                    Debug.Log(randCard.Color);
                    return;
                }
                //Debug.Log(cardSlots);
            }
        }
    }

    // This method is for the automaticly fils out the board. \\
    public async void AutomaticDrawPile()
    {
        //Card randCard = deck[0];

        if (deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];
            for (int i = 0; i < availbleCardSlots.Length; i++)
            {
                if (availbleCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    randCard.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = true;

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
                        if (RainbowCount == 3)
                        {
                              CheckCards();
                        }
                    }
                    Debug.Log(randCard.Color);
                    return;
                }
                //Debug.Log(cardSlots);
            }
        }
    }

    #endregion DrawFunctions



    // This method is for checking the cards on the board. \\
    // It checks to make sure that there are not more than 3 Rainbow cards at ones. \\
    public void /*Task*/ CheckCards()
    {
        //await Task.Delay(1000);
        for (int b = 0; b < availbleDiscardPileCardSlots.Length; b++)
        {
            Card delete = board[0];
            delete.transform.position = discardPileDestination[b].position;
            availbleDiscardPileCardSlots[b] = false;
            availbleCardSlots[b] = true;
            delete.gameObject.SetActive(false);
            board.Remove(delete);
            discardPile.Add(delete);
            RainbowCount = 0;
        }
    }

    // This methos finds the cardslot the user presed & passes the id off. \\ 
    public void CardSlots(Card card, int i)
    {
        CardSlotsID cardslotsid = new CardSlotsID();
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
    }

    // This method is for changeing the playces of the cards. \\
    public void CardColorPick(Card card, int i)
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
            // Card delete = board[0];
            Card delete = card;
            delete.transform.position = discardPileDestination[0].position;
            availbleDiscardPileCardSlots[0] = false;
            availbleCardSlots[i] = true;
            delete.gameObject.SetActive(false);
            board.Remove(delete);
            discardPile.Add(delete);
            discardPile.Clear();
            availbleDiscardPileCardSlots[0] = true;
            PlayerPickCount++;
        }
        else if (PlayerPickCount > 1)
        {
            Debug.Log("Du kan ikke tr�kke flere kort!" +
                " Du m� maks tr�kke 2 kort pr tur!");
        }
        Debug.Log("Player pick count = " + PlayerPickCount);
    }

    // This method takes the players cards and plays them. \\
    public void PlayCardHand(Card card)
    {
        // Here we check for the color of the card. \\
        if (card.Color == "Black")
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
        else if (card.Color == "Blue")
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
        else if (card.Color == "Brown")
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
        else if (card.Color == "Green")
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
        else if (card.Color == "Orange")
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
        else if (card.Color == "Purple")
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
        else if (card.Color == "White")
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
        else if (card.Color == "Yellow")
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
        else if (card.Color == "Rainbow")
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
        Debug.Log("Du har fjernet 1 " + card.Color + " kort");
    }

    #endregion Random/Extra



    #region BTNS

    /// <summary>
    /// This region "BTNS" are for alle the btn funcktions that are in this script. \\
    /// </summary>


    // This part is for the btns that moves the cards from the "board" to the players hand. \\

    // This method is for the first pick btn on the board. \\
    public void Button1()
    {
        CardSlotsID slot = new CardSlotsID();
        slot = cardslot1.GetComponent<CardSlotsID>();
        foreach (Card card in board)
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card, 0);
            }
        }
    }

    // This method is for the secound pick btn on the board. \\
    public void Button2()
    {
        CardSlotsID slot = new CardSlotsID();
        slot = cardslot2.GetComponent<CardSlotsID>();
        foreach (Card card in board)
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card, 1);
            }
        }
    }

    // This method is for the third pick btn on thr board. \\
    public void Button3()
    {
        CardSlotsID slot = new CardSlotsID();
        slot = cardslot3.GetComponent<CardSlotsID>();
        foreach (Card card in board)
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card, 2);
            }
        }
    }

    // This method is for the fourth pick btn on the board. \\
    public void Button4()
    {
        CardSlotsID slot = new CardSlotsID();
        slot = cardslot4.GetComponent<CardSlotsID>();
        foreach (Card card in board)
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card, 3);
            }
        }
    }

    // This method is for the fith pick btn on the board. \\
    public void Button5()
    {
        CardSlotsID slot = new CardSlotsID();
        slot = cardslot5.GetComponent<CardSlotsID>();
        foreach (Card card in board)
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card, 4);
            }
        }
    }



    // This part is for the "Play" btns. The play btns are the ones witch is used to "play" the cards from the players hands. \\

    // This method is for the first pick btn on the board. \\
    // This is for the "Black" card. \\
    public void PlayBTN1()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Black";
        }
        PlayCardHand(card);
    }

    // This method is for the secound pick btn on the board. \\
    // This is for the "Blue" card. \\
    public void PlayBTN2()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Blue";
        }
        PlayCardHand(card);
    }

    // This method is for the third pick btn on the board. \\
    // This is for the "Brown" card. \\
    public void PlayBTN3()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Brown";
        }
        PlayCardHand(card);
    }

    // This method is for the fourth pick btn on the board. \\
    // This is for the "Green" card. \\
    public void PlayBTN4()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Green";
        }
        PlayCardHand(card);
    }

    // This method is for the fith pick btn on the board. \\
    // This is for the "Orange" card. \\
    public void PlayBTN5()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Orange";
        }
        PlayCardHand(card);
    }

    // This method is for the Sixth pick btn on the board. \\
    // This is for the "Purple" card. \\
    public void PlayBTN6()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Purple";
        }
        PlayCardHand(card);
    }

    // This method is for the Seventh pick btn on the board. \\
    // This is for the "White" card. \\
    public void PlayBTN7()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "White";
        }
        PlayCardHand(card);
    }

    // This method is for the Eight pick btn on the board. \\
    // This is for the "Yellow" card. \\
    public void PlayBTN8()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Yellow";
        }
        PlayCardHand(card);
    }

    // This method is for the nith pick btn on the board. \\
    // This is for the "Rainbow" card. \\
    public void PlayBTN9()
    {
        // This method set the color of the card, then runs the method that retracks "1" from the playerTotal. \\
        Card card = new Card();
        {
            card.Color = "Rainbow";
        }
        PlayCardHand(card);
    }

    #endregion BTNS
}
