using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObserveController : MonoBehaviour, IInteractable
{

    [Header("Player components")]
    private PlayerController player;
    public GameObject pivotPlayerView;
    public BreathCamera mainCamara;
    public CameraController camaraController;


    [Header("Observer components")]
    public Text textObserver;
    public Transform objectTransform;
    public GameObject observerCanvas;
    public GameObject crosshair;

    private Transform textView;

    [HideInInspector]
    public GameObject observingObject;
    [SerializeField] private LayerMask DetectLayerMask;
    RaycastHit rayCastHit;
    bool hit;


    [Header("Values")]
    public float transitionSpeed = 5;
    public float rotationSpeed = 100f;

   
    bool draging;
    bool doneTransition = false;
    private bool active = false;

  

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }


    public void Interact()
    {
        AblePlayer(false);    
        textObserver.text = observingObject.GetComponent<ObserveObject>().text;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        doneTransition = false;
        StartCoroutine(ObjetTransition(observingObject.transform, pivotPlayerView.transform, false));

    }



    private void Update()
    {
        if (active)
        {
            if (Input.GetButtonDown("Interact") && doneTransition == true) //Quit menu
            {

                AblePlayer(true);
                objectTransform.position = observingObject.GetComponent<Object>().startPos;
                objectTransform.rotation = observingObject.GetComponent<Object>().startRot;

                
                doneTransition = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                StartCoroutine(ObjetTransition(observingObject.transform, objectTransform, true));

               
            }


            if (Input.GetMouseButtonUp(0))
            {
                draging = false;
            }
            if (Input.GetMouseButton(0))
            {
                draging = true;
            }

            RayCastCheck();
        }

       

    }

    private void FixedUpdate()
    {
        if (draging)
        {
            float x = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float y = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

            observingObject.GetComponent<Rigidbody>().AddTorque(Vector3.down * x);
            observingObject.GetComponent<Rigidbody>().AddTorque(Vector3.right * y);


        }
    }



    void RayCastCheck()
    {
        if (observingObject != null)
        {
            hit = Physics.Linecast(observingObject.transform.GetChild(0).transform.position, mainCamara.transform.position, out rayCastHit, DetectLayerMask.value);
            Debug.DrawLine(observingObject.transform.GetChild(0).transform.position, mainCamara.GetComponent<Transform>().position, Color.green);
            if (!hit && doneTransition)
            {
                // print(rayCastHit.transform.name);
                //Physics.Linecast(observingObject.transform.GetChild(0).transform.position, player.GetComponent<Transform>().position, out rayCastHit, DetectLayerMask.value)
                observerCanvas.transform.GetChild(0).gameObject.SetActive(true);
                print("SHOw");
            }
            else
            {
                observerCanvas.transform.GetChild(0).gameObject.SetActive(false);
                print("HIDE");
            }
        }
    }
  
  
    void AblePlayer(bool unable)
    {
        crosshair.SetActive(unable);
        mainCamara.enabled = unable;
        player.enabled = unable;
        camaraController.enabled = unable;
        active = !unable;
        
    }

    IEnumerator ObjetTransition(Transform pointA, Transform pointB, bool activePuzzle)
    {
        if (activePuzzle)
        {
            observerCanvas.SetActive(false);
            observingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            observerCanvas.SetActive(true);
            observingObject.GetComponent<ObserveObject>().isObserving = true;
            observingObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }

        while (Vector3.Distance(pointA.position, pointB.position) > 0.01f)
        {
            doneTransition = false;

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
            observingObject.GetComponent<ObserveObject>().isObserving = false;
        }

        doneTransition = true;

        StopCoroutine("ObjetTransition");
    }
}
