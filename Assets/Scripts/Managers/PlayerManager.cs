using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private Button blackButton;
        [SerializeField] private Button blueButton;
        [SerializeField] private Button brownButton;
        [SerializeField] private Button greenButton;
        [SerializeField] private Button orangeButton;
        [SerializeField] private Button purpleButton;
        [SerializeField] private Button whiteButton;
        [SerializeField] private Button yellowButton;
        [SerializeField] private Button rainbowButton;

        [SerializeField] private GameObject Player1;
        [SerializeField] private GameObject Player2;
        [SerializeField] private GameObject Player3;
        [SerializeField] private GameObject Player4;
        [SerializeField] private GameObject Player5;

        [SerializeField] private NetworkObject Statplace;

        [SerializeField] private List<PlayerStat> m_stats;
        public List<PlayerStat> stats { get { return m_stats; } set { m_stats = value; } }

        private bool gameStarted = false;
        private int playerCount = 0;
        private PlayerStat m_firstPlayer;
        public PlayerStat firstPlayer { get { return m_firstPlayer; } set { m_firstPlayer = value; } }
        private void Start()
        {
            MyGlobalServerRpc();
        }

        // This metode runs on the server and called by ever client once they joind into the game
        [ServerRpc(RequireOwnership = false)]
        public void MyGlobalServerRpc(ServerRpcParams serverRpcParams = default)
        {
            // This gets the sender client id, and adds one to the over all player count
            int clientID = Convert.ToInt32(serverRpcParams.Receive.SenderClientId);
            playerCount++;
            Debug.Log("playercount: " + playerCount + " " + gameObject);

            // This gets a gameobject/prefab depending on th player count
            GameObject prefab = PrefabChoser(playerCount);
           
            // This Instantiate the prefab as a NetworkObject
            NetworkObject meh = Instantiate(prefab).GetComponent<NetworkObject>();
           
            // This spawn the Network Object with the sender client's ownership, then try to reparent it
            meh.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
            meh.TrySetParent(Statplace.transform, false);
            
            // This get the PlayerStat component from the Network Object
            PlayerStat stat = meh.GetComponent<PlayerStat>();

            // These are setting some data for the client's stat
            stat.clientId = serverRpcParams.Receive.SenderClientId;
            stat.ownerID = clientID;

            // Here the client gets its first 4 card
            stat.hand = GameManager.Instance.DealCards(clientID, stat.hand);
            
            // Here the client gets the ticket that it can choose from
            stat.tickets = GameManager.Instance.DealTickets(clientID, stat.tickets, true);

            // This adds the client stat to the stat list, which includes every clients stat
            stats.Add(stat);

            // This sets the target client to the ClientRpcParams
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { serverRpcParams.Receive.SenderClientId }
                }
            };

            // This is a new int array
            int[] ticketIDs = new int[stat.tickets.Count];

            // This goes trough the stat ticket list, and adds the tickets id to the array
            for (int i = 0; i < stat.tickets.Count; i++)
            {
                ticketIDs[i] = stat.tickets[i].ticketID;
            }

            // This metode is called to spawn the tickets locacaly
            GameManager.Instance.SpawnTicketsLocalyClientRpc(ticketIDs, stat.clientId, clientRpcParams);
        }

        private void Update()
        {
            if (!IsServer) return;

            // If the game is not started yet it will go trough the player list and check if everyone is ready, then draw a random player who will be the first
            if (!gameStarted)
            {
                foreach (PlayerStat stat in stats)
                {
                    // Returns if any of the players are not ready
                    if (!stat.isReady) return;
                }
                //Gets a random player and makes it to the first player
                int randomplayer = UnityEngine.Random.Range(0, stats.Count);
                firstPlayer = stats[randomplayer];
                stats[randomplayer].myTurn = true;

                // Sends some data for the Turn Manager, so later we can use them
                TurnM.Instance.firstPlayerId = stats[randomplayer].ownerID;
                TurnM.Instance.currentPlayerIndex = randomplayer;
                TurnM.Instance.players = stats;
                gameStarted = true;

                // This sends the the first players id to the turn started metode
                TurnM.Instance.TurnStarted(firstPlayer.clientId, true);
                Debug.Log("Game started! First player: "+stats[randomplayer].clientId);
            }


        }

        // This metode gets an int and sets the GameObject to one of the already declared prefab, then returns the gamobject
        public GameObject PrefabChoser(int id)
        {
            GameObject prefab = null;
            if(id == 1) { prefab = Player1; }
            else if(id == 2) { prefab = Player2; }
            else if(id==3) { prefab = Player3; }
            else if(id == 4) { prefab = Player4; }
            else if(id == 5) { prefab = Player5; }
            return prefab;
        }

        public void Enable_Disable()
        {
            if(blackButton.interactable)
            {
                blackButton.interactable = false;
            }else { blackButton.interactable = true; }
            if (blueButton.interactable)
            {
                blueButton.interactable = false;
            }
            else { blueButton.interactable = true; }
            if (brownButton.interactable)
            {
                brownButton.interactable = false;
            }
            else { brownButton.interactable = true; }
            if (greenButton.interactable)
            {
                greenButton.interactable = false;
            }
            else { greenButton.interactable = true; }
            if (orangeButton.interactable)
            {
                orangeButton.interactable = false;
            }
            else { orangeButton.interactable = true; }
            if (purpleButton.interactable)
            {
                purpleButton.interactable = false;
            }
            else { purpleButton.interactable = true; }
            if (whiteButton.interactable)
            {
                whiteButton.interactable = false;
            }
            else { whiteButton.interactable = true; }
            if (yellowButton.interactable)
            {
                yellowButton.interactable = false;
            }
            else { yellowButton.interactable = true; }
            if (rainbowButton.interactable)
            {
                rainbowButton.interactable = false;
            }
            else { rainbowButton.interactable = true; }

        }

    }
}
