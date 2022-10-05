using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using System;
using System.Data;
using System.Data.SqlTypes;
using UnityEngine.Networking;
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField] TMP_InputField userName;

    

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/*
    // Method for inserting into database. \\
    IEnumerator InsertIntoDatabase()
    {
        
    }
*/
}
