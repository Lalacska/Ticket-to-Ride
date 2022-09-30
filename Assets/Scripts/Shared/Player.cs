using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singeltone<Player>
{
    public int ID;
    public string playername;
    public string color;
    public int station = 3;
    public int trains = 50;
    public int cards;
    public int destinationTickets;
    public bool myTurn;
    public Camera playercam;

    public Player(int ID, string playername, string color, bool myTurn)
    {
        this.ID = ID;
        this.playername = playername;
        this.color = color;
        this.myTurn = myTurn;
    }
}
