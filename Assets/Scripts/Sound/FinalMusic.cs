using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalMusic : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("hola");
            FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/Final_joc");
            Destroy(gameObject);
        }
    }
    
    
}
