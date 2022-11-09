using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameStartManager : Singeltone<GameStartManager>
{
    [SerializeField]
    public List<Camera> Cameras;

    public void AssignPlayersToCameras(List<Player> players)
    {
        foreach (Player p in players)
        {

        }
    }
    private void Start()
    {
        
    }
}
