using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
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

        [SerializeField] private List<PlayerStat> Stats;

        private int playerCount = 0;

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
            stat.tickets = GameManager.Instance.DealTickets(clientID, stat.tickets);

            if (playerCount == 1)
            {
                stat.myTurn = true;
            }
            Stats.Add(stat);

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

            GameManager.Instance.SpawnTicketsLocalyClientRpc(ticketIDs, clientRpcParams);
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
