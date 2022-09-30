using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : Singeltone<TurnManager>
{
    public int currentPlayerTurn;

    public void StartTurnGamplay(int playerID)
    {
        currentPlayerTurn = playerID;
        PlayerManager.Instance.AssignTurn(currentPlayerTurn);
    }

    public void StartTurn()
    {

    }

    public void EndTurn()
    {

    }
}
