using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerManager : Singeltone<ServerPlayerManager>
{
    public List<PlayerInGame> players = new List<PlayerInGame>();
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;

    }

    private void NetworkManager_OnClientConnectedCallback(ulong obj)
    {
        CreatePlayerInfo();
        Debug.Log("mmmmm");
    }

    private void CreatePlayerInfo()
    {
        PlayerInGame player = new PlayerInGame();
        player.name = UserData.username;
        player.ID = UserData.userId;
        player.clientId = NetworkManager.LocalClientId;
        players.Add(player);
    }
}
