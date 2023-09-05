using Assets.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


public class Highlight : MonoSingleton<Highlight>
{
    [SerializeField] private Material emissiveMaterial;
    [SerializeField] private Renderer objectToChange;
    private bool emissionOn;

    private void Start()
    {
        emissiveMaterial = objectToChange.GetComponent<Renderer>().material;
        emissiveMaterial.DisableKeyword("_EMISSION");
        emissionOn = false;
    }

    public void Enable_Disable()
    {
        if (emissionOn)
        {
            TurnEmissionOff();
        }else
        { 
            TurnEmissionOn();
        }
    }

    public void TurnEmissionOff()
    {
        emissiveMaterial.DisableKeyword("_EMISSION");
        emissionOn = false;
    }
    public void TurnEmissionOn()
    {
        emissiveMaterial.EnableKeyword("_EMISSION");
        emissionOn = true;
    }

}

