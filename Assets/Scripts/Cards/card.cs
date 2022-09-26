using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;

    public int handIndex;

    private GameManager gm;

    public string Color = "";

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnMouseDown()
    {
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
