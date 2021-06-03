using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatueSides : MonoBehaviour , IInteractable , IAnimationTouch
{

  //  public MoveableStatue.Sides side;
    public MovingStatue.Sides side;

    //[SerializeField] private MoveableStatue statue;
    [SerializeField] private MovingStatue statue;

    private Transform playerTransform;
    private PlayerController playerController;

    private bool ActiveSide;

    [SerializeField] private Transform positonChild;

    [SerializeField] private GameObject otherSide1;
    [SerializeField] private GameObject otherSide2;
    [SerializeField] private GameObject otherSide3;

    [SerializeField] private Transform lockCameraPoint;
    
    private bool deActivated = true;

    private bool cooldowned = false;

    [SerializeField] private float moveToSpeed = 1f;

    private PickUp pickController;
    
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        pickController = FindObjectOfType<PickUp>();
        playerTransform = playerController.transform;
    }

    public void Interact()
    {
        pickController.activePuzzle = true;

        if(ActiveSide)
            return;

        if (deActivated == false)
            return;

        cooldowned = false;

        deActivated = false;
        ActiveSide = true;
        
        playerController.SetCurrentPuzzle(this);
        playerController.DisableController(true, true, true, true);        
        
        StartCoroutine(AttachPlayer());
        StartCoroutine(LookAt());
        
        otherSide1.SetActive(false);
        otherSide2.SetActive(false);
        otherSide3.SetActive(false);

        StartCoroutine(Cooldown());
    }

    private IEnumerator LookAt()
    {
        while (!playerController.cameraController.LookAt(lockCameraPoint.position, 3f))
        {
            yield return null;
        }

        
    }

    private IEnumerator AttachPlayer()
    {
        float magnitude = (positonChild.position - playerTransform.position).sqrMagnitude;
        bool x = true;
        while (magnitude > 0.01 * 0.01)
        {
            playerTransform.position = Vector3.MoveTowards(playerTransform.position, positonChild.position, moveToSpeed * Time.deltaTime);
            
            magnitude = (positonChild.position - playerTransform.position).sqrMagnitude;

            if (magnitude < 0.7f * 0.7f && x)
            {
                playerController.AnimatorSetBool("P3.1", true);
                x = false;
            }
            
            yield return null;
        }
        
        if (x)
        {
            playerController.AnimatorSetBool("P3.1", true);
            x = false;
        }
        
        playerTransform.position = positonChild.position;
        playerTransform.parent = positonChild;
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.1f);
        cooldowned = true;
    }

    private void LateUpdate()
    {
        if (ActiveSide)
        {
            if (Input.GetButtonDown("Interact") && cooldowned && !statue.IsActive())
            {
                pickController.activePuzzle = false;

                StopAllCoroutines();
                
                deActivated = false;
                
                ActiveSide = false;
                
                playerTransform.parent = null;
                playerController.EnableController(true,true,true, true);
                
                otherSide1.SetActive(true);
                otherSide2.SetActive(true);
                otherSide3.SetActive(true);
            }
        }
        else
        {
            deActivated = true;
        }
    }

    public void Touch()
    {
        throw new NotImplementedException();
    }

    public void Finished()
    {
        playerController.CancelCurrentPuzzle();
        statue.ChangeSide(side);
        
        playerController.DettachHand();
        playerController.ChangeLookCloserState(true, false, true, new Vector2(-80, 80));
    
    }
}
