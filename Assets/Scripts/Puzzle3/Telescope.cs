using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telescope : MonoBehaviour , IInteractable
{
    private PlayerController _playerController;
    
    private bool active = false;

    private bool cancelCooldown = false;

    private TelescopeRotation _cameraController;

    private Camera mainCamera;
    [SerializeField] private Camera telescopeCamera;

    [SerializeField] private GameObject telescopeCanvas;
    [SerializeField] private Transform pivotCamera;

    [SerializeField] private Text coordsX;
    [SerializeField] private Text coordsY;
    private void Start()
    {
        mainCamera = Camera.main;
        
        _playerController = FindObjectOfType<PlayerController>();
        _cameraController = GetComponentInChildren<TelescopeRotation>();
    }

    public void Interact()
    {
        active = true;
        _playerController.DisableController(true, true, true);
        cancelCooldown = false;
        StartCoroutine(Cooldown());

        _cameraController.enabled = true;
        telescopeCamera.enabled = true;
        mainCamera.enabled = false;
        telescopeCanvas.SetActive(true);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.1f);
        cancelCooldown = true;
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetButtonDown("Interact") && cancelCooldown)
            {
               Cancel();
            }

            Vector3 rotation = pivotCamera.rotation.eulerAngles;

            coordsX.text = rotation.x.ToString();
            coordsY.text = rotation.y.ToString();
        }
    }

    private void Cancel()
    {
        _playerController.EnableController(true, true, true);
        telescopeCanvas.SetActive(false);
        
        active = false;

        _cameraController.enabled = false;
        telescopeCamera.enabled = false;
        mainCamera.enabled = true;
        telescopeCanvas.SetActive(false);
    }
}
