using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singeltone<PlayerManager>
{
    public List<PlayerInGame> players = new List<PlayerInGame>();

    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach(PlayerInGame player in players)
        {
            player.myTurn = player.ID == currentPlayerTurn;
        }
    }
}
