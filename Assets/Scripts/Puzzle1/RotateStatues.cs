using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateStatues : MonoBehaviour , IInteractable , IAnimationTouch
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
    
    
    private PlayerController playerController;
    private Transform playerTransform;
    [SerializeField] private Transform lockCameraPoint;

    [SerializeField] private Transform[] lockPoints;
    
    private float moveToSpeed = 1f;

    private CanvasTutorial canvasTutorial;

    public string soundPathRotation = "event:/INGAME/Puzzle 1/1.1/Rotate";

    public string soundPathSelect = "event:/INGAME/Puzzle 1/1.1/SelectStatue";

    private bool narrativeAudio;
    TextBox textBox;

    private Crosshair crosshair;

    private void Awake()
    {
        crosshair = FindObjectOfType<Crosshair>();
        
        canvasTutorial = FindObjectOfType<CanvasTutorial>();
        rotateObjective = new GameObject().transform;
        rotateObjective.parent = transform.parent;
        rotateObjective.localRotation = transform.localRotation;
        textBox = this.gameObject.GetComponent<TextBox>();
    }

    public void Interact()
    {
        if(rotating)
            return;

        canvasTutorial.TutorialPuzzle11(true);

        playerController.SetCurrentPuzzle(this);
        playerController.DisableController(true, true, true, true);

        StartCoroutine(LookAt(false));
        StartCoroutine(AttachPlayer());

        waitNoOtherInput = true;
    }

    private void Update()
    {
        if (inControl && !rotating && !waitNoOtherInput)
        {
           if (Input.GetAxisRaw("Horizontal") < -0.5f)
           {
               FMODUnity.RuntimeManager.PlayOneShot(soundPathRotation, gameObject.transform.position);
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

        if (Input.GetButtonDown("Interact") && inControl && !waitNoOtherInput )
        {
            inControl = false;
            playerController.ChangeLookCloserState(false, false, false);
            StartCoroutine(LookAt(true));
            canvasTutorial.TutorialPuzzle11(false);
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
    
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerTransform = playerController.transform;
    }

    private IEnumerator LookAt(bool ending)
    {
        while (!playerController.cameraController.LookAt(lockCameraPoint.position, 8f))
        {
            yield return null;
        }

        if (ending)
        {
            playerController.ReAttachHand();
            playerController.CancelCurrentPuzzle();
            playerController.AnimatorSetBool("P1", false);
            playerController.EnableController(true,true,true,true);
        }
    }

    private void LateUpdate()
    {
        if (inControl)
        {
            crosshair.ChangeCrosshairState(false,false);
        }
    }

    private IEnumerator AttachPlayer()
    {
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
                lastMagnitude = currentMagnitude;
                positionChild = trans;
            }
        }

        while ((positionChild.position - playerTransform.position).sqrMagnitude > 0.01 * 0.01)
        {
            playerController.playerMovement.SimulateHeadBobbing();
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, positionChild.position, moveToSpeed * Time.deltaTime);
            
            yield return null;
        }
        
        while (!playerController.cameraController.LookAt(lockCameraPoint.position, 8f))
        {
            yield return null;
        }
        
        playerController.AnimatorSetBool("P1", true);
        
        playerTransform.position = positionChild.position;
    }

    private void RotateObjective(bool left)
    {
        if (left)
        {
            rotateObjective.localRotation = Quaternion.Euler(rotateObjective.eulerAngles.x, rotateObjective.eulerAngles.y + 60, rotateObjective.eulerAngles.z);

            if (narrativeAudio  == false)
            {
                if (textBox != null)
                {
                    narrativeAudio = true;
                    textBox.StartTextPuzzle();
                }
               
            }
        }
        else
        {
            rotateObjective.localRotation = Quaternion.Euler(rotateObjective.eulerAngles.x, rotateObjective.eulerAngles.y - 60, rotateObjective.eulerAngles.z);
        }
    }

    public void Touch()
    {
        rotating = true;
        CamaraShake.ShakeOnce(1, 3, new Vector3(0.03f, 0.02f));

    }

    public void Finished(int control)
    {
        if (control == 1)
        {
            playerController.DettachHand();
            playerController.ChangeLookCloserState(true, true, true, new Vector2(-25, 55 )); 
            inControl = true;
            
            return;
        }
        
        waitNoOtherInput = false;
    }
}
