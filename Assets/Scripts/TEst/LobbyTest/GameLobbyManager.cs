using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

public class GameLobbyManager : Singeltone<GameLobbyManager>
{
    private Lobby lobby;
    private string joincode;
    private const string JoinCodeKey = "j";
    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
    //public async Task<bool> CreateLobby(string username, string lobbyName, int maxplayer)
    //{
    //    LobbyPlayerData playerData = new LobbyPlayerData();
    //    playerData.Initialize(AuthenticationService.Instance.PlayerId, username, gamertag: "HostPlayer");

    //    try
    //    {
    //        Debug.Log("Relay Server starting");

    //        // Create a relay allocation and generate a join code to share with the lobby
    //        Allocation a = await RelayService.Instance.CreateAllocationAsync(maxplayer);

    //        RelayHostData relayHostData = new RelayHostData
    //        {
    //            Key = a.Key,
    //            Port = (ushort)a.RelayServer.Port,
    //            AllocationID = a.AllocationId,
    //            AllocationIDBytes = a.AllocationIdBytes,
    //            IPv4Address = a.RelayServer.IpV4,
    //            ConnectionData = a.ConnectionData,
    //        };

    //        relayHostData.JoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

    //        // Create a lobby, adding the relay join code to the lobby
    //        CreateLobbyOptions options = new CreateLobbyOptions()
    //        {
    //            Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, relayHostData.JoinCode) },
    //            {"LobbyStatus", new DataObject(visibility: DataObject.VisibilityOptions.Public, value: "running") } },
    //        };

    //        // Set the game room to use the relay allocation
    //        Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);



    //        // Heartbeat the lobby every 15 seconds.
    //        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

    //        Debug.Log("Relay Server got created");
    //        Debug.Log("The lobby was created");
    //        Debug.Log(lobby.LobbyCode);
    //        Debug.Log(lobby.Players);
    //        foreach (Player player in lobby.Players)
    //        {
    //            Debug.Log(player);
    //        }

    //        LobbyScene.lobby = lobby;
    //        LobbyScene.code = relayHostData.JoinCode;

    //        UserData.lobbyID = lobby.Id;
    //        UserData.lobby = lobby;

    //        LobbyScene.Instance.DisplayCode();
    //        NetworkManager.Singleton.StartHost();

    //        return true;
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogFormat("Failed creating a lobby");
    //        Debug.Log(e);
    //        return false;
    //    }
    //}

    // Hearbeat to the lobby, so it won't die
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
