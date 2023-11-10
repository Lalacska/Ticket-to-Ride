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


    // This metode spawns a card in to the selected card area
    public void SpawnCard(GameObject prefab, string color)
    {
        bool canSpawn = true;

        //Debug.Log("Max card: " + maxCard);
        //Debug.Log(color);
        //Debug.Log("Cards count: " + cardObjects.Count);

        // Check if the total card count is less than the maximum allowed.
        if (cardObjects.Count < maxCard.Value)
        {
            if (neededRainbowCards != 0)
            {
                // If Rainbow cards are needed, check if the color is not "Rainbow" and if there's still room for more cards.
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
                // Instantiate the card object and add it to the list.
                GameObject card = Instantiate(prefab, SelectorArea.transform);
                cardObjects.Add(card);

                // Set the card type for random despawning.
                RandomDespawn rand = card.GetComponent<RandomDespawn>();
                rand.cardType = RandomDespawn.Type.Card;

                // Handle Rainbow card selection.
                if (color == "Rainbow")
                {
                    selectedRainbowCards++;
                }

                // Update the selected card count.
                selectedCards.text = cardObjects.Count.ToString();

                // If the maximum allowed cards have been reached, disable card buttons.
                if (cardObjects.Count == maxCard.Value)
                {
                    DisableCardButtons();
                }

                // Update card counters and player's hand on the server.
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

    // Handle everything that comes with despawning the card except the objects destruction
    public void DespawnCard(GameObject go)
    {
        string color = "none";
        RandomDespawn rd = go.GetComponent<RandomDespawn>();

        // Update card counters and player's hand on the server.
        CardCounterHandler(rd.Color, false);
        HandlePlayerHandServerRpc(rd.Color, true);

        // Checks if the card is a normal or an extra card, so it can remove it from the right list and refresh the right counter
        if (rd.cardType == RandomDespawn.Type.Card)
        {
            // Remove the object from the list
            cardObjects.Remove(go);

            // Handle Rainbow card deselection.
            if (rd.Color == "Rainbow")
            {
                selectedRainbowCards--;
            }

            selectedCards.text = cardObjects.Count.ToString();
            Debug.Log("Cards count: " + cardObjects.Count);

            // Find the color of the remaining non-Rainbow cards, if any.
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
            // Remove the object from the list and refreshes the counter
            extraCardsObjects.Remove(go);
            selectedTunnelCards.text = extraCardsObjects.Count.ToString();

            // Set the color to the played card's color.
            color = playedCardColor;

        }


        EnableCardButtons(color);
    }

    // This one spawns the tunnel card ands highlight them if they match with the played color
    public void SpawnTunnelCard(GameObject prefab, string color)
    {
        Debug.Log(color);

        // Create a new tunnel card game object and adds it to the list.
        GameObject card = Instantiate(prefab, TunnelArea.transform);
        tunnelObjects.Add(card);

        // Gets the button component
        Button button = card.GetComponent<Button>();

        // Get the random despawn object and set the type
        RandomDespawn rand = card.GetComponent<RandomDespawn>();
        rand.cardType = RandomDespawn.Type.Tunnel;

        // Modify the colors to visually indicate that the card has matching color or is Rainbow
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

        // Disables the button
        button.interactable = false;

        // Check if the required number of tunnel cards has been played.
        if (tunnelCardColors.Count == 3)
        {
            Debug.Log("Played Card Color from spawn " + playedCardColor);
            int counter = 0;

            // Count the played cards matching the played card color or Rainbow.
            foreach (string tunnelcardcolor in tunnelCardColors.ToList())
            {
                if(tunnelcardcolor == playedCardColor || tunnelcardcolor == "Rainbow")
                {
                    counter++;
                }
            }

            // Changes the texts
            neededTunnelCards.text = counter.ToString();
            neededExtraCards = counter;
            Debug.Log(neededTunnelCards.text);

            beforeTunnelPulling = false;
            EnableCardButtons(playedCardColor);
        }
    }

    // This metode spawns the extra card if the player need to use more card after claiming a tunnel
    public void SpawnExtraCard(GameObject prefab, string color)
    {

        // Check if the maximum number of extra cards has been reached.
        if (extraCardsObjects.Count < neededExtraCards)
        {
            // Create a new extra card game object.
            GameObject card = Instantiate(prefab, ExtraCardArea.transform);
            extraCardsObjects.Add(card);
            RandomDespawn rand = card.GetComponent<RandomDespawn>();
            rand.cardType = RandomDespawn.Type.ExtraCard;

            // Update the text displaying the selected tunnel cards count.
            selectedTunnelCards.text = extraCardsObjects.Count.ToString();

            if (extraCardsObjects.Count == neededExtraCards)
            {
                // Disable card buttons when the maximum number of extra cards has been reached.
                DisableCardButtons();
            }

            // Handle the cards counters and the cards on the server
            CardCounterHandler(color, true);
            HandlePlayerHandServerRpc(color, false);
            EnableCardButtons(color);
        }
        else
        {
            Debug.Log("You can't select more cards!");
        }
    }


    // Cheks how many station the sender has left, and sets the amount while enabling the card buttons
    [ServerRpc(RequireOwnership = false)]
    public void CheckStationsNumberServerRpc(ServerRpcParams serverRpcParams = default)
    {
        // Get the sender client ID
        ulong clientId = serverRpcParams.Receive.SenderClientId;

        // Set the target client
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };

        // Find the player corresponding to the client ID.
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientId)
            {
                player = stat;
            }
        }

        // Check if the player has remaining stations.
        if (player != null && player.stations.Value > 0)
        {
            // Calculate the maximum number of cards the player can select.
            string amount = (4 - player.stations.Value).ToString();
            maxCard.Value = (4 - player.stations.Value);

            // Send the maximum card count and enable card buttons.
            SetTextClientRpc(amount);
            EnableCardButtonsClientRpc(clientRpcParams: clientRpcParams);

        }
        else
        {
            // The player can't have more stations, so set isValid to false.
            isValid.Value = false;
            Debug.Log("The player can't have more station");
        }
    }

    // This metode is triggered when the player clicks on the play button after choosing cards to play
    public void PlayAction()
    {
        bool isTunnel = false;

        // Check if the action is for a tunnel.
        if (_type == Type.Tunnel)
        {
            isTunnel = true;
        }

        //Debug.Log("Play Action");
        //Debug.Log("Type " + _type);

        playedCardColor = "";

        // Determine the color of the played cards (excluding Rainbow).
        foreach (FixedString32Bytes cardColor in selectedCardColors.ToList())
        {
            if (cardColor.ToString() != "Rainbow")
            {
                playedCardColor = cardColor.ToString();
            }
        }

        // If no specific color was selected, use Rainbow as the color.
        if (playedCardColor == "")
        {
            playedCardColor = "Rainbow";
        }

        // Check if the action type is not a tunnel or it's not before tunnel pulling.
        if (_type != Type.Tunnel || !beforeTunnelPulling)
        {
            if (_type != Type.Station)
            {
                // Choose and claim the route if its not a station
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
            }

            // Destroy card objects, clear the list, update selected card count, and send cards to the discard pile.
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

        // Handle different action by types.
        switch (_type)
        {
            case Type.Station:
                // Choose a city and set it as taken.
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
            case Type.Tunnel:
                // Before we pulled the cards
                if (beforeTunnelPulling)
                {
                    // Enable tunnel components and draw tunnel cards.
                    TunnelComponents.SetActive(true);

                    for (int i = 0; i < 3; i++)
                    {
                        GameManager.Instance.TunnelDrawServerRpc();
                    }

                    // Disable the buttons for tunnel cards.
                    foreach (GameObject gameObject in cardObjects.ToList())
                    {
                        Button button = gameObject.GetComponent<Button>();
                        button.interactable = false;
                    }

                }
                // After we choosed extra cards
                else
                {
                    // Clear tunnel-related objects and disable tunnel components.
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

    // Checks ownership and sends data for the next metode
    
    [ClientRpc]
    public void DrawTunnelCardClientRpc(string color, bool host, ClientRpcParams clientRpcParams = default)
    {
        // If this client is the owner and not the host, return without taking any action.
        if (IsOwner && !host) return;

        // Add the drawn tunnel card color to the tunnelCardColors list, then set color and prefab for the tunnel
        tunnelCardColors.Add(color);
        SetColorAndPrefab(color, true);
    }

    // Moves the cards from the selection into the discardpile
    [ServerRpc(RequireOwnership = false)]
    public void CardsToDiscardPileServerRpc(ServerRpcParams serverRpcParams = default)
    {
        foreach (Card card in cardsInSelection.ToList())
        {
            cardsInSelection.Remove(card);
            GameManager.Instance.discardPile.Add(card);
        }
    }

    // This metode is triggered when the player clicks on the Cancel button in the card selector
    public void CancelAction()
    {
        // Disable selector area and buttons
        Enable_DisableSelectorArea(false);
        DisableCardButtons();

        // This goes trough every card object, gives cards back to the player, handle the counter and destroys the objects from the selector are
        foreach (GameObject gameObject in cardObjects.ToList())
        {
            RandomDespawn rd = gameObject.GetComponent<RandomDespawn>();
            HandlePlayerHandServerRpc(rd.Color, true);
            CardCounterHandler(rd.Color, false);
            Destroy(gameObject);
        }

        // Turn off route highlights.
        foreach (GameObject go in GameManager.Instance.routes.ToList())
        {
            Route route = go.GetComponent<Route>();
            route.HighlightOff();
        }

        // Clear the cardObjects list and update the selected card count.
        cardObjects.Clear();
        selectedCards.text = cardObjects.Count.ToString();

        // Handle post-action based on whether it's before or after tunnel pulling.
        if (beforeTunnelPulling)
        {
            TurnM.Instance.Enable_DisableActionChooser(true);
        }
        else
        {
            // Destroy tunnel card objects, clear the tunnelObjects list, and end the turn if it's after tunnel pulling.
            foreach (GameObject tunnel in tunnelObjects.ToList())
            {
                Destroy(tunnel);
            }
            tunnelObjects.Clear();
            TurnM.Instance.EndTurn();
        }

        // Deactivate tunnel components.
        TunnelComponents.SetActive(false);
    }


    // This is just a client rpc metode calling the real metode, so we can call it from the server too
    [ClientRpc]
    public void EnableCardButtonsClientRpc(string color = "none", ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Enable ClientRpc");
        EnableCardButtons(color);
    }

    // This metode enables the right card buttons for the player
    public void EnableCardButtons(string color = "none")
    {
        // If the color is not specified, use the routeColor.
        if (routeColor != string.Empty && color == "none")
        {
            color = routeColor;
        }

        // Initialize variables for button availability.
        bool available = false;
        bool onlyRainbow = true;
        string buttonname = "";

        // Map the color to the corresponding button name.
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

        // Debugging output.
        Debug.Log(buttonname);
        Debug.Log(beforeTunnelPulling);
        Debug.Log(extraCardsObjects.Count);

        // Check if there are cards in the cardObjects list, and if there are colored or only Rainbow
        if (cardObjects.Count > 0 || (!beforeTunnelPulling && extraCardsObjects.Count > 0))
        {
            foreach (GameObject go in cardObjects)
            {
                // If any card from the card list has a color, sets onlyRainbow false
                RandomDespawn card = go.GetComponent<RandomDespawn>();
                if (card.Color != "Rainbow")
                {
                    onlyRainbow = false;
                }
            }
        }
        else { onlyRainbow = false;}

        // Iterate through available buttons.
        foreach (GameObject gameObject in Buttons)
        {
            Button buttonComponent = gameObject.GetComponent<Button>();

            // Checks a bunch of things fo the current button
            // Checks if the player can play more cards, checks it also the buttons mean to spawn extra cards, just a different list
            if ((cardObjects.Count != maxCard.Value || cardObjects.Count == 0) || 
                (!beforeTunnelPulling && (extraCardsObjects.Count != neededExtraCards || (extraCardsObjects.Count == 0 && neededExtraCards != 0))))
            {
                // Check if the player has cards left from the current color
                available = CheckCardAmount(gameObject.name);

                Debug.Log("Color : " + color + " onlyRainbow " + onlyRainbow + " routeColor " + routeColor);

                // If the button is available there is to type of interaction
                if (available)
                {
                    // This checks the color, if the color is grey or none, enables every available button
                    if ((color == "Grey" || color == "none" || (onlyRainbow && routeColor == "Grey") || (onlyRainbow && routeColor == "none")) && beforeTunnelPulling)
                    {
                        Debug.Log(color + " " + onlyRainbow);
                        Debug.Log("all");
                        buttonComponent.interactable = true;
                    }
                    else
                    {
                        // If we have a specific color, it checks the button's color and only enable it,
                        // if it has the same color as the route or the already selected card or if its rainbow
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

        // Enable or disable the play button based on how much card the player has selected
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

    // Iterate through the selected cards and checks the colors, if there is one which is not rainbow it sends it back
    public string CheckSelectedCardColor()
    {
        string color = string.Empty;
        foreach (GameObject card in cardObjects.ToList())
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

    // Checks if the player choosed enough card, and enables play button
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

    // This metode handles cards on the server

    [ServerRpc(RequireOwnership = false)]
    public void HandlePlayerHandServerRpc(string color, bool dispawn, ServerRpcParams serverRpcParams = default)
    {
        bool foundCard = false;
        player = null;
        ulong clientID = serverRpcParams.Receive.SenderClientId;

        // Iterate through the PlayerManager's stats to find the player with the given client ID.
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            if (stat.clientId == clientID)
            {
                player = stat;
            }
        }

        // Check if cards should be despawned or spawned.
        if (dispawn)
        {
            // Iterate through the cards in the selection list until it finds a card with the same color
            // then sets the bool true, and removes the card from selection and adds back to the players hand
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
            // Iterate through the cards in the players hand list until it finds a card with the same color
            // then sets the bool true, and removes the card from the players hand and adds it to the selection
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

        // Create an array to store card colors for synchronization.
        FixedString32Bytes[] cardColors = new FixedString32Bytes[cardsInSelection.Count];

        // Populate the array with the colors of cards in the selection list.
        for (int i = 0; i < cardColors.Length; i++)
        {
            cardColors[i] = new FixedString32Bytes(cardsInSelection[i].Color);
        }

        // Synchronize the card colors with the clients.
        GetCardColorsFromServerClientRpc(cardColors);

    }

    // Synchronize the selected card colors with the server.
    // This method is called on the client to update the selectedCardColors list.
    [ClientRpc]
    public void GetCardColorsFromServerClientRpc(FixedString32Bytes[] cardColors, ClientRpcParams clientRpcParams = default)
    {
        // Create a new list and populate it with the received card colors.
        selectedCardColors = new List<FixedString32Bytes>(cardColors);
    }

    // Card buttons hit this metode with a string, which calls another metode for setting prefabs
    public void PlayCardBTN(string button)
    {
        SetColorAndPrefab(button);
    }

    // This metode selects the right prefab and color, then calls the right spawning metode based on the conditions
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

        // Calls the right spawning metode
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

}

