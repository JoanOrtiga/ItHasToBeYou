using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesButtons : MonoBehaviour , IInteractable
{
    private Switches switchController;

    [SerializeField] private bool up;

    private void Awake()
    {
        switchController = GetComponentInParent<Switches>();
    }


    public void Interact()
    {
        if(up)
            switchController.GoUp(true);
        else
            switchController.GoDown(true);
        
    }
}
