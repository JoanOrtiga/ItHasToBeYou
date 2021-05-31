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

    private PlayerController playerController;
    private BreathCamera camera;
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

    private MeshRenderer ringZeroMesh;
    private MeshRenderer ringOneMesh;
    private MeshRenderer ringTwoMesh;

    public bool[] allClues;

    //public int degreeBig = 90;
    //public int degreeSmall = 45;
    public float rotationSpeedHigh = 5f;
    public float rotationSpeedLow = 5f;

    public Animator door;

    private Animator puzzleAnimator;

   
    private float timeWaitSound;
    private bool correctMetals;


    private bool playSoundOne = false;
    private void Start()
    {
        puzzleAnimator = puzzle.GetComponent<Animator>();

        allClues = new bool[3];
        playerController = FindObjectOfType<PlayerController>();
        camera = FindObjectOfType<BreathCamera>();

        ringZero = puzzle.transform.GetChild(0);
        ringOne = puzzle.transform.GetChild(1);
        ringTwo = puzzle.transform.GetChild(2);

        ringZeroMesh = ringZero.GetChild(0).GetComponent<MeshRenderer>();
        ringOneMesh = ringOne.GetChild(0).GetComponent<MeshRenderer>();
        ringTwoMesh = ringTwo.GetChild(0).GetComponent<MeshRenderer>();

        //ringOne.transform.Rotate(0, 0, 90, Space.Self);
        //ringTwo.transform.Rotate(0, 0, -90, Space.Self);
    }

    public void Interact()
    {
        initialPositionCam.position = camera.transform.position;
        initialPositionCam.rotation = camera.transform.rotation;

        StartCoroutine(CamaraTransition(camera.transform, viewCamara, false));
        activePuzzle = true;
    }


    void Update()
    {
        timeWaitSound += Time.deltaTime;


        if (activePuzzle)
        {
            if (Input.GetButtonDown("Interact") && activeCameraTransition == false)
            {
                if (allClues[0] == true && allClues[1] == true && allClues[2] == true)
                {
                    puzzleAnimator.Play("Puzzle2GetDown");
                    playSoundOne = false;
                    FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/PlanetarioGetUp", ringOne.transform.position);

                    correctMetals = false;
                }

                activePuzzle = false;
                puzzleAnimator.enabled = true;

                StartCoroutine(CamaraTransition(camera.transform, initialPositionCam, true));

                playerController.EnableController(true, true);
            }


            if (Input.GetAxisRaw("Horizontal") >= 0.3f) //Gira derecha
            {
                if (timeWaitSound > 0.3 && correctMetals)
                {

                    FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/Rotation", gameObject.transform.position);
                    timeWaitSound = 0;
                }
                
                puzzleAnimator.enabled = false;
                RotateRing(true);
            }
            else if (Input.GetAxisRaw("Horizontal") <= -0.3f) //Gira izquierda
            {

                if (timeWaitSound > 0.3 && correctMetals)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/Rotation", gameObject.transform.position);
                    timeWaitSound = 0;
                }
               
                puzzleAnimator.enabled = false;
                RotateRing(false);
            }

            if (Input.GetButtonDown("Select"))
            {
                interactingRing++;
                if (interactingRing >= 3)
                {
                    interactingRing = 0;
                }

                ColorMat();
            }

            if (allClues[0] == true && allClues[1] == true && allClues[2] == true && activeCameraTransition)
            {
                PlayAnimation();
            }
        }

        Win();

        //print( "Local Euler RING ZERO: " + ringZero.transform.localEulerAngles.z);
        //print( "Local Euler RING ONE: " + ringOne.transform.localEulerAngles.z);
        //print( "Local Euler RING TWO: " + ringTwo.transform.localEulerAngles.z);
    }


    private void PlayAnimation()
    {
        print("MAKE ANIMATION Go down");
        if (playSoundOne == false)
        {
            playSoundOne = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/PlanetarioGetUp", ringOne.transform.position);
            print("SOUND");
        }
        puzzleAnimator.enabled = true;
        puzzleAnimator.Play("Puzzle2GetUp");
        correctMetals =true;
    }

    private void Win()
    {
        if ((ringZero.transform.localEulerAngles.z < 168 && ringZero.transform.localEulerAngles.z > 152) &&
            (ringOne.transform.localEulerAngles.z < 348 && ringOne.transform.localEulerAngles.z > 338)
            && (ringTwo.transform.localEulerAngles.z < 65 && ringTwo.transform.localEulerAngles.z > 45))
        {
            print("Correct rotation");
            if (allClues[0] == true && allClues[1] == true && allClues[2] == true)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/Puerta", door.transform.position);

                   this.gameObject.GetComponent<TextBox>().StartTextPuzzle();
            }
        }
    }

    private void RotateRing(bool rotationUp) // true Derecha & False Izquierda
    {
        switch (interactingRing)
        {
            case 0:
                ringZero.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedHigh : 1 * -rotationSpeedHigh), 0,
                    Space.Self);
                ringOne.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedLow : 1 * -rotationSpeedLow),0 , Space.Self);
                ringTwo.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedLow : 1 * -rotationSpeedLow), 0, Space.Self);

                break;

            case 1:
                ringZero.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedLow : 1 * -rotationSpeedLow), 0 ,
                    Space.Self);
                ringOne.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedHigh : 1 * -rotationSpeedHigh), 0,
                    Space.Self);
                ringTwo.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedLow : 1 * -rotationSpeedLow), 0, Space.Self);
                break;

            case 2:
                ringZero.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedLow : 1 * -rotationSpeedLow), 0,
                    Space.Self);
                ringOne.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedLow : 1 * -rotationSpeedLow), 0, Space.Self);
                ringTwo.transform.Rotate(0, (rotationUp ? 1 * rotationSpeedHigh : 1 * -rotationSpeedHigh), 0,
                    Space.Self);
                break;
        }
    }

    private void ColorMat()
    {
        switch (interactingRing)
        {
            case 0:
                ringZeroMesh.material = SelectedMat;
                ringOneMesh.material = NormalMat;
                ringTwoMesh.material = NormalMat;
                break;

            case 1:
                ringZeroMesh.material = NormalMat;
                ringOneMesh.material = SelectedMat;
                ringTwoMesh.material = NormalMat;
                break;


            case 2:
                ringZeroMesh.material = NormalMat;
                ringOneMesh.material = NormalMat;
                ringTwoMesh.material = SelectedMat;
                break;

            default:
                break;
        }
    }

    IEnumerator CamaraTransition(Transform pointA, Transform pointB, bool activePuzzle_)
    {
        while (Vector3.Distance(pointA.position, pointB.position) > 0.01f)
        {
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

        if (activePuzzle_)
        {
            playerController.EnableController(true, true, true, true);
        }

        activeCameraTransition = false;
        
        StopCoroutine("CamaraTransition");
    }
}