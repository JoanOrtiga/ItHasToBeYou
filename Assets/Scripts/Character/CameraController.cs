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

    [Header("LookCloser")] [SerializeField]
    private Vector2 lookCloserXLimit = new Vector2(-10, 10);

    [SerializeField] private Vector2 lookCloserYLimit = new Vector2(-90, 90);
    public bool lookCloser;
    private float initialYaw;
    private float initialPitch;
    private bool changeX;
    private bool changeY;

    public Transform GetPitchObject()
    {
        return pitchTransform;
    }

    private void Awake()
    {
        pitchTransform = transform.GetChild(0).transform;
        cam = GetComponentInChildren<Camera>();

        yaw = transform.eulerAngles.y;
        desiredYaw = yaw;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        pitch = pitchTransform.localRotation.eulerAngles.x;
        desiredPitch = pitch;
    }

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void LateUpdate()
    {
        if (lookCloser)
        {
            if (changeY)
            {
                desiredPitch -= inputVector.y * sensitivity.y * Time.deltaTime;
                desiredPitch = Mathf.Clamp(desiredPitch, lookCloserYLimit.x, lookCloserYLimit.y);

                pitch = Mathf.Lerp(pitch, desiredPitch, smoothAmount.y * Time.deltaTime);
                pitchTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
            }

            if (changeX)
            {
                desiredYaw += inputVector.x * sensitivity.x * Time.deltaTime;
                desiredYaw = Mathf.Clamp(desiredYaw, lookCloserXLimit.x + initialYaw, lookCloserXLimit.y + initialYaw);
                yaw = Mathf.Lerp(yaw, desiredYaw, smoothAmount.x * Time.deltaTime);
                transform.eulerAngles = new Vector3(0f, yaw, 0f);
            }
        }
        else
        {
            desiredYaw += inputVector.x * sensitivity.x * Time.deltaTime;
            desiredPitch -= inputVector.y * sensitivity.y * Time.deltaTime;
            
            desiredPitch = Mathf.Clamp(desiredPitch, lookAngleMinMax.x, lookAngleMinMax.y);
            yaw = Mathf.Lerp(yaw, desiredYaw, smoothAmount.x * Time.deltaTime);
            pitch = Mathf.Lerp(pitch, desiredPitch, smoothAmount.y * Time.deltaTime);
            transform.eulerAngles = new Vector3(0f, yaw, 0f);
            pitchTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
        }
    }

    public bool LookAt(Vector3 point, float speed)
    {
        Vector3 direction = point - pitchTransform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);

        Vector3 totalRotation =
            new Vector3(pitchTransform.localRotation.eulerAngles.x, transform.rotation.eulerAngles.y);
        Quaternion finalRotation = Quaternion.Lerp(Quaternion.Euler(totalRotation), toRotation, speed * Time.deltaTime);

        pitchTransform.localRotation = Quaternion.Euler(finalRotation.eulerAngles.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, finalRotation.eulerAngles.y, 0);

        yaw = transform.rotation.eulerAngles.y;
        pitch = pitchTransform.localRotation.eulerAngles.x;
        
        desiredYaw = yaw;
        desiredPitch = pitch;

        bool veryClose = Mathf.Approximately(Mathf.Abs(Quaternion.Dot(pitchTransform.rotation, toRotation)), 1.0f);

        return veryClose;
    }

    public void LookAtBruto(Vector3 point)
    {
        
        Vector3 direction = point - pitchTransform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction, transform.up);

        Vector3 totalRotation =
            new Vector3(pitchTransform.localRotation.eulerAngles.x, transform.rotation.eulerAngles.y);
        Quaternion finalRotation = toRotation;

        pitchTransform.localRotation = Quaternion.Euler(finalRotation.eulerAngles.x, 0, 0);
        transform.rotation = Quaternion.Euler(0, finalRotation.eulerAngles.y, 0);

        yaw = transform.rotation.eulerAngles.y;
        pitch = pitchTransform.localRotation.eulerAngles.x;
        
        desiredYaw = yaw;
        desiredPitch = pitch;
    }

    public void ChangeInitialYaw(bool x, bool y, Vector2 maxPitch)
    {
        initialYaw = transform.eulerAngles.y;
        lookCloserYLimit = maxPitch;
        changeX = x;
        changeY = y;

        ResetDesires();
    }

    public void ResetDesires()
    {
        yaw = transform.rotation.eulerAngles.y;
        pitch = pitchTransform.localRotation.eulerAngles.x;
        

        desiredYaw = yaw;
        desiredPitch = pitch;

        inputVector = new Vector2(0, 0);
    }


    public void SafeResetDesires()
    {
        yaw = transform.localRotation.eulerAngles.y;
        pitch = pitchTransform.localRotation.eulerAngles.x;
        desiredYaw = yaw;
        desiredPitch = pitch;

        inputVector = new Vector2(0, 0);
    }
}