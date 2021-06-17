using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNextLevel : MonoBehaviour
{
    public GameObject level;


    private void OnTriggerEnter(Collider other)
    {
        level.SetActive(true);
    }
}
