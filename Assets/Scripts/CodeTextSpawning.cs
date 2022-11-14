using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CodeTextSpawning : Singeltone<CodeTextSpawning>
{
    public void Start()
    {
        if (IsServer)
        {
            // Server subscribes to the NetworkSceneManager.OnSceneEvent event
            NetworkManager.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
        }
    }
    public void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        // OnSceneEvent is very useful for many things
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    Debug.Log("A");
                    if (sceneEvent.ClientId != NetworkManager.LocalClientId)
                    {
                        GetComponent<TextMeshProUGUI>().text = UserData.lobby.LobbyCode;
                    }
                    break;
                }
        }
    }
}
