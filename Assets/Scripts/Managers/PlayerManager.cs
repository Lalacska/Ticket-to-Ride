using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singeltone<PlayerManager>
{
    public List<Player> players = new List<Player>();

    internal void AssignTurn(int currentPlayerTurn)
    {
        foreach(Player player in players)
        {
            player.myTurn = player.ID == currentPlayerTurn;
        }
    }
}
