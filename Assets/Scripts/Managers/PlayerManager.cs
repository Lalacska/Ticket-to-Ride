using Assets.Scripts.Cards;
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

        //public override void OnNetworkSpawn()
        //{
        //    MyGlobalServerRpc();
        //}

        [ServerRpc(RequireOwnership = false)]
        public void MyGlobalServerRpc(ServerRpcParams serverRpcParams = default)
        {
            int clientID = Convert.ToInt32(serverRpcParams.Receive.SenderClientId);
            playerCount++;
            Debug.Log("playercount: " + playerCount + " " + gameObject);

            GameObject prefab = PrefabChoser(playerCount);
            NetworkObject meh = Instantiate(prefab).GetComponent<NetworkObject>();
            meh.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
            meh.TrySetParent(Statplace.transform, false);
            PlayerStat stat = meh.GetComponent<PlayerStat>();

            stat.clientId = serverRpcParams.Receive.SenderClientId;
            stat.ownerID = clientID;
            stat.hand = GameManager.Instance.DealCards(clientID, stat.hand);
            stat.tickets = GameManager.Instance.DealTickets(clientID, stat.tickets, true);

            stats.Add(stat);

            // This set the ClientRpc
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { serverRpcParams.Receive.SenderClientId }
                }
            };

            int[] ticketIDs = new int[stat.tickets.Count];


            for (int i = 0; i < stat.tickets.Count; i++)
            {
                ticketIDs[i] = stat.tickets[i].ticketID;
            }

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
                    if (!stat.isReady) return;
                }
                int randomplayer = UnityEngine.Random.Range(0, stats.Count);
                firstPlayer = stats[randomplayer];
                stats[randomplayer].myTurn = true;
                TurnM.Instance.firstPlayerId = stats[randomplayer].ownerID;
                TurnM.Instance.currentPlayerIndex = randomplayer;
                TurnM.Instance.players = stats;
                gameStarted = true;

                TurnM.Instance.TurnStarted(firstPlayer.clientId);
                Debug.Log("Game started! First player: "+stats[randomplayer].clientId);
            }


        }

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
