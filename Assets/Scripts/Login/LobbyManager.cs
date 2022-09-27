using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class LobbyManager : Singeltone<LobbyManager>
{
    private Lobby lobby;
    private UnityTransport transport;
    private string joincode;
    private const string JoinCodeKey = "j";

    public void StarTheRoom()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void JoinToRelay(string code)
    {
           NetworkManager.Singleton.StartClient();
    }



    public async void CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            //Create a relay allocation and generate a join code to share with the lobby
            Allocation a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            joincode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            //Create a lobby, adding the relay join code to the lobby
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joincode) } }
            };
            lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            // Heartbeat the lobby every 15 seconds.
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            Debug.Log("a");

            //Set the game room to use the relay allocation
            transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            Debug.Log("The lobby was created");
            Debug.Log(lobby.LobbyCode);
            Debug.Log(lobby.Players);
        }
        catch (Exception e)
        {
            Debug.LogFormat("Failed creating a lobby");
            Debug.Log(e);
        }


    }

    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined");

            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);
            
            SetTransformAsClient(a);

            NetworkManager.Singleton.StartClient();
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
            // Quick-join a random lobby 
            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            // If we found a lobby, grab the relay allocation details
            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);


            SetTransformAsClient(a);

            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.Log("No lobbies available via quick join");
        }
    }

    private void SetTransformAsClient(JoinAllocation a)
    {
        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
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
