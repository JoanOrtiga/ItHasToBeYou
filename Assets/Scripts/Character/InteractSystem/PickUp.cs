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

    private bool onHand;

    public GameObject handCenter;


    [Header("The pick up object propeties")]
    Transform objectPickUp;
    Rigidbody objectPickUpRigidBody;
    ObjectPlace objectPlace;

    public enum Interaction
    {
        pickUp, interact, none
    }

    Interaction interaction;

    private void Awake()
    {
        StartCoroutine(CheckForObject());
    }
    private void Start()
    {
        objectLayer = LayerMask.NameToLayer("Object");
        interactLayer = LayerMask.NameToLayer("Interactable");
    }
    // Update is called once per frame
    void Update()
    {
        if (hitted)
        {
            if (interaction == Interaction.pickUp)
            {
                print("Object");
                //UI " E to Pick Up Object"
                if (Input.GetButtonDown("Interact"))
                {
                    print("PickUp");
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

        }
        else if(Input.GetButtonDown("Interact"))
        {
            PlaceObject();
        }
    }

    IEnumerator CheckForObject()
    {
        while (true)
        {
            ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            hitted = Physics.Raycast(ray, out rayCastHit, pickUpDistance, DetectLayerMask.value);

            if (hitted)
            {
                if (rayCastHit.transform.gameObject.layer == objectLayer)
                {
                    interaction = Interaction.pickUp;
                }
                else if (rayCastHit.transform.gameObject.layer == interactLayer)
                {
                    interaction = Interaction.interact;
                }
            }
            else
            {
                interaction = Interaction.none;
            }

            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }
        }
    }
    
    private void PickUpObject()
    {
        StopCoroutine(CheckForObject());

        objectPickUp = rayCastHit.transform;

        objectPickUpRigidBody = objectPickUp.GetComponent<Rigidbody>();
        objectPlace = objectPickUp.GetComponent<ObjectPlace>();

        objectPickUpRigidBody.isKinematic = true;
        objectPickUpRigidBody.constraints = RigidbodyConstraints.FreezeAll;

        objectPickUp.SetParent(handCenter.transform);

        objectPickUp.position = handCenter.transform.position;
    }

    private void PlaceObject()
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

    }
}
