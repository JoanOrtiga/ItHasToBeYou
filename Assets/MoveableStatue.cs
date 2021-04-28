using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class MoveableStatue : MonoBehaviour
{
    public enum Sides
    {
        front,
        left,
        right,
        back
    }

    private Sides playerSide;

    public enum Shape
    {
        green,
        blue,
        yellow
    }

    public enum RoadType
    {
        undefined, line, circular
    }

    private RoadType currentRoadType = RoadType.undefined;

    public Shape statue;

    private PathCreator pathCreator;
    private float closestTimeOnPath;

    private float speed;
    [SerializeField] private float circleSpeed = 0.1f;
    [SerializeField] private float linearSpeed = 0.5f;

    private Transform playerTransform;

    private bool active = false;

    [SerializeField] private PathFeeder _pathFeeder;

    private bool lineActive = false;

    private Sides lastSide;

    [SerializeField] private Transform[] otherStatues;
    [SerializeField] private Transform[] obstacles;

    [SerializeField] private float collisionRange = 1f;

    [SerializeField] private GameObject target;
    [SerializeField] private float targetRange;

    public void ChangeSide(Sides side)
    {
        this.playerSide = side;
        StartCoroutine(WaitForActivation());

        if ((currentRoadType == RoadType.circular && (side == Sides.back || side == Sides.front)) ||
            (currentRoadType == RoadType.line && (side == Sides.left || side == Sides.right)))
        {
            lineActive = true;
            return;
        }

        if (side == Sides.left || side == Sides.right)
        {
            PathCreator newPath = _pathFeeder.GetLinearPath(transform.position, out lineActive);

            if (lineActive)
            {
                pathCreator = newPath;
                speed = linearSpeed;

                currentRoadType = RoadType.line;
                
                closestTimeOnPath = pathCreator.path.GetClosestTimeOnPath(transform.position);
                transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Stop);
            }
        }
        else
        {
            PathCreator newPath = _pathFeeder.GetCircularPath(transform.position, out lineActive);
            
            if (lineActive)
            {
                pathCreator = newPath;
                speed = linearSpeed;

                currentRoadType = RoadType.circular;
                
                closestTimeOnPath = pathCreator.path.GetClosestTimeOnPath(transform.position);
                transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Stop);
            }

            speed = circleSpeed;

            closestTimeOnPath = pathCreator.path.GetClosestTimeOnPath(transform.position);
            transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
        }

    }

    IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(0.01f);
        active = true;
    }

    private void Awake()
    {
        pathCreator = _pathFeeder.GetCircularPath(transform.position, out lineActive);

        playerTransform = GameObject.FindObjectOfType<PlayerController>().transform;
        closestTimeOnPath = pathCreator.path.GetClosestTimeOnPath(transform.position);
        transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
        transform.position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);

        speed = circleSpeed;
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

        if (!active)
            return;
        
        if(!lineActive)
            return;

        float verticalInput = Input.GetAxisRaw("Vertical");

        float lastTime = closestTimeOnPath;
        
        switch (playerSide)
        {
            case Sides.front:
                if (verticalInput < -0.3f)
                {
                    
                    closestTimeOnPath += speed * Time.deltaTime;

                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                    position.y = transform.position.y;

                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                        
                    
                    transform.position = position;
                    transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                }
                else if (verticalInput > 0.3f)
                {
                    closestTimeOnPath -= speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                    position.y = transform.position.y;
                    
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                }

                break;
            case Sides.back:
                if (verticalInput > 0.3f)
                {
                    closestTimeOnPath += speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                    position.y = transform.position.y;
                    
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                }
                else if (verticalInput < -0.3f)
                {
                    closestTimeOnPath -= speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Loop);
                    position.y = transform.position.y;
                    
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Loop);
                }
                break;

            case Sides.left:

                closestTimeOnPath = Mathf.Clamp01(closestTimeOnPath);

                if (verticalInput < -0.3f)
                {
                    closestTimeOnPath += speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Stop);
                    position.y = transform.position.y;
                   
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    //   transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, Reversep);
                }
                else if (verticalInput > 0.3f)
                {
                    closestTimeOnPath -= speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Stop);
                    position.y = transform.position.y;
                    
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    //   transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, Reversep);
                }
                break;

            case Sides.right:

                closestTimeOnPath = Mathf.Clamp01(closestTimeOnPath);
                
                if (verticalInput > 0.3f)
                {
                    closestTimeOnPath += speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Stop);
                    position.y = transform.position.y;
                    
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    //   transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, Reversep);
                }
                else if (verticalInput < -0.3f)
                {
                    closestTimeOnPath -= speed * Time.deltaTime;
                    Vector3 position = pathCreator.path.GetPointAtTime(closestTimeOnPath, EndOfPathInstruction.Stop);
                    position.y = transform.position.y;
                    
                    if (!CanIKeepMoving(position))
                    {
                        closestTimeOnPath = lastTime;
                        return;
                    }
                    
                    transform.position = position;
                    // transform.rotation = pathCreator.path.GetRotation(closestTimeOnPath, EndOfPathInstruction.Stop);
                }
                break;
        }
    }
    
    private bool CanIKeepMoving(Vector3 moveTo)
    {
        for (int i = 0; i < otherStatues.Length; i++)
        {
            if ((otherStatues[i].position - moveTo).sqrMagnitude < collisionRange * collisionRange)
            {
                return false;
            }
        }
        
        for (int i = 0; i < obstacles.Length; i++)
        {
            if ((obstacles[i].position - moveTo).sqrMagnitude < collisionRange * collisionRange)
            {
                return false;
            }
        }

        if ((target.transform.position - moveTo).sqrMagnitude < targetRange * targetRange)
        {
            target.SetActive(true);
        }
        else
        {
            target.SetActive(false);
        }
        
        return true;
    }
}