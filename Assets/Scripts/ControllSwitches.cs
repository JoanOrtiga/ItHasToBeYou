using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ControllSwitches : MonoBehaviour, IInteractable
{
    [SerializeField] private Camera camera;

    private bool active;

    private PlayerController playerController;

    [SerializeField] private Switches[] switches;
    [SerializeField] private Renderer[] woods;
    private Material[] woodsMaterial;

    private int position = 1;

    [SerializeField] private float speed = 1f;

    private bool transitioning = false;

    private GameObject saveCameraPosition;

    public Crosshair crosshair;
    private void Awake()
    {
        crosshair = FindObjectOfType<Crosshair>();
        saveCameraPosition = new GameObject();
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
        if (Solved())
            return;
        
        if(transitioning)
            return;
        
        if(active)
            return;
        
        saveCameraPosition.transform.position =playerController.mainCamera.transform.position;
        saveCameraPosition.transform.rotation = playerController.mainCamera.transform.rotation;
        
        StartCoroutine(CamaraTransition(playerController.mainCamera.transform, camera.transform, true));

      /*  active = true;
        playerController.DisableController(true, true, true, true);
        camera.enabled = true;
        playerController.mainCamera.enabled = false;*/
        woodsMaterial[position].EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        if (!active)
            return;

        if (Solved())
            return;
        
        if(transitioning)
            return;
        

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (position >= 2)
                return;

            position++;
            woodsMaterial[position - 1].DisableKeyword("_EMISSION");
            woodsMaterial[position].EnableKeyword("_EMISSION");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (position <= 0)
                return;

            position--;
            woodsMaterial[position + 1].DisableKeyword("_EMISSION");
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
            StartCoroutine(CamaraTransition(playerController.mainCamera.transform, saveCameraPosition.transform, false));

            active = false;

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
            
            
            StartCoroutine(CamaraTransition(playerController.mainCamera.transform, saveCameraPosition.transform, false));

            active = false;

            woodsMaterial[0].DisableKeyword("_EMISSION");
            woodsMaterial[1].DisableKeyword("_EMISSION");
            woodsMaterial[2].DisableKeyword("_EMISSION");

            gameObject.tag = "Untagged";
            this.enabled = false;
        }

        return solved;
    }

    IEnumerator CamaraTransition(Transform pointA, Transform pointB, bool activePuzzle_)
    {
        playerController.DisableController(true, true, true, true);
        crosshair.ChangeCrosshairState(false,false);
        active = activePuzzle_;
        transitioning = true;
        
        while (Vector3.Distance(pointA.position, pointB.position) > 0.005f)
        {
            pointA.position = Vector3.Lerp(pointA.position, pointB.position, Time.deltaTime * speed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(pointA.rotation.eulerAngles.x, pointB.rotation.eulerAngles.x,
                    Time.deltaTime * speed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.y, pointB.rotation.eulerAngles.y,
                    Time.deltaTime * speed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.z, pointB.rotation.eulerAngles.z,
                    Time.deltaTime * speed));

            pointA.eulerAngles = currentAngle;
            yield return null;
        }

        if (!activePuzzle_)
        {
            pointA.localPosition = Vector3.zero;
            pointA.rotation = pointB.rotation;
            playerController.EnableController(true, true, true, true);
            crosshair.ChangeCrosshairState(true,true);
        }

        transitioning = false;
        
        StopCoroutine("CamaraTransition");
    }

    private void LateUpdate()
    {
        if(active)
            crosshair.ChangeCrosshairState(false,false);
    }
}