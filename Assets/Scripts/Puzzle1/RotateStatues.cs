using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateStatues : MonoBehaviour , IInteractable , AnimationTouch
{
    public enum StatueSide
    {
        Front_Left, Front, Front_Right, Back_Right, Back, Back_Left
    }

    public StatueSide currentState;
    public StatueSide objectiveState;

    [SerializeField] private float rotationSpeed;

    private bool rotating = false;
    private bool inControl;

    private Transform rotateObjective;

    private bool waitNoOtherInput = false;

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

        playerController.SetCurrentPuzzle(this);
        playerController.DisableController(true, true, true, true);
    

        StartCoroutine(LookAt());
        StartCoroutine(AttachPlayer());
    }

    private void Update()
    {
        if (inControl && !rotating && !waitNoOtherInput)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5f)
            {
                RotateObjective(false);
                waitNoOtherInput = true;
            }
            else if (Input.GetAxisRaw("Horizontal") < -0.5f)
            {
                playerController.AnimatorSetTrigger("P1_MoveLeft");
                RotateObjective(true);
                waitNoOtherInput = true;
            }
        }
        
        if (rotating)
        { 
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rotateObjective.localRotation, rotationSpeed * Time.deltaTime);

            if (Math.Abs(transform.localRotation.eulerAngles.y - rotateObjective.localRotation.eulerAngles.y) < Mathf.Epsilon)
            {
                rotating = false;
             
                
                UpdateStatueSide();
            }
        }

        if (Input.GetButtonDown("Interact") && inControl)
        {
            playerController.ChangeLookCloserState(false);
            playerController.EnableController(true,true,true,true);
            playerController.CancelCurrentPuzzle();
            playerController.AnimatorSetBool("P1", false);
            playerController.ReAttachHand();
            inControl = false;
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
    
    
    
    
    private PlayerController playerController;
    private Transform playerTransform;
    [SerializeField] private Transform lockCameraPoint;

    [SerializeField] private Transform[] lockPoints;
    
    private float moveToSpeed = 1f;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerTransform = playerController.transform;
    }

    private IEnumerator LookAt()
    {
        while (!playerController.cameraController.LookAt(lockCameraPoint.position, 3f))
        {
            yield return null;
        }

        inControl = true;
        playerController.DettachHand();
        playerController.ChangeLookCloserState(true);
    }

    private IEnumerator AttachPlayer()
    {
        playerController.DisableController(true, true, true);

        float lastMagnitude;
        float currentMagnitude;
        Transform positionChild = null;

        lastMagnitude = (lockPoints[0].position - playerTransform.position).sqrMagnitude;
        positionChild = lockPoints[0];
        foreach (var trans in lockPoints)
        {
            currentMagnitude = (trans.position - playerTransform.position).sqrMagnitude;
            
            if (currentMagnitude < lastMagnitude)
            {
                currentMagnitude = lastMagnitude;
                positionChild = trans;
            }
        }
        
        playerController.AnimatorSetBool("P1", true);

        while ((positionChild.position - playerTransform.position).sqrMagnitude > 0.01 * 0.01)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, positionChild.position, moveToSpeed * Time.deltaTime);
            
            yield return null;
        }
        
        playerTransform.position = positionChild.position;
    }

    private void RotateObjective(bool left)
    {
        if (left)
        {
            rotateObjective.localRotation = Quaternion.Euler(rotateObjective.eulerAngles.x, rotateObjective.eulerAngles.y + 60, rotateObjective.eulerAngles.z);
 
        }
        else
        {
            rotateObjective.localRotation = Quaternion.Euler(rotateObjective.eulerAngles.x, rotateObjective.eulerAngles.y - 60, rotateObjective.eulerAngles.z);

        }
    }

    public void Touch()
    {
        rotating = true;
        waitNoOtherInput = false;
    }
}
