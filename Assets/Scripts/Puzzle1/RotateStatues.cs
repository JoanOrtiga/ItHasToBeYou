using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
        
        finalRotation = transform.localRotation.eulerAngles;

        finalRotation.y = Mathf.RoundToInt(finalRotation.y);

        finalRotation.y = finalRotation.y + 60;
    }

    private void Update()
    {
        if (rotating)
        {

            
            
            
         //  transform.localRotation = Quaternion.RotateTowards(transform.localRotation, quater.Euler(finalRotation), rotationSpeed * Time.deltaTime);

           print(transform.localRotation.eulerAngles.y + " " + Quaternion.Euler(finalRotation).eulerAngles.y);

           /* if (Math.Abs(transform.localRotation.y - Quaternion.Euler(finalRotation).y) < Mathf.Epsilon)
            {
                rotating = false;
                
                UpdateStatueSide();
            }*/
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
