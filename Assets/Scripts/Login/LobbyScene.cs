using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScene : Singeltone<LobbyScene>
{
    public GameObject codeToSpawn;
    [SerializeField] private TMP_Text joinCode;
    public static Lobby lobby;

    private void Start()
    {
        joinCode.text = UserData.lobby.LobbyCode;
    }
    public void StartButton()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameBoard", LoadSceneMode.Single);
        //Debug.Log(lobby.Players);
        //foreach (Player player in lobby.Players)
        //{
        //    Debug.Log(player);
        //}
    }

    public void Button()
    {
        Debug.Log("a");
        NetworkObject player = NetworkManager.LocalClient.PlayerObject;
        Button button = player.GetComponentInChildren<Button>();
        Debug.Log(player);
        Debug.Log(button);
        //Button button = player.GetComponent<Button>();
        //Debug.Log(button);
        //button.colors.normalColor = new Color(113,255,69);
        ColorBlock c = button.colors;
        c.normalColor = new Color(113, 255, 69);
        button.colors = c;
    }



}
