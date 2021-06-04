using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesPuzzle : MonoBehaviour , IPuzzleSolver
{
    [SerializeField] private Switches[] allSwitchesMid;

    [SerializeField] private Animator animator;
    private Animator myAnimator;
    private bool x = true;
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Solved() || Input.GetKeyDown(KeyCode.B))
        {
            if (!x)
                return;
            myAnimator.enabled = true;
            myAnimator.SetTrigger("Rotate");
            animator.SetTrigger("AlignToStars");
            this.enabled = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 3/Escaleras Elevandose/Escalera", this.gameObject.transform.position);


        }
    }

    public bool Solved()
    {
        for (int i = 0; i < allSwitchesMid.Length; i++)
        {
            if (!allSwitchesMid[i].Solved())
                return false;
        }
        
        return true;
    }

  
}