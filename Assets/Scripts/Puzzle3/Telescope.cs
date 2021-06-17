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

    public string interactSoundPath = "event:/INGAME/Puzzle 3/Telescopi/InteractTelescopi";
    [SerializeField] private Crosshair crosshair;

    private CanvasTutorial canvasTutorial;

    [SerializeField] private DialsController dialsController;
    
    private void Start()
    {

        crosshair = FindObjectOfType<Crosshair>();
        canvasTutorial = FindObjectOfType<CanvasTutorial>();
        mainCamera = Camera.main;
        
        _playerController = FindObjectOfType<PlayerController>();
        _cameraController = GetComponentInChildren<TelescopeRotation>();
    }

    public void Interact()
    {
        if(!this.enabled)
            return;
        
        FMODUnity.RuntimeManager.PlayOneShot(interactSoundPath, gameObject.transform.position);
        canvasTutorial.TutorialPuzzle32(true);

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
            crosshair.ChangeCrosshairState(false, false);

            if (Input.GetButtonDown("Interact") && cancelCooldown)
            {
                FMODUnity.RuntimeManager.PlayOneShot(interactSoundPath, gameObject.transform.position);
                canvasTutorial.TutorialPuzzle32(false);

                Cancel();
            }

            Vector3 rotation = pivotCamera.rotation.eulerAngles;

            coordsX.text = Mathf.RoundToInt(rotation.x).ToString();
            coordsY.text = Mathf.RoundToInt(rotation.y).ToString();
        }

        if (dialsController.Solved())
        {
            this.enabled = false;
            Destroy(this);
        }
    }

    private void Cancel()
    {
        crosshair.ChangeCrosshairState(false, false);
        _playerController.EnableController(true, true, true,true);
        telescopeCanvas.SetActive(false);
        
        active = false;

        _cameraController.enabled = false;
        telescopeCamera.enabled = false;
        mainCamera.enabled = true;
        telescopeCanvas.SetActive(false);
    }

    private void LateUpdate()
    {
        if (active)
        {
            crosshair.ChangeCrosshairState(false, false);
        }
    }
}
