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

public class Route : MonoBehaviour
{
    private string m_routeName;
    [SerializeField] private bool m_isDouble;
    [SerializeField] private NetworkVariable<bool> m_isClaimed = new NetworkVariable<bool>(false);
    [SerializeField] private RouteType routeType;

    public string routeName { get { return m_routeName; } set { m_routeName = value; } }
    public NetworkVariable<bool> isClaimed { get { return m_isClaimed; } set { m_isClaimed = value; } }


    private Material emissiveMaterial;

    #region Variables

    public GameObject route;


    public bool playerChoice;

    private enum RouteType { Route, Tunnel}
    public enum playerConnected { P1, P2, P3, P4, P5 }
    #endregion

    private void Awake()
    {
        routeName = gameObject.name;
    }

    // Start is called before the first frame update
    void Start()
    {
        //GetChildTransform();
    }

    public void GetChildTransform()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("Transform: " + child +" "+child.position);
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            if (isClaimed.Value) return;
            foreach (Transform child in transform)
            {
                Transform highlight = child.Find("Highlight");
                highlight.gameObject.SetActive(true);
            }
            //TurnEmissionOn();
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            foreach (Transform child in transform)
            {
                Transform highlight = child.Find("Highlight");
                highlight.gameObject.SetActive(false);
            }
            //TurnEmissionOff();
        }
    }


    public void SetType(bool isTunnel)
    {
        if (isTunnel)
        {
            routeType = RouteType.Tunnel;
        }
        else
        {
            routeType = RouteType.Route;
        }
    }

    public void HighlightOn()
    {
        foreach (Transform child in transform)
        {
            Transform highlight = child.Find("Highlight");
            highlight.gameObject.SetActive(true);
        }
    }

    public void HighlightOff()
    {
        foreach (Transform child in transform)
        {
            Transform highlight = child.Find("Highlight");
            highlight.gameObject.SetActive(false);
        }
    }

    public void HeyFromParent()
    {
        if (m_isDouble)
        {
            Transform transform = gameObject.transform.parent;
            GameObject go = transform.gameObject;
            Debug.Log("Hey I'm doouble " + go.name);
        }
        else
        {
            Debug.Log("Hey " + gameObject.name);
        }
        
    }


    public void GetColorFromName(string name)
    {
        string color;
        string[] words = name.Split(' ');
        if (words.Length > 0)
        {
            // Return the first word
            color = words[0];
        }
        else
        {
            // No words found, return an empty string or handle it as needed
            color = string.Empty;
        }
        Debug.Log("Color: "+color);
    }


    //private void ClaimedRoute()
    //{

    //    if (playerChoice == true)
    //    {
    //        isClaimed = true;
    //    }
    //    else if (playerChoice == false)
    //    {
    //        isClaimed = false;
    //    }

    //}





}
