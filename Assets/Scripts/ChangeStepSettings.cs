using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStepSettings : MonoBehaviour
{
    [SerializeField] private float stepOffset = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().stepOffset = stepOffset;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().stepOffset = 0.2f;
        }
    }
}
