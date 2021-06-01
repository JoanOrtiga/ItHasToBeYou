using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFloor : MonoBehaviour
{


    public bool walkWood;
    public bool upStearsWood;
    public bool upStearsStone;
    public bool upStearsMetal;

    public FootStepSound player;
  


    void Start()
    {
       // player = GetComponent<FootStepSound>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("PLAYER");
            player.upStearsStone = false;
            player.walkWood = false;
            player.upStearsMetal = false;
            player.upStearsWood = false;

            if (walkWood) player.walkWood = true;
            if (upStearsWood) player.upStearsWood = true;
            if (upStearsStone) player.upStearsStone = true;
            if (upStearsMetal) player.upStearsMetal = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.upStearsStone = false;
            player.walkWood = true;
            player.upStearsMetal = false;
            player.upStearsWood = false;
           
        }
    }
}
