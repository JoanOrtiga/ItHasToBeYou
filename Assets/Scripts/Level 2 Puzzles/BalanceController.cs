using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceController : MonoBehaviour, IInteractable
{
    [Header("Camara Staff")]
    public BreathCamera camera;
    public Transform puzzlePositionCam;
    public Transform initialPositionCam;
    public float transitionSpeed;



    [Header("Puzzle objects")]
    public GameObject popUp;
    public GameObject[] canvas;   
    public PlacePlate[] metalsPlatePlace;
    public GameObject[] popUpObject;

    private bool activeCameraTransition = false;
    private bool activePuzzle;
    private BoxCollider colliderPuzzle;
    private PlayerController playerController;
    private int indexCanvas = 0;

    public GameObject[] balancePlace;
    public bool[] balance;
    public float[] balanceWeight;
    public int[] indexSelected;


    public Animator balanceAnimator;
    //private bool activePuzzle;
    private void Start()
    {

        colliderPuzzle = gameObject.GetComponent<BoxCollider>();
        playerController = FindObjectOfType<PlayerController>();
        camera = FindObjectOfType<BreathCamera>();
        popUp.SetActive(false);

        for (int i = 0; i < canvas.Length; i++)
        {
            canvas[i].SetActive(false);
        }

        
    }

    private void Update()
    {
        if (MetalsOnPlace())
        {       
            popUp.SetActive(true);
            colliderPuzzle.enabled = true;
           
            if (Input.GetButtonDown("Interact"))
            {

                canvas[indexCanvas].SetActive(true);
                activePuzzle = true;
                StartCoroutine(CamaraTransition(camera.transform, initialPositionCam, true));
            }

            if (balance[0] == true && balance[1] == true)
            {
                colliderPuzzle.enabled = false;
                popUp.SetActive(false);

                for (int i = 0; i < canvas.Length; i++)
                {
                    canvas[i].SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (balanceWeight[0] > balanceWeight[1])
                    {
                        
                        balanceAnimator.Play("BasculaLeftUpAnim");
                    }
                    else if (balanceWeight[0] < balanceWeight[1])
                    {
                        
                        balanceAnimator.Play("BasculaRightUpAnim");
                    }
                    else if (balanceWeight[0] == balanceWeight[1])
                    {
                       
                        balanceAnimator.Play("BasculaIdleAnim");
                    }

                    canvas[0].SetActive(true);

                  

                    balancePlace[0].transform.GetChild(0).position = metalsPlatePlace[indexSelected[0]].transform.position;
                    balancePlace[1].transform.GetChild(0).position = metalsPlatePlace[indexSelected[1]].transform.position;

                    balancePlace[0].transform.GetChild(0).parent = metalsPlatePlace[indexSelected[0]].transform;
                    balancePlace[1].transform.GetChild(0).parent = metalsPlatePlace[indexSelected[1]].transform;


                    balanceWeight[0] = 0;
                    balanceWeight[1] = 0;

                    indexSelected[0] = -1;
                    indexSelected[1] = -1;

                    balance[0] = false;
                    balance[1] = false;

                    indexCanvas = 0;
                

                }


            }
            else if (activePuzzle && (balance[0] == false || balance[1] == false))
            {
 
                if (Input.GetKeyDown(KeyCode.A))
                {
                    checkSelector(false);               
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    checkSelector(true);
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    if (balance[0] == false)
                    {
                        balance[0] = true;
                        balanceWeight[0] = metalsPlatePlace[indexCanvas].transform.GetChild(1).GetComponent<MineralsProperties>().weightMetal;
                        metalsPlatePlace[indexCanvas].transform.GetChild(1).position = balancePlace[0].transform.position;
                        indexSelected[0] = indexCanvas;
                        metalsPlatePlace[indexCanvas].transform.GetChild(1).transform.parent = balancePlace[0].transform;
                        

                        indexCanvas += 1;
                        while ((indexCanvas == indexSelected[0] || indexCanvas == indexSelected[1]))
                        {
                            indexCanvas += 1;
                        }

                        if (indexCanvas > 4) indexCanvas = 0;



                        for (int i = 0; i < canvas.Length; i++)
                        {
                            canvas[i].SetActive(false);
                        }
                        
                        canvas[indexCanvas].SetActive(true);


                    }
                    else if (balance[1] == false)
                    {
                        balance[1] = true;


                        indexSelected[1] = indexCanvas;
                        canvas[indexCanvas].SetActive(true);
                        balanceWeight[1] = metalsPlatePlace[indexCanvas].transform.GetChild(1).GetComponent<MineralsProperties>().weightMetal;
                        metalsPlatePlace[indexCanvas].transform.GetChild(1).position = balancePlace[1].transform.position;
                        metalsPlatePlace[indexCanvas].transform.GetChild(1).transform.parent = balancePlace[1].transform;

                        indexCanvas += 1;
                        while (indexCanvas == indexSelected[0] || indexCanvas == indexSelected[1])
                        {
                            indexCanvas += 1;
                        }

                        if (indexCanvas > 4) indexCanvas = 0;



                        for (int i = 0; i < canvas.Length; i++)
                        {
                            canvas[i].SetActive(false);
                        }

                        

                       
                        

                        if (balanceWeight[0] > balanceWeight[1])
                        {
                           
                            balanceAnimator.Play("BasculaLeftDownAnim");
                        }
                        else if (balanceWeight[0] < balanceWeight[1])
                        {
                           
                            balanceAnimator.Play("BasculaRightDownAnim");
                        }
                        else if (balanceWeight[0] == balanceWeight[1])
                        {
                            
                            balanceAnimator.Play("BasculaIdleAnim");
                        }
                    }
                   
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    CamaraTransition(camera.transform, initialPositionCam, true);

                }

                popUpObject[0].SetActive(false);
                popUpObject[1].SetActive(false);
                popUpObject[2].SetActive(false);
                popUpObject[3].SetActive(false);
                popUpObject[4].SetActive(false);
                popUp.SetActive(false);
                colliderPuzzle.enabled = false;

            }
               
        }
        else
        {

            popUpObject[0].SetActive(true);
            popUpObject[1].SetActive(true);
            popUpObject[2].SetActive(true);
            popUpObject[3].SetActive(true);
            popUpObject[4].SetActive(true);
            popUp.SetActive(false);
            colliderPuzzle.enabled = false;
            //canvas[indexCanvas].SetActive(false);
        }

    }

    public void checkSelector(bool direction)  // 0 left 1 Right
    {
        if (direction == false)
        {
            indexCanvas -= 1;
            while (indexCanvas == indexSelected[0] || indexCanvas == indexSelected[1])
            {
                indexCanvas -= 1;
            }

            if (indexCanvas < 0) indexCanvas = 4;



            for (int i = 0; i < canvas.Length; i++)
            {
                canvas[i].SetActive(false);
            }
            canvas[indexCanvas].SetActive(true);


        }
        else if (direction )
        {

            indexCanvas += 1;
            while (indexCanvas == indexSelected[0] || indexCanvas == indexSelected[1])
            {
                indexCanvas += 1;
            }

            if (indexCanvas > 4) indexCanvas = 0;



            for (int i = 0; i < canvas.Length; i++)
            {
                canvas[i].SetActive(false);
            }

            canvas[indexCanvas].SetActive(true);

        }

    }

    public void Interact()
    {
        initialPositionCam.position = camera.transform.position;
        initialPositionCam.rotation = camera.transform.rotation;
        activePuzzle = true;

        StartCoroutine(CamaraTransition(camera.transform, puzzlePositionCam, true));
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

        
        
        activeCameraTransition = false;

        StopCoroutine("CamaraTransition");
    }


    private bool MetalsOnPlace()
    {
        if (metalsPlatePlace[0].hasBeenPlaced && metalsPlatePlace[1].hasBeenPlaced && metalsPlatePlace[2].hasBeenPlaced && metalsPlatePlace[3].hasBeenPlaced && metalsPlatePlace[4].hasBeenPlaced)
        {
            return (true);
        }
        else
        {
            return (false);
        }
    }

}
