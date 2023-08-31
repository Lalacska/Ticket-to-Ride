using Assets.Scripts.Cards;
using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToPool : Singleton<ReturnToPool>
{
    [SerializeField] private NetworkObject go;
    private Card card;
    private GameManager gameManager;

    private void Awake()
    {
        card = go.GetComponent<Card>();
        gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (card.transform.position == gameManager.discardPileDestination.transform.position)
        {
            Debug.Log(gameManager.isDeckEmpty);
            if (gameManager.isDeckEmpty)
            {
                GameObject prefab = gameManager.GetPrefabByColor(card.Color);
                NetworkObjectPool.Instance.ReturnNetworkObject(go, prefab);
                gameManager.CardCounterControll(card.Color);
                Debug.Log("Returned");
            }
        }
        
    }

}

