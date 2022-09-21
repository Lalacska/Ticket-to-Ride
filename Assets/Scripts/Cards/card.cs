using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card : MonoBehaviour
{
    public int id;
    public string cardName;
    public string cardDescription;

    public Card()
    {

    }

    public Card(int ID, string CardName, string CardDescription)
    {
        id = ID;
        cardName = CardName;
        cardDescription = CardDescription;
    }
}
