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
        // Cast a ray from the camera to the mouse position.
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        // Check if the left mouse button is pressed.
        if (Input.GetMouseButtonDown(0))
        {
            // When the player click on the station it act as a button
            // Check if the ray hits a collider and the collider's game object is the same as this game object.
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Collision");

                // Get the parent transform and the parent game object.
                Transform transform = gameObject.transform.parent;
                GameObject go = transform.gameObject;

                // Get the Route component from the parent game object and call the ClaimRoute method.
                Route route = go.GetComponent<Route>();
                route.ClaimRoute();
            }
        }
    }

    
}
