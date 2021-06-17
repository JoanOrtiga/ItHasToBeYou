using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor1 : MonoBehaviour
{
    private int booksShelfCounter = 0;
    [SerializeField] private int bookShelfsToOpen = 4;

    private Animator animator;

    public string openDoorPath;
    
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

    public void AddBookShelf()
    {
        booksShelfCounter++;

        if (booksShelfCounter >= bookShelfsToOpen)
        {
            pickUp.CustomDrop();
            FMODUnity.RuntimeManager.PlayOneShot(openDoorPath, transform.position);
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
        }
    }
}
