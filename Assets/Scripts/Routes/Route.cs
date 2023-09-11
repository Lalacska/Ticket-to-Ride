using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{

    #region Variables

    public GameObject route;

    public bool isClaimed;

    public bool playerChoice;


    public enum playerConnected { P1, P2, P3, P4, P5 }


    #endregion





    private void ClaimedRoute()
    {
        /*
        if (playerConnected.P1 == // PlayerID \\ )
        {

            if (playerChoice == true)
            {
                isClaimed = true;
            }
            else if (playerChoice == false)
            {
                isClaimed = false;
            }

        }
        else if (playerConnected.P2 == // PlayerID \\ )
        {
            if (playerChoice == true)
            {
                isClaimed = true;
            }
            else if (playerChoice == false)
            {
                isClaimed = false;
            }
        }
        else if (playerConnected.P3 == // PlayerID \\ )
        {
            if (playerChoice == true)
            {
                isClaimed = true;
            }
            else if (playerChoice == false)
            {
                isClaimed = false;
            }
        }
        else if (playerConnected.P4 == // PlayerID \\ )
        {
            if (playerChoice == true)
            {
                isClaimed = true;
            }
            else if (playerChoice == false)
            {
                isClaimed = false;
            }
        }
        else
        {
            if (playerChoice == true)
            {
                isClaimed = true;
            }
            else if (playerChoice == false)
            {
                isClaimed = false;
            }
        }
        */


        //if (playerChoice == flase)
        //{
        //    isClaimed = false;
        //}
        //else if (playerChoice == true)
        //{
        //    isClaimed = true;
        //}

        //if (playerChoice == flase)
        //{
        //    isClaimed = false;
        //}
        //else if (playerChoice == true)
        //{
        //    isClaimed = false;
        //}

        //if (playerChoice == flase)
        //{
        //    isClaimed = true;
        //}
        //else if (playerChoice == true)
        //{
        //    isClaimed = false;
        //}

        if (playerChoice == true)
        {
            isClaimed = true;
        }
        else if (playerChoice == false)
        {
            isClaimed = false;
        }

    }



    // Start is called before the first frame update
    void Start()
    {
        
    }


    
}
