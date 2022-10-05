using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;
using UnityEngine.SceneManagement;

public class LobbyManager : Singeltone<LobbyManager>
{
    private Lobby lobby;
    private string joincode;
    private const string JoinCodeKey = "j";
    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();


    public async Task<bool> CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            Debug.Log("Relay Server starting");

            //Create a relay allocation and generate a join code to share with the lobby
            Allocation a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            RelayHostData relayHostData = new RelayHostData
            {
                Key = a.Key,
                Port = (ushort)a.RelayServer.Port,
                AllocationID = a.AllocationId,
                AllocationIDBytes = a.AllocationIdBytes,
                IPv4Address = a.RelayServer.IpV4,
                ConnectionData = a.ConnectionData,
            };

            relayHostData.JoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            //Create a lobby, adding the relay join code to the lobby
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, relayHostData.JoinCode) } }
            };
            lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            //Set the game room to use the relay allocation
            Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
              
            // Heartbeat the lobby every 15 seconds.
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            Debug.Log("Relay Server got created");
            Debug.Log("The lobby was created");
            Debug.Log(lobby.LobbyCode);
            Debug.Log(lobby.Players);
            LobbyScene.Instance.Getlobby(lobby);
            NetworkManager.Singleton.StartHost();

            return true;
        }
        catch (Exception e)
        {
            Debug.LogFormat("Failed creating a lobby");
            Debug.Log(e);

            return false;
        }


    }

    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            Debug.Log("Joining to lobby");
            var lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined");

            Debug.Log("Joining to Relay");
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            RelayJoinData relayJoinData = new RelayJoinData
            {
                Key = a.Key,
                Port = (ushort)a.RelayServer.Port,
                AllocationID = a.AllocationId,
                AllocationIDBytes = a.AllocationIdBytes,
                IPv4Address = a.RelayServer.IpV4,
                ConnectionData = a.ConnectionData,
                HostConnectionData = a.HostConnectionData,
                JoinCode = lobby.Data[JoinCodeKey].Value,
            };

            SetTransformAsClient(a);

            Debug.Log("Joined to relay");
            Debug.Log(lobby.Players);
            SceneManager.LoadScene("LobbyScene");
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
            Debug.Log("Joining to lobby");
            // Quick-join a random lobby 
            var lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log(lobby);
            Debug.Log("Joined");

            Debug.Log("Joining to Relay");
            // If we found a lobby, grab the relay allocation details
            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);


           SetTransformAsClient(a);
            Debug.Log("Joined to relay");
            Debug.Log(lobby.Players);
            SceneManager.LoadScene("LobbyScene");

        }
        catch (Exception)
        {
            Debug.Log("No lobbies available via quick join");
        }
    }

   private void SetTransformAsClient(JoinAllocation a)
    {
        Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
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
