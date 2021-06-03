using System;
using System.Collections;
using System.Collections.Generic;
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
        playerController.DisableController(true,true,true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            myAnimator.SetTrigger("P0 OpenDoor");
            playerController.AnimatorSetTrigger("P0 OpenDoor");
            //this.enabled = false;
        }
    }

    public void Touch()
    {
        print("Nothing To touch");
    }

    public void Finished()
    {
        //Control player parenting.
        playerController.CancelCurrentPuzzle();
        playerController.transform.parent = null;
        playerController.EnableController(true,true,true);

    }
}
