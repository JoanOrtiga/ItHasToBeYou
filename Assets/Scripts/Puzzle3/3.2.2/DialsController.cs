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

    [SerializeField] private Camera dialsCamera;

    private bool cooldown;

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

    public string clickSoundPath;
    private float timeSound;


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
        playerController = FindObjectOfType<PlayerController>();
    }


    public void Interact()
    {
        
       
        playerController.DisableController(true, true, true);
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

        timeSound += Time.deltaTime;

        if (!active)
            return;

        if (Input.GetButtonDown("Interact") && cooldown)
        {
            playerController.EnableController(true, true, true);
            mainCamera.enabled = true;
            dialsCamera.enabled = false;
            active = false;
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
               
                dialsCamera.transform.localPosition = Vector3.Lerp(dialsCamera.transform.localPosition,
                    cameraDial1.transform.localPosition, cameraSpeed * Time.deltaTime);

                if ((dialsCamera.transform.position - cameraDial1.transform.position).sqrMagnitude <
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
                
                dialsCamera.transform.localPosition = Vector3.Lerp(dialsCamera.transform.localPosition,
                    cameraDial2.transform.localPosition, cameraSpeed * Time.deltaTime);

                if ((dialsCamera.transform.position - cameraDial2.transform.position).sqrMagnitude <
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

        Vector2 dialRotation = new Vector2(dial1.rotation.eulerAngles.x, dial2.rotation.eulerAngles.x);

        if (dialRotation.x >= dial1Range.x && dialRotation.x <= dial1Range.y)
        {
            dial1Correct = true;
        }
        else
        {
            dial1Correct = false;
        }

        if ((dialRotation.y >= dial2Range.x && dialRotation.y <= 360) ||
            (dialRotation.y >= 0 && dialRotation.y <= dial2Range.y))
        {
            dial2Correct = true;
        }
        else
        {
            dial2Correct = false;
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
}