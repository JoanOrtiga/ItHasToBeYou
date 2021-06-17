using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissive : MonoBehaviour
{
    public Material material;

    private void OnTriggerEnter(Collider other)
    {
        material.SetColor("_EmissionColor", Color.white * 2f);
    }

    private void OnTriggerExit(Collider other)
    {
        material.SetColor("_EmissionColor", Color.black);
    }
}
