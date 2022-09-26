using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public List<Card> DestinationTicket = new List<Card>();

    public List<Card> discardPile = new List<Card>();

    public Transform[] cardSlots;
    public Transform[] cardSlotsDestination;

    public bool[] availbleCardSlots;
    public bool[] availbleDestinationCardSlots;

    public Text deckSizeText;
    public Text decksSizeText;

    public Text discardPileText;

    // This method is for the Train-Destination Drawpile. \\
    public void Drawcard()
    {
        if(deck.Count >= 1)
        {
            Card randCard = deck[Random.Range(0, deck.Count)];

            for (int i = 0; i < availbleCardSlots.Length; i++)
            {
                if(availbleCardSlots[i] == true)
                {
                    randCard.gameObject.SetActive(true);
                    randCard.handIndex = i;

                    randCard.transform.position = cardSlots[i].position;
                    randCard.hasBeenPlayed = false;

                    availbleCardSlots[i] = false;
                    deck.Remove(randCard);
                    return;
                }
            }
        }
    }

    // This method is for the Train-Destination Drawpile. \\
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

    public void PickCard()
    {

        return;
    }


    public void Shuffle()
    {
        if(discardPile.Count >= 1)
        {
            foreach(Card card in discardPile)
            {
                deck.Add(card);
            }
            discardPile.Clear();
        }
    }

    private void Update()
    {
        deckSizeText.text = deck.Count.ToString();
        decksSizeText.text = DestinationTicket.Count.ToString();
        //discardPileText.text = discardPile.Count.ToString();
    }
}
