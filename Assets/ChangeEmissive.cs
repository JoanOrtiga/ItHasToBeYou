using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEmissive : MonoBehaviour
{
    public Material material;
    public Transform referencePoint;

    private PlayerController playerController;

    private bool onRange = false;

    public AnimationCurve distance;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if(!onRange)
            return;

        material.SetColor("_EmissionColor", Color.white * distance.Evaluate((playerController.transform.position - referencePoint.position).magnitude));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onRange = true;
        //material.SetColor("_EmissionColor", Color.white * 2f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            onRange = false;
        //material.SetColor("_EmissionColor", Color.black);
    }
}
