using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticles : MonoBehaviour
{
    private ParticleSystem[] particleSystems;

    private float timer;
    private bool activated = false;
    private void Start()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        
        foreach (var particle in particleSystems)
        {
            particle.Stop();
        }
    }

    public void CreateParticles(float time)
    {
        timer = time;
        activated = true;
        
        foreach (var particle in particleSystems)
        {
            particle.Play();
        }
    }

    private void Update()
    {
        if (activated)
        {
            if (timer <= 0)
            {
                StopEmmitting();
                activated = false;
            }
            
            timer -= Time.deltaTime;
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
