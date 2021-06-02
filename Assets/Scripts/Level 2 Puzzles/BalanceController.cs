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

    public GameObject[] canvas;
    public GameObject[] balancePlace;


    private bool activeCameraTransition = false;
    public int indexCanvas = 0;
    //private bool activePuzzle;
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }

        canvas[0].SetActive(true);
    }

    private void Update()
    {
        if (activeCameraTransition == false)
        {
            if (Input.GetButtonDown("Interact"))
            {
                StartCoroutine(CamaraTransition(camera.transform, initialPositionCam, true));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                indexCanvas -= 1;
                if (indexCanvas < 0) indexCanvas = 4;
               
                for (int i = 0; i < canvas.Length; i++)
                {
                    canvas[i].SetActive(false);
                }
                canvas[indexCanvas].SetActive(true);


            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                indexCanvas += 1;
                if (indexCanvas > 4) indexCanvas = 0;

                for (int i = 0; i < canvas.Length; i++)
                {
                    canvas[i].SetActive(false);
                }
                canvas[indexCanvas].SetActive(true);


            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                


            }



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
