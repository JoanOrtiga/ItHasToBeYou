using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] [Range(0.0f, 1.0f)] private float movementSmoothTime;

    private Vector2 currentDir = Vector2.zero;
    private Vector2 currentDirSpeed = Vector2.zero;
    
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
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
        
        Vector3 movement = (transform.forward * currentDir.y + transform.right * currentDir.x);

        characterController.Move(movement * (movementSpeed * Time.deltaTime));
    }
}
