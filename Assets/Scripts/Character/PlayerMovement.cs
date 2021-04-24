using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] [Range(0.0f, 1.0f)] private float movementSmoothTime;

    [SerializeField] [Tooltip("If gravity is set to 0 or >, it will get project gravity.")]
    private float gravity = 1f;


    [SerializeField] private float slopeForceRayLength;
    [SerializeField] private float slopeForce;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirSpeed = Vector2.zero;

    private float velocityY;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (gravity >= 0)
        {
            gravity = Physics.gravity.y;
        }
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 inputDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, inputDir, ref currentDirSpeed, movementSmoothTime);

        if (OnSlope())
        {
            print("hola");
            velocityY += gravity * Time.deltaTime * slopeForce;
        }
        else
        {
            if (characterController.isGrounded)
                velocityY = 0.0f;

            velocityY += gravity * Time.deltaTime;
        }

        Vector3 movement = (transform.forward * currentDir.y + transform.right * currentDir.x) * movementSpeed +
                           Vector3.up * velocityY;

        characterController.Move(movement * Time.deltaTime);
    }

    private bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit,
            characterController.height / 2 * slopeForceRayLength))
        {
            if (hit.normal == Vector3.up)
                return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (characterController != null)
            Gizmos.DrawRay(transform.position, Vector3.down * (characterController.height / 2 * slopeForceRayLength));
    }
}