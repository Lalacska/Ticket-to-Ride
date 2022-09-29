using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private TMP_Text joinCode;

    // Start is called before the first frame update
    void Start()
    {
        joinCode.text = "Hallo!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
