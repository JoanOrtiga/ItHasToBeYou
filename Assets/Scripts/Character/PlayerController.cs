using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour , IAnimationTouch
{
    public CameraController cameraController { get; private set; }
    public PlayerMovement playerMovement { get; private set; }
    public PickUp pickUpSystem { get; private set; }
    public CharacterController characterController { get; private set; }

    public BreathCamera breathCamera { get; private set; }

    [SerializeField] private Animator handAnimator;
    private Transform hand;

    private IAnimationTouch puzzleTouchController;

    private Vector3 initialHandPosition;
    private Quaternion initialHandRotation;

    Quaternion saveCameraRot;
    Quaternion savePivotRot;
    private Transform pivot;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        cameraController = transform.GetComponentInChildren<CameraController>();
        playerMovement = GetComponent<PlayerMovement>();
        pickUpSystem = GetComponent<PickUp>();
        breathCamera = GetComponentInChildren<BreathCamera>();

        pivot = cameraController.transform.GetChild(0);

        hand = handAnimator.transform;
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

        if (camera)
            cameraController.enabled = false;
        if (pickUp)
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
    public void EnableController(bool movement = false, bool camera = false, bool pickUp = false, bool breath = false)
    {
        if (movement)
        {
            playerMovement.enabled = true;
            characterController.enabled = true;
        }

        if (camera)
            cameraController.enabled = true;

        if (pickUp)
            pickUpSystem.enabled = true;

        if (breath)
            breathCamera.enabled = false;
    }

   

    public void SetCurrentPuzzle(IAnimationTouch touch)
    {
        puzzleTouchController = touch;
    }

    public void CancelCurrentPuzzle()
    {
        puzzleTouchController = null;
    }

    public void DettachHand()
    {
        initialHandPosition = hand.position;
        initialHandRotation = hand.rotation;

        saveCameraRot = cameraController.transform.localRotation;
        savePivotRot = pivot.localRotation;

        hand.parent = playerMovement.transform;

        hand.position = initialHandPosition;
        hand.rotation = initialHandRotation;
    }

    public void ReAttachHand()
    {
        Quaternion currentCameraRot = cameraController.transform.localRotation;
        Quaternion currentPivotRot = pivot.localRotation;

        cameraController.transform.localRotation = saveCameraRot;
        pivot.localRotation = savePivotRot;

        hand.parent = breathCamera.transform.parent;

        hand.position = initialHandPosition;
        hand.rotation = initialHandRotation;

        cameraController.transform.localRotation = currentCameraRot;
        pivot.localRotation = currentPivotRot;
    }

   

    public void ChangeLookCloserState(bool state, bool x, bool y)
    {
        cameraController.enabled = state;
        cameraController.lookCloser = state;
        cameraController.ChangeInitialYaw(x, y);
    }
    
    //Animator
    public void Touch()
    {
        if (puzzleTouchController != null)
            puzzleTouchController.Touch();
        else
        {
            Debug.Log("Puzzle Is null");
        }
    }

    public void Finished()
    {
        if (puzzleTouchController != null)
        {
            puzzleTouchController.Finished();
        }
        else
        {
            Debug.Log("Puzzle Is null");
        }
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
}