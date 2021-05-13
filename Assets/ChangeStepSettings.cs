using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStepSettings : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().stepOffset = 0.5f;
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
