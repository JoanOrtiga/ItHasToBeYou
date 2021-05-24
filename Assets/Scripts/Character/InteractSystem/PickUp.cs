using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    Ray ray;
    RaycastHit rayCastHit;
    bool hitted = false;
    
    [SerializeField] private float checkEveryTime = 0.02f;
    
    [SerializeField] private float pickUpDistance = 3f;
    [SerializeField] private float leaveDistance = 3f;
    [SerializeField] private LayerMask DetectLayerMask;

    private int objectLayer;
    private int interactLayer;
    private int placeObjectLayer;
    private int lookObjectLayer;

    private Animator handAnimator;

    private bool onHand;

    public GameObject handCenter;
    private GameObject placeObjectPosition;

    [Header("The pick up object propeties")]
    Transform objectPickUp;

    Rigidbody objectPickUpRigidBody;
    ObjectParameters objectParameters;

    ObjectOnHand objectRotation;
    // Quaternion objectRotation;

    private Camera mainCamera;

    public Crosshair crosshairController;
    public GameObject observeController;

    [HideInInspector] public bool activePuzzle = false;


    public enum Interaction
    {
        drop,
        interactPuzzle,
        placeObject,
        observe,
        none
    }

    Interaction interaction;

    private void Awake()
    {
        mainCamera = Camera.main;
        //StartCoroutine(CheckForObject());
        
        InvokeRepeating("CheckForObject", checkEveryTime,checkEveryTime);
    }

    private void Start()
    {
        handAnimator = handCenter.GetComponent<Animator>();
        objectLayer = LayerMask.NameToLayer("Object");
        interactLayer = LayerMask.NameToLayer("Interactable");
        placeObjectLayer = LayerMask.NameToLayer("PlaceObject");
        lookObjectLayer = LayerMask.NameToLayer("LookObject");
    }

    // Update is called once per frame
    void Update()
    {

        //print(interaction);
        //print(hitted);

        if (hitted)
        {
            if (interaction == Interaction.drop && onHand == false)
            {
                //UI " E to Pick Up Object"
                if (Input.GetButtonDown("Interact"))
                {
                    PickUpObject();
                }
            }
            else if (interaction == Interaction.interactPuzzle)
            {
                //UI "E to Interact"
                if (Input.GetButtonDown("Interact"))
                {
                    Interact();
                }
            }
            else if (interaction == Interaction.placeObject && onHand)
            {
                crosshairController.ChangeCrosshairState(true, false);
                
                if (Input.GetButtonDown("Interact"))
                {
                
                    PlaceObject();
                }
            }
            else if (interaction == Interaction.observe)
            {
                if (Input.GetButtonDown("Interact") && rayCastHit.transform.gameObject.GetComponent<ObserveObject>().isObserving == false)
                {
                    ObservObject();
                }
            }
            else if (Input.GetButtonDown("Interact") && onHand)
            {
                print("DROP OBJECT");
                DropObject();
            }
        }
        

        if (onHand)
        {
            if (Input.GetMouseButtonDown(0) && handAnimator.GetBool("LookClose"))
            {
                handAnimator.SetBool("LookClose", false);
            }
            else if (Input.GetMouseButtonDown(0) && !handAnimator.GetBool("LookClose"))
            {
                if (objectPickUp.GetComponent<TextBox>() != null &&
                    objectPickUp.GetComponent<TextBox>().lookCloseObject)
                {
                    objectPickUp.GetComponent<TextBox>().StartText();
                }

                handAnimator.SetBool("LookClose", true);
            }
        }
    }

    void CheckForObject()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        hitted = Physics.Raycast(ray, out rayCastHit, pickUpDistance, DetectLayerMask.value);


        if (hitted && activePuzzle == false)
        {

           
            
            if (rayCastHit.transform.gameObject.layer == objectLayer)
            {
               
                interaction = Interaction.drop;
            }
            else if (rayCastHit.transform.gameObject.layer == interactLayer)
            {
             
                if (rayCastHit.transform.gameObject.CompareTag("ObserveObject"))
                {
                    interaction = Interaction.observe;
                   
                }
                else if (rayCastHit.transform.gameObject.CompareTag("PuzzleInteractable"))
                {
                    interaction = Interaction.interactPuzzle;
                    crosshairController.ChangeCrosshairState(true, false);
                }
                else if (rayCastHit.transform.CompareTag("PuzzleInteractableWithoutIcon"))
                {
                    interaction = Interaction.interactPuzzle;
                }
               
            }
            else if (rayCastHit.transform.gameObject.layer == placeObjectLayer)
            {
                if (rayCastHit.transform.gameObject.GetComponent<PlaceMaterial>().hasBeenPlaced == false && onHand)
                {
                    interaction = Interaction.placeObject;
                    placeObjectPosition = rayCastHit.transform.gameObject;
                }
            }
            else if (rayCastHit.transform.gameObject.layer == lookObjectLayer)
            {
                rayCastHit.transform.gameObject.GetComponent<TextBox>().StartText();
            }
            else
            {
               
                interaction = Interaction.none;
            }

            if (!(rayCastHit.transform.gameObject.CompareTag("PuzzleInteractable")))
            {
              
                crosshairController.ChangeCrosshairState(false, false);
            }
        }
        else
        {
           
            interaction = Interaction.none;
            crosshairController.ChangeCrosshairState(false, false);
        }
    }

    private void PlaceObject()
    {
        objectPickUp.transform.parent = placeObjectPosition.transform;
        objectPickUp.position = placeObjectPosition.transform.position;
        objectPickUp.GetComponent<ObjectParameters>().hasBeenPlaced = true;
        placeObjectPosition.GetComponent<PlaceMaterial>().hasBeenPlaced = true;
        handAnimator.SetBool("LookClose", false);
        onHand = false;
    }

    private void ObservObject()
    {
        if (rayCastHit.transform.gameObject.GetComponent<TextBox>() != null)
        {
            rayCastHit.transform.gameObject.GetComponent<TextBox>().StartText();
        }

        observeController.GetComponent<ObserveController>().observingObject = rayCastHit.transform.gameObject;
        observeController.GetComponent<IInteractable>().Interact();
    }

    private void PickUpObject()
    {
        CancelInvoke("CheckForObject");

            objectPickUp = rayCastHit.transform;
        objectPickUp.GetComponent<ObjectParameters>().DisablePopUp(true);
        objectPickUpRigidBody = objectPickUp.GetComponent<Rigidbody>();
        objectRotation = objectPickUp.GetComponent<ObjectOnHand>();
        objectParameters = objectPickUp.GetComponent<ObjectParameters>();


        objectPickUpRigidBody.isKinematic = true;
        objectPickUpRigidBody.constraints = RigidbodyConstraints.FreezeAll;

        if (objectPickUp.GetComponent<ObjectParameters>().hasBeenPlaced == true)
        {
            objectPickUp.transform.parent.gameObject.GetComponent<PlaceMaterial>().hasBeenPlaced = false;
            objectPickUp.GetComponent<ObjectParameters>().hasBeenPlaced = false;
        }


        if (objectPickUp.GetComponent<TextBox>() != null)
        {
            if (!objectPickUp.GetComponent<TextBox>().lookCloseObject)
            {
                objectPickUp.GetComponent<TextBox>().StartText();
            }
        }

        objectPickUp.SetParent(handCenter.transform);

        objectPickUp.position = handCenter.transform.position;

        objectPickUp.localRotation = Quaternion.identity;
        objectPickUp.localRotation = Quaternion.Euler(objectRotation.rotation);

        onHand = true;
    }

    private void DropObject()
    {
        float dist = Vector3.Distance(handCenter.transform.position, objectParameters.startPos);

        if (objectPickUp != null)
        {
            objectPickUp.SetParent(null);
            objectPickUpRigidBody.constraints = RigidbodyConstraints.None;
            objectPickUpRigidBody.isKinematic = false;
            objectPickUp.GetComponent<ObjectParameters>().DisablePopUp(false);

            if (dist < leaveDistance)
            {
                objectParameters.ReLocate();
            }
        }

        onHand = false;
        
        InvokeRepeating("CheckForObject", checkEveryTime, checkEveryTime);
    }

    private void Interact()
    {
        rayCastHit.transform.GetComponent<IInteractable>().Interact();

        if (rayCastHit.transform.GetComponent<TextBox>() != null)
        {
            rayCastHit.transform.GetComponent<TextBox>().StartText();
        }
    }
}