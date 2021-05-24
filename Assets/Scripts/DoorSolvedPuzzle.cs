using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSolvedPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject puzzleController;

    private IPuzzleSolver puzzle;
    
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        puzzle = puzzleController.GetComponent<IPuzzleSolver>();
    }

    private void Update()
    {
        if(puzzle.Solved() || Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
        }
    }
}
