using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnloader : MonoBehaviour
{
    public GameObject level;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            level.SetActive(true);
        }
    }
}
