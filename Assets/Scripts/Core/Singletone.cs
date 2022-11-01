using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

//public class Singeltone<T> : MonoBehaviour where T : Component
//{
//    private static T _instance;

//    public static T Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                T[] objs = FindObjectsOfType<T>();
//                if (objs.Length > 0)
//                {
//                    T instance = objs[0];
//                    _instance = instance;
//                }
//                else
//                {
//                    GameObject go = new GameObject();
//                    go.name = typeof(T).Name;
//                    _instance = go.AddComponent<T>();
//                    DontDestroyOnLoad(go);
//                }
//            }
//            return _instance;
//        }
//    }
//}
public class Singeltone<T> : NetworkBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] objs = FindObjectsOfType<T>();
                if (objs.Length > 0)
                {
                    T instance = objs[0];
                    _instance = instance;
                }
                else
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).Name;
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }
}