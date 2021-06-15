using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OpenFirstDoor : MonoBehaviour , IAnimationTouch
{
    private PlayerController playerController;
    private Animator myAnimator;

    [SerializeField] private GameObject letter;

    public bool destroyLetter = false;
    private bool running = false;
    
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        playerController.SetCurrentPuzzle(this);
        playerController.DisableController(true,true,true, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !running)
        {
            myAnimator.SetTrigger("P0 OpenDoor");
            playerController.AnimatorSetTrigger("P0 OpenDoor");
            running = true;
        }

        if (Input.GetKeyDown(KeyCode.E) && destroyLetter)
        {
            playerController.AnimatorSetBool("ReadLetter", false);
        }
    }

    public void Touch()
    {
        print("Nothing To touch");
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(1f);
        destroyLetter = true;
    }
    
    public void Finished(int control)
    {
        if (control == 2)
        {
            playerController.cameraController.ResetDesires();
            playerController.CancelCurrentPuzzle();
            myAnimator.enabled = false;
            playerController.transform.parent = null;
            playerController.EnableController(true,true,true, true);
            Destroy(letter, 0.2f);
            this.enabled = false;

            return;
        }
        
        playerController.AnimatorSetBool("ReadLetter", true);
        letter.SetActive(true);
        StartCoroutine(CoolDown());
    }
}
