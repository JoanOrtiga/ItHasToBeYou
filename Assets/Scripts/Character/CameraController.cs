using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform pitchTransform;
    private Camera cam;

    private float yaw;
    private float pitch;
    private float desiredYaw;
    private float desiredPitch;

    private Vector2 inputVector;
    
    [SerializeField] private Vector2 sensitivity = Vector2.one; 
    [SerializeField] private Vector2 smoothAmount = Vector2.zero;
    [SerializeField] private Vector2 lookAngleMinMax = new Vector2(-90, 90);
    
    private void Awake()
    {
        pitchTransform = transform.GetChild(0).transform;
        cam = GetComponentInChildren<Camera>();
        
        yaw = transform.eulerAngles.y;
        desiredYaw = yaw;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
    }

    private void LateUpdate()
    {
        desiredYaw += inputVector.x * sensitivity.x * Time.deltaTime;
        desiredPitch -= inputVector.y * sensitivity.y * Time.deltaTime;

        desiredPitch = Mathf.Clamp(desiredPitch,lookAngleMinMax.x,lookAngleMinMax.y);
        
        yaw = Mathf.Lerp(yaw,desiredYaw, smoothAmount.x * Time.deltaTime);
        pitch = Mathf.Lerp(pitch,desiredPitch, smoothAmount.y * Time.deltaTime);
        
        transform.eulerAngles = new Vector3(0f,yaw,0f);
        pitchTransform.localEulerAngles = new Vector3(pitch,0f,0f);
    }
}
