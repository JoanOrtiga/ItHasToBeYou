using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor1 : MonoBehaviour
{
    private int booksCounter = 0;
    [SerializeField] private int booksToOpen = 3;

    private Animator animator;

    public string openDoorPath;
    public string closeDoorPath;

    private PickUp pickUp;
    private void Awake()
    {
        pickUp = FindObjectOfType<PickUp>();
        
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            pickUp.CustomDrop();
            FMODUnity.RuntimeManager.PlayOneShot(openDoorPath, transform.position);
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
        }
    }

    public void AddBook()
    {
        booksCounter++;
        
        if (booksCounter >= booksToOpen)
        {
            pickUp.CustomDrop();
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
        }
    }
    
    public void SubstractBook()
    {
        booksCounter--;
    }
}
