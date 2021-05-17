using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSolvedPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject puzzleController;

    private IPuzzleSolver puzzle;
    
    private Animator animator;

    private bool opened = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        puzzle = puzzleController.GetComponent<IPuzzleSolver>();
        
        
    }

    private void Update()
    {
        if(puzzle.Solved() && opened is false)
        {
            animator.SetTrigger("OpenDoor");
            opened = true;
        }
    }
}
