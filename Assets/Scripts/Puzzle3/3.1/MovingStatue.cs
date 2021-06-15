using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class MovingStatue : MonoBehaviour, IPuzzleSolver
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

    [SerializeField] private Transform target;
    [SerializeField] private float targetRange;
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

    private bool imNearStopPoint;
    private Vector3 nearStopPoint;

    private float lastDirection;
    private PlayerController playerController;

    [SerializeField] private Transform lockCameraPoint;

    private float time;

    private bool transitioning;

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
        playerController = FindObjectOfType<PlayerController>();
        nearStopPoint = new Vector3();
        transform.LookAt(rotationPoint, Vector3.up);

        /*  transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
              transform.rotation.eulerAngles.z);*/
    }

    IEnumerator WaitForActivation()
    {
        yield return new WaitForSeconds(1f);
        active = true;
    }

    public bool IsActive()
    {
        return active;
    }

    public static float GetAngleOnAxis(Vector3 self, Vector3 other, Vector3 axis)
    {
        Vector3 perpendicularSelf = Vector3.Cross(axis, self);
        Vector3 perpendicularOther = Vector3.Cross(axis, other);
        return Vector3.SignedAngle(perpendicularSelf, perpendicularOther, axis);
    }

    private void Update()
    {
        if (imNearStopPoint)
        {
            if (currentRoadType == RoadType.circular)
            {
                MoveToPoint(nearStopPoint);
            }
            else if (currentRoadType == RoadType.line)
            {
                MoveToPoint(nearStopPoint);
            }

            if ((nearStopPoint - transform.position).sqrMagnitude <= 0.06f * 0.06f)
            {
                transform.position = nearStopPoint;
                imNearStopPoint = false;
                active = false;
                return;
            }

            return;
        }

        if (active is false)
            return;

        if (transitioning)
            return;

        if (Input.GetButtonDown("Interact"))
        {
            if (statuePathFinder.IsInStopPoint(transform.position))
            {
                StartCoroutine(LookAt());
                transitioning = true;
                return;
            }
            else if (statuePathFinder.NearStopPoint(transform.position, out nearStopPoint))
            {
                StartCoroutine(LookAt());
                transitioning = true;
                imNearStopPoint = true;
                return;
            }
        }

        verticalInput = Input.GetAxisRaw("Vertical");

        if (verticalInput != 0)
        {
            lastDirection = verticalInput;
            playerController.playerMovement.SimulateHeadBobbing();
        }


        if (verticalInput < 0.1f && verticalInput > -0.1f)
        {
            playerController.AnimatorSetBool("P3.1_PushBackward", false);
            playerController.AnimatorSetBool("P3.1_PushForward", false);
        }
        else if (verticalInput > 0.1f)
        {
            playerController.AnimatorSetBool("P3.1_PushBackward", false);
            playerController.AnimatorSetBool("P3.1_PushForward", true);
        }
        else if (verticalInput < -0.1f)
        {
            playerController.AnimatorSetBool("P3.1_PushForward", false);
            playerController.AnimatorSetBool("P3.1_PushBackward", true);
        }

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

        torre.LookAt(rotationPoint, Vector3.up);

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

    public bool Solved()
    {
        if ((target.position - transform.position).sqrMagnitude <= targetRange * targetRange)
        {
            return true;
        }

        return false;
    }

    private IEnumerator LookAt()
    {
        playerController.ChangeLookCloserState(false, false, false);

        while (!playerController.cameraController.LookAt(lockCameraPoint.position, 5f))
        {
            yield return null;
        }

        yield return null;

        playerController.ReAttachHand();

        playerController.AnimatorSetBool("P3.1", false);

        transitioning = false;
        active = false;
    }
}