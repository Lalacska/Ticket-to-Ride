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
using System.Threading;
using UnityEditor.SceneManagement;
using TMPro;
using System.Text;

public class LobbyManager : Singleton<LobbyManager>
{
    private static Lobby lobby;
    private static CancellationTokenSource _updateLobbySource, _heartbeatSource;
    private const string JoinCodeKey = "j";
    private const int LobbyRefreshRate = 2; // Rate limits at 2
    private const int HeartbeatInterval = 15;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
    public static event Action<Lobby> CurrentLobbyRefreshed;

    // Create a Lobby \\
    public async void CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            Debug.Log("Relay Server starting");

            // Create a relay allocation and generate a join code to share with the lobby \\
            Allocation a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);

            // Set the relay host data \\
            RelayHostData relayHostData = new RelayHostData
            {
                Key = a.Key,
                Port = (ushort)a.RelayServer.Port,
                AllocationID = a.AllocationId,
                AllocationIDBytes = a.AllocationIdBytes,
                IPv4Address = a.RelayServer.IpV4,
                ConnectionData = a.ConnectionData,
            };

            // Get join code to the Relay \\
            relayHostData.JoinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

            // Create a lobby, adding the relay join code to the lobby \\
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject> {
                    { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, relayHostData.JoinCode) } },
            
            };
            lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            // Set the game room to use the relay allocation \\
            Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            // Heartbeat the lobby every 15 seconds. \\
            Heartbeat();

            Debug.Log("Relay Server got created");
            Debug.Log("The lobby was created");
            
            // Set some local variable \\
            UserData.lobbyID = lobby.Id;
            UserData.lobby = lobby;

            // Refreshes the lobby periodically \\
            PeriodicallyRefreshLobby();

            // Start host and change scene to LobbyScene \\
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        }
        catch (Exception e)
        {
            // If there is something wrong it writes out the problem \\
            Debug.LogFormat("Failed creating a lobby");
            Debug.Log(e);
        }
    }

    // Uses the lobbyCode to join a specific lobby \\
    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            // Joining the lobby with the code \\
            Debug.Log("Joining to lobby");
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined");

            // Getting the relay data \\
            Debug.Log("Joining to Relay");
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            // Set the relay data \\
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

            // Connects client to the relay and starts client \\
            Client(a);

        }
        catch (LobbyServiceException e)
        {
            // If there is something wrong it writes out the problem \\
            Debug.Log(e);
        }
    }

    // Finds a public lobby and join \\
    public async void QuickJoin()
    {
        try
        {
            Debug.Log("Joining to lobby");

            // Quick-join to a random lobby 
            lobby = await LobbyService.Instance.QuickJoinLobbyAsync();

            Debug.Log(lobby);
            Debug.Log("Joined");
            Debug.Log("Joining to Relay");

            // If we found a lobby, grab the relay allocation details
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            // Connects client to the relay and starts client \\
            Client(a);

        }
        catch (Exception)
        {
            Debug.Log("No lobbies available via quick join");
        }
    }

    // Connects clients to the relay, set some local variable, and start client \\
    public void Client(JoinAllocation allocation)
    {
        // Start the method to which connects clients to the relay \\
        SetTransformAsClient(allocation);
        Debug.Log("Joined to relay");

        UserData.lobbyID = lobby.Id;
        UserData.lobby = lobby;

        // Start client \\
        NetworkManager.Singleton.StartClient();
    }

    // Client transport
    private void SetTransformAsClient(JoinAllocation a)
    {
        Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
    }

    // Handles when the players leaving the lobby \\
    public async void LeaveLobby()
    {
        // Send Cancellation request to the tokens \\
        _heartbeatSource?.Cancel();
        _updateLobbySource?.Cancel();
        try
        {
            // Runs when the client is the Host \\
            if (NetworkManager.Singleton.IsHost)
            {
                try
                {
                    // Switch everyone back to the Join-Create Game Scene \\
                    NetworkManager.Singleton.SceneManager.LoadScene("Join-Create Game", LoadSceneMode.Single);
                    await Task.Delay(1000);
                    // Shut down the host and delete the lobby, set the lobby variable to null \\
                    NetworkManager.Singleton.Shutdown();
                    await LobbyService.Instance.DeleteLobbyAsync(UserData.lobby.Id);
                    lobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
            // Runs when we have a simple client \\
            else
            {
                // Shut down the connection and remove the player from the lobby \\
                NetworkManager.Singleton.Shutdown();
                await LobbyService.Instance.RemovePlayerAsync(UserData.lobbyID, UserData.userId);
                lobby = null;
            }
            // Load tje Join-Create Game scene \\
            SceneManager.LoadScene("Join-Create Game");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    // Heartbeat to the lobby, so it won't die \\
    private static async void Heartbeat()
    {
        // Makes a new token and until cancellation is requested it pings the lobby every now and then to kep it allive \\
        _heartbeatSource = new CancellationTokenSource();
        while (!_heartbeatSource.IsCancellationRequested && lobby != null)
        {
            await Lobbies.Instance.SendHeartbeatPingAsync(lobby.Id);
            await Task.Delay(HeartbeatInterval * 1000);
        }
    }

    // Refreshes the lobby so it stays accurate \\
    private static async void PeriodicallyRefreshLobby()
    {
        // Makes a new token and until cancellation is requested it refreshes the lobby every now and then \\
        _updateLobbySource = new CancellationTokenSource();
        await Task.Delay(LobbyRefreshRate * 1000);
        while (!_updateLobbySource.IsCancellationRequested && lobby != null)
        {
            lobby = await Lobbies.Instance.GetLobbyAsync(lobby.Id);
            CurrentLobbyRefreshed?.Invoke(lobby);
            await Task.Delay(LobbyRefreshRate * 1000);
        } 
    }
}
