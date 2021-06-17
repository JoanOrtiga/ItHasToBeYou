using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class DialsController : MonoBehaviour, IInteractable, IPuzzleSolver
{
    private PlayerController playerController;
    private Camera mainCamera;

    private bool active;


    [SerializeField] private Transform cameraDial1;
    [SerializeField] private Transform cameraDial2;
    [SerializeField] private Transform dial1;
    [SerializeField] private Transform dial2;

    [SerializeField] private float cameraSpeed = 1.0f;
    [SerializeField] private float maxDistance = 0.01f;
    [SerializeField] private float dialSpeed = 10f;
    [SerializeField] private Vector2 dial1Range;
    [SerializeField] private Vector2 dial2Range;

    private bool dial1Correct = false;
    private bool dial2Correct = false;
    
    private Vector3 targetRotation;
    private bool rotating;

    [SerializeField] private float angleAtATime = 10f;

    public string clickSoundPath = "event:/INGAME/Puzzle 3/Dials/Dials";
    public string selectSoundPath = "event:/INGAME/Puzzle 3/Dials/SelectDials";
    private float timeSound;

    private bool audioOnce = false;

    private CanvasTutorial canvasTutorial;

    [SerializeField] private float transitionSpeed = 5f;
    private bool transitioning = false;

    private Transform savePosition;

    [SerializeField] private GameObject popUpInteraction;
    public TextBox narrativeTextBox;
    private bool narrativeDone;

    public GameObject switches;
    
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
        canvasTutorial = FindObjectOfType<CanvasTutorial>();
        playerController = FindObjectOfType<PlayerController>();
        savePosition = new GameObject().transform;
    }


    public void Interact()
    {
        if(!this.enabled)
            return;
        
        savePosition.position = playerController.mainCamera.transform.position;
        savePosition.rotation = playerController.mainCamera.transform.rotation;

        switch (state)
        {
            case DialState.dial1:
                StartCoroutine(CameraTransition(playerController.mainCamera.transform, cameraDial1.transform, true));
                break;
            case DialState.dial2:
                StartCoroutine(CameraTransition(playerController.mainCamera.transform, cameraDial2.transform, true));
                break;
            case DialState.transitioningDial1:
                StartCoroutine(CameraTransition(playerController.mainCamera.transform, cameraDial1.transform, true));

                break;
            case DialState.transitioningDial2:
                StartCoroutine(CameraTransition(playerController.mainCamera.transform, cameraDial2.transform, true));
                break;
        }
        
        canvasTutorial.TutorialPuzzle33(true);
       
      /*  playerController.DisableController(true, true, true);
        active = true;
        dialsCamera.enabled = true;
        mainCamera.enabled = false;
*/
    }

    private void Update()
    {
        if (!active)
            return;
        
        if (transitioning)
            return;
        
        timeSound += Time.deltaTime;


        if (Input.GetButtonDown("Interact"))
        {
            StartCoroutine(CameraTransition(playerController.mainCamera.transform, savePosition, false));
            
            canvasTutorial.TutorialPuzzle33(false);
        }

        float direction = Input.GetAxisRaw("Vertical");

        if (direction != 0)
        {
            if (timeSound > 0.25f)
            {
                FMODUnity.RuntimeManager.PlayOneShot(clickSoundPath, gameObject.transform.position);
                timeSound = 0;
            }
           
        }

        switch (state)
        {
            case DialState.dial1:
                if (Input.GetAxisRaw("Horizontal") >= 0.3f)
                {
                    if (rotating)
                    {
                        dial1.localRotation= Quaternion.Euler(targetRotation);
                        rotating = false;
                    }
                    state = DialState.transitioningDial2;
                }

                RotateDial(dial1, direction);
                break;
            case DialState.dial2:

                if (Input.GetAxisRaw("Horizontal") <= -0.3f)
                {
                    if (rotating)
                    {
                        dial2.localRotation= Quaternion.Euler(targetRotation);
                        rotating = false;
                    }
                    
                    state = DialState.transitioningDial1;
                }

                RotateDial(dial2, direction);
                break;
            case DialState.transitioningDial1:
                if (audioOnce == true)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(selectSoundPath, gameObject.transform.position);

                    audioOnce = false;
                }
                
                playerController.mainCamera.transform.position = Vector3.Lerp(playerController.mainCamera.transform.position,
                    cameraDial1.transform.position, cameraSpeed * Time.deltaTime);

                if ((playerController.mainCamera.transform.position - cameraDial1.transform.position).sqrMagnitude <
                    maxDistance * maxDistance)
                {
                    state = DialState.dial1;
                }

                RotateDial(dial1, direction);

                if (Input.GetAxisRaw("Horizontal") >= 0.3f)
                {
                    if (rotating)
                    {
                        dial1.localRotation= Quaternion.Euler(targetRotation);
                        rotating = false;
                    }
                    state = DialState.transitioningDial2;
                }
                break;
            
            case DialState.transitioningDial2:

                if (audioOnce == false)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(selectSoundPath, gameObject.transform.position);

                    audioOnce = true;
                }
                
                playerController.mainCamera.transform.position = Vector3.Lerp(playerController.mainCamera.transform.position,
                    cameraDial2.transform.position, cameraSpeed * Time.deltaTime);

                if ((playerController.mainCamera.transform.position - cameraDial2.transform.position).sqrMagnitude <
                    maxDistance * maxDistance)
                {
                    state = DialState.dial2;
                }

                RotateDial(dial2, direction);

                if (Input.GetAxisRaw("Horizontal") <= -0.3f)
                {
                    if (rotating)
                    {
                        dial2.localRotation= Quaternion.Euler(targetRotation);
                        rotating = false;
                    }
                    state = DialState.transitioningDial1;
                }
                break;
        }

        Vector2 dialRotation = new Vector2(dial1.localRotation.eulerAngles.z, dial2.localRotation.eulerAngles.z);
        
        
        if (dialRotation.x >= dial1Range.x && dialRotation.x <= dial1Range.y)
        {
            dial1Correct = true;
        }
        else
        {
            dial1Correct = false;
        }

        
        if (dialRotation.y >= dial2Range.x && dialRotation.y <= dial2Range.y)
        {
            dial2Correct = true;
        }
        else
        {
            dial2Correct = false;
        }

        if (dial1Correct && dial2Correct)
        {
            StartCoroutine(CameraTransition(playerController.mainCamera.transform, savePosition, false));
            popUpInteraction.SetActive(false);
            canvasTutorial.TutorialPuzzle33(false);
            if (narrativeDone == false)
            {
                narrativeTextBox.StartTextPuzzle();
                narrativeDone = true;
            }
            //
            switches.SetActive(true);
            this.enabled = false;
        }
    }

    private void RotateDial(Transform dial, float direction)
    {
       // dial.Rotate(new Vector3(0, direction * dialSpeed * Time.deltaTime, 0));

       if (!rotating)
       {
           if (direction != 0)
           {
               rotating = true;
               targetRotation = dial.localRotation.eulerAngles;
               targetRotation.z += direction * angleAtATime;
           }
           
       }
       else
       {
           dial.localRotation = Quaternion.RotateTowards(dial.localRotation, Quaternion.Euler(targetRotation), dialSpeed * Time.deltaTime);

           if (dial.localRotation == Quaternion.Euler(targetRotation))
           {
               dial.localRotation = Quaternion.Euler(targetRotation);
               rotating = false;
           }
       }
        
    }

    public bool Solved()
    {
        return dial1Correct && dial2Correct;
    }
    
    IEnumerator CameraTransition(Transform pointA, Transform pointB, bool activePuzzle_)
    {
        playerController.DisableController(true, true, true, true);
        active = activePuzzle_;
        transitioning = true;
        
        while (Vector3.Distance(pointA.position, pointB.position) > 0.005f)
        {
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

        if (!activePuzzle_)
        {
            pointA.localPosition = Vector3.zero;
            pointA.rotation = pointB.rotation;
            playerController.EnableController(true, true, true, true);
            active = false;
        }
  
        transitioning = false;
        
        StopCoroutine("CamaraTransition");
    }
}