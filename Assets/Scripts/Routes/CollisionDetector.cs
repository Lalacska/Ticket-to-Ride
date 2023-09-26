using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CollisionDetector : MonoSingleton<CollisionDetector>
{
    private void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            // When the player click on the station it act as a button
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Collision");
                Transform transform = gameObject.transform.parent;
                GameObject go = transform.gameObject;
                Route route = go.GetComponent<Route>();
                route.GetColorFromName(gameObject.name);
                route.HeyFromParent();
            }
        }
    }

    
}
