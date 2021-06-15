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

    [HideInInspector] public bool onHand;

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

    [SerializeField] private Color pickupColor = Color.yellow;

    [HideInInspector] public bool canDrop = true;

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
                
                    PlaceObject(1);
                }
            }
            else if (interaction == Interaction.observe)
            {
                if (Input.GetButtonDown("Interact") && rayCastHit.transform.gameObject.GetComponent<ObserveObject>().isObserving == false)
                {
                    ObservObject();
                }
            }
            else if (Input.GetButtonDown("Interact") && onHand && canDrop)
            {
                print("DROP OBJECT");
                DropObject();
            }
        }
        
       

        if (onHand)
        {

            objectPickUp.GetComponent<ObjectParameters>().popUp.SetActive(false);
            //if (Input.GetMouseButtonDown(0))
            //{
            //    //if (objectPickUp.GetComponent<TextBox>() != null && objectPickUp.GetComponent<TextBox>().lookCloseObject && objectPickUp.GetComponent<TextBox>().isPickUp)
            //    //{
                    
            //    //    objectPickUp.GetComponent<TextBox>().StartTextGetClose();
            //    //}

            //   // handAnimator.SetBool("LookClose", true);
            //}
        }
        
    }

    public void CustomDrop()
    {
        if (objectPickUp != null)
        {
            objectPickUp.GetComponent<ObjectParameters>().DisablePopUp(false);
            Destroy(objectPickUp.gameObject, 1.3f);
            FMODUnity.RuntimeManager.PlayOneShot(objectPickUp.GetComponent<ObjectParameters>().dropPath, transform.position);
        }

        onHand = false;
        
        InvokeRepeating("CheckForObject", checkEveryTime, checkEveryTime);
        canDrop = true;
    }

    
    private SpriteRenderer popupSPR;
    private bool changingColor = false;

    IEnumerator FromWhiteToPickUpColor()
    {
        for (int i = 0; i < 130; i++)
        {
            if(popupSPR != null)
                popupSPR.color = Color.Lerp(popupSPR.color, pickupColor, 0.04f);
            yield return new WaitForSeconds(0.01f); 
        }
       
    }

    void CheckForObject()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        hitted = Physics.Raycast(ray, out rayCastHit, pickUpDistance, DetectLayerMask.value);

        if (hitted && activePuzzle == false)
        {
            SpriteRenderer x = rayCastHit.transform.GetComponent<ObjectParameters>()?.popUp.GetComponent<SpriteRenderer>();
            if (x != null)
            {
                if (popupSPR == null)
                {
                    StopCoroutine(FromWhiteToPickUpColor());
                    popupSPR = x;
                    StartCoroutine(FromWhiteToPickUpColor());
                }
                else if (x != popupSPR)
                {
                    StopCoroutine(FromWhiteToPickUpColor());
                    popupSPR.color = Color.white;
                    popupSPR = x;
                    StartCoroutine(FromWhiteToPickUpColor());
                }
            }
            else
            {
                if (popupSPR != null)
                {
                    StopCoroutine(FromWhiteToPickUpColor());
                    popupSPR.color = Color.white;
                    popupSPR = null; 
                }
            }
            
            /*
            if (popupSPR == null)
            {
                popupSPR = rayCastHit.transform.GetComponent<ObjectParameters>()?.popUp.GetComponent<SpriteRenderer>();
                if (popupSPR != null)
                {
                    if(!changingColor)
                        StartCoroutine(FromWhiteToPickUpColor());
                }
                else
                {
                    StopCoroutine(FromWhiteToPickUpColor());
                    changingColor = false;
                }
            }
            else
            {
                if (rayCastHit.transform.GetComponent<ObjectParameters>()?.popUp.GetComponent<SpriteRenderer>() != popupSPR)
                {
                    StopCoroutine(FromWhiteToPickUpColor());
                    changingColor = false;
                    popupSPR.color = Color.white;
                    popupSPR = rayCastHit.transform.GetComponent<ObjectParameters>()?.popUp.GetComponent<SpriteRenderer>();
                } 
            }


            if (popupSPR != null)
            {
                if(!changingColor)
                    StartCoroutine(FromWhiteToPickUpColor());
            
            }*/
            
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
                if (rayCastHit.transform.gameObject.GetComponent<PlaceMaterial>() != null)
                {
                    if (rayCastHit.transform.gameObject.GetComponent<PlaceMaterial>().hasBeenPlaced == false && onHand)
                    {
                        interaction = Interaction.placeObject;
                        placeObjectPosition = rayCastHit.transform.gameObject;
                    }
                }
                else if (rayCastHit.transform.gameObject.GetComponent<PlacePlate>() != null)
                {
                    if (rayCastHit.transform.gameObject.GetComponent<PlacePlate>().hasBeenPlaced == false && onHand)
                    {
                        interaction = Interaction.placeObject;
                        placeObjectPosition = rayCastHit.transform.gameObject;
                    }
                }
               
            }
            else if (rayCastHit.transform.gameObject.layer == lookObjectLayer)
            {
                if (rayCastHit.transform.gameObject.GetComponent<TextBox>().textDone == false)
                {
                    rayCastHit.transform.gameObject.GetComponent<TextBox>().StartText();
                }
                
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
            if (popupSPR != null)
            {
                StopCoroutine(FromWhiteToPickUpColor());
                popupSPR.color = Color.white;
                popupSPR = null; 
            }
            
            interaction = Interaction.none;
            crosshairController.ChangeCrosshairState(false, false);
        }
    }

    private void PlaceObject(int puzzlePlace) //1 is balance y 2
    {

        objectPickUp.GetComponent<ObjectParameters>().DisablePopUp(false);
        objectPickUp.transform.parent = placeObjectPosition.transform;
        objectPickUp.position = placeObjectPosition.transform.position;
        objectPickUp.GetComponent<ObjectParameters>().hasBeenPlaced = true;


        if (placeObjectPosition.GetComponent<PlacePlate>() != null)
        {
            placeObjectPosition.GetComponent<PlacePlate>().hasBeenPlaced = true;
            FMODUnity.RuntimeManager.PlayOneShot(placeObjectPosition.GetComponent<PlacePlate>().placeSoundPath, transform.position);
        } 
        else if (placeObjectPosition.GetComponent<PlaceMaterial>() != null)
        {

            placeObjectPosition.GetComponent<PlaceMaterial>().hasBeenPlaced = true;
            FMODUnity.RuntimeManager.PlayOneShot(placeObjectPosition.GetComponent<PlaceMaterial>().placeSoundPath, transform.position);
           // handAnimator.SetBool("LookClose", false);
        }
       



        onHand = false;
    }

    private void ObservObject()
    {
        if (rayCastHit.transform.gameObject.GetComponent<TextBox>() != null)
        {
            if (rayCastHit.transform.gameObject.GetComponent<TextBox>().textDone == false)
            {
                rayCastHit.transform.gameObject.GetComponent<TextBox>().StartText();
            }
           
        }

        observeController.GetComponent<ObserveController>().observingObject = rayCastHit.transform.gameObject;
        observeController.GetComponent<IInteractable>().Interact();
    }

    private void PickUpObject()
    {
        //CancelInvoke("CheckForObject");
        
       
        objectPickUp = rayCastHit.transform;

        objectParameters = objectPickUp.GetComponent<ObjectParameters>();
        
        FMODUnity.RuntimeManager.PlayOneShot(objectParameters.pickUpPath, transform.position);

        objectParameters.DisablePopUp(true);
        canDrop = objectParameters.canDrop;
        objectPickUpRigidBody = objectPickUp.GetComponent<Rigidbody>();
        objectRotation = objectPickUp.GetComponent<ObjectOnHand>();
       


        objectPickUpRigidBody.isKinematic = true;
        objectPickUpRigidBody.constraints = RigidbodyConstraints.FreezeAll;

        if (objectPickUp.GetComponent<ObjectParameters>().hasBeenPlaced == true)
        {
            if (objectPickUp.transform.parent.gameObject.GetComponent<PlaceMaterial>() != null)
            {
                objectPickUp.transform.parent.gameObject.GetComponent<PlaceMaterial>().hasBeenPlaced = false;

            }
            else if (objectPickUp.transform.parent.gameObject.GetComponent<PlacePlate>() != null)
            {
                objectPickUp.transform.parent.gameObject.GetComponent<PlacePlate>().hasBeenPlaced = false;

            }
            objectPickUp.GetComponent<ObjectParameters>().hasBeenPlaced = false;
        }
      
      


        if (objectPickUp.GetComponent<TextBox>() != null)
        {
            objectPickUp.GetComponent<TextBox>().StartText();
           
        }

        objectPickUp.SetParent(handCenter.transform);

        objectPickUp.position = handCenter.transform.position;

        objectPickUp.localRotation = Quaternion.identity;
        objectPickUp.localRotation = Quaternion.Euler(objectRotation.desiredRotation);
        objectPickUp.localPosition = objectRotation.desiredPosition;

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

            FMODUnity.RuntimeManager.PlayOneShot(objectPickUp.GetComponent<ObjectParameters>().dropPath, transform.position);

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