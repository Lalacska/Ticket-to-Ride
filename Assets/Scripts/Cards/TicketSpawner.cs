using Assets.Scripts.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;



public class TicketSpawner : Singleton<TicketSpawner>
{
    [SerializeField] private GameObject Ticket_1;
    [SerializeField] private GameObject Ticket_2;
    [SerializeField] private GameObject Ticket_3;
    [SerializeField] private GameObject Ticket_4;
    [SerializeField] private GameObject Ticket_5;
    [SerializeField] private GameObject Ticket_6;
    [SerializeField] private GameObject Ticket_7;
    [SerializeField] private GameObject Ticket_8;
    [SerializeField] private GameObject Ticket_9;
    [SerializeField] private GameObject Ticket_10;
    [SerializeField] private GameObject Ticket_11;
    [SerializeField] private GameObject Ticket_12;
    [SerializeField] private GameObject Ticket_13;
    [SerializeField] private GameObject Ticket_14;
    [SerializeField] private GameObject Ticket_15;
    [SerializeField] private GameObject Ticket_16;
    [SerializeField] private GameObject Ticket_17;
    [SerializeField] private GameObject Ticket_18;
    [SerializeField] private GameObject Ticket_19;
    [SerializeField] private GameObject Ticket_20;
    [SerializeField] private GameObject Ticket_21;
    [SerializeField] private GameObject Ticket_22;
    [SerializeField] private GameObject Ticket_23;
    [SerializeField] private GameObject Ticket_24;
    [SerializeField] private GameObject Ticket_25;
    [SerializeField] private GameObject Ticket_26;
    [SerializeField] private GameObject Ticket_27;
    [SerializeField] private GameObject Ticket_28;
    [SerializeField] private GameObject Ticket_29;
    [SerializeField] private GameObject Ticket_30;
    [SerializeField] private GameObject Ticket_31;
    [SerializeField] private GameObject Ticket_32;
    [SerializeField] private GameObject Ticket_33;
    [SerializeField] private GameObject Ticket_34;
    [SerializeField] private GameObject Ticket_35;
    [SerializeField] private GameObject Ticket_36;
    [SerializeField] private GameObject Ticket_37;
    [SerializeField] private GameObject Ticket_38;
    [SerializeField] private GameObject Ticket_39;
    [SerializeField] private GameObject Ticket_40;
    [SerializeField] private GameObject Ticket_41;
    [SerializeField] private GameObject Ticket_42;
    [SerializeField] private GameObject Ticket_43;
    [SerializeField] private GameObject Ticket_44;
    [SerializeField] private GameObject Ticket_45;
    [SerializeField] private GameObject Ticket_46;


    public GameObject TicketChoser(int ticketID)
    {
        GameObject ticket = new GameObject();
        switch (ticketID)
        {
            case 1:
                ticket = Ticket_1;
                break;
            case 2:
                ticket = Ticket_2;
                break;
            case 3:
                ticket = Ticket_3;
                break;
            case 4:
                ticket = Ticket_4;
                break;
            case 5:
                ticket = Ticket_5;
                break;
            case 6:
                ticket = Ticket_6;
                break;
            case 7:
                ticket = Ticket_7;
                break;
            case 8:
                ticket = Ticket_8;
                break;
            case 9:
                ticket = Ticket_9;
                break;
            case 10:
                ticket = Ticket_10;
                break;
            case 11:
                ticket = Ticket_11;
                break;
            case 12:
                ticket = Ticket_12;
                break;
            case 13:
                ticket = Ticket_13;
                break;
            case 14:
                ticket = Ticket_14;
                break;
            case 15:
                ticket = Ticket_15;
                break;
            case 16:
                ticket = Ticket_16;
                break;
            case 17:
                ticket = Ticket_17;
                break;
            case 18:
                ticket = Ticket_18;
                break;
            case 19:
                ticket = Ticket_19;
                break;
            case 20:
                ticket = Ticket_20;
                break;
            case 21:
                ticket = Ticket_21;
                break;
            case 22:
                ticket = Ticket_22;
                break;
            case 23:
                ticket = Ticket_23;
                break;
            case 24:
                ticket = Ticket_24;
                break;
            case 25:
                ticket = Ticket_25;
                break;
            case 26:
                ticket = Ticket_26;
                break;
            case 27:
                ticket = Ticket_27;
                break;
            case 28:
                ticket = Ticket_28;
                break;
            case 29:
                ticket = Ticket_29;
                break;
            case 30:
                ticket = Ticket_30;
                break;
            case 31:
                ticket = Ticket_31;
                break;
            case 32:
                ticket = Ticket_32;
                break;
            case 33:
                ticket = Ticket_33;
                break;
            case 34:
                ticket = Ticket_34;
                break;
            case 35:
                ticket = Ticket_35;
                break;
            case 36:
                ticket = Ticket_36;
                break;
            case 37:
                ticket = Ticket_37;
                break;
            case 38:
                ticket = Ticket_38;
                break;
            case 39:
                ticket = Ticket_39;
                break;
            case 40:
                ticket = Ticket_40;
                break;
            case 41:
                ticket = Ticket_41;
                break;
            case 42:
                ticket = Ticket_42;
                break;
            case 43:
                ticket = Ticket_43;
                break;
            case 44:
                ticket = Ticket_44;
                break;
            case 45:
                ticket = Ticket_45;
                break;
            case 46:
                ticket = Ticket_46;
                break;
            default:
                ticket = null;
                break;
        }
        return ticket;

    }



}

