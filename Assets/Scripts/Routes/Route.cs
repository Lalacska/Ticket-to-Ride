using Assets.Scripts.Managers;
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

public class Route : Singleton<Route>
{
    [SerializeField] private string m_routeName;
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

    [SerializeField] private List<GameObject> m_tiles;
    public List<GameObject> Tiles { get { return m_tiles; } set { m_tiles = value; } }

    private Material emissiveMaterial;

    #region Variables


    private enum RouteType { Route, Tunnel }
    #endregion

    // This metode runs first, and set some basic variables up
    private void Awake()
    {
        neededLocomotiv = 0;
        lenght = CountChilds();
        routeName = SetRouteName();
        points = PointCounter(lenght);
        Debug.Log(routeName + " need " + lenght+ " "+ routeColor + " cards and " + neededLocomotiv+ " locomotiv to get " + points + " points.");

    }

    // This metode is only important if the route has 2 part
    // If gameobject name doesn't contais a "-", it gets and sets its parent objects name 
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

    // This set isClaimed to true on the server
    [ServerRpc(RequireOwnership = false)]
    public void SetIsClaimedServerRpc(ServerRpcParams serverRpcParams = default)
    {
        isClaimed.Value = true;
    }

    // This calls the AutoSelectCards metode
    public void ClaimRoute()
    {
        CardSelector.Instance.AutoSelectCards(routeType.ToString(), routeColor, lenght, neededLocomotiv, routeName);
    }

    // This counts the child objects and add them to the Tiles list
    public int CountChilds()
    {
        string color = "";
        int counter = 0;
        foreach (Transform child in transform)
        {
            GameObject go = child.gameObject;
            Tiles.Add(go);

            // This checks the color if a tile is a Locomotive it adds plus one to the neededLocomotive
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

    // This sets a point for the route according how long the route is
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

    // This sets the type of the route
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

    // This finds and activates the Highlight element in all the children
    public void HighlightOn()
    {
        foreach (Transform child in transform)
        {
            Transform highlight = child.Find("Highlight");
            if (highlight != null)
            {
                highlight.gameObject.SetActive(true);
            }
        }
    }

    // This finds and deactivates the Highlight element in all the children
    public void HighlightOff()
    {
        foreach (Transform child in transform)
        {
            Transform highlight = child.Find("Highlight");
            if(highlight != null)
            {
                highlight.gameObject.SetActive(false);
            }
        }
    }

    // Gets a color name splits it up and sends the first word back
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
