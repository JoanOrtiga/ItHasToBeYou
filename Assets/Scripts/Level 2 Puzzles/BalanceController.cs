using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceController : MonoBehaviour, IInteractable
{

    private PlayerController playerController;
    public BreathCamera camera;
    public Transform puzzlePositionCam;
    public Transform initialPositionCam;
    public float transitionSpeed;


    private bool activeCameraTransition = false;
    //private bool activePuzzle;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && activeCameraTransition == false)
        {
            StartCoroutine(CamaraTransition(camera.transform, initialPositionCam, true));
        }
    }
    public void Interact()
    {
        initialPositionCam.position = camera.transform.position;
        initialPositionCam.rotation = camera.transform.rotation;


        StartCoroutine(CamaraTransition(camera.transform, puzzlePositionCam, true));
    }

    
    IEnumerator CamaraTransition(Transform pointA, Transform pointB, bool activePuzzle_)
    {
        
        while (Vector3.Distance(pointA.position, pointB.position) > 0.01f)
        {
            print("Moving Camera");
            activeCameraTransition = true;
            playerController.DisableController(true, true, true, true);

            pointA.position = Vector3.Lerp(pointA.position, pointB.position, Time.deltaTime * transitionSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(pointA.rotation.eulerAngles.x, pointB.rotation.eulerAngles.x,
                    Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.y, pointB.rotation.eulerAngles.y,
                    Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.z, pointB.rotation.eulerAngles.z,
                    Time.deltaTime * transitionSpeed));

            pointA.eulerAngles = currentAngle;
            yield return null;
        }

        
        
        activeCameraTransition = false;
        print("Stop transition camaraa");
        StopCoroutine("CamaraTransition");
    }

}
