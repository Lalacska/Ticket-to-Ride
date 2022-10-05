using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;

public class GameStartManager : NetworkManager
{
    [SerializeField]
    public List<Camera> Cameras;

    public void AssignPlayersToCameras(List<Player> players)
    {
        foreach(Player p in players)
        {

        }
    }

}
