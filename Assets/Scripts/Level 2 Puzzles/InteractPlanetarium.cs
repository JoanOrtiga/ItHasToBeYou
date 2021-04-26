using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractPlanetarium : MonoBehaviour, IInteractable
{
    public Transform viewCamara;
    public float transitionSpeed;

    public Transform playerCamara;
    private GameObject player;
    private GameObject cameraController;

    

    private bool activePuzzle = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        cameraController = player.transform.Find("Camera Controller").gameObject;
        playerCamara = player.transform.Find("Main Camera").gameObject.transform;

    }
    public void Interact()
    {
        activePuzzle = true;

    }
    void Update()
    {
        if (activePuzzle)
        {
            if (Input.GetButtonDown("Interact"))
            {
                activePuzzle = false;
                player.GetComponent<PlayerController>().enabled = true;
                cameraController.GetComponent<CameraController>().enabled = true;
            }

            player.GetComponent<PlayerController>().enabled = false;
            cameraController.GetComponent<CameraController>().enabled = false;

            playerCamara.position = Vector3.Lerp(playerCamara.position, viewCamara.position, Time.deltaTime * transitionSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(playerCamara.rotation.eulerAngles.x, viewCamara.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(playerCamara.rotation.eulerAngles.y, viewCamara.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(playerCamara.rotation.eulerAngles.z, viewCamara.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

            playerCamara.eulerAngles = currentAngle;
        }
    }
    


}
