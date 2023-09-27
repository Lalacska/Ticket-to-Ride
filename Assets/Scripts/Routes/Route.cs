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
    [SerializeField] private int m_lenght;
    [SerializeField] private int m_points;
    [SerializeField] private string m_routeColor;
    [SerializeField] private int m_neededLocomotiv;

    public string routeName { get { return m_routeName; } set { m_routeName = value; } }
    public NetworkVariable<bool> isClaimed { get { return m_isClaimed; } set { m_isClaimed = value; } }
    public int lenght { get { return m_lenght; } set { m_lenght = value; } }
    public int points { get { return m_points; } set { m_points = value; } }
    public string routeColor { get { return m_routeColor; } set { m_routeColor = value; } }
    public int neededLocomotiv { get { return m_neededLocomotiv; } set { m_neededLocomotiv = value; } }

    private Material emissiveMaterial;

    #region Variables

    public GameObject route;


    public bool playerChoice;

    private enum RouteType { Route, Tunnel }
    public enum playerConnected { P1, P2, P3, P4, P5 }
    #endregion

    private void Awake()
    {
        neededLocomotiv = 0;
        lenght = CountChilds();
        routeName = SetRouteName();
        points = PointCounter(lenght);
        Debug.Log(routeName + " need " + lenght+ " "+ routeColor + " cards and " + neededLocomotiv+ " locomotiv to get " + points + " points.");

    }

    public string SetRouteName()
    {
        string name = gameObject.name;
        if (!gameObject.name.Contains("-"))
        {
            Transform transform = gameObject.transform.parent;
            GameObject parent = transform.gameObject;
            name = parent.name;
        }
        return name;
    }

    // Start is called before the first frame update
    void Start()
    {
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


    public int CountChilds()
    {
        string color = "";
        int counter = 0;
        foreach (Transform child in transform)
        {
            GameObject go = child.gameObject;
            color = GetColorFromName(go.name);
            if (color == "Locomotiv")
            {
                neededLocomotiv++;
            } else if (routeColor == string.Empty)
            {
                routeColor = color;
            }
            counter++;
        }
        return counter;
    }

    public int PointCounter(int lenght)
    {
        int points = 0;
        switch (lenght)
        {
            case 1:
                points = 1;
                break;
            case 2:
                points = 2;
                break;
            case 3:
                points = 4;
                break;
            case 4:
                points = 7;
                break;
            case 6:
                points = 15;
                break;
            case 8:
                points = 21;
                break;
        }
        return points;
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


    public string GetColorFromName(string name)
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
        return color;
    }

}
