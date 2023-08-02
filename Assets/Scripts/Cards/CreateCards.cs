using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Cards
{
    public class CreateCards : Singleton<CreateCards>
    {

        //[SerializeField] private Transform BlackPrefab;

        //private void Start()
        //{
        //    Debug.Log("hez");
        //    Something();
        //}
        ////public override void OnNetworkSpawn()
        ////{
        ////    Something();
        ////}

        //private void Something()
        //{
        //    //GameObject obj = GameObject.FindGameObjectWithTag("GameManager");
        //    //GameManager GM = obj.gameObject.GetComponent<GameManager>();
        //    Transform go = Instantiate(BlackPrefab, Vector3.up, Quaternion.identity);
        //    //Card card = go.GetComponent<Card>();
        //    //card.CardID = 16;
        //    //GM.deck.Add(card);
        //    go.GetComponent<NetworkObject>().Spawn(true);

        //}
    }
}
