using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ObserveController : MonoBehaviour, IInteractable
{
    public GameObject crosshair;
    [HideInInspector]
    public GameObject observingObject;

    private GameObject player;
    public GameObject pivotPlayerView;
    public GameObject mainCamara;
    public GameObject camaraController;
    public Text textObserver;


    public float transitionSpeed = 5;
    private bool active = false;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }
    private void Update()
    {
        if (active)
        {
            if (Input.GetButtonDown("QuitInteract"))
            {
                crosshair.SetActive(true);
                mainCamara.GetComponent<BreathCamera>().enabled = true;
                player.GetComponent<PlayerController>().enabled = true;
                camaraController.GetComponent<CameraController>().enabled = true;
                active = false;
                gameObject.SetActive(false);
            }
        }
        
    }

    public void Interact()
    {
        //observingObject.GetComponent<Rigidbody>().useGravity = false;
        crosshair.SetActive(false);
        mainCamara.GetComponent<BreathCamera>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;
        camaraController.GetComponent<CameraController>().enabled = false;
        textObserver.text = observingObject.GetComponent<ObserveObject>().text;
        active = true;

        StartCoroutine(ObjetTransition(observingObject.transform, pivotPlayerView.transform, true));

    }
    IEnumerator ObjetTransition(Transform pointA, Transform pointB, bool activePuzzle)
    {

        while (Vector3.Distance(pointA.position, pointB.position) > 0.05f)
        {
            

            pointA.position = Vector3.Lerp(pointA.position, pointB.position, Time.deltaTime * transitionSpeed);

            Vector3 currentAngle = new Vector3(
                Mathf.LerpAngle(pointA.rotation.eulerAngles.x, pointB.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.y, pointB.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
                Mathf.LerpAngle(pointA.rotation.eulerAngles.z, pointB.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

            pointA.eulerAngles = currentAngle;
            yield return null;


        }
      
        StopCoroutine("ObjetTransition");
    }

}
