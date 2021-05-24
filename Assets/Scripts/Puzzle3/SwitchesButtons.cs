using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesButtons : MonoBehaviour , IInteractable
{
    [SerializeField] private Switches switchController;

    [SerializeField] private bool up;

    public void Interact()
    {
        if(up)
            switchController.GoUp(true);
        else
            switchController.GoDown(true);
        
    }
}
