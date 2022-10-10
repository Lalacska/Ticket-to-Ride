using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyScene : Singeltone<LobbyScene>
{
    [SerializeField] private TMP_Text joinCode;
    public static Lobby lobby;
    public static string code;


    public void StartButton()
    {
        Debug.Log(lobby.Players);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player);
        }
    }
    public void DisplayCode()
    {
        joinCode.text = code;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
