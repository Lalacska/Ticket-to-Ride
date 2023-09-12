using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckStations : MonoSingleton<CheckStations>
{
    [SerializeField] private GameObject TurnPanel;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
       button = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnPanel.activeInHierarchy == true)
        {
            Debug.Log("Active");
            CheckPlayersStationServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void CheckPlayersStationServerRpc(ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("ServerRpc");
        Debug.Log(PlayerManager.Instance.stats);
        foreach (PlayerStat stat in PlayerManager.Instance.stats)
        {
            //Debug.Log("player");
            //if (stat.myTurn && stat.stations == 0)
            //{
            //    Debug.Log("Inactive");
            //    button.interactable = false;
            //}
        }
    }



}
