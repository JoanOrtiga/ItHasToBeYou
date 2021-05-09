using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObserveController : MonoBehaviour, IInteractable
{

    [Header("Player components")]
    private GameObject player;
    public GameObject pivotPlayerView;
    public GameObject mainCamara;
    public GameObject camaraController;


    [Header("Observer components")]
    public Text textObserver;
    public Transform objectTransform;
    public GameObject observerCanvas;
    public GameObject crosshair;

    [HideInInspector]
    public GameObject observingObject;



    [Header("Values")]
    public float transitionSpeed = 5;
    public float rotationSpeed = 100f;

   
    bool draging;
    bool doneTransition = false;
    private bool active = false;

  

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }


    public void Interact()
    {
        AblePlayer(false);    
        textObserver.text = observingObject.GetComponent<ObserveObject>().text;
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
        doneTransition = false;
        StartCoroutine(ObjetTransition(observingObject.transform, pivotPlayerView.transform, false));

    }



    private void Update()
    {
        if (active)
        {
            if (Input.GetButtonDown("QuitInteract") && doneTransition == true)
            {

                AblePlayer(true);
                
                //observingObject.transform.position = observingObject.GetComponent<Object>().startPos;
                objectTransform.position = observingObject.GetComponent<Object>().startPos;
                objectTransform.rotation = observingObject.GetComponent<Object>().startRot;
                doneTransition = false;
                StartCoroutine(ObjetTransition(observingObject.transform, objectTransform, true));

               
            }
        }

        if (Input.GetMouseButtonUp(0) && active == true){
            draging = false;
        }
        
    }

   
   

    IEnumerator ObjetTransition(Transform pointA, Transform pointB, bool activePuzzle)
    {
        if (activePuzzle)
        {
            observerCanvas.SetActive(false);
        }
        else
        {
            observerCanvas.SetActive(true);
            observingObject.GetComponent<ObserveObject>().isObserving = true;
        }

        while (Vector3.Distance(pointA.position, pointB.position) > 0.01f)
        {
            

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
            observingObject.GetComponent<ObserveObject>().isObserving =false;
        }

        doneTransition = true;
       
        StopCoroutine("ObjetTransition");
    }
    void OnDrag()
    {
        draging = true;
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

    void AblePlayer(bool unable)
    {
        crosshair.SetActive(unable);
        mainCamara.GetComponent<BreathCamera>().enabled = unable;
        player.GetComponent<PlayerController>().enabled = unable;
        camaraController.GetComponent<CameraController>().enabled = unable;
        active = !unable;
        
    }
}
