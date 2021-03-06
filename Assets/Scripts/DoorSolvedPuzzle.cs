using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSolvedPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject puzzleController;

    private IPuzzleSolver puzzle;
    
    private Animator animator;
    [SerializeField] private ActivateParticles activateParticles;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        puzzle = puzzleController.GetComponent<IPuzzleSolver>();
    }

    private void Update()
    {
        if(puzzle.Solved())
        {
            animator.SetTrigger("OpenDoor");
            activateParticles.CreateParticles(5f);
            this.enabled = false;
        }
    }
}
