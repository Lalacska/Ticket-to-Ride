using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public static class UserData 
{
    public static string username;
    public static string userId;
    public static string lobbyID;
    public static bool LoggedIn { get { return username != null; } }
    public static void LoggedOut() { username = null; }
    public static List<NetworkConnection> m_connections = new List<NetworkConnection>();
    public static NetworkDriver networkDriver;
}
