using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatuePathFinder : MonoBehaviour
{
    [SerializeField] private Transform[] stopPoints;
    [SerializeField] private LinearPath[] intersectionPoints;
    [SerializeField] private Transform[] obstaclePoints;
    [SerializeField] private Transform[] movingStatues;

    [SerializeField] private float[] circularRanges;
    
    [SerializeField] private float stopRange = 0.2f;
    [SerializeField] private float intersectionRange = 0.2f;
    [SerializeField] private float collisionRange = 0.2f;

    /// <summary>
    /// Is statue near a stop point?
    /// </summary>
    /// <returns></returns>
    public bool IsInStopPoint(Vector3 statuePosition)
    {
        foreach (var point in stopPoints)
        {
            if ((point.position - statuePosition).sqrMagnitude < stopRange * stopRange)
            {
                return true;
            }
        }

       // return false;
        return true;
    }

    public bool IsInIntersection(Vector3 statuePosition)
    {
        foreach (var linearPath in intersectionPoints)
        {
            if ((linearPath.start.position - statuePosition).sqrMagnitude < intersectionRange * intersectionRange)
            {
                return true;
            }
            
            if ((linearPath.end.position - statuePosition).sqrMagnitude < intersectionRange * intersectionRange)
            {
                return true;
            }
        }

        return false;
    }

    public LinearPath GetLinearPath(Vector3 statuePosition)
    {
        foreach (var linearPath in intersectionPoints)
        {
            if ((linearPath.start.position - statuePosition).sqrMagnitude < intersectionRange * intersectionRange)
            {
                return linearPath;
            }
            
            if ((linearPath.end.position - statuePosition).sqrMagnitude < intersectionRange * intersectionRange)
            {
                return linearPath;
            }
        }

        return new LinearPath();
    }
    
    public bool CanIKeepMoving(Vector3 moveTo)
    {
        foreach (var obstacle in obstaclePoints)
        {
            if ((obstacle.position - moveTo).sqrMagnitude < collisionRange * collisionRange)
            {
                return false;
            }
        }
        
        foreach (var statue in movingStatues)
        {
            if ((statue.position - moveTo).sqrMagnitude < collisionRange * collisionRange)
            {
                return false;
            }
        }

        return true;
    }

    public int WhatCircularPath(Vector3 position)
    {
        float magnitude = (position - transform.position).magnitude;
        
        for (int i = 0; i < circularRanges.Length-1; i++)
        {
            if (magnitude > circularRanges[i] && magnitude < circularRanges[i+1])
            {
                return i;
            }
        }
        
        return -1;
    }
}

[Serializable]
public struct LinearPath
{
    public Transform start;
    public Transform end;
}
