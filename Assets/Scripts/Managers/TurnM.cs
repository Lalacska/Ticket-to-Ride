using Assets.Scripts.Managers;
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
    [SerializeField] private Button buildStationbtn;
    private bool haveStations;

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

    // This metode is called when the player finished their action and their needs to end
    public void EndTurn()
    {

        //Enable_DisableActionChooser();

        Debug.Log("End turn");
        // This disables the end buttom
        Enable_DisableEndTurnBtn();
        SwitchTurnServerRpc();
        
    }

    // This method changes the turn to the next player. \\
    [ServerRpc(RequireOwnership = false)]
    public void SwitchTurnServerRpc(ServerRpcParams serverRpcParams = default)
    {
        // This gets the current player and set its turn to false
        ulong clientId;
        PlayerStat player = players[currentPlayerIndex];
        player.myTurn = false;

        // Adds one to the currentPlayerIndex 
        currentPlayerIndex++;
        Debug.Log("Du har skiftet tur!");
        Debug.Log("PlayerCount: " + players.Count + " CurrentIndex: "+currentPlayerIndex);

        // If the currentPlayerIndey is bigger than the players list, its set the index back to 0
        if(currentPlayerIndex > players.Count-1)
        {
            currentPlayerIndex = 0;

            // Adds one to the over all TurnCount
            TurnCount++;
        }

        // Gets the new current players data, and set its myturn to true
        clientId = players[currentPlayerIndex].clientId;
        player = players[currentPlayerIndex];
        player.myTurn = true;

        // This checks if the current players has more station or not
        if(player.stations.Value == 0)
        {
            haveStations = false;
        }
        else
        {
            haveStations = true;
        }
        turnCounter++;

        // Sends the client id to the turnStarted metode
        TurnStarted(clientId, haveStations);
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

    // This metode gets the clientId, sets the target client and then calls a clientrpc with the target
    public void TurnStarted(ulong clientId, bool _haveStations)
    {
        currentPlayerId = clientId;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] {clientId }
            }
        };

        // Sends the clientrpc params with the target client to a client rpc metode
        ActivateTurnClientRpc(_haveStations, clientRpcParams);
    }


    // This metode runs on the client and make Action Chooser visible

    [ClientRpc]
    public void ActivateTurnClientRpc(bool _haveStations, ClientRpcParams clientRpcParams = default)
    {
        Debug.Log(" Here");

        Debug.Log("Your Turn! ");

        // If the player has no more station this set the build station button uninteractable 
        if (!_haveStations)
        {
            buildStationbtn.interactable = false;
        }
        else
        {
            buildStationbtn.interactable = true;
        }

        Enable_DisableActionChooser();
        Enable_DisableEndTurnBtn();


    }


    // ToggleBtn. \\
    // Enable or Disable the end turn button depending if its on or off
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
    
    // It hides or make Action Chooser visible depending if its on or off
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
