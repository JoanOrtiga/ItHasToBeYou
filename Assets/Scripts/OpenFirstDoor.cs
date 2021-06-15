using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenFirstDoor : MonoBehaviour , IAnimationTouch
{
    private PlayerController playerController;
    private Animator myAnimator;
    
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        playerController.SetCurrentPuzzle(this);
        playerController.DisableController(true,true,true, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            myAnimator.SetTrigger("P0 OpenDoor");
            playerController.AnimatorSetTrigger("P0 OpenDoor");
            this.enabled = false;
        }
    }

    public void Touch()
    {
        print("Nothing To touch");
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.7f);
        playerController.cameraController.ResetDesires();
        playerController.CancelCurrentPuzzle();
        myAnimator.enabled = false;
        playerController.transform.parent = null;
        playerController.EnableController(true,true,true, true);
    }
    
    public void Finished(int control)
    {
        //Control player parenting.
        StartCoroutine(CoolDown());
    }
}
