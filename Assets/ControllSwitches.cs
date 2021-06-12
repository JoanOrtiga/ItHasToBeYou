using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ControllSwitches : MonoBehaviour , IInteractable
{
    [SerializeField] private Camera camera;
    
    private bool active;

    private PlayerController playerController;

    [SerializeField] private Switches[] switches;
    [SerializeField] private Renderer[] woods;
    private Material[] woodsMaterial;
    
    private int position = 1;

    [SerializeField] private float speed = 1f;
    
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        woodsMaterial = new Material[3];
        woodsMaterial[0] = woods[0].material;
        woodsMaterial[1] = woods[1].material;
        woodsMaterial[2] = woods[2].material;
        
        woodsMaterial[0].SetColor("_EmissionColor", Color.yellow);
        woodsMaterial[1].SetColor("_EmissionColor", Color.yellow);
        woodsMaterial[2].SetColor("_EmissionColor", Color.yellow);
    }

    public void Interact()
    {
        active = true;
        playerController.DisableController(true,true,true,true);
        camera.enabled = true;
        playerController.mainCamera.enabled = false;
        woodsMaterial[position].EnableKeyword("_EMISSION");

    }

    private void Update()
    {
        if(!active)
            return;

        if (Solved())
            return;
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(position >= 2)
                return;
            
            position++;
            woodsMaterial[position-1].DisableKeyword("_EMISSION");
            woodsMaterial[position].EnableKeyword("_EMISSION");

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if(position <= 0)
                return;
            
            position--;
            woodsMaterial[position+1].DisableKeyword("_EMISSION");
            woodsMaterial[position].EnableKeyword("_EMISSION");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            switches[position].GoDown(true);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            switches[position].GoUp(true);
        }

        if (Input.GetButtonDown("Interact"))
        {
            active = false;
            playerController.EnableController(true,true,true,true);
            playerController.mainCamera.enabled = true;
            camera.enabled = false;
            
            woodsMaterial[0].DisableKeyword("_EMISSION");
            woodsMaterial[1].DisableKeyword("_EMISSION");
            woodsMaterial[2].DisableKeyword("_EMISSION");
        }
    }

    private bool Solved()
    {
        bool solved = true;
        foreach (var sw in switches)
        {
            if (!sw.Solved())
            {
                solved = false;
            }
        }

        if (solved)
        {
            this.enabled = false;
            active = false;
            playerController.EnableController(true,true,true,true);
            playerController.mainCamera.enabled = true;
            camera.enabled = false;
            
            woodsMaterial[0].DisableKeyword("_EMISSION");
                        woodsMaterial[1].DisableKeyword("_EMISSION");
                        woodsMaterial[2].DisableKeyword("_EMISSION");
        }

        return solved;
    }
}
