using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SpawnCode : Singeltone<SpawnCode>
{
    [SerializeField] private GameObject code;
    [SerializeField] private TMP_Text joinCode;
    public void Start()
    {
        if (IsServer)
        {
            //Instantiate(code).GetComponent<NetworkObject>().Spawn();

        }
    }
   
}
