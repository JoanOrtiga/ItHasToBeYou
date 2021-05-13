using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate3rFloor : MonoBehaviour
{
    public GameObject floor1;
    public GameObject floor2;
    public GameObject floor3;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            floor3.SetActive(true);
            floor1.SetActive(false);
        }
    }
}
