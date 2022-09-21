using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card> ();

    void Awake()
    {
        cardList.Add(new Card(0, "Lilla", "Det her er et Lillat tog"));
        cardList.Add(new Card(1, "Bl�", "Det her er et Bl�t tog"));
        cardList.Add(new Card(2, "Brun", "Det her er et Brunt tog"));
        cardList.Add(new Card(3, "Gr�n", "Det her er et Gr�nt tog"));
        cardList.Add(new Card(4, "Gul", "Det her er et Gult tog"));
        cardList.Add(new Card(5, "Sort", "Det her er et Sort tog"));
        cardList.Add(new Card(6, "R�d", "Det her er et R�dt tog"));
        cardList.Add(new Card(7, "None", "None"));
    }
}
