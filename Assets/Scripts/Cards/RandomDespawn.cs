using Assets.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;



public class RandomDespawn : MonoSingleton<RandomDespawn>
{
    [SerializeField] private string m_Color;
    public string Color { get { return m_Color; } set { m_Color = value; } }
    public void DespawnObject()
    {
        CardSelector.Instance.DispawnCard(gameObject);
        Debug.Log("Hello I'm gonna destroy myself");
        Destroy(gameObject); 

    }

}

