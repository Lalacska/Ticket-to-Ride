using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to the in game player
public class PlayerInGame : Singleton<PlayerInGame>
{
    public string ID;
    public ulong clientId;
    public string playername;
    public string color;
    public int station = 3;
    public int trains = 50;
    public int cards;
    public int destinationTickets;
    public bool myTurn;
    public Camera playercam;

    public PlayerInGame(string username)
    {
        this.playername = username;
    }
}
