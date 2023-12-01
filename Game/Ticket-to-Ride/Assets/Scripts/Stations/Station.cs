using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Station : Singleton<Station>
{
    [SerializeField] private string m_stationName;
    private bool m_isActive = false;
    [SerializeField] private NetworkVariable<bool> m_isTaken = new NetworkVariable<bool>(false);

    public string stationName { get { return m_stationName; } set { m_stationName = value; } }

    public bool isActive { get { return m_isActive; } set { m_isActive = value; } }

    public NetworkVariable<bool> isTaken { get { return m_isTaken; } set { m_isTaken = value; } }


    private Material emissiveMaterial;
    private Renderer objectToChange;

    public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    void Update()
    {
        // Cast a ray from the camera to the mouse position.
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        // If the Station is not taken this lets the user click on it
        if (!isTaken.Value)
        {

            // Check if the left mouse button is pressed.
            if (Input.GetMouseButtonDown(0))
            {
                // When the player click on the station it act as a button
                // Check if the ray hits a collider and the collider's game object is the same as this game object.
                if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Button Clicked ");

                    // If the station is active, it calls a server rpc from CardSelector
                    if (isActive)
                    {
                        Debug.Log(stationName);
                        CardSelector.Instance.AutoSelectCards("Station", "none", m_name: stationName);
                        Debug.Log("A");

                    }
                    OnClick.Invoke();
                }
            }
        }
    }


    void Start()
    {
        // Store the reference to the current game object as the definedButton.
        definedButton = gameObject;

        // Get the material and renderer components of the current game object.
        emissiveMaterial = gameObject.GetComponent<Renderer>().material;
        objectToChange = gameObject.GetComponent<Renderer>();

        // Disable the "_EMISSION" keyword in the emissive material.
        emissiveMaterial.DisableKeyword("_EMISSION");


    }

    // This turns off the emission for the object
    public void TurnEmissionOff()
    {
        emissiveMaterial.DisableKeyword("_EMISSION");
        isActive = false;
    }

    // This turna on the emission for the object
    public void TurnEmissionOn()
    {
        emissiveMaterial.EnableKeyword("_EMISSION");
        isActive = true;
    }

    // Sets the Network Variable to true
    [ServerRpc(RequireOwnership = false)]
    public void SetIsTakenServerRpc(ServerRpcParams serverRpcParams = default)
    {
        isTaken.Value = true;
    }
}

