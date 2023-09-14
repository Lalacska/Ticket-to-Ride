﻿using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSelector : Singleton<CardSelector>
{

    [SerializeField] private GameObject CardSelectorArea;
    [SerializeField] private GameObject SelectorArea;

    [SerializeField] private GameObject BlackPrefabCanvas;
    [SerializeField] private GameObject BluePrefabCanvas;
    [SerializeField] private GameObject BrownPrefabCanvas;
    [SerializeField] private GameObject GreenPrefabCanvas;
    [SerializeField] private GameObject OrangePrefabCanvas;
    [SerializeField] private GameObject PurplePrefabCanvas;
    [SerializeField] private GameObject WhitePrefabCanvas;
    [SerializeField] private GameObject YellowPrefabCanvas;
    [SerializeField] private GameObject RainbowPrefabCanvas;


    [SerializeField] private TMP_Text selectedCards;
    [SerializeField] private TMP_Text neededCards;


    [SerializeField] private List<GameObject> Buttons;

    [SerializeField] private GameObject CancelButton;
    [SerializeField] private GameObject PlayButton;



    private List<GameObject> cards;
    private int cardCounter;
    
    


    private PlayerStat player;


    private NetworkVariable<int> m_maxCard = new NetworkVariable<int>();
    [SerializeField] private NetworkVariable<bool> m_isValid = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isValid { get { return m_isValid; } set { m_isValid = value; } }
    public NetworkVariable<int> maxCard { get { return m_maxCard; } set { m_maxCard = value; } }



    public void AutoSelectCards(string type, string color, int amount = 6, int neededRainbow = 0)
    {
        cards = new List<GameObject>();
        Debug.Log("Amount: " + amount);
        Enable_DisableSelectorArea();
        ResetIsValidServerRpc();
        if (type == "Station")
        {
            CheckStationsNumberServerRpc();

            Debug.Log("Max card: " + maxCard);

        }
        else if(type == "Route")
        {
            SetCardAmountServerRpc(amount);
            //neededCards.text = maxCard.Value.ToString();
        }
        else if(type == "Tunnel")
        {
            SetCardAmountServerRpc(amount);
            //neededCards.text = maxCard.Value.ToString();
        }
    }

    #region Station

    [ServerRpc(RequireOwnership = false)]
    public void CheckStationsNumberServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        // Set the target client
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientId)
            {
                player = stat;
            }
        }

        if(player != null && player.stations.Value > 0)
        {
            Debug.Log("Valid");
            Debug.Log(player.stations.Value);
            string amount = (4 - player.stations.Value).ToString();
            maxCard.Value = (4 - player.stations.Value);

            SetTextClientRpc(amount);
            EnableCardButtonsClientRpc();

            //Debug.Log("Max card: " + maxCard);
            //player.stations.Value--;
            //Debug.Log(player.stations.Value);



        }
        else
        {
            isValid.Value = false;
            Debug.Log("The player can't have more station");
            return;
        }
    }


    //[ClientRpc]
    //public void EnableCardButtonsClientRpc(string amount, ClientRpcParams clientRpcParams)
    //{
    //    neededCards.text = amount;
    //    Debug.Log(PlayerStat.Instance.localCards);
    //}
    
    [ClientRpc]
    public void SetTextClientRpc(string amount, ClientRpcParams clientRpcParams = default)
    {
        neededCards.text = amount;
    }



    [ClientRpc]
    public void EnableCardButtonsClientRpc(string color = "none", ClientRpcParams clientRpcParams = default)
    {
        
        string buttonname = "";
        switch (color)
        {
            case "Black":
                buttonname = "Black-Btn";
                break;
            case "Blue":
                buttonname = "Blue-Btn";
                break;
            case "Brown":
                buttonname = "Brown-Btn";
                break;
            case "Green":
                buttonname = "Green-Btn";
                break;
            case "Orange":
                buttonname = "Orange-Btn";
                break;
            case "Purple":
                buttonname = "Purple-Btn";
                break;
            case "White":
                buttonname = "White-Btn";
                break;
            case "Yellow":
                buttonname = "Yellow-Btn";
                break;
            case "Rainbow":
                buttonname = "Rainbow-Btn";
                break;
            case "Grey":
                break;
        }

        Debug.Log(buttonname);
        
        foreach(GameObject gameObject in Buttons)
        {
            Debug.Log(gameObject.name);
            Debug.Log(color);

            Button buttonComponent = gameObject.GetComponent<Button>();

            if(color == "Grey" || color == "none")
            {

                Debug.Log("all");
                buttonComponent.interactable = true;
            }else
            {
                
                if(gameObject.name == buttonname || gameObject.name == "Rainbow-Btn")
                {

                    Debug.Log(buttonname);
                    buttonComponent.interactable = true;
                }
            }
        }


    }

    public void DisableCardButtons()
    {
        foreach (GameObject gameObject in Buttons)
        {
            Debug.Log(gameObject.name);

            Button buttonComponent = gameObject.GetComponent<Button>();

            if(buttonComponent.interactable)
            {
                buttonComponent.interactable = false;
            }
        }
    }




    public void test()
    {
       Debug.Log( EventSystem.current.currentSelectedGameObject.name);
    }




    #endregion

    [ServerRpc(RequireOwnership = false)]
    public void ResetIsValidServerRpc(ServerRpcParams serverRpcParams = default)
    {
        isValid.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SetCardAmountServerRpc(int amount, ServerRpcParams serverRpcParams = default)
    {
        maxCard.Value = amount;
    }

    public void Enable_DisableSelectorArea()
    {
        if (CardSelectorArea.activeInHierarchy)
        {
            CardSelectorArea.SetActive(false);
        }
        else
        {
            CardSelectorArea.SetActive(true);
        }
    }



    public void CardCounter()
    {

    }


    public void TestSpawn()
    {
        Debug.Log("Max card: " + maxCard);
        Debug.Log("Cards count: " + cards.Count);
        if (cards.Count < maxCard.Value)
        {
            GameObject card = Instantiate(BlackPrefabCanvas, SelectorArea.transform);
            cards.Add(card);
        }
        else
        {
            Debug.Log("You can't add more cards!");
        }

    }


    public void SpawnCard(GameObject prefab)
    {
        Debug.Log("Max card: " + maxCard);
        Debug.Log("Cards count: " + cards.Count);
        if (cards.Count < maxCard.Value)
        {
            GameObject card = Instantiate(prefab, SelectorArea.transform);
            cards.Add(card);
            selectedCards.text = cards.Count.ToString();
            if(cards.Count == maxCard.Value)
            {
                DisableCardButtons();
            }
        }
        else
        {
            Debug.Log("You can't add more cards!");
        }

    }

    public void DispawnCard(GameObject go)
    {
        cards.Remove(go);
        selectedCards.text = cards.Count.ToString();
        Debug.Log("Cards count: " + cards.Count);
        foreach (GameObject card in cards)
        {
            Debug.Log(card.name);
        }
        EnableCardButtonsClientRpc();
    }



    // This methode is for the card play buttons, it gets an int from the button, sets the color
    // then calls PlayCardHand methode with the color
    public void PlayCardBTN(int button)
    {
        GameObject prefab = null;
        string color = "";
        switch (button)
        {
            case 1:
                color = "Black";
                prefab = BlackPrefabCanvas;
                break;
            case 2:
                color = "Blue";
                prefab = BluePrefabCanvas;
                break;
            case 3:
                color = "Brown";
                prefab = BrownPrefabCanvas;
                break;
            case 4:
                color = "Green";
                prefab = GreenPrefabCanvas;
                break;
            case 5:
                color = "Orange";
                prefab = OrangePrefabCanvas;
                break;
            case 6:
                color = "Purple";
                prefab = PurplePrefabCanvas;
                break;
            case 7:
                color = "White";
                prefab = WhitePrefabCanvas;
                break;
            case 8:
                color = "Yellow";
                prefab = YellowPrefabCanvas;
                break;
            case 9:
                color = "Rainbow";
                prefab = RainbowPrefabCanvas;
                break;
        }
        if(prefab == null) return;
        SpawnCard(prefab);
    }

}

