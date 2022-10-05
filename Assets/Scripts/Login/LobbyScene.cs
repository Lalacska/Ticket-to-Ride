using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyScene : Singeltone<LobbyScene>
{
    [SerializeField] private TMP_Text joinCode;
    private Lobby lobby;

    public void Getlobby(Lobby lobby_)
    {
        this.lobby = lobby_;
    }

    public void StartButton()
    {
        Debug.Log(lobby.Players);
    }

    // Start is called before the first frame update
    void Start()
    {
        joinCode.text = "Hallo!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
