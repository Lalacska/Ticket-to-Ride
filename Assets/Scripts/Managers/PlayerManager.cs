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
            Debug.Log(playerCount);
            GameObject prefab = PrefabChoser(playerCount);
            NetworkObject meh = Instantiate(prefab).GetComponent<NetworkObject>();
            meh.SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
            meh.TrySetParent(Statplace.transform, false);
            PlayerStat stat = meh.GetComponent<PlayerStat>();
            stat.ownerID = clientID;
            stat.hand = GameManager.Instance.DealCards(clientID, stat.hand);
            if(playerCount == 1)
            {
                stat.myTurn = true;
            }
            Stats.Add(stat);
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

    }
}
