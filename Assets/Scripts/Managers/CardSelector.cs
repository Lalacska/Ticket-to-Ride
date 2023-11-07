﻿using Assets.Scripts.Managers;
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

    // Areas
    [SerializeField] private GameObject CardSelectorArea;
    [SerializeField] private GameObject SelectorArea;
    [SerializeField] private GameObject TunnelArea;
    [SerializeField] private GameObject ExtraCardArea;
    [SerializeField] private GameObject TunnelComponents;

    // Prefabs
    [SerializeField] private GameObject BlackPrefabCanvas;
    [SerializeField] private GameObject BluePrefabCanvas;
    [SerializeField] private GameObject OrangePrefabCanvas;
    [SerializeField] private GameObject GreenPrefabCanvas;
    [SerializeField] private GameObject RedPrefabCanvas;
    [SerializeField] private GameObject PinkPrefabCanvas;
    [SerializeField] private GameObject WhitePrefabCanvas;
    [SerializeField] private GameObject YellowPrefabCanvas;
    [SerializeField] private GameObject RainbowPrefabCanvas;

    // Texts
    [SerializeField] private TMP_Text selectedCards;
    [SerializeField] private TMP_Text neededCards;
    [SerializeField] private TMP_Text selectedTunnelCards;
    [SerializeField] private TMP_Text neededTunnelCards;

    // Card buttons
    [SerializeField] private List<GameObject> Buttons;
    [SerializeField] private Button PlayButton;

    // List for card objects
    private List<GameObject> cardObjects;
    private List<GameObject> tunnelObjects;
    private List<GameObject> extraCardsObjects;
    private List<string> tunnelCardColors;

    [SerializeField] private List<Card> cardsInSelection;
    [SerializeField] private List<Card> tunnelCardsInSelection;

    private List<FixedString32Bytes> selectedCardColors;


    // Counters
    private int neededExtraCards;

    private int BlackCardCounter;
    private int BlueCardCounter;
    private int OrangeCardCounter;
    private int GreenCardCounter;
    private int RedCardCounter;
    private int PinkCardCounter;
    private int WhiteCardCounter;
    private int YellowCardCounter;
    private int RainbowCardCounter;

    private int neededRainbowCards;
    private int selectedRainbowCards;

    // Needed variables
    private string placeName;
    private string routeColor;
    private string playedCardColor;

    private PlayerStat player;

    private bool m_beforeTunnelPulling = true;
    public bool beforeTunnelPulling { get { return m_beforeTunnelPulling; } set { m_beforeTunnelPulling = value; } }


    // Network Variables
    private NetworkVariable<int> m_maxCard = new NetworkVariable<int>();
    [SerializeField] private NetworkVariable<bool> m_isValid = new NetworkVariable<bool>(true);
    public NetworkVariable<bool> isValid { get { return m_isValid; } set { m_isValid = value; } }
    public NetworkVariable<int> maxCard { get { return m_maxCard; } set { m_maxCard = value; } }

    // Sets the variables
    public void SetUp()
    {
        TunnelComponents.SetActive(false);
        neededExtraCards = 0;
        selectedRainbowCards = 0;
        routeColor = string.Empty;
        beforeTunnelPulling = true;
        cardsInSelection.Clear();
        extraCardsObjects = new List<GameObject>();
        cardObjects = new List<GameObject>();
        tunnelObjects = new List<GameObject>();
        tunnelCardColors = new List<string>();
        selectedCardColors = new List<FixedString32Bytes>();
    }
    
    // This metode is hit by every time someone chooses an action where it needs to select card after
    public void AutoSelectCards(string type, string color, int amount = 6, int neededRainbow = 0, string m_name = "")
    {
        // Set up and enable the card selector area, and reset counters.
        SetUp();
        Enable_DisableSelectorArea(true);
        ResetCardCounters();
        ResetIsValidServerRpc();
        SetCounters();
        placeName = m_name;

        // This sets the type general type according what we get from the previous metode, and also runs type specific logic
        if (type == "Station")
        {
            _type = Type.Station;
            GameManager.Instance.TurnOffHighlightStation();
            CheckStationsNumberServerRpc();

            //Debug.Log("Max card: " + maxCard.Value);
            //Debug.Log("MaxAmount: " + maxCard.Value + " CardCount: " + cardObjects.Count);

        }
        else
        {
            // Sets the route or tunnel type, and some other variables
            if (type == "Route")
            {
                _type = Type.Route;
            }
            else
            {
                _type = Type.Tunnel;
            }

            neededRainbowCards = neededRainbow;
            routeColor = color;
            GameManager.Instance.TurnOffHighlightRoutes();
            SetCardAmountServerRpc(amount);
            EnableCardButtons(color);
        }
    }


    //
    public void SpawnCard(GameObject prefab, string color)
    {
        bool canSpawn = true;

        //Debug.Log("Max card: " + maxCard);
        //Debug.Log(color);
        //Debug.Log("Cards count: " + cardObjects.Count);

        if (cardObjects.Count < maxCard.Value)
        {
            if (neededRainbowCards != 0)
            {
                if ((color != "Rainbow" && cardObjects.Count < maxCard.Value - (neededRainbowCards - selectedRainbowCards))
                || color == "Rainbow")
                {
                    canSpawn = true;
                }
                else
                {
                    canSpawn = false;
                }
            }

            if (canSpawn)
            {
                GameObject card = Instantiate(prefab, SelectorArea.transform);
                cardObjects.Add(card);
                RandomDespawn rand = card.GetComponent<RandomDespawn>();
                rand.cardType = RandomDespawn.Type.Card;

                if (color == "Rainbow")
                {
                    selectedRainbowCards++;
                }
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
                Debug.Log("You can't select this card!");
            }
        }
        else
        {
            Debug.Log("You can't select more cards!");
        }
    }

    // Need to use RandomDespawn type to despawn extracards
    public void DespawnCard(GameObject go)
    {
        string color = "none";
        RandomDespawn rd = go.GetComponent<RandomDespawn>();

        CardCounterHandler(rd.Color, false);
        HandlePlayerHandServerRpc(rd.Color, true);

        if (rd.cardType == RandomDespawn.Type.Card)
        {
            cardObjects.Remove(go);
            if (rd.Color == "Rainbow")
            {
                selectedRainbowCards--;
            }

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
        }
        else if (rd.cardType == RandomDespawn.Type.ExtraCard)
        {
            extraCardsObjects.Remove(go);
            selectedTunnelCards.text = extraCardsObjects.Count.ToString();

            color = playedCardColor;

        }

        Debug.Log(color);

        EnableCardButtons(color);
    }


    public void SpawnTunnelCard(GameObject prefab, string color)
    {
        Debug.Log(color);

        GameObject card = Instantiate(prefab, TunnelArea.transform);
        Button button = card.GetComponent<Button>();
        tunnelObjects.Add(card);
        RandomDespawn rand = card.GetComponent<RandomDespawn>();
        rand.cardType = RandomDespawn.Type.Tunnel;


        if (color == playedCardColor || color == "Rainbow")
        {
            var colors = button.colors;
            var disabledColor = colors.disabledColor;
            var normalColor = colors.normalColor;
            disabledColor.a = 255f;
            disabledColor.r = 255f;
            disabledColor.g = 0f;
            disabledColor.b = 0f;
            colors.disabledColor = disabledColor;
            normalColor.a = 0f;
            colors.normalColor = normalColor;
            button.colors = colors;
        }

        button.interactable = false;

        if (tunnelCardColors.Count == 3)
        {
            Debug.Log("Played Card Color from spawn " + playedCardColor);
            int counter = 0;
            foreach (string tunnelcardcolor in tunnelCardColors.ToList())
            {
                if(tunnelcardcolor == playedCardColor || tunnelcardcolor == "Rainbow")
                {
                    counter++;
                }
            }
            Debug.Log(counter);
            neededTunnelCards.text = counter.ToString();
            neededExtraCards = counter;
            Debug.Log(neededTunnelCards.text);

            beforeTunnelPulling = false;
            EnableCardButtons(playedCardColor);
        }
    }

    public void SpawnExtraCard(GameObject prefab, string color)
    {

        if (extraCardsObjects.Count < neededExtraCards)
        {
            GameObject card = Instantiate(prefab, ExtraCardArea.transform);
            extraCardsObjects.Add(card);
            RandomDespawn rand = card.GetComponent<RandomDespawn>();
            rand.cardType = RandomDespawn.Type.ExtraCard;

            selectedTunnelCards.text = extraCardsObjects.Count.ToString();
            if (extraCardsObjects.Count == neededExtraCards)
            {
                DisableCardButtons();
            }
            CardCounterHandler(color, true);
            HandlePlayerHandServerRpc(color, false);
            EnableCardButtons(color);
        }
        else
        {
            Debug.Log("You can't select more cards!");
        }
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
        bool isTunnel = false;

        if (_type == Type.Tunnel)
        {
            isTunnel = true;
        }

        Debug.Log("Play Action");
        Debug.Log("Type " + _type);

        playedCardColor = "";
        foreach (FixedString32Bytes cardColor in selectedCardColors.ToList())
        {
            Debug.Log("Played Card Color 1" + cardColor);
            if (cardColor.ToString() != "Rainbow")
            {
                playedCardColor = cardColor.ToString();
            }
        }
        Debug.Log("Played Card Color 2" + playedCardColor);
        if (playedCardColor == "")
        {
            playedCardColor = "Rainbow";
        }
        Debug.Log("Played Card Color 3" + playedCardColor);


        if (_type != Type.Tunnel || !beforeTunnelPulling)
        {
            GameManager.Instance.ChooseRoute(placeName, routeColor, isTunnel);
            foreach (GameObject gameObject in GameManager.Instance.routes)
            {
                Route route = gameObject.GetComponent<Route>();
                if (route.routeName == placeName)
                {
                    route.SetIsClaimedServerRpc();
                    Debug.Log("Built");
                }
            }
            foreach (GameObject go in cardObjects)
            {
                Destroy(go);
            }
            cardObjects.Clear();
            selectedCards.text = cardObjects.Count.ToString();
            CardsToDiscardPileServerRpc();
            TurnM.Instance.EndTurn();
            Enable_DisableSelectorArea(false);
        }

        switch (_type)
        {
            case Type.Station:
                GameManager.Instance.ChooseCity(placeName);
                foreach (GameObject gameObject in GameManager.Instance.stations)
                {
                    Station station = gameObject.GetComponent<Station>();
                    if (station.stationName == placeName)
                    {
                        station.SetIsTakenServerRpc();
                        Debug.Log("Built");
                    }
                }
                break;
            //case Type.Route:
            //    Debug.Log("Route");
            //    GameManager.Instance.ChooseRoute(placeName, routeColor);
            //    foreach (GameObject gameObject in GameManager.Instance.routes)
            //    {
            //        Route route = gameObject.GetComponent<Route>();
            //        if (route.routeName == placeName)
            //        {
            //            route.SetIsClaimedServerRpc();
            //            Debug.Log("Built");
            //        }
            //    }
            //    break;
            case Type.Tunnel:
                if (beforeTunnelPulling)
                {
                    TunnelComponents.SetActive(true);

                    for (int i = 0; i < 3; i++)
                    {
                        GameManager.Instance.TunnelDrawServerRpc();
                    }


                    foreach(GameObject gameObject in cardObjects.ToList())
                    {
                        Button button = gameObject.GetComponent<Button>();
                        button.interactable = false;
                    }

                }
                else
                {
                    foreach (GameObject go in tunnelObjects.ToList())
                    {
                        Destroy(go);
                    }
                    foreach (GameObject go in extraCardsObjects.ToList())
                    {
                        Destroy(go);
                    }
                    tunnelObjects.Clear();
                    extraCardsObjects.Clear();
                    selectedTunnelCards.text = "0";
                    TunnelComponents.SetActive(false);
                }
                break;
        }


        
    }

    [ClientRpc]
    public void DrawTunnelCardClientRpc(string color, bool host, ClientRpcParams clientRpcParams = default)
    {
        if (IsOwner && !host) return;
        tunnelCardColors.Add(color);
        SetColorAndPrefab(color, true);
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

    public void CancelAction()
    {
        Enable_DisableSelectorArea(false);
        foreach (GameObject gameObject in cardObjects.ToList())
        {
            RandomDespawn rd = gameObject.GetComponent<RandomDespawn>();
            HandlePlayerHandServerRpc(rd.Color, true);
            CardCounterHandler(rd.Color, false);
            Destroy(gameObject);
        }

        foreach(GameObject go in GameManager.Instance.routes.ToList())
        {
            Route route = go.GetComponent<Route>();
            route.HighlightOff();
        }
        cardObjects.Clear();
        selectedCards.text = cardObjects.Count.ToString();

        if (beforeTunnelPulling)
        {
            TurnM.Instance.Enable_DisableActionChooser(true);
        }
        else
        {
            foreach(GameObject tunnel in tunnelObjects.ToList())
            {
                Destroy(tunnel);
            }
            tunnelObjects.Clear();
            TurnM.Instance.EndTurn();
        }

        TunnelComponents.SetActive(false);
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
        Debug.Log(color);
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
        }

        Debug.Log(buttonname);
        Debug.Log(beforeTunnelPulling);
        Debug.Log(extraCardsObjects.Count);

        if(cardObjects.Count > 0 || (!beforeTunnelPulling && extraCardsObjects.Count > 0))
        {
            foreach (GameObject go in cardObjects)
            {
                RandomDespawn card = go.GetComponent<RandomDespawn>();
                if (card.Color != "Rainbow")
                {
                    onlyRainbow = false;
                }
            }
        }
        else { onlyRainbow = false;}
        

        foreach (GameObject gameObject in Buttons)
        {
            Button buttonComponent = gameObject.GetComponent<Button>();

            if ((cardObjects.Count != maxCard.Value || cardObjects.Count == 0) || 
                (!beforeTunnelPulling && (extraCardsObjects.Count != neededExtraCards || (extraCardsObjects.Count == 0 && neededExtraCards != 0))))
            {
                available = CheckCardAmount(gameObject.name);

                Debug.Log("Color : " + color + " onlyRainbow " + onlyRainbow + " routeColor " + routeColor);

                if (available)
                {
                    if ((color == "Grey" || color == "none" || (onlyRainbow && routeColor == "Grey") || (onlyRainbow && routeColor == "none")) && beforeTunnelPulling)
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


    // Gets a button name, cuts and sends the color back from it
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

        if (cardObjects.Count == maxCard.Value && beforeTunnelPulling)
        {
            PlayButton.interactable = true;
        }
        else if (extraCardsObjects.Count == neededExtraCards && !beforeTunnelPulling)
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
            foreach (Card card in cardsInSelection.ToList())
            {
                if (!foundCard)
                {
                    if (card.Color == color)
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
            foreach (Card card in player.hand.ToList())
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

        FixedString32Bytes[] cardColors = new FixedString32Bytes[cardsInSelection.Count];
        for(int i = 0; i < cardColors.Length; i++)
        {
            cardColors[i] = new FixedString32Bytes(cardsInSelection[i].Color);
        }
        GetCardColorsFromServerClientRpc(cardColors);

    }

    [ClientRpc]
    public void GetCardColorsFromServerClientRpc(FixedString32Bytes[] cardColors, ClientRpcParams clientRpcParams = default)
    {
        selectedCardColors = new List<FixedString32Bytes>(cardColors);
    }

    public void PlayCardBTN(string button)
    {
        SetColorAndPrefab(button);
    }

        // This methode is for the card play buttons, it gets an int from the button, sets the color
        // then calls PlayCardHand methode with the color
    public void SetColorAndPrefab(string color, bool tunnel = false)
    {
        GameObject prefab = null;
        switch (color)
        {
            case "Black":
                prefab = BlackPrefabCanvas;
                break;
            case "Blue":
                prefab = BluePrefabCanvas;
                break;
            case "Orange":
                prefab = OrangePrefabCanvas;
                break;
            case "Green":
                prefab = GreenPrefabCanvas;
                break;
            case "Red":
                prefab = RedPrefabCanvas;
                break;
            case "Pink":
                prefab = PinkPrefabCanvas;
                break;
            case "White":
                prefab = WhitePrefabCanvas;
                break;
            case "Yellow":
                prefab = YellowPrefabCanvas;
                break;
            case "Rainbow":
                prefab = RainbowPrefabCanvas;
                break;
        }
        if (prefab == null) return;

        if (!tunnel && beforeTunnelPulling)
        {
            SpawnCard(prefab, color);
        }
        else if (!beforeTunnelPulling)
        {
            SpawnExtraCard(prefab,color);
        }
        else
        {
            SpawnTunnelCard(prefab, color);
        }
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




    #endregion

}

