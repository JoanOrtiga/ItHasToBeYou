using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialsController : MonoBehaviour, IInteractable
{
    private PlayerController _playerController;
    private Camera mainCamera;

    private bool active;

    [SerializeField] private Camera dialsCamera;

    private bool cooldown;

    [SerializeField] private Transform cameraDial1;
    [SerializeField] private Transform cameraDial2;
    [SerializeField] private Transform dial1;
    [SerializeField] private Transform dial2;

    [SerializeField] private float cameraSpeed = 1.0f;
    [SerializeField] private float maxDistance = 0.01f;
    private enum DialState
    {
        transitioningDial2,
        transitioningDial1,
        dial1,
        dial2
    }

    private DialState state = DialState.dial1;

    private void Awake()
    {
        mainCamera = Camera.main;
        _playerController = FindObjectOfType<PlayerController>();
    }


    public void Interact()
    {
        _playerController.DisableController();
        active = true;
        dialsCamera.enabled = true;
        mainCamera.enabled = false;

        cooldown = false;

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.1f);
        cooldown = true;
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetButtonDown("Interact") && cooldown)
            {
                _playerController.EnableController();
                mainCamera.enabled = true;
                dialsCamera.enabled = false;
                active = false;
            }

            switch (state)
            {
                case DialState.dial1:
                    if (Input.GetAxisRaw("Horizontal") <= -0.3f)
                    {
                        state = DialState.transitioningDial2;
                    }
                    break;
                case DialState.dial2:
                    if (Input.GetAxisRaw("Horizontal") >= 0.3f)
                    {
                        state = DialState.transitioningDial1;
                    }
                    break;
                case DialState.transitioningDial1:
                    dialsCamera.transform.localPosition += Vector3.Lerp(dialsCamera.transform.localPosition, cameraDial1.transform.localPosition, cameraSpeed * Time.deltaTime);

                    if ((dialsCamera.transform.position - cameraDial1.transform.position).sqrMagnitude < maxDistance * maxDistance)
                    {
                        state = DialState.dial1;    
                    }
                    break;
                case DialState.transitioningDial2:
                    dialsCamera.transform.localPosition += Vector3.Lerp(dialsCamera.transform.localPosition, cameraDial2.transform.localPosition, cameraSpeed * Time.deltaTime);
                    
                    if ((dialsCamera.transform.position - cameraDial2.transform.position).sqrMagnitude < maxDistance * maxDistance)
                    {
                        state = DialState.dial2;
                    }
                    break;
            }
        }
    }
}