using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHand : MonoBehaviour
{
    private PlayerController playerController;
    public Vector3 handRotation;
    public Vector3 handPosition;
    
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    
     

    public void Touch()
    {
        playerController.Touch();
    }

    public void Finished()
    {
        playerController.Finished();
    }
}
