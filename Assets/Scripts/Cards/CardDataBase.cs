using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card> ();

    void Awake()
    {
        cardList.Add(new Card(0, "Lilla", "Det her er et Lillat tog"));
        cardList.Add(new Card(1, "Blå", "Det her er et Blåt tog"));
        cardList.Add(new Card(2, "Brun", "Det her er et Brunt tog"));
        cardList.Add(new Card(3, "Grøn", "Det her er et Grønt tog"));
        cardList.Add(new Card(4, "Gul", "Det her er et Gult tog"));
        cardList.Add(new Card(5, "Sort", "Det her er et Sort tog"));
        cardList.Add(new Card(6, "Rød", "Det her er et Rødt tog"));
        cardList.Add(new Card(7, "None", "None"));
    }
}
