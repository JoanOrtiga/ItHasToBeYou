using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractPlanetarium : MonoBehaviour, IInteractable
{
    public Transform viewCamara;
    public float transitionSpeed;
    public Material SelectedMat;
    public Material NormalMat;

    public Transform playerCamara;
    private GameObject player;
    private GameObject cameraController;

    private int interactingRing;
    private float time;

    private bool activePuzzle = false;

    public GameObject puzzle;
    private Transform ringZero;
    private Transform ringOne;
    private Transform ringTwo;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        cameraController = player.transform.Find("Camera Controller").gameObject;

        ringZero = puzzle.transform.GetChild(0);
        ringOne= puzzle.transform.GetChild(1);
        ringTwo = puzzle.transform.GetChild(2);
        

    }
    public void Interact()
    {
        activePuzzle = true;

    }


    void Update()
    {
       

        if (activePuzzle)
        {
            CameraTransition();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                activePuzzle = false;
                player.GetComponent<PlayerController>().enabled = true;
                cameraController.GetComponent<CameraController>().enabled = true;
                print("Exit game");
            }

            
            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Gira derecha
            {
                RotateRing(0);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                RotateRing(1);            
            }

            if (Input.GetButtonDown("Interact"))
            {
                interactingRing++;
                if (interactingRing >= 3)
                {
                    interactingRing = 0;
                }
                
                ColorMat();
            }
           

           
        }
    }

    private void RotateRing(float rotationWay) // 0 Derecha & 1 Izquierda
    {
        switch (interactingRing)
        {
            case 0:
               
                if (rotationWay == 0)
                {
                    print("Muevo 1 a la derecha");
                    
                    ringZero.transform.Rotate(0, 90, 0, Space.Self);
                    ringOne.transform.Rotate(0, 30, 0, Space.Self);
                }
                else
                {
                    print("Muevo 1 a la izquierda");
                    ringZero.transform.Rotate(0, -90, 0, Space.Self);
                    ringOne.transform.Rotate(0, -30, 0, Space.Self);
                }
                break;

            case 1:
               
                if (rotationWay == 0)
                {
                    print("Muevo 1 i 2 a la derecha");
                    ringZero.transform.Rotate(0, 30, 0, Space.Self);
                    ringOne.transform.Rotate(0, 90, 0, Space.Self);
                    ringTwo.transform.Rotate(0, 30, 0, Space.Self);
                }
                else
                {
                    print("Muevo 1 i 2 a la derecha");
                    ringZero.transform.Rotate(0, -30, 0, Space.Self);
                    ringOne.transform.Rotate(0, -90, 0, Space.Self);
                    ringTwo.transform.Rotate(0, -30, 0, Space.Self);
                }
                break;

            case 2:

                if (rotationWay == 0)
                {
                    ringZero.transform.Rotate(0, 10, 0, Space.Self);
                    ringOne.transform.Rotate(0, 30, 0, Space.Self);
                    ringTwo.transform.Rotate(0, 90, 0, Space.Self);
                }
                else
                {
                    ringZero.transform.Rotate(0, -10, 0, Space.Self);
                    ringOne.transform.Rotate(0, -30, 0, Space.Self);
                    ringTwo.transform.Rotate(0, -90, 0, Space.Self);
                }
                break;

            default:
                break;
        }
    }

    private void ColorMat()
    {
        switch (interactingRing)
        {
            case 0:
                ringZero.GetComponent<MeshRenderer>().material = SelectedMat;
                ringOne.GetComponent<MeshRenderer>().material = NormalMat;
                ringTwo.GetComponent<MeshRenderer>().material = NormalMat;
                break;

            case 1:
                ringZero.GetComponent<MeshRenderer>().material = NormalMat;
                ringOne.GetComponent<MeshRenderer>().material = SelectedMat;
                ringTwo.GetComponent<MeshRenderer>().material = NormalMat;
                break;


            case 2:
                ringZero.GetComponent<MeshRenderer>().material = NormalMat;
                ringOne.GetComponent<MeshRenderer>().material = NormalMat;
                ringTwo.GetComponent<MeshRenderer>().material = SelectedMat;
                break;

            default:
                break;
        }
    }
    private void CameraTransition()
    {
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
