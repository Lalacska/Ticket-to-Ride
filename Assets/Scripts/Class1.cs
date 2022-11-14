using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class Class1 : Singeltone<Class1>
    {
        private void Update()

        {

            // Check if the user is holding down the left mouse button on this frame.

            if (Mouse.current.leftButton.isPressed)

            {
                Debug.Log(IsOwner);
            }
        }
    }
}
