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

    public Text discardPileText;


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
}
