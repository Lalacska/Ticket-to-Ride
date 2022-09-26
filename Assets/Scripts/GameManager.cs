using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public List<Card> deck = new List<Card>();
    public List<Card> discardPile = new List<Card>();

    public Transform[] cardSlots;
    public bool[] availbleCardSlots;

    public Text deckSizeText;
    public Text discardPileText;

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
        //discardPileText.text = discardPile.Count.ToString();
    }
}
