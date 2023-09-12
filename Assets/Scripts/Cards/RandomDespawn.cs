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
    public void TestButton()
    {
        CardSelector.Instance.TestButton2(gameObject);
        Debug.Log("Hello I'm gonna destroy myself");
        Destroy(gameObject); 

    }

}

