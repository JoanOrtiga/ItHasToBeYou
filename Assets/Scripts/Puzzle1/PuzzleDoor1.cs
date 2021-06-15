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

    public LupaPista lupaPista;

    private void Awake()
    {
        animator = GetComponent<Animator>();
     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            FMODUnity.RuntimeManager.PlayOneShot(openDoorPath, transform.position);
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
            lupaPista.pickUpLupa = false;

            gameObject.GetComponent<TextBox>().StartTextPuzzle();
        }
    }

    public void AddBook()
    {
        

        booksCounter++;
        
        if (booksCounter >= booksToOpen)
        {
            FMODUnity.RuntimeManager.PlayOneShot(openDoorPath, transform.position);
            animator.SetTrigger("OpenDoor");
            this.enabled = false;
            lupaPista.pickUpLupa = false;
            gameObject.GetComponent<TextBox>().StartTextPuzzle();
        }
    }
    
    public void SubstractBook()
    {
        booksCounter--;
    }
}
