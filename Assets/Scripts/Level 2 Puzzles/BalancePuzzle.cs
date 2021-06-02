using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePuzzle : MonoBehaviour
{
    public PlacePlate[] metalsList;
    public GameObject popUpPuzzle;
    

  
    void Update()
    {
        if (MetalsOnPlace())
        {
            popUpPuzzle.SetActive(true);
        }
        else
        {
            popUpPuzzle.SetActive(false );
        }


    }

  

    private bool MetalsOnPlace()
    {
        if (metalsList[0].hasBeenPlaced && metalsList[1].hasBeenPlaced && metalsList[2].hasBeenPlaced && metalsList[3].hasBeenPlaced && metalsList[4].hasBeenPlaced)
        { 
            return (true);
        }
        else
        {
            return (false);
        }
    }
}
