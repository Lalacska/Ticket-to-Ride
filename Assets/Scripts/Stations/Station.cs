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
    private bool m_isTaken = false;

    public string stationName { get { return m_stationName; } set { m_stationName = value; } }

    public bool isActive { get { return m_isActive; } set { m_isActive = value; } }

    public bool isTaken { get { return m_isTaken; } set { m_isTaken = value; } }

    private Material emissiveMaterial;
    private Renderer objectToChange;

    public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked ");
                if (isActive)
                {
                    Debug.Log("Light off");
                    TurnEmissionOff();
                    isActive = false;
                }
                else
                {
                    Debug.Log("Light on");
                    TurnEmissionOn();
                    isActive = true;
                }
                //Highlight.Instance.Enable_Disable();
                OnClick.Invoke();
            }
        }
    }

    void Start()
    {
        definedButton = this.gameObject;
        emissiveMaterial = this.gameObject.GetComponent<Renderer>().material;
        objectToChange = this.gameObject.GetComponent<Renderer>();
        emissiveMaterial.DisableKeyword("_EMISSION");

    }

    public void TurnEmissionOff()
    {
        emissiveMaterial.DisableKeyword("_EMISSION");
        isActive = false;


    }
    public void TurnEmissionOn()
    {
        emissiveMaterial.EnableKeyword("_EMISSION");
        isActive = true;
     
    }
}

