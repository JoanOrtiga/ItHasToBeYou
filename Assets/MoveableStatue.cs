using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class MoveableStatue : MonoBehaviour
{
    public enum Sides
    {
        front, left, right, back
    }

    private Sides playerSide;
    
    public enum  Shape
    {
        green, blue, yellow    
    }

    public Shape statue;

    private PathCreator pathCreator;
    private float closestTimeOnPath;

    [SerializeField] private float speed = 0.1f;

    private Transform playerTransform;

    private bool active = false;

    public void ChangeSide(Sides side)
    {
        this.playerSide = side;
        StartCoroutine(WaitForActivation());
    }

    IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(0.01f);
        active = true;
    }
    
    private void Awake()
    {
        pathCreator = GameObject.FindObjectOfType<PathCreator>();
        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
        closestTimeOnPath = pathCreator.path.GetClosestTimeOnPath(transform.position);
        transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
        transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetButtonDown("Interact"))
            {
                active = false;
            }
        }

        if(!active)
            return;
        
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        
        
        switch (playerSide)
        {
                case Sides.front:
                    if (verticalInput < -0.3f)
                    {
                        closestTimeOnPath += speed * Time.deltaTime;
                        transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                        transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                    }
                    else if (verticalInput > 0.3f)
                    {
                        closestTimeOnPath -= speed * Time.deltaTime;
                        transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                        transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                    }
                    break;
                case Sides.back:
                    if (verticalInput > 0.3f)
                    {
                        closestTimeOnPath += speed * Time.deltaTime;
                        transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                        transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                    }
                    else if (verticalInput < -0.3f)
                    {
                        closestTimeOnPath -= speed * Time.deltaTime;
                        transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                        transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                    }
                    break;
        }
        
        
    }
}
