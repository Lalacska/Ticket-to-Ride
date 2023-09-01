using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Card : Singleton<Card>
{
    [SerializeField] private NetworkObject go;

    [SerializeField] private bool m_hasBeenPlayed;
    [SerializeField] private int m_handIndex;
    [SerializeField] private string m_Color;
    [SerializeField] private int m_ownerID;
    [SerializeField] private int m_CardID;

    public bool hasBeenPlayed { get { return m_hasBeenPlayed; } set { m_hasBeenPlayed = value; } }
    public int handIndex { get { return m_handIndex; } set { m_handIndex = value; } }
    public string Color { get { return m_Color; } set { m_Color = value; } }
    public int ownerID { get { return m_ownerID; } set { m_ownerID = value; } }
    public int CardID { get { return m_CardID; } set { m_CardID = value; } }

    public NetworkObject ReleaseCard()
    {
        GameObject prefab = GameManager.Instance.GetPrefabByColor(Color,false);
        NetworkObjectPool.Instance.ReturnNetworkObject(go, prefab);
        Debug.Log("Returned");
        return go;
    }




}

