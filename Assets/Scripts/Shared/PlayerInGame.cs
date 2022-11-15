using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInGame : Singeltone<PlayerInGame>
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

}
