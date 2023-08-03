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
        [SerializeField] private GameObject Player1;
        [SerializeField] private GameObject Player2;
        [SerializeField] private GameObject Player3;
        [SerializeField] private GameObject Player4;
        [SerializeField] private GameObject Player5;

        [SerializeField] private List<PlayerStat> Stats;

        public override void OnNetworkSpawn()
        {
            MyGlobalServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void MyGlobalServerRpc(ServerRpcParams serverRpcParams = default)
        {
            var clientid = serverRpcParams.Receive.SenderClientId;
            Debug.Log("Hello " + clientid);
            int id = Convert.ToInt32(clientid);

            foreach(PlayerStat stat in Stats)
            {
                if(id == stat.StatCardID - 1)
                {
                    stat.ownerID = id;
                    GameObject StatCard = PrefabChoser(stat.StatCardID);
                    StatCard.GetComponent<NetworkObject>().Spawn();

                }
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

    }
}
