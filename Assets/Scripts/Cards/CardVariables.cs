using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class CardVariables : Singleton<CardVariables>
{
    public bool hasBeenPlayed;

    public int handIndex;

    public string Color;

    public int ownerID;

    public int CardID;
}

