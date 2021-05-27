using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraController cameraController { get; private set; }
    public PlayerMovement playerMovement { get; private set; }
    public PickUp pickUpSystem { get; private set; }
    public CharacterController characterController { get; private set; }

    public BreathCamera breathCamera { get; private set; }

    [SerializeField] private Animator handAnimator;

    private AnimationTouch puzzleTouchController;
    
    private void Awake() 
    {
        characterController = GetComponent<CharacterController>();
        cameraController = transform.GetComponentInChildren<CameraController>();
        playerMovement = GetComponent<PlayerMovement>();
        pickUpSystem = GetComponent<PickUp>();
        breathCamera = GetComponentInChildren<BreathCamera>();
    }

    /// <summary>
    ///  Parameters == true DeActivates that component
    /// </summary>
    /// <param name="movement"></param>
    /// <param name="camera"></param>
    /// <param name="pickUp"></param>
    /// <param name="breath"></param>
    public void DisableController(bool movement = false, bool camera = false, bool pickUp = false, bool breath = false)
    {
        if (movement)
        {
            playerMovement.enabled = false;
            characterController.enabled = false;
        }
           
        if(camera)
            cameraController.enabled = false;
        if(pickUp)
            pickUpSystem.enabled = false;

        if (breath)
            breathCamera.enabled = false;

    }

    /// <summary>
    /// Parameters == true Activates that component
    /// </summary>
    /// <param name="movement"></param>
    /// <param name="camera"></param>
    /// <param name="pickUp"></param>
    /// <param name="breath"></param>
    public void EnableController(bool movement = false, bool camera = false, bool pickUp= false, bool breath = false)
    {
        if (movement)
        {
            playerMovement.enabled = true;
            characterController.enabled = true;
        }
        
        if(camera)
            cameraController.enabled = true;
        
        if(pickUp)
            pickUpSystem.enabled = true;
        
        if (breath)
            breathCamera.enabled = false;
    }
    
    public void AnimatorSetFloat(string name, float value)
    {
        handAnimator.SetFloat(name, value);
    }
    public void AnimatorSetBool(string name, bool value)
    {
        handAnimator.SetBool(name, value);
    }
    public void AnimatorSetInteger(string name, int value)
    {
        handAnimator.SetInteger(name, value);
    }
    public void AnimatorSetTrigger(string name)
    {
        handAnimator.SetTrigger(name);
    }

    public void SetCurrentPuzzle(AnimationTouch touch)
    {
        puzzleTouchController = touch;
    }

    public void CancelCurrentPuzzle()
    {
        puzzleTouchController = null;
    }
    
    public void Touch()
    {
        puzzleTouchController.Touch();
    }
}