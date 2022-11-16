using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class CreateScene : MonoBehaviour
{
    [SerializeField] TMP_InputField lobbyname;
    [SerializeField] Slider players;

    //Send the data to CreateLobby function then send the user to another scene
    public void CreateButtonclick()
    {
        string lobbyName = lobbyname.GetComponent<TMP_InputField>().text;
        int maxPlayers = Convert.ToInt32(players.value);
        Task<bool> runing = LobbyManager.Instance.CreateLobby(lobbyName, maxPlayers);

        //SceneManager.LoadScene("LobbyScene");
    }

   public void CloseButton()
    {
        SceneManager.LoadScene("Join-Create Game");
    }
}