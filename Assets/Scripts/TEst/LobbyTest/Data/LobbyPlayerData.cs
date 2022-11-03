using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyPlayerData
{
    private string _id;
    private string _name;
    private string _gamertag;
    private bool _isReady;

    public void Initialize(string id, string name, string gamertag)
    {
        _id = id;
        _name = name;
        _gamertag = gamertag;
    }

    public void Initialize(Dictionary<string, PlayerDataObject> playerData)
    {
        UpdateState(playerData);
    }

    public void UpdateState(Dictionary<string, PlayerDataObject> playerData)
    {
        if (playerData.ContainsKey("Id"))
        {
            _id = playerData["Id"].Value;
        }
        if (playerData.ContainsKey("Name"))
        {
            _name = playerData["Name"].Value;
        }
        if (playerData.ContainsKey("Gamertag"))
        {
            _gamertag = playerData["Gamertag"].Value;
        }
        if (playerData.ContainsKey("IsReady"))
        {
            _isReady = playerData["IsReady"].Value == "True";
        }
    }

    public Dictionary<string, string> Serialize()
    {
        return new Dictionary<string, string>()
        {
            {"Id", _id },
            {"Name", _name },
            {"Gamertag", _gamertag },
            {"IsReady", _isReady.ToString()}, //True or False
        };
    }
}
