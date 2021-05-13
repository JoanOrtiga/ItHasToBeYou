using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateStatues : MonoBehaviour , IInteractable
{
    public enum StatueSide
    {
        Front_Left, Front, Front_Right, Back_Right, Back, Back_Left
    }

    public StatueSide currentState;
    public StatueSide objectiveState;

    [SerializeField] private float rotationSpeed;

    private bool rotating = false;

    private Vector3 finalRotation;

    public void Interact()
    {
        if(rotating)
            return;
        
        rotating = true;

        finalRotation = transform.rotation.eulerAngles;

        finalRotation.y = finalRotation.y + 60;
    }

    private void Update()
    {
        if (rotating)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(finalRotation), rotationSpeed * Time.deltaTime);

            if (transform.rotation == Quaternion.Euler(finalRotation))
            {
                rotating = false;
                
                UpdateStatueSide();
            }
        }
    }

    public bool Solved()
    {
        return currentState == objectiveState;
    }

    private void UpdateStatueSide()
    {
        if ((int) currentState == 5)
        {
            currentState = StatueSide.Front_Left;
        }
        else
            currentState = (StatueSide) ((int) currentState + 1);
    }
}
