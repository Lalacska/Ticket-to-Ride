using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public static class UserData 
{
    public static string username;
    public static string userId;
    public static string lobbyID;
    public static bool LoggedIn { get { return username != null; } }
    public static void LoggedOut() { username = null; }

    public static Lobby lobby;
}
