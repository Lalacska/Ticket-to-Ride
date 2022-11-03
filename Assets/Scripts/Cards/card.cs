using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles all the values for the card. \\
public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;

    public int handIndex;

    private GameManager gm;

    public string Color = "";

    public int ownerID;

    public int CardID;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
        if(hasBeenPlayed == false)
        {
            transform.position += Vector3.up * 5;
            hasBeenPlayed = true;
            gm.availbleCardSlots[handIndex] = true;
            Invoke("MoveToDiscardPile", 2f);
        }
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }


}
