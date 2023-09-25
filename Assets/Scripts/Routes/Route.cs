using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Route : MonoBehaviour
{













    private Material emissiveMaterial;

    #region Variables

    public GameObject route;

    public bool isClaimed;

    public bool playerChoice;


    public enum playerConnected { P1, P2, P3, P4, P5 }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        GetChildTransform();
    }

    public void GetChildTransform()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("Transform: " + child +" "+child.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            foreach (Transform child in transform)
            {
                Transform highlight = child.Find("Highlight");
                highlight.gameObject.SetActive(true);
            }
            //TurnEmissionOn();
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            foreach (Transform child in transform)
            {
                Transform highlight = child.Find("Highlight");
                highlight.gameObject.SetActive(false);
            }
            //TurnEmissionOff();
        }
    }

    // This turns off the emission for the object
    public void TurnEmissionOff()
    {
        emissiveMaterial.DisableKeyword("_EMISSION");
    }

    // This turna on the emission for the object
    public void TurnEmissionOn()
    {
        emissiveMaterial.EnableKeyword("_EMISSION");
    }

    private void ClaimedRoute()
    {
        
        if (playerChoice == true)
        {
            isClaimed = true;
        }
        else if (playerChoice == false)
        {
            isClaimed = false;
        }

    }




    
}
