using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MovingStatue : MonoBehaviour
{
    public enum Sides
    {
        front,
        left,
        right,
        back
    }

    public enum RoadType
    {
        undefined,
        line,
        circular
    }

    [SerializeField] private Transform rotationPoint;
    [SerializeField] private StatuePathFinder statuePathFinder;

    [SerializeField] private float[] circularMovingSpeed;
    [SerializeField] private float linearMovingSpeed = 10f;

    [SerializeField] private Transform torre;

    private RoadType currentRoadType = RoadType.circular;

    private Sides playerSide;

    private bool active = false;

    private float verticalInput;

    private LinearPath linearPath;

    public void ChangeSide(Sides side)
    {
        this.playerSide = side;
        StartCoroutine(WaitForActivation());

        switch (currentRoadType)
        {
            case RoadType.circular:
                if (side == Sides.left || side == Sides.right)
                {
                    if (statuePathFinder.IsInIntersection(transform.position))
                    {
                        currentRoadType = RoadType.line;
                        linearPath = statuePathFinder.GetLinearPath(transform.position);
                    }
                }

                break;
            case RoadType.line:
                if (side == Sides.back || side == Sides.front)
                {
                    if (statuePathFinder.IsInIntersection(transform.position))
                    {
                        currentRoadType = RoadType.circular;
                    }
                }

                break;
        }
    }

    private void Awake()
    {
        //transform.LookAt(rotationPoint, Vector3.up);

      /*  transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z);*/
    }

    IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(0.01f);
        active = true;
    }

    private void Update()
    {
        if (active is false)
            return;

        if (Input.GetButtonDown("Interact"))
        {
            if (statuePathFinder.IsInStopPoint(transform.position))
            {
                active = false;
                return;
            }
        }

        verticalInput = Input.GetAxisRaw("Vertical");

        if (currentRoadType == RoadType.circular)
        {
            if (playerSide == Sides.front)
            {
                RotateArround(1);
            }
            else if (playerSide == Sides.back)
            {
                RotateArround(-1);
            }
        }
        else if (currentRoadType == RoadType.line)
        {
            if (playerSide == Sides.left)
            {
                if (verticalInput > 0)
                    MoveToPoint(linearPath.end.position);
                else if (verticalInput < 0)
                    MoveToPoint(linearPath.start.position);
            }
            else if (playerSide == Sides.right)
            {
                if (verticalInput > 0)
                    MoveToPoint(linearPath.start.position);
                else if (verticalInput < 0)
                    MoveToPoint(linearPath.end.position);
            }
        }
    }

    private void RotateArround(int direction = 1)
    {
        Vector3 lastPosition = transform.position;
        Quaternion lastRotation = transform.rotation;

        transform.RotateAround(rotationPoint.position, Vector3.up,
            verticalInput * direction * GetCircularSpeed() * Time.deltaTime);

      //  torre.LookAt(rotationPoint, Vector3.up);

        if (!statuePathFinder.CanIKeepMoving(transform.position, transform))
        {
            transform.position = lastPosition;
            transform.rotation = lastRotation;
        }
    }

    private void MoveToPoint(Vector3 point)
    {
        Vector3 lastPosition = transform.position;
        Quaternion lastRotation = transform.rotation;

        transform.position = Vector3.MoveTowards(transform.position, point,
            linearMovingSpeed * Time.deltaTime);

        if (!statuePathFinder.CanIKeepMoving(transform.position, transform))
        {
            transform.position = lastPosition;
            transform.rotation = lastRotation;
        }
    }

    private float GetCircularSpeed()
    {
        return circularMovingSpeed[statuePathFinder.WhatCircularPath(transform.position)];
    }
}