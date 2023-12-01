using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using Unity.Services.Lobbies.Models;
using UnityEngine;

// A static class to save some local variable \\
public static class UserData 
{
    public static string username;
    public static string userId;
    public static string lobbyID;
    public static Lobby lobby;
    public static ulong clientId;
}
