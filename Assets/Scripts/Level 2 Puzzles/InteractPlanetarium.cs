using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractPlanetarium : MonoBehaviour, IInteractable
{
    [SerializeField]private Transform viewCamara;
    [SerializeField]private float transitionSpeed;
    [SerializeField]private Material SelectedMat;
    [SerializeField]private Material NormalMat;
    
    [SerializeField]private Transform playerCamara;
    private GameObject player;
    private GameObject cameraController;
    [SerializeField]private Transform initialPositionCam;
    private int interactingRing = 3;
    private float time;

    private bool activePuzzle = false;
    private bool activeCameraTransition = false;
    private bool onTrigger = false;

    [SerializeField]private GameObject puzzle;
    private Transform ringZero;
    private Transform ringOne;
    private Transform ringTwo;

    public bool[] allClues;

    public int degreeBig = 90;
    public int degreeSmall = 45;
    private void Start()
    {
        allClues = new bool[3];
        player = FindObjectOfType<PlayerController>().gameObject;
        cameraController = player.transform.Find("Camera Controller").gameObject;
        

        ringZero = puzzle.transform.GetChild(0);
        ringOne= puzzle.transform.GetChild(1);
        ringTwo = puzzle.transform.GetChild(2);
        

    }
    public void Interact()
    {
       
        initialPositionCam.position = playerCamara.position;
        initialPositionCam.rotation = playerCamara.rotation;

        StartCoroutine(CamaraTransition(playerCamara, viewCamara,false));
        activePuzzle = true;

    }

  

    void Update()
    {
       

        if (activePuzzle)
        {
            if (Input.GetButtonDown("QuitInteract") && activeCameraTransition == false)
            {
                activePuzzle = false;
                
                StartCoroutine(CamaraTransition(playerCamara, initialPositionCam,true));
                player.GetComponent<PlayerController>().enabled = true;
                cameraController.GetComponent<CameraController>().enabled = true;

            }

            
            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Gira derecha
            {
                print("MOUSE");
                RotateRing(0);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                print("MOUSE2");
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

        Win();

        //print((ringZero.transform.localRotation.eulerAngles.z % 360 == 0)+ "Local Euler: " + ringTwo.transform.localRotation.eulerAngles.z);
        //print((ringOne.transform.localRotation.eulerAngles.z % 360 == 0) + "Local Euler: " + ringTwo.transform.localRotation.eulerAngles.z);
        //print((ringTwo.transform.localRotation.eulerAngles.z % 360 == 0) + "Local Euler: " + ringTwo.transform.localRotation.eulerAngles.z);
    }

    private void Win()
    {
        

        if (ringZero.rotation.eulerAngles.z % 360 ==  0 && ringOne.rotation.eulerAngles.z % 360 == 0 && ringTwo.rotation.eulerAngles.z % 360 == 0)
        {
            print("Correct rotation");
            if (allClues[0] == true && allClues[1] == true && allClues[2] == true)
            {
                print("WIN");
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
                    ringZero.transform.Rotate(0, 0, degreeBig, Space.Self);
                    ringOne.transform.Rotate(0, 0, degreeSmall, Space.Self);
                    ringTwo.transform.Rotate(0, 0, degreeSmall, Space.Self);
                    //Quaternion watedRotation = Quaternion.Euler(0, 0, 60);
                    //Quaternion currentRotation = ringZero.transform.rotation;
                    //ringZero.transform.rotation = Quaternion.RotateTowards(currentRotation, watedRotation, Time.deltaTime * 1);



                }
                else
                {
                    ringZero.transform.Rotate(0, 0, -degreeBig, Space.Self);
                    ringOne.transform.Rotate(0, 0, -degreeSmall, Space.Self);
                    ringTwo.transform.Rotate(0, 0, -degreeSmall, Space.Self);
                }
                break;

            case 1:
               
                if (rotationWay == 0)
                {
                    ringZero.transform.Rotate(0, 0, degreeSmall, Space.Self);
                    ringOne.transform.Rotate(0, 0, degreeBig, Space.Self);
                    ringTwo.transform.Rotate(0, 0, degreeSmall, Space.Self);
                }
                else
                { 
                    ringZero.transform.Rotate(0, 0, -degreeSmall, Space.Self);
                    ringOne.transform.Rotate(0, 0, -degreeBig, Space.Self);
                    ringTwo.transform.Rotate(0, 0, -degreeSmall, Space.Self);
                }
                break;

            case 2:

                if (rotationWay == 0)
                {
                    ringZero.transform.Rotate(0, 0, degreeSmall, Space.Self);
                    ringOne.transform.Rotate(0, 0, degreeSmall, Space.Self);
                    ringTwo.transform.Rotate(0, 0, degreeBig, Space.Self);
                }
                else
                {
                    ringZero.transform.Rotate(0, 0, -degreeSmall, Space.Self);
                    ringOne.transform.Rotate(0, 0, -degreeSmall, Space.Self);
                    ringTwo.transform.Rotate(0, 0, -degreeBig, Space.Self);
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
    IEnumerator CamaraTransition(Transform pointA, Transform pointB, bool activePuzzle)
    {

        while (Vector3.Distance(pointA.position, pointB.position) > 0.05f)
        {
            activeCameraTransition = true;
            playerCamara.GetComponent<BreathCamera>().enabled = false;
            player.GetComponent<PlayerController>().enabled = false;
            cameraController.GetComponent<CameraController>().enabled = false;

            pointA.position = Vector3.Lerp(pointA.position, pointB.position, Time.deltaTime * transitionSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(pointA.rotation.eulerAngles.x, pointB.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.y, pointB.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.z, pointB.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

            pointA.eulerAngles = currentAngle;
            yield return null;


        }
        if (activePuzzle)
        {
            playerCamara.GetComponent<BreathCamera>().enabled = true;
            player.GetComponent<PlayerController>().enabled = true;
            cameraController.GetComponent<CameraController>().enabled = true;
        }
        activeCameraTransition = false;
        StopCoroutine("CamaraTransition");
    }


}
