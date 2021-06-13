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
   [HideInInspector] public BreathCamera camera;
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
    private bool winPuzzle = false;

    public bool finishAnimation = true;

    public CanvasTutorial canvasTutorial;

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

        finishAnimation = true;

        canvasTutorial = FindObjectOfType<CanvasTutorial>();

        //ringOne.transform.Rotate(0, 0, 90, Space.Self);
        //ringTwo.transform.Rotate(0, 0, -90, Space.Self);
    }

    public void Interact()
    {
        if (finishAnimation)
        {
            initialPositionCam.position = camera.transform.position;
            initialPositionCam.rotation = camera.transform.rotation;

            StartCoroutine(CamaraTransition(camera.transform, viewCamara, false));
            activePuzzle = true;

            canvasTutorial.TutorialPuzzle22(true);
        }
    }


    void Update()
    {
        timeWaitSound += Time.deltaTime;

        if (activePuzzle)
        {
            if (Input.GetButtonDown("Interact") && activeCameraTransition == false && finishAnimation == true)
            {
                if (allClues[0] == true && allClues[1] == true && allClues[2] == true) //QUIT
                {
                    if (winPuzzle == false)
                    {
                        puzzleAnimator.Play("Puzzle2GetDown");
                        playSoundOne = false;
                        
                        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/PlanetarioGetUp", ringOne.transform.position);
                        canvasTutorial.TutorialPuzzle22(false);
                        correctMetals = false;
                    }
                   
                }
                canvasTutorial.TutorialPuzzle22(false);

                activePuzzle = false;
                puzzleAnimator.enabled = true;

                StartCoroutine(CamaraTransition(camera.transform, initialPositionCam, true));

                playerController.EnableController(true, true);
            }

            if (finishAnimation)
            {
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
                    FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/SelectRing", ringOne.position);
                    interactingRing++;
                    if (interactingRing >= 3)
                    {
                        interactingRing = 0;
                    }

                    ColorMat();
                }
            }
           
            if (winPuzzle == false)
            {
                if (allClues[0] == true && allClues[1] == true && allClues[2] == true && activeCameraTransition)
                {
                    PlayAnimation();
                }
            }
            
        }

        Win();

//       print( "Local ROTATION RING ZERO: " + ringZero.transform.eulerAngles.x + "" + ((ringZero.transform.eulerAngles.x < 320 && ringZero.transform.eulerAngles.x > 313)));
   //    print( "Local ROTATION RING ONE: " + ringOne.transform.eulerAngles.x + "" + (ringOne.transform.eulerAngles.x < 10 && ringOne.transform.eulerAngles.x > 0));
  //      print("Local ROTATION RING TWO: " + ringTwo.transform.eulerAngles.x + "" + (ringTwo.transform.eulerAngles.x < 309 && ringTwo.transform.eulerAngles.x > 295));

    }


    private void PlayAnimation()
    {
        if (playSoundOne == false)
        {
            playSoundOne = true;
            FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/PlanetarioGetUp", ringOne.transform.position);
        }
        puzzleAnimator.enabled = true;
        puzzleAnimator.Play("Puzzle2GetUp");
        correctMetals =true;
    }

    private void Win()
    {

        //(ringZero.transform.localEulerAngles.y < 248.5 && ringZero.transform.localEulerAngles.y > 245) &&
        //    (ringOne.transform.localEulerAngles.y < 71.5 && ringOne.transform.localEulerAngles.y > 67)
        //    && (ringTwo.transform.localEulerAngles.y < 251.5 && ringTwo.transform.localEulerAngles.y > 247.5)

        if ((ringZero.transform.eulerAngles.x < 320 && ringZero.transform.eulerAngles.x > 309) &&
            (ringOne.transform.eulerAngles.x < 10 && ringOne.transform.eulerAngles.x > 0)
            && (ringTwo.transform.eulerAngles.x < 309 && ringTwo.transform.eulerAngles.x > 295))
        {
            if (allClues[0] == true && allClues[1] == true && allClues[2] == true )
            {
                if (winPuzzle == false)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/Puerta", door.transform.position);
                    FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 2/Planetario/PlanetarioGetUp", ringOne.transform.position);

                }
                winPuzzle = true;
                door.Play("EndPuzzleDoorOpen");

                puzzleAnimator.Play("Puzzle2GetDown");
                playSoundOne = false;
                
               

                correctMetals = false;
                //   this.gameObject.GetComponent<TextBox>().StartTextPuzzle();
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
            finishAnimation = true;
        }
        playSoundOne = false;
        activeCameraTransition = false;
        
        StopCoroutine("CamaraTransition");
    }
}