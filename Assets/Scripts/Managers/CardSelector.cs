using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum Type { None, Route, Tunnel, Station}
public class CardSelector : Singleton<CardSelector>
{
    private Type _type;

    [SerializeField] private GameObject CardSelectorArea;
    [SerializeField] private GameObject SelectorArea;

    [SerializeField] private GameObject BlackPrefabCanvas;
    [SerializeField] private GameObject BluePrefabCanvas;
    [SerializeField] private GameObject OrangePrefabCanvas;
    [SerializeField] private GameObject GreenPrefabCanvas;
    [SerializeField] private GameObject RedPrefabCanvas;
    [SerializeField] private GameObject PinkPrefabCanvas;
    [SerializeField] private GameObject WhitePrefabCanvas;
    [SerializeField] private GameObject YellowPrefabCanvas;
    [SerializeField] private GameObject RainbowPrefabCanvas;


    [SerializeField] private TMP_Text selectedCards;
    [SerializeField] private TMP_Text neededCards;


    [SerializeField] private List<GameObject> Buttons;

    [SerializeField] private Button CancelButton;
    [SerializeField] private Button PlayButton;



    private List<GameObject> cardObjects;

    [SerializeField] private List<Card> cardsInSelection;



    private int BlackCardCounter;
    private int BlueCardCounter;
    private int OrangeCardCounter;
    private int GreenCardCounter;
    private int RedCardCounter;
    private int PinkCardCounter;
    private int WhiteCardCounter;
    private int YellowCardCounter;
    private int RainbowCardCounter;

    private string placeName;
    private bool isColorless;
    private string routeColor;

    private PlayerStat player;


    private NetworkVariable<int> m_maxCard = new NetworkVariable<int>();
    [SerializeField] private NetworkVariable<bool> m_isValid = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isValid { get { return m_isValid; } set { m_isValid = value; } }
    public NetworkVariable<int> maxCard { get { return m_maxCard; } set { m_maxCard = value; } }



    public void AutoSelectCards(string type, string color, int amount = 6, int neededRainbow = 0, string m_name = "")
    {
        routeColor = string.Empty;
        cardsInSelection.Clear();
        cardObjects = new List<GameObject>();
        Debug.Log("Amount: " + amount);
        Enable_DisableSelectorArea(true);
        ResetCardCounters();
        ResetIsValidServerRpc();
        SetCounters();
        placeName = m_name;
        if (type == "Station")
        {
            _type = Type.Station;
            Debug.Log("MaxAmount: "+ maxCard.Value + " CardCount: " + cardObjects.Count);
            GameManager.Instance.TurnOffHighlightStation();
            CheckStationsNumberServerRpc();

            Debug.Log("Max card: " + maxCard.Value);

        }
        else
        {
            routeColor = color;
            Debug.Log("CardSelector color: " +color);
            SetCardAmountServerRpc(amount);
            GameManager.Instance.TurnOffHighlightRoutes();
            Debug.Log("I'm " + m_name + " and I'm a " + type);
            EnableCardButtons(color);
        }
        //else if (type == "Route")
        //{
        //    _type = Type.Route;
        //    SetCardAmountServerRpc(amount);
        //    GameManager.Instance.TurnOffHighlightRoutes();
        //    Debug.Log("I'm "+ m_name+ " and I'm a " + type);
        //    //neededCards.text = maxCard.Value.ToString();
        //}
        //else if (type == "Tunnel")
        //{
        //    _type = Type.Tunnel;
        //    SetCardAmountServerRpc(amount);
        //    GameManager.Instance.TurnOffHighlightRoutes();
        //    Debug.Log("I'm " + m_name + " and I'm a " + type);
        //    //neededCards.text = maxCard.Value.ToString();
        //}
    }



    public void SpawnCard(GameObject prefab, string color)
    {
        Debug.Log("Max card: " + maxCard);
        Debug.Log(color);
        Debug.Log("Cards count: " + cardObjects.Count);
        if (cardObjects.Count < maxCard.Value)
        {
            GameObject card = Instantiate(prefab, SelectorArea.transform);
            cardObjects.Add(card);
            selectedCards.text = cardObjects.Count.ToString();
            if (cardObjects.Count == maxCard.Value)
            {
                DisableCardButtons();
            }
            CardCounterHandler(color, true);
            HandlePlayerHandServerRpc(color, false);
            EnableCardButtons(color);

        }
        else
        {
            Debug.Log("You can't add more cards!");
        }
    }



