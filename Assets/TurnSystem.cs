using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TurnState { Start, PlayerTurn, OpponentTurn, Won, Lost }

public class TurnSystem : MonoBehaviour
{

    public TurnState state;

    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.Start;

        SetupTurn();

    }

    void SetupTurn()
    {

    }
}
