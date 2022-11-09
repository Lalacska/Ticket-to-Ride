using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singeltone<GameManager>
{
    // Here we make list for the diffrent kinds of cards piles. \\
    public List<Card> deck = new List<Card>();
    public List<Card> DestinationTicket = new List<Card>();
    public List<Card> SpecialDestinationTicket = new List<Card>();
    public List<Card> board = new List<Card>();
    public List<Card> discardPile = new List<Card>();


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


    // This method runs, when the programs starts. \\
    private void Start()
    {
        DrawcardSpecialDestination();
    }

    // This method runs every frame & updates the scenes. \\
    private void Update()
    {
        deckSizeText.text = deck.Count.ToString();
        decksSizeText.text = DestinationTicket.Count.ToString();
        //deckssSizeText.text = SpecialDestinationTicket.Count.ToString();
        //discardPileText.text = discardPile.Count.ToString();

        // This if statment runs the automatic board filler until there are no more cards left in the deck. \\
        if(deck.Count >= 1)
        {
            AutomaticDrawPile();
        }
    }

    // This method is for the Train-Destination Drawpile. \\
    public async void Drawcard()
    {
        Card randCard = deck[0];

        if (deck.Count >= 1)
        {
            //Card randCard = deck[Random.Range(0, deck.Count)];
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
                            await CheckCards();
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
                             await CheckCards();
                        }
                    }
                    Debug.Log(randCard.Color);
                    return;
                }
                //Debug.Log(cardSlots);
            }
        }
    }

    // This method is for checking the cards on the board. \\
    // It checks to make sure that there are not more than 3 Rainbow cards at ones. \\
    public async Task CheckCards()
    {
        await Task.Delay(1000);
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

    // This method is for the first pick btn on the board. \\
    public void Button1()
    {
        CardSlotsID slot = new CardSlotsID();
        slot = cardslot1.GetComponent<CardSlotsID>();
        foreach (Card card in board)
        {
            if (card.CardID == slot.cardslotCardID)
            {
                CardColorPick(card,0);
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
                CardColorPick(card,1);
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
                CardColorPick(card,2);
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
                CardColorPick(card,3);
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
                CardColorPick(card,4);
            }
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
            //Card delete = board[0];
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
            Debug.Log("Du kan ikke trække flere kort!" +
                " Du må maks trække 2 kort pr tur!");
        }
        Debug.Log("Player pick count = " + PlayerPickCount);
    }



    /// <summary>
    /// HHHHHEEEEEEEEEEEEELLLLLLLLLPPPPPPPPPPPP
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="i"></param>

    // This methos finds the cardslot the user presed & passes the id off. \\ 
    public void HandSlots(PlayerHand hand, int i)
    {
        PlayerSlotsID handslotsid = new PlayerSlotsID();
        if (i == 0)
        {
            handslotsid = Handslot1.GetComponent<PlayerSlotsID>();
        }
        else if (i == 1)
        {
            handslotsid = Handslot2.GetComponent<PlayerSlotsID>();
        }
        else if (i == 2)
        {
            handslotsid = Handslot3.GetComponent<PlayerSlotsID>();
        }
        else if (i == 3)
        {
            handslotsid = Handslot4.GetComponent<PlayerSlotsID>();
        }
        else if (i == 4)
        {
            handslotsid = Handslot5.GetComponent<PlayerSlotsID>();
        }
        else if (i == 5)
        {
            handslotsid = Handslot6.GetComponent<PlayerSlotsID>();
        }
        else if (i == 6)
        {
            handslotsid = Handslot7.GetComponent<PlayerSlotsID>();
        }
        else if (i == 7)
        {
            handslotsid = Handslot8.GetComponent<PlayerSlotsID>();
        }
        else if (i == 8)
        {
            handslotsid = Handslot9.GetComponent<PlayerSlotsID>();
        }
        handslotsid.playerslotCardID = hand.HandID;
    }


    // This method is for the first pick btn on the board. \\
    public void PlayBTN1()
    {
        PlayerSlotsID hand = new PlayerSlotsID();
        hand = Handslot1.GetComponent<PlayerSlotsID>();
        foreach (PlayerHand card in board)
        {
            if (card.HandID == hand.playerslotCardID)
            {
                PlayCardHand(hand, 0);
            }
        }
    }
    public void PlayCardHand(PlayerHand hand, int i)
    {
        Card card = new Card();



        // Here we check for the color of the card. \\
        if (card.Color == "Black")
        {
            IBlackPlayerCount--;
            TBlackPlayerCount.text = IBlackPlayerCount.ToString();
        }
        else if (card.Color == "Blue")
        {
            IBluePlayerCount--;
            TBluePlayerCount.text = IBluePlayerCount.ToString();
        }
        else if (card.Color == "Brown")
        {
            IBrownPlayerCount--;
            TBrownPlayerCount.text = IBrownPlayerCount.ToString();
        }
        else if (card.Color == "Green")
        {
            IGreenPlayerCount--;
            TGreenPlayerCount.text = IGreenPlayerCount.ToString();
        }
        else if (card.Color == "Orange")
        {
            IOrangePlayerCount--;
            TOrangePlayerCount.text = IOrangePlayerCount.ToString();
        }
        else if (card.Color == "Purple")
        {
            IPurplePlayerCount--;
            TPurplePlayerCount.text = IPurplePlayerCount.ToString();
        }
        else if (card.Color == "White")
        {
            IWhitePlayerCount--;
            TWhitePlayerCount.text = IWhitePlayerCount.ToString();
        }
        else if (card.Color == "Yellow")
        {
            IYellowPlayerCount--;
            TYellowPlayerCount.text = IYellowPlayerCount.ToString();
        }
        else if (card.Color == "Rainbow")
        {
            IRainbowPlayerCount--;
            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
        }
    }
}
