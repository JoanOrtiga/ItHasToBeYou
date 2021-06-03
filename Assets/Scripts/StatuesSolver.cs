using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatuesSolver : MonoBehaviour
{
    [SerializeField] private MovingStatue[] movingStatues;

    [SerializeField] private Transform die;
 
    [SerializeField] private Animator animator;

    [SerializeField] private Transform centralPoint;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float radius;

    private bool x = true;
    private void Update()
    {
        bool solved = true;
        foreach (var statue in movingStatues)
        {
            if (!statue.Solved())
            {
                solved = false;
            }
        }

        if (solved || Input.GetKeyDown(KeyCode.L))
        {
            if ((playerTransform.position - centralPoint.position).sqrMagnitude > radius * radius)
            {
                if (!x)
                    return;
                animator.SetTrigger("OpenDoor");
                StartCoroutine(Die());
                x = false;
                FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 3/Escaleras Elevandose/Escalera", this.gameObject.transform.position);

            }
            
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(centralPoint != null)
            Gizmos.DrawWireSphere(centralPoint.position, radius);
    }

    private IEnumerator Die()
    {
        for (int i = 0; i < 500; i++)
        {
            die.position = die.position - (Vector3.up * Time.deltaTime);
            yield return null;
        }

        this.enabled = false;

    }
}
