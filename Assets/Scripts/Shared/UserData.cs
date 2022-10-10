using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserData 
{
    public static string username;
    public static bool LoggedIn { get { return username != null; } }
    public static void LoggedOut() { username = null; } 
}
