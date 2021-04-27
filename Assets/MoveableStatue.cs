using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class MoveableStatue : MonoBehaviour , IInteractable
{
    private enum Sides
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
        if (Input.GetAxisRaw("Vertical") > 0.3f)
        {
            closestTimeOnPath += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
            transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
        }
        else if (Input.GetAxisRaw("Vertical") < -0.3f)
        {
            closestTimeOnPath -= speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
            transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
        }
    }

    public void Interact()
    {
        float angle = AngleDir(transform.forward, playerTransform.forward, transform.up);
        
        print(angle);
        if (angle >= 0.8)
        {
            
        }   
    }
    
    
    //returns -1 when to the left, 1 to the right, and 0 for forward/backward
    public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
 
        if (dir > 0.0f) {
            return 1.0f;
        } else if (dir < 0.0f) {
            return -1.0f;
        } else {
            return 0.0f;
        }
    }  
}
