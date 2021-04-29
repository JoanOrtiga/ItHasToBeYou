using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;
    [Range(1f, 100f)] [SerializeField] private float smoothRotateSpeed = 5f;
    [Range(1f, 100f)] [SerializeField] private float smoothInputSpeed = 5f;
    [Range(1f, 100f)] [SerializeField] private float smoothVelocitySpeed = 5f;
    [Range(1f, 100f)] [SerializeField] private float smoothFinalDirectionSpeed = 5f;
    [Range(0f, 1f)] [SerializeField] private float moveBackwardsSpeedPercent = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float moveSideSpeedPercent = 0.75f;
    [Range(1f, 100f)] [SerializeField] private float smoothHeadBobSpeed = 5f;

    [SerializeField] private HeadBobData headBobData;

    [Range(0.01f, 1f)] [SerializeField] private float rayLength = 0.1f;
    [Range(0.01f,1f)][SerializeField] private float raySphereRadius = 0.2f;

    [SerializeField] private LayerMask groundLayer = ~0;

    private CharacterController characterController;

    private float stickToGroundForce;
    private Vector3 finalMove;

    private float gravity = -9.8f;


    private Vector2 inputVector;
    private Vector2 smoothInputVector;

    private float smoothCurrentSpeed;
    private float currentSpeed;

    private Vector3 finalMoveDir;
    private Vector3 smoothFinalMoveDir;

    private RaycastHit hitInfo;
    private float finalRayLength;

    private Transform yawTransform;
    private CameraController cameraController;

    private HeadBob headBob;

    private bool mIsGrounded;

    private PickUp _pickUp;

    private void Awake()
    {
        _pickUp = GetComponent<PickUp>();
        cameraController = GetComponentInChildren<CameraController>();
        yawTransform = cameraController.transform;
        characterController = GetComponent<CharacterController>();

        headBob = new HeadBob(headBobData, moveBackwardsSpeedPercent, moveSideSpeedPercent);

        mIsGrounded = true;
        finalRayLength = rayLength + characterController.center.y;
    }

    public void DisableController()
    {
        characterController.enabled = false;
        cameraController.enabled = false;
        this.enabled = false;
        _pickUp.enabled = false;
    }

    public void EnableController()
    {
        this.enabled = true;
        characterController.enabled = true;
        cameraController.enabled = true;
        _pickUp.enabled = true;
    }

    private void Update()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        if (yawTransform != null)
            RotateTowardsCamera();

        CheckGround();
        HeadBobbing();
        Move();
    }

    private void HeadBobbing()
    {
        
        
        if (inputVector != Vector2.zero &&
            mIsGrounded)
        {
            headBob.ScrollHeadBob(inputVector);
            yawTransform.localPosition = Vector3.Lerp(yawTransform.localPosition,
                (Vector3.up * headBob.currentStateHeight) + headBob.finalOffset, Time.deltaTime * smoothHeadBobSpeed);
           // print(headBob.finalOffset);
        }
        else
        {
           
            if (!headBob.Resetted)
            {
                headBob.ResetHeadBob();
            }


            yawTransform.localPosition = Vector3.Lerp(yawTransform.localPosition,
                new Vector3(0f, headBob.currentStateHeight, 0f), Time.deltaTime * smoothHeadBobSpeed);
        }

        if (inputVector == Vector2.zero &&
            characterController.isGrounded)
        {
            if (!headBob.Resetted)
            {
                headBob.ResetHeadBob();
            }

            yawTransform.localPosition = Vector3.Lerp(yawTransform.localPosition,
                new Vector3(0f, headBob.currentStateHeight, 0f), Time.deltaTime * smoothHeadBobSpeed);
        }
        
       
    }

    private void RotateTowardsCamera()
    {
        Quaternion _currentRot = transform.rotation;
        Quaternion _desiredRot = yawTransform.rotation;

        transform.rotation = Quaternion.Slerp(_currentRot, _desiredRot, Time.deltaTime * smoothRotateSpeed);
    }

    private void CheckGround()
    {
        Vector3 _origin = transform.position + characterController.center;
        bool _hitGround = Physics.SphereCast(_origin,raySphereRadius,Vector3.down,out hitInfo,finalRayLength,groundLayer);

        mIsGrounded = _hitGround ? true : false;
    }
    
    private void Move()
    {
        Vector2 normalizedInputVector = inputVector.normalized;
        

        smoothInputVector = Vector2.Lerp(smoothInputVector, normalizedInputVector, Time.deltaTime * smoothInputSpeed);

        smoothCurrentSpeed = Mathf.Lerp(smoothCurrentSpeed, currentSpeed, Time.deltaTime * smoothVelocitySpeed);

        smoothFinalMoveDir = Vector3.Lerp(smoothFinalMoveDir, finalMoveDir, Time.deltaTime * smoothFinalDirectionSpeed);

        Vector3 _vDir = transform.forward * smoothInputVector.y;
        Vector3 _hDir = transform.right * smoothInputVector.x;

        Vector3 _desiredDir = _vDir + _hDir;
        Vector3 _flattenDir = _desiredDir;
        
        if (characterController.isGrounded)
            _flattenDir = Vector3.ProjectOnPlane(_desiredDir, hitInfo.normal);

        finalMoveDir = _flattenDir;

        currentSpeed = walkSpeed;
        currentSpeed = !(normalizedInputVector != Vector2.zero) ? 0f : currentSpeed;
        currentSpeed = normalizedInputVector.y == -1 ? currentSpeed * moveBackwardsSpeedPercent : currentSpeed;
        currentSpeed = normalizedInputVector.x != 0 && normalizedInputVector.y == 0 ? currentSpeed * moveSideSpeedPercent : currentSpeed;

        Vector3 _finalVector = smoothFinalMoveDir * smoothCurrentSpeed;

        // We have to assign individually in order to make our character jump properly because before it was overwriting Y value and that's why it was jerky now we are adding to Y value and it's working
        finalMove.x = _finalVector.x;
        finalMove.z = _finalVector.z;

        if (characterController.isGrounded
        ) // Thanks to this check we are not applying extra y velocity when in air so jump will be consistent
            finalMove.y +=
                _finalVector
                    .y; //so this makes our player go in forward dir using slope normal but when jumping this is making it go higher so this is weird

        //Gravity
        if (characterController.isGrounded)
        {
            finalMove.y = -stickToGroundForce;
        }
        else
        {
            finalMove += Vector3.up * gravity * Time.deltaTime;
        }

        characterController.Move(finalMove * Time.deltaTime);
    }
}