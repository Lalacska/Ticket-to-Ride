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


public enum TurnState { Start, PlayerTurn, OpponentTurn, Won, Lost, End }

public class TurnM : Singleton<TurnM>
{


    #region Variables

    public int PlayerPickCount = 0;

    public int TurnCount = 0;

    private int turnCounter = 0;

    private int m_firstPlayerId;
    public int firstPlayerId { get { return m_firstPlayerId; } set { m_firstPlayerId = value; } }

    private ulong m_currentPlayerId;
    public ulong currentPlayerId { get { return m_currentPlayerId; } set { m_currentPlayerId = value; } }

    private int m_currentPlayerIndex;
    public int currentPlayerIndex { get { return m_currentPlayerIndex; } set { m_currentPlayerIndex = value; } }


    [SerializeField] private List<PlayerStat> m_players;
    public List<PlayerStat> players { get { return m_players; } set { m_players = value; } }

    [SerializeField] private Button endTurnBtn;

    public GameObject SwitchTrun;
    public GameObject GameBoard;
    public GameObject Turn;


    #endregion


    public TurnState state;
    // Start is called before the first frame update
    void Start()
    {
        state = TurnState.Start;

    }
    public void EndTurn()
    {

        Enable_DisableActionChooser();
        Enable_DisableEndTurnBtn();
        SwitchTurnServerRpc();
    }

    // This method changes the turn to the next player. \\
    [ServerRpc(RequireOwnership = false)]
    public void SwitchTurnServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId;
        PlayerStat player = players[currentPlayerIndex];
        player.myTurn = false;

        currentPlayerIndex++;
        Debug.Log("Du har skiftet tur!");
        Debug.Log("PlayerCount: " + players.Count + " CurrentIndex: "+currentPlayerIndex);
        if(currentPlayerIndex > players.Count-1)
        {
            currentPlayerIndex = 0;
        }
        clientId = players[currentPlayerIndex].clientId;
        player = players[currentPlayerIndex];
        player.myTurn = true;
        turnCounter++;
        TurnStarted(clientId);
        Debug.Log(" CurrentIndex: " + currentPlayerIndex);
        
    }

    public void Switch()
    {
        if (TurnCount < 10)
        {
            if (state == TurnState.Start)
            {
                state = TurnState.PlayerTurn;
                TurnCount++;
            }
            else if (state == TurnState.PlayerTurn)
            {
                state = TurnState.OpponentTurn;
                TurnCount++;
            }
            else if (state == TurnState.OpponentTurn)
            {
                state = TurnState.PlayerTurn;
                TurnCount++;
            }
            else
            {
                state = TurnState.Lost;
            }
        }
        else
        {
            Debug.Log("Spillet er slut");
            state = TurnState.End;
            
            SwitchTrun.SetActive(false);
        }
    }


    public void TurnStarted(ulong clientId)
    {
        currentPlayerId = clientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] {clientId }
            }
        };

        ActivateTurnClientRpc(clientRpcParams);
    }

    [ClientRpc]
    public void ActivateTurnClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.Log(" Here");

        Debug.Log("Your Turn! ");
        Enable_DisableActionChooser();
        Enable_DisableEndTurnBtn();


    }


    // ToggleBtn. \\
    // 
    public void Enable_DisableEndTurnBtn()
    {
        if (endTurnBtn.interactable)
        {
            endTurnBtn.interactable = false;
        }else
        {
            endTurnBtn.interactable = true;
        }
    }
    public void Enable_DisableActionChooser()
    {
        if(!Turn.activeInHierarchy)
        {
            Turn.SetActive(true);
        }
        else
        {
            Turn.SetActive(false);
        }
    }

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
