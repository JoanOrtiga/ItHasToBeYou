using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticles : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private float timer;
    private void Start()
    {
        
        foreach (var particle in particleSystems)
        {
            particle.Stop();
        }
    }

    private void CreateParticles(float time)
    {
        timer = time;
        
        foreach (var particle in particleSystems)
        {
            particle.Play();
        }
    }

    private void StopEmmitting()
    {
        
        foreach (var particle in particleSystems)
        {
            particle.Stop();
        }
    }
}
