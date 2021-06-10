using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCameraPuzzle : MonoBehaviour
{
    public InteractPlanetarium puzzleTrigger;
    public float transitionSpeed;
    public GameObject finalCameraPosition;
    private bool activeCameraTransition;


    public void ZoomIn()
    {
       
        activeCameraTransition = true;
    }

    private void Update()
    {
        if (activeCameraTransition)
        {
            StartCoroutine(CamaraTransition(puzzleTrigger.camera.transform, finalCameraPosition.transform));
        }
    }

    public void FinishAnimation()
    {
        puzzleTrigger.finishAnimation = true;
    }
    public void StartAnimation()
    {
        CamaraShake.ShakeOnce(4, 5, new Vector3(0.03f, 0.03f));
        puzzleTrigger.finishAnimation = false;
    }



    IEnumerator CamaraTransition(Transform pointA, Transform pointB)
    {
        while (Vector3.Distance(pointA.position, pointB.position) > 0.005f)
        {
            activeCameraTransition = true;
           

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
        puzzleTrigger.finishAnimation = true;

        StopCoroutine("CamaraTransition");
    }

}
