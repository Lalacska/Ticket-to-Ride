using Assets.Scripts.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;



public enum TurnState { Start, PlayerTurn, OpponentTurn, Won, Lost }

public class TurnM : Singleton<TurnM>
{


    #region Variables

    public int PlayerPickCount = 0;

    #endregion


    public TurnState state;
    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.Start;

    }

    // This method changes the turn to the next player. \\
    public void SwitchTurn()
    {
        Debug.Log("Du har skiftet tur!");
        PlayerPickCount = 0;
        //lastcaz= "";
        GameManager.Instance.AutomaticDrawPile();
        //AutomaticDrawPile();
    }

    public void Switch()
    {
        if (state == TurnState.Start)
        {
            state = TurnState.PlayerTurn;

        }
        else
        {
            state = TurnState.Lost;
        }
    }


    // ToggleBtn. \\
    // 
    public GameObject GameBoard;
    public GameObject Turn;
    public void Togglebtn()
    {
        if (GameBoard.activeInHierarchy == false)
        {
            GameBoard.SetActive(true);

            //slot = cardslot4.GetComponent<CardSlotsID>();
            //foreach (CardVariables card in board.ToList())
            //{
            //    if (card.CardID == slot.cardslotCardID)
            //    {
            //        CardColorPick(card, 3);
            //    }
            //}
        }
        //else
        //{
        //    gameboard.setactive(false);
        //}

        Turn.SetActive(false);

    }

}
