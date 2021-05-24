using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor1 : MonoBehaviour
{
    private int booksCounter = 0;
    [SerializeField] private int booksToOpen = 3;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
        }
    }

    public void AddBook()
    {
        booksCounter++;

        if (booksCounter >= booksToOpen)
        {
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
        }
    }
    
    public void SubstractBook()
    {
        booksCounter--;
    }
}
