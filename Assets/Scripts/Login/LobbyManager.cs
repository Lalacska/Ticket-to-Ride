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

public class LobbyManager : Singeltone<LobbyManager>
{
    private static Lobby lobby;
    private static CancellationTokenSource _updateLobbySource, _heartbeatSource;
    private const string JoinCodeKey = "j";
    private const int LobbyRefreshRate = 2; // Rate limits at 2
    private const int HeartbeatInterval = 15;

    public UnityTransport Transport => NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();
    public static event Action<Lobby> CurrentLobbyRefreshed;

    private static Dictionary<ulong, PlayerInGame> clientData;

    private void Start()
    {
        //NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;
    }
    private void OnDestroy()
    {
        //NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            clientData.Remove(clientId);
        }
        throw new NotImplementedException();
    }

    // Create Lobby
    public async Task<bool> CreateLobby(string lobbyName, int maxPlayers)
    {
        try
        {
            Debug.Log("Relay Server starting");

            // Create a relay allocation and generate a join code to share with the lobby
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

            // Create a lobby, adding the relay join code to the lobby
            CreateLobbyOptions options = new CreateLobbyOptions()
            {
                Data = new Dictionary<string, DataObject> {
                    { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, relayHostData.JoinCode) } },
            
            };
            lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            // Set the game room to use the relay allocation
            Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);


            // Heartbeat the lobby every 15 seconds.
            Heartbeat();

            Debug.Log("Relay Server got created");
            Debug.Log("The lobby was created");
            Debug.Log(lobby.LobbyCode);
            Debug.Log(lobby.Players);
            foreach(Player player in lobby.Players)
            {
                Debug.Log(player);
            }
            
            UserData.lobbyID = lobby.Id;
            UserData.lobby = lobby;

            clientData = new Dictionary<ulong, PlayerInGame>();
            clientData[NetworkManager.Singleton.LocalClientId] = new PlayerInGame(UserData.username);


            PeriodicallyRefreshLobby();

            Debug.Log(relayHostData.JoinCode);



            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogFormat("Failed creating a lobby");
            Debug.Log(e);
            return false;
        }
    }

    // Uses the lobbyCode to join a specific lobby
    public async void JoinLobby(string lobbyCode)
    {
        try
        {
            // Joining the lobby with the code
            Debug.Log("Joining to lobby");
            lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            Debug.Log("Joined");

            // Setting relay Allocation
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
            Client();

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    // Finds a public lobby and join
    public async void QuickJoin()
    {
        try
        {
            Debug.Log("Joining to lobby");
            // Quick-join a random lobby 
            lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Debug.Log(lobby);
            Debug.Log("Joined");

            Debug.Log("Joining to Relay");
            // If we found a lobby, grab the relay allocation details
            JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            SetTransformAsClient(a);
            Debug.Log("Joined to relay");
            Client();

        }
        catch (Exception)
        {
            Debug.Log("No lobbies available via quick join");
        }
    }

    public void Client()
    {
        UserData.lobbyID = lobby.Id;
        UserData.lobby = lobby;
        PeriodicallyRefreshLobby();
        var playload = JsonUtility.ToJson(new ConnectionPlayload()
        {
            playerName = UserData.username
        });

        byte[] playeloadBytes = Encoding.ASCII.GetBytes(playload);

        NetworkManager.Singleton.NetworkConfig.ConnectionData = playeloadBytes;
        NetworkManager.Singleton.StartClient();
    }

    // Client transport
    private void SetTransformAsClient(JoinAllocation a)
    {
        Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
    }

    public async void LeaveLobby()
    {
        _heartbeatSource?.Cancel();
        _updateLobbySource?.Cancel();
        try
        {
            
            if (NetworkManager.Singleton.IsHost)
            {
                try
                {
                    NetworkManager.Singleton.SceneManager.LoadScene("Join-Create Game", LoadSceneMode.Single);
                    await Task.Delay(1000);
                    NetworkManager.Singleton.Shutdown();
                    await LobbyService.Instance.DeleteLobbyAsync(UserData.lobby.Id);
                    lobby = null;
                }
                catch (LobbyServiceException e)
                {
                    Debug.Log(e);
                }
            }
            else
            {
                NetworkManager.Singleton.Shutdown();
                await LobbyService.Instance.RemovePlayerAsync(UserData.lobbyID, UserData.userId);
                lobby = null;
            }
            SceneManager.LoadScene("Join-Create Game");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    // Hearbeat to the lobby, so it won't die
    private static async void Heartbeat()
    {
        _heartbeatSource = new CancellationTokenSource();
        while (!_heartbeatSource.IsCancellationRequested && lobby != null)
        {
            await Lobbies.Instance.SendHeartbeatPingAsync(lobby.Id);
            await Task.Delay(HeartbeatInterval * 1000);
        }
    }

    private static async void PeriodicallyRefreshLobby()
    {
        _updateLobbySource = new CancellationTokenSource();
        await Task.Delay(LobbyRefreshRate * 1000);
        while (!_updateLobbySource.IsCancellationRequested && lobby != null)
        {
            lobby = await Lobbies.Instance.GetLobbyAsync(lobby.Id);
            CurrentLobbyRefreshed?.Invoke(lobby);
            await Task.Delay(LobbyRefreshRate * 1000);
        } 
    }

    public static PlayerInGame? GetPlayerData(ulong clientId)
    {
        if(clientData.TryGetValue(clientId, out PlayerInGame playerInGame))
        {
            return playerInGame;
        }
        return null;
    }

}
