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
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        // If the Station is not taken this lets the user click on it
        if (!isTaken.Value)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // When the player click on the station it act as a button
                if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Button Clicked ");
                    // If the station is active, it sends a serverRpc metode then runs ChooseCity
                    if (isActive/*.Value*/)
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
        definedButton = gameObject;
        emissiveMaterial = gameObject.GetComponent<Renderer>().material;
        objectToChange = gameObject.GetComponent<Renderer>();
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

