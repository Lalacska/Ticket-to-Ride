using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class CreateScene : Singeltone<CreateScene>
{
    [SerializeField] TMP_InputField lobbyname;
    [SerializeField] Slider players;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateButtonclick()
    {
         string lobbyName = lobbyname.GetComponent<TMP_InputField>().text;
         int maxPlayers = Convert.ToInt32(players.value);
         CreateLobbyOptions options = new CreateLobbyOptions();
         options.IsPrivate = false;

        LobbyManager.Instance.CreateLobby(lobbyName, maxPlayers, options);
    }
}