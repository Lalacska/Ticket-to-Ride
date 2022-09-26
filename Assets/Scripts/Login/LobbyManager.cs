using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyManager : Singeltone<LobbyManager>
{
    private Lobby lobby;
    public async void CreateLobby(string lobbyName, int maxPlayer, CreateLobbyOptions options)
    {
        lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, options);

        // Heartbeat the lobby every 15 seconds.
        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

        Debug.Log("The lobby was created");
        Debug.Log(lobby.LobbyCode);
        Debug.Log(lobby.Players);


    }

    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    public async void QuickJoin()
    {
        try
        {
            // Quick-join a random lobby with a maximum capacity of 10 or more players.
            QuickJoinLobbyOptions options = new QuickJoinLobbyOptions();

            options.Filter = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.MaxPlayers,
                    op: QueryFilter.OpOptions.GE,
                    value: "10")
            };

            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync(options);

            // ...
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);

        while (true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }

    }
}
