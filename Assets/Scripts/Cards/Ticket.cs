using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Ticket : MonoBehaviour
{
    [SerializeField] private int m_ticketID;
    [SerializeField] private int m_ownerID;
    [SerializeField] private int m_score;
    [SerializeField] private string m_station1;
    [SerializeField] private string m_station2;
    [SerializeField] private bool m_isSpecial;
    [SerializeField] private bool m_isChosen;

    public int ticketID { get { return m_ticketID; } set { m_ticketID = value; } }
    public int ownerID { get { return m_ownerID; } set { m_ownerID = value; } }
    public int score { get { return m_score; } set { m_score = value; } }
    public string station1 { get { return m_station1; } set { m_station1 = value; } }
    public string station2 { get { return m_station2; } set { m_station2 = value; } }
    public bool isSpecial { get { return m_isSpecial; } set { m_isSpecial = value; } }
    public bool isChosen { get { return m_isChosen; } set { m_isChosen = value; } }

    private Material emissiveMaterial;
    private Renderer objectToChange;

    public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    void Update()
    {
        // Cast a ray from the camera to the mouse position
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the ray hits the current game object
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                // Toggle emission and log state
                if (isChosen)
                {
                    Debug.Log("Light off");
                    TurnEmissionOff();
                }else 
                {
                    Debug.Log("Light on");
                    TurnEmissionOn(); 
                }
                OnClick.Invoke();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        definedButton = gameObject;
        emissiveMaterial = gameObject.GetComponent<Renderer>().material;
        objectToChange = gameObject.GetComponent<Renderer>();
        emissiveMaterial.DisableKeyword("_EMISSION");
        isChosen = false;
    }

    // Turn off emission
    public void TurnEmissionOff()
    {
        emissiveMaterial.DisableKeyword("_EMISSION");
        isChosen = false;
    }

    // Turn on emission
    public void TurnEmissionOn()
    {
        emissiveMaterial.EnableKeyword("_EMISSION");
        isChosen = true;
    }

}

