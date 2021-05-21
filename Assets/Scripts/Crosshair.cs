using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Transform player;
    public GameObject crosshairCanvas;

   public GameObject observeCrosshair_;
   public GameObject interactCrosshair_;
    private void Start()
    {
        interactCrosshair_ = crosshairCanvas.transform.GetChild(0).gameObject;
        observeCrosshair_ = crosshairCanvas.transform.GetChild(1).gameObject;
    }


    public void EnableCrosshair( bool interactCrosshair, bool observeCrosshair)
    {
        interactCrosshair_.SetActive(interactCrosshair ? true : false);
        observeCrosshair_.SetActive(observeCrosshair ? true : false);
    }
    public void DisableCrosshair(bool interactCrosshair, bool observeCrosshair)
    {
        interactCrosshair_.SetActive(interactCrosshair ? true : false);
        observeCrosshair_.SetActive(observeCrosshair ? true : false);
    }



}
