using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Transform player;
    public GameObject crosshairCanvas;



    public void InteractCrosshair(bool active)
    {
        if (active)
        {
            crosshairCanvas.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            crosshairCanvas.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }

   
}
