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

    private bool onHand;

    public GameObject handCenter;
    private GameObject placeObjectPosition;

    [Header("The pick up object propeties")]
    Transform objectPickUp;

    Rigidbody objectPickUpRigidBody;
    Object objectPlace;
    Quaternion objectRotation;

    private Camera mainCamera;

    public enum Interaction
    {
        drop,
        interact,
        placeObject,
        none
    }

    Interaction interaction;

    private void Awake()
    {
        mainCamera = Camera.main;
        //StartCoroutine(CheckForObject());
        InvokeRepeating("CheckForObject", 0f, 0.2f);
    }

    private void Start()
    {
        objectLayer = LayerMask.NameToLayer("Object");
        interactLayer = LayerMask.NameToLayer("Interactable");
        placeObjectLayer = LayerMask.NameToLayer("PlaceObject");
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
        }
        else if (Input.GetButtonDown("Interact") && onHand)
        {
            DropObject();
        }
    }

    private void CheckForObject()
    {
        ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        hitted = Physics.Raycast(ray, out rayCastHit, pickUpDistance, DetectLayerMask.value);

        if (hitted)
        {
            if (rayCastHit.transform.gameObject.layer == objectLayer)
            {
                interaction = Interaction.drop;
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
                }
            }
        }
        else
        {
            interaction = Interaction.none;
        }

       // yield return null;
    }

    private void OnDisable()
    {
       // StopCoroutine(CheckForObject());
    }

    private void OnEnable()
    {
       // StartCoroutine(CheckForObject());
    }

    private void PlaceObject()
    {
        objectPickUp.transform.parent = placeObjectPosition.transform;
        objectPickUp.position = placeObjectPosition.transform.position;
        objectPickUp.GetComponent<Object>().hasBeenPlaced = true;
        placeObjectPosition.GetComponent<PlaceMaterial>().hasBeenPlaced = true;
        onHand = false;
    }

    private void PickUpObject()
    {
     //   StopCoroutine(CheckForObject());


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


        if (objectPickUp.GetComponent<InteractionTextBox>() != null)
        {
            objectPickUp.GetComponent<InteractionTextBox>().StartText();
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

        if (rayCastHit.transform.GetComponent<InteractionTextBox>() != null)
        {
            rayCastHit.transform.GetComponent<InteractionTextBox>().StartText();
        }
    }
}