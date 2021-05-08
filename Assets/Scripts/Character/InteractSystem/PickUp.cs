using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    Ray ray;
    RaycastHit rayCastHit;
    bool hitted = false;
    [SerializeField] private float pickUpDistance = 3f;
    [SerializeField] private float leaveDistance = 3f;
    [SerializeField] private LayerMask DetectLayerMask;

    private int objectLayer;
    private int interactLayer;
    private int placeObjectLayer;
    private int lookObjectLayer;
    private int observeObjectLayer;

    private bool onHand;

    public GameObject handCenter;
    private GameObject placeObjectPosition;

    [Header("The pick up object propeties")]
    Transform objectPickUp;
    Rigidbody objectPickUpRigidBody;
    Object objectPlace;
    Quaternion objectRotation;

    private Camera mainCamera;

    public GameObject crosshair;
    public GameObject observeCanvas;

    public enum Interaction
    {
        drop, interact,placeObject, observe, none
    }

    Interaction interaction;

    private void Awake()
    {
        mainCamera = Camera.main;
        //StartCoroutine(CheckForObject());
    }
    private void Start()
    {
       
        objectLayer = LayerMask.NameToLayer("Object");
        interactLayer = LayerMask.NameToLayer("Interactable");
        placeObjectLayer = LayerMask.NameToLayer("PlaceObject");
        lookObjectLayer = LayerMask.NameToLayer("LookObject");
        observeObjectLayer = LayerMask.NameToLayer("ObserveObject");
    }
    // Update is called once per frame
    void Update()
    {
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
            else if (interaction == Interaction.interact)
            {
                //UI "E to Interact"
                if (Input.GetButtonDown("Interact"))
                {
                    Interact();
                }
                
            }
            else if (interaction == Interaction.placeObject && onHand)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    PlaceObject();
                }
            }
            else if (interaction == Interaction.observe)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    ObservObject();
                }
               
            }

        }
        else if (Input.GetButtonDown("Interact") && onHand)
        {
            DropObject();
            
        }
      
    }

    IEnumerator CheckForObject()
    {
        while (true)
        {
            ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            hitted = Physics.Raycast(ray, out rayCastHit, pickUpDistance, DetectLayerMask.value);

            if (hitted)
            {
                

                if (rayCastHit.transform.gameObject.layer == objectLayer)
                {
                    interaction = Interaction.drop;
                    crosshair.transform.GetChild(0).gameObject.SetActive(true);
                }
                else if (rayCastHit.transform.gameObject.layer == interactLayer)
                {
                    interaction = Interaction.interact;
                   
                }
                else if (rayCastHit.transform.gameObject.layer == placeObjectLayer)
                {
                    if (rayCastHit.transform.gameObject.GetComponent<PlaceMaterial>().hasBeenPlaced == false)
                    {
                        interaction = Interaction.placeObject;
                        placeObjectPosition = rayCastHit.transform.gameObject;
                        crosshair.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    
                }
                else if (rayCastHit.transform.gameObject.layer == lookObjectLayer)
                {
                    rayCastHit.transform.gameObject.GetComponent<TextBox>().StartText();
                }
                else if (rayCastHit.transform.gameObject.layer == observeObjectLayer)
                {
                    crosshair.transform.GetChild(1).gameObject.SetActive(true);
                    interaction = Interaction.observe;
                }
            }
            else
            {
                interaction = Interaction.none;
                crosshair.transform.GetChild(0).gameObject.SetActive(false);
                crosshair.transform.GetChild(1).gameObject.SetActive(false);
            }

            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }
        }
    }

    private void OnDisable()
    {
        StopCoroutine(CheckForObject());
    }

    private void OnEnable()
    {
        StartCoroutine(CheckForObject());
    }

    private void PlaceObject()
    {
        objectPickUp.transform.parent = placeObjectPosition.transform;
        objectPickUp.position = placeObjectPosition.transform.position;
        objectPickUp.GetComponent<Object>().hasBeenPlaced = true;
        placeObjectPosition.GetComponent<PlaceMaterial>().hasBeenPlaced = true;
        onHand = false;
    }

    private void ObservObject()
    {
        print("OBSERVE");
        observeCanvas.SetActive(true);
        observeCanvas.GetComponent<ObserveController>().observingObject = rayCastHit.transform.gameObject;
        observeCanvas.GetComponent<IInteractable>().Interact();
    }
    private void PickUpObject()
    {
        StopCoroutine(CheckForObject());
        

        objectPickUp = rayCastHit.transform;

        objectPickUpRigidBody = objectPickUp.GetComponent<Rigidbody>();
        objectPlace = objectPickUp.GetComponent<Object>();

       

        objectPickUpRigidBody.isKinematic = true;
        objectPickUpRigidBody.constraints = RigidbodyConstraints.FreezeAll;

        if (objectPickUp.GetComponent<Object>().hasBeenPlaced == true)
        {
            objectPickUp.transform.parent.gameObject.GetComponent<PlaceMaterial>().hasBeenPlaced = false;
            objectPickUp.GetComponent<Object>().hasBeenPlaced = false;
        }

       
        if (objectPickUp.GetComponent<TextBox>() != null)
        {
            objectPickUp.GetComponent<TextBox>().StartText();
        }
        objectPickUp.SetParent(handCenter.transform);

        objectPickUp.position = handCenter.transform.position;

        objectPickUp.localRotation = Quaternion.identity;
        objectPickUp.localRotation = Quaternion.Euler(objectPlace.rotation);

        onHand = true;
    }

    private void DropObject()
    {
        float dist = Vector3.Distance(handCenter.transform.position, objectPlace.startPos);

        if (objectPickUp != null)
        {
            objectPickUp.SetParent(null);
            objectPickUpRigidBody.constraints = RigidbodyConstraints.None;
            objectPickUpRigidBody.isKinematic = false;

            if (dist < leaveDistance)
            {
                objectPlace.ReLocate();
            }
        }

        onHand = false;
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