    public void DespawnCard(GameObject go)
    {
        string color = "none";
        RandomDespawn rd = go.GetComponent<RandomDespawn>();
        CardCounterHandler(rd.Color, false);
        HandlePlayerHandServerRpc(rd.Color, true);
        cardObjects.Remove(go);


        selectedCards.text = cardObjects.Count.ToString();
        Debug.Log("Cards count: " + cardObjects.Count);

        foreach (GameObject card in cardObjects)
        {
            RandomDespawn randomD = card.GetComponent<RandomDespawn>();
            Debug.Log(card.name);
            if (randomD.Color != "Rainbow")
            {
                color = randomD.Color;
            }
        }
        EnableCardButtons(color);
    }





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

        if (player != null && player.stations.Value > 0)
        {
            Debug.Log("Valid");
            Debug.Log(player.stations.Value);
            string amount = (4 - player.stations.Value).ToString();
            maxCard.Value = (4 - player.stations.Value);
            Debug.Log("MaxAmount: " + maxCard.Value /*+ " CardCount: " + cardObjects.Count*/);


            SetTextClientRpc(amount);
            EnableCardButtonsClientRpc(clientRpcParams: clientRpcParams);

        }
        else
        {
            isValid.Value = false;
            Debug.Log("The player can't have more station");
            return;
        }
    }


    public void PlayAction()
    {
        Enable_DisableSelectorArea(false);

        switch (_type)
        {
            case Type.Station:
                GameManager.Instance.ChooseCity(placeName);
                foreach (GameObject gameObject in GameManager.Instance.stations)
                {
                    Station station = gameObject.GetComponent<Station>();
                    if(station.stationName == placeName)
                    {
                        station.SetIsTakenServerRpc();
                        Debug.Log("Built");
                    }
                }
                StationHandlerServerRpc();
                break;
            case Type.Route:

                break;
            case Type.Tunnel:
                break;
        }
        foreach(GameObject go in cardObjects)
        {
            Destroy(go);
        }
        cardObjects.Clear();
        selectedCards.text = cardObjects.Count.ToString();
        CardsToDiscardPileServerRpc();
        TurnM.Instance.EndTurn();
    }


    [ServerRpc(RequireOwnership = false)]
    public void CardsToDiscardPileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        foreach (Card card in cardsInSelection.ToList())
        {
            cardsInSelection.Remove(card);
            GameManager.Instance.discardPile.Add(card);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StationHandlerServerRpc(ServerRpcParams serverRpcParams = default)
    {
        ulong clientId = serverRpcParams.Receive.SenderClientId;
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientId)
            {
                player = stat;
            }
        }
        player.stations.Value--;
        Debug.Log(player.stations.Value);
    }



    public void CancelAction()
    {
        Enable_DisableSelectorArea(false);
        foreach (GameObject gameObject in cardObjects)
        {
            RandomDespawn rd = gameObject.GetComponent<RandomDespawn>();
            HandlePlayerHandServerRpc(rd.Color, true);
        }
        TurnM.Instance.Enable_DisableActionChooser(true);
    }




    [ClientRpc]
    public void EnableCardButtonsClientRpc(string color = "none", ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Enable ClientRpc");
        EnableCardButtons(color);
    }


    public void EnableCardButtons(string color = "none")
    {
        Debug.Log(routeColor);
        if (routeColor != string.Empty && color == "none")
        {
            color = routeColor;
        }
        Debug.Log("Enable Client");
        bool available = false;
        bool onlyRainbow = true;
        string buttonname = "";
        switch (color)
        {
            case "Black":
                buttonname = "Black-Btn";
                break;
            case "Blue":
                buttonname = "Blue-Btn";
                break;
            case "Orange":
                buttonname = "Orange-Btn";
                break;
            case "Green":
                buttonname = "Green-Btn";
                break;
            case "Red":
                buttonname = "Red-Btn";
                break;
            case "Pink":
                buttonname = "Pink-Btn";
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
            case "none":
                break;
        }

        Debug.Log(buttonname);

        if(cardObjects.Count > 0)
        {
            foreach (GameObject go in cardObjects)
            {
                Debug.Log(" MEEEP cards");
                RandomDespawn card = go.GetComponent<RandomDespawn>();
                if (card.Color != "Rainbow")
                {
                    onlyRainbow = false;
                }
            }
        }
        else { onlyRainbow = false;}
        

        //Debug.Log("MaxAmount: " + maxCard.Value + " CardCount: " + cardObjects.Count);

        foreach (GameObject gameObject in Buttons)
        {
            //Debug.Log("something");
            Button buttonComponent = gameObject.GetComponent<Button>();

            //Debug.Log("MaxAmount: " + maxCard.Value + " CardCount: " + cardObjects.Count);

            if (cardObjects.Count != maxCard.Value || cardObjects.Count == 0)
            {
                available = CheckCardAmount(gameObject.name);
                //Debug.Log(gameObject.name);
                //Debug.Log(color);

                //Debug.Log("Available: " +available);

                Debug.Log("Color : " + color + " onlyRainbow " + onlyRainbow + " routeColor " + routeColor);

                if (available)
                {
                    if (color == "Grey" || color == "none" || (onlyRainbow && routeColor == "Grey") || (onlyRainbow && routeColor == "none"))
                    {
                        Debug.Log(color + " " + onlyRainbow);
                        Debug.Log("all");
                        buttonComponent.interactable = true;
                    }
                    else
                    {
                        if (gameObject.name == buttonname || gameObject.name == "Rainbow-Btn" 
                            || routeColor == GetColorFromString(gameObject.name) || GetColorFromString(gameObject.name) == CheckSelectedCardColor())
                        {
                            Debug.Log("This button is active: "+ buttonname);
                            buttonComponent.interactable = true;
                        }
                        else
                        {
                            buttonComponent.interactable = false;
                        }
                    }
                }
                else
                {
                    buttonComponent.interactable = false;
                }
            }
            else
            {
                buttonComponent.interactable = false;
            }
        }
        Enable_DisablePlayButton();

    }


    public string GetColorFromString(string buttonname)
    {
        string color = string.Empty;
        int index = buttonname.IndexOf("-");
        if (index >= 0)
        {
            color = buttonname.Substring(0, index);
        }
        return color;
    }

    public string CheckSelectedCardColor()
    {
        string color = string.Empty;
        foreach (GameObject card in cardObjects)
        {
            RandomDespawn randomD = card.GetComponent<RandomDespawn>();
            Debug.Log(card.name);
            if (randomD.Color != "Rainbow")
            {
                color = randomD.Color;
            }
        }
        return color;
    }

    public void Enable_DisablePlayButton()
    {

        if (cardObjects.Count == maxCard.Value)
        {
            PlayButton.interactable = true;
        }
        else { PlayButton.interactable = false; }
    }



   
    

   
   

   


    

    [ServerRpc(RequireOwnership = false)]
    public void HandlePlayerHandServerRpc(string color, bool dispawn, ServerRpcParams serverRpcParams = default)
    {
        bool foundCard = false;
        player = null;
        ulong clientID = serverRpcParams.Receive.SenderClientId;

        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientID)
            {
                player = stat;
            }
        }

        if (dispawn)
        {
            foreach(Card card in cardsInSelection.ToList())
            {
                if (!foundCard)
                {
                    if(card.Color == color)
                    {
                        foundCard = true;
                        cardsInSelection.Remove(card);
                        player.hand.Add(card);
                    }
                }
            }

        }
        else
        {
            foreach(Card card in player.hand.ToList())
            {
                if (!foundCard)
                {
                    if (card.Color == color)
                    {
                        foundCard = true;
                        cardsInSelection.Add(card);
                        player.hand.Remove(card);
                    }
                }
            }
        }
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
                color = "Orange";
                prefab = OrangePrefabCanvas;
                break;
            case 4:
                color = "Green";
                prefab = GreenPrefabCanvas;
                break;
            case 5:
                color = "Red";
                prefab = RedPrefabCanvas;
                break;
            case 6:
                color = "Pink";
                prefab = PinkPrefabCanvas;
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
        SpawnCard(prefab, color);
    }


   

    // Simple and small metodes which don't call new metodes
    #region Handlers 


    // Enables and disables selecetor area
    public void Enable_DisableSelectorArea(bool enable)
    {
        if (enable)
        {
            CardSelectorArea.SetActive(true);
        }
        else
        {
            CardSelectorArea.SetActive(false);
        }
    }

    // This goes troug all the Buttons and makes them not interactable if they were interactable before
    public void DisableCardButtons()
    {
        foreach (GameObject gameObject in Buttons)
        {
            Debug.Log(gameObject.name);

            Button buttonComponent = gameObject.GetComponent<Button>();

            if (buttonComponent.interactable)
            {
                buttonComponent.interactable = false;
            }
        }
    }

    // Gets a buttonName, and before sends a bool back, checks if the counter for that button is more then 0
    // if it is, it's sets the bool true and sends the bool back
    public bool CheckCardAmount(string buttonname)
    {
        bool availableCard = false;

        switch (buttonname)
        {
            case "Black-Btn":
                if (BlackCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Blue-Btn":
                if (BlueCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Orange-Btn":
                if (OrangeCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Green-Btn":
                if (GreenCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Red-Btn":
                if (RedCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Pink-Btn":
                if (PinkCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "White-Btn":
                if (WhiteCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Yellow-Btn":
                if (YellowCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
            case "Rainbow-Btn":
                if (RainbowCardCounter > 0)
                {
                    availableCard = true;
                }
                break;
        }
        return availableCard;
    }

    // Gets a color and a bool, it checks the color then the bool,
    // and if the bool is true it takes one away from the coresponding counter, else it adds one to it
    public void CardCounterHandler(string color, bool spawn)
    {
        switch (color)
        {
            case "Black":
                if (spawn)
                {
                    BlackCardCounter--;
                }
                else
                {
                    BlackCardCounter++;
                }
                break;
            case "Blue":
                if (spawn)
                {
                    BlueCardCounter--;
                }
                else
                {
                    BlueCardCounter++;
                }
                break;
            case "Orange":
                if (spawn)
                {
                    OrangeCardCounter--;
                }
                else
                {
                    OrangeCardCounter++;
                }
                break;
            case "Green":
                if (spawn)
                {
                    GreenCardCounter--;
                }
                else
                {
                    GreenCardCounter++;
                }
                break;
            case "Red":
                if (spawn)
                {
                    RedCardCounter--;
                }
                else
                {
                    RedCardCounter++;
                }
                break;
            case "Pink":
                if (spawn)
                {
                    PinkCardCounter--;
                }
                else
                {
                    PinkCardCounter++;
                }
                break;
            case "White":
                if (spawn)
                {
                    WhiteCardCounter--;
                }
                else
                {
                    WhiteCardCounter++;
                }
                break;
            case "Yellow":
                if (spawn)
                {
                    YellowCardCounter--;
                }
                else
                {
                    YellowCardCounter++;
                }
                break;
            case "Rainbow":
                if (spawn)
                {
                    RainbowCardCounter--;
                }
                else
                {
                    RainbowCardCounter++;
                }
                break;
        }
    }

    // Set isVaild to true on the server
    [ServerRpc(RequireOwnership = false)]
    public void ResetIsValidServerRpc(ServerRpcParams serverRpcParams = default)
    {
        isValid.Value = true;
    }

    // Resets card counters
    public void ResetCardCounters()
    {
        BlackCardCounter = 0;
        BlueCardCounter = 0;
        OrangeCardCounter = 0;
        GreenCardCounter = 0;
        RedCardCounter = 0;
        PinkCardCounter = 0;
        WhiteCardCounter = 0;
        YellowCardCounter = 0;
        RainbowCardCounter = 0;
    }

    // Gets the localcards dictionary from playerStat, and sets the right value for the right counter
    public void SetCounters()
    {
        Dictionary<FixedString128Bytes, int> localcards = PlayerStat.Instance.localCards;
        foreach (KeyValuePair<FixedString128Bytes, int> kvp in localcards.ToList())
        {
            Debug.Log("Key: " + kvp.Key + "  Value: " + kvp.Value);
        }
        BlackCardCounter = localcards.ElementAt(0).Value;
        BlueCardCounter = localcards.ElementAt(1).Value;
        OrangeCardCounter = localcards.ElementAt(2).Value;
        GreenCardCounter = localcards.ElementAt(3).Value;
        RedCardCounter = localcards.ElementAt(4).Value;
        PinkCardCounter = localcards.ElementAt(5).Value;
        WhiteCardCounter = localcards.ElementAt(6).Value;
        YellowCardCounter = localcards.ElementAt(7).Value;
        RainbowCardCounter = localcards.ElementAt(8).Value;
    }

    // Sets the text on the client
    [ClientRpc]
    public void SetTextClientRpc(string amount, ClientRpcParams clientRpcParams = default)
    {
        neededCards.text = amount;
    }

    // Set maxCard amount on the server
    [ServerRpc(RequireOwnership = false)]
    public void SetCardAmountServerRpc(int amount, ServerRpcParams serverRpcParams = default)
    {
        maxCard.Value = amount;
        SetTextClientRpc(amount.ToString());
    }


    #endregion

    // Test for stuff DELETE THESE LATER!!!!
    #region Tests

    public void test()
    {
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }

    public void TestSpawn()
    {
        Debug.Log("Max card: " + maxCard);
        Debug.Log("Cards count: " + cardObjects.Count);
        if (cardObjects.Count < maxCard.Value)
        {
            GameObject card = Instantiate(BlackPrefabCanvas, SelectorArea.transform);
            cardObjects.Add(card);
        }
        else
        {
            Debug.Log("You can't add more cardObjects!");
        }

    }



    //[ClientRpc]
    //public void EnableCardButtonsClientRpc(string amount, ClientRpcParams clientRpcParams)
    //{
    //    neededCards.text = amount;
    //    Debug.Log(PlayerStat.Instance.localCards);
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void EnableCardButtonsServerRpc(string color = "none", ServerRpcParams serverRpcParams = default) 
    //{
    //    ulong clientID = serverRpcParams.Receive.SenderClientId;
    //    // Set the target client
    //    ClientRpcParams clientRpcParams = new ClientRpcParams
    //    {
    //        Send = new ClientRpcSendParams
    //        {
    //            TargetClientIds = new ulong[] { clientID }
    //        }
    //    };

    //    EnableCardButtonsClientRpc(color: color, clientRpcParams: clientRpcParams);
    //}


    #endregion

}

