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
    public enum Type { Card, Tunnel, ExtraCard }
    
    [SerializeField] private string m_Color;
    private Type m_cardType;

    public Type cardType { get { return m_cardType; } set { m_cardType = value; } }
    public string Color { get { return m_Color; } set { m_Color = value; } }

    // Invokes CardSelector's despawn method and destroys the attached game object.
    public void DespawnObject()
    {
        CardSelector.Instance.DespawnCard(gameObject);
        Debug.Log("Hello I'm gonna destroy myself");
        Destroy(gameObject); 
    }
}

