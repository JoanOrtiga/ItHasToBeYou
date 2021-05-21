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

    private Transform rotateObjective;

    private void Awake()
    {
        rotateObjective = new GameObject().transform;
        rotateObjective.parent = transform.parent;
        rotateObjective.localRotation = transform.localRotation;
    }

    public void Interact()
    {
        if(rotating)
            return;
        
        rotating = true;

        rotateObjective.localRotation = Quaternion.Euler(rotateObjective.eulerAngles.x, rotateObjective.eulerAngles.y + 60, rotateObjective.eulerAngles.z);
    }

    private void Update()
    {
        if (rotating)
        { 
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotateObjective.localRotation, rotationSpeed * Time.deltaTime);
            

            if (Math.Abs(transform.localRotation.eulerAngles.y - rotateObjective.localRotation.eulerAngles.y) < Mathf.Epsilon)
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
