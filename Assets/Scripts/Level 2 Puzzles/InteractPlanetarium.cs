using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractPlanetarium : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform viewCamara;
    [SerializeField] private float transitionSpeed;
    [SerializeField] private Material SelectedMat;
    [SerializeField] private Material NormalMat;

    [SerializeField] private Transform playerCamara;
    private GameObject player;
    private GameObject cameraController;
    [SerializeField] private Transform initialPositionCam;
    private int interactingRing = 3;
    private float time;

    private bool activePuzzle = false;
    private bool activeCameraTransition = false;
    private bool onTrigger = false;

    [SerializeField] private GameObject puzzle;
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
        ringOne = puzzle.transform.GetChild(1);
        ringTwo = puzzle.transform.GetChild(2);

        //ringOne.transform.Rotate(0, 0, 90, Space.Self);
        //ringTwo.transform.Rotate(0, 0, -90, Space.Self);
    }
    public void Interact()
    {

        initialPositionCam.position = playerCamara.position;
        initialPositionCam.rotation = playerCamara.rotation;

        StartCoroutine(CamaraTransition(playerCamara, viewCamara, false));
        activePuzzle = true;

    }



    void Update()
    {


        if (activePuzzle)
        {
            if (Input.GetButtonDown("QuitInteract") && activeCameraTransition == false)
            {
                activePuzzle = false;

                StartCoroutine(CamaraTransition(playerCamara, initialPositionCam, true));
                player.GetComponent<PlayerController>().enabled = true;
                cameraController.GetComponent<CameraController>().enabled = true;

            }


            if (Input.GetAxis("Mouse ScrollWheel") > 0) //Gira derecha
            {
                print("MOUSE");
                RotateRing(true);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0) //Gira izquierda
            {
                print("MOUSE2");
                RotateRing(false);
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

        print((ringZero.transform.localRotation.eulerAngles.z % 360 == 0) + "Local Euler RING ZERO: " + ringZero.transform.localEulerAngles.z);
        print((ringOne.transform.localRotation.eulerAngles.z % 360 == 0) + "Local Euler RING ONE: " + ringOne.transform.localEulerAngles.z);
        print((ringTwo.transform.localRotation.eulerAngles.z % 360 == 0) + "Local Euler RING TWO: " + ringTwo.transform.localEulerAngles.z);
        // ringTwo.transform.localEulerAngles.z // 
    }

    private void Win()
    {


        if (ringZero.transform.localEulerAngles.z < 6 && ringOne.transform.localEulerAngles.z < 6 && ringTwo.transform.localEulerAngles.z < 6)
        {
            print("Correct rotation");
            if (allClues[0] == true && allClues[1] == true && allClues[2] == true)
            {
                this.gameObject.GetComponent<TextBox>().StartTextPuzzle();
                print("WIN");
            }
        }
    }

    private void RotateRing(bool rotationUp) // 0 Derecha & 1 Izquierda
    {
        switch (interactingRing)
        {
            case 0:
                ringZero.transform.Rotate(0, 0, (rotationUp ? degreeBig : -degreeBig), Space.Self);
                ringOne.transform.Rotate(0, 0, (rotationUp ? degreeSmall : -degreeSmall), Space.Self);
                ringTwo.transform.Rotate(0, 0, (rotationUp ? degreeSmall : -degreeSmall), Space.Self);
                break;

            case 1:
                ringZero.transform.Rotate(0, 0, (rotationUp ? degreeSmall : -degreeSmall), Space.Self);
                ringOne.transform.Rotate(0, 0, (rotationUp ? degreeBig : -degreeBig), Space.Self);
                ringTwo.transform.Rotate(0, 0, (rotationUp ? degreeSmall : -degreeSmall), Space.Self);
                break;

            case 2:
                ringZero.transform.Rotate(0, 0, (rotationUp ? degreeSmall : -degreeSmall), Space.Self);
                ringOne.transform.Rotate(0, 0, (rotationUp ? degreeSmall : -degreeSmall), Space.Self);
                ringTwo.transform.Rotate(0, 0, (rotationUp ? degreeBig : -degreeBig), Space.Self);     
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
