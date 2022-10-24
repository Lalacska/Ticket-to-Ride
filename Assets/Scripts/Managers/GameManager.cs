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
    public List<Card> HandSlots = new List<Card>();

    // This part is for the slots/areas where the cards will be shown/displayed. \\
    public Transform[] cardSlots;
    public Transform[] cardSlotsDestination;
    public Transform[] cardSlotsSpecialDestination;
    public Transform[] discardPileDestination;
    public Transform[] handSlots;

    // This is bools for availble slots, the cards can be playsed in. \\
    public bool[] availbleCardSlots;
    public bool[] availbleDestinationCardSlots;
    public bool[] availbleSpecialDestinationCardSlots;
    public bool[] availbleDiscardPileCardSlots;
    public bool[] availbleHandslots;

    // This is a int for keeping track of Rainbow Cards. \\
    public int RainbowCount = 0;

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
                    // Here we check if a Rainbow cards is drawn onto the board. \\
                    if (randCard.Color == "Rainbow")
                    {
                        RainbowCount++;
                        Debug.Log(RainbowCount);
                    }
                    if (availbleCardSlots[4] == false)
                    {
                        // If more than 3 Rainbow cards are on the field, the board is cleared. \\
                        if (RainbowCount == 3)
                        {
                            await CheckCards();
                        }
                    }
                    Debug.Log(randCard.Color);
                    return;
                }
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

    // ??? \\ 
    public void PickCard()
    {

        return;
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
        Card card = board[0];
        Debug.Log(card.Color);

        // Here we check for the color of the card. \\
        if(card.Color == "Black")
        {
            IBlackPlayerCount++;
            TBlackPlayerCount.text = IBlackPlayerCount.ToString();
        }
        else if(card.Color == "Blue")
        {
            IBluePlayerCount++;
            TBluePlayerCount.text = IBluePlayerCount.ToString();
        }
        else if(card.Color == "Brown")
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
            IRainbowPlayerCount++;
            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
        }
        // When the color has been found, it will be added to the playerhand.\\
        Card delete = board[0];
        delete.transform.position = discardPileDestination[0].position;
        availbleDiscardPileCardSlots[0] = false;
        availbleCardSlots[0] = true;
        delete.gameObject.SetActive(false);
        board.Remove(delete);
        discardPile.Add(delete);

        discardPile.Clear();
    }

    // This method is for the secound pick btn on the board. \\
    public void Button2()
    {
        Card card = board[1];
        Debug.Log(card.Color);

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
            IRainbowPlayerCount++;
            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
        }

        Card delete = board[1];
        delete.transform.position = discardPileDestination[1].position;
        availbleDiscardPileCardSlots[1] = false;
        availbleCardSlots[1] = true;
        delete.gameObject.SetActive(false);
        board.Remove(delete);
        discardPile.Add(delete);

        discardPile.Clear();
    }

    // This method is for the third pick btn on thr board. \\
    public void Button3()
    {
        Card card = board[2];
        Debug.Log(card.Color);

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
            IRainbowPlayerCount++;
            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
        }

        Card delete = board[2];
        delete.transform.position = discardPileDestination[2].position;
        availbleDiscardPileCardSlots[2] = false;
        availbleCardSlots[2] = true;
        delete.gameObject.SetActive(false);
        board.Remove(delete);
        discardPile.Add(delete);

        discardPile.Clear();
    }

    // This method is for the fourth pick btn on the board. \\
    public void Button4()
    {
        Card card = board[3];
        Debug.Log(card.Color);

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
            IRainbowPlayerCount++;
            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
        }

        Card delete = board[3];
        delete.transform.position = discardPileDestination[3].position;
        availbleDiscardPileCardSlots[3] = false;
        availbleCardSlots[3] = true;
        delete.gameObject.SetActive(false);
        board.Remove(delete);
        discardPile.Add(delete);

        discardPile.Clear();
    }

    // This method is for the fith pick btn on the board. \\
    public void Button5()
    {
        Card card = board[4];
        Debug.Log(card.Color);

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
            IRainbowPlayerCount++;
            TRainbowPlayerCount.text = IRainbowPlayerCount.ToString();
        }

        Card delete = board[4];
        delete.transform.position = discardPileDestination[4].position;
        availbleDiscardPileCardSlots[4] = false;
        availbleCardSlots[4] = true;
        delete.gameObject.SetActive(false);
        board.Remove(delete);
        discardPile.Add(delete);

        discardPile.Clear();
    }
}
