using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePastDoor : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("CloseDoor");
            this.enabled = false;
        }
    }
}
