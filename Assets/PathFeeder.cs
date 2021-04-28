using System;
using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;

public class PathFeeder : MonoBehaviour
{
    [SerializeField] private List<PathCreator> circularPaths;
    [SerializeField] private List<PathCreator> linearPaths;

    [SerializeField] private float maxRange = 1.5f;
    
 

    public PathCreator GetLinearPath(Vector3 position, out bool lineActive)
    {
        return GetPath(position, linearPaths, out lineActive);
    }
    
    public PathCreator GetCircularPath(Vector3 position, out bool lineActive)
    {
        return GetPath(position, circularPaths, out lineActive);
    }
    
    private PathCreator GetPath(Vector3 position, List<PathCreator> paths, out bool lineActive)
    {
        PathCreator path = null;
        float shortestDistance = 500000;

        lineActive = false;

        foreach (PathCreator pathing in paths)
        {
            List<Vector3> points = pathing.bezierPath.GetAllPoints();

            for (int i = 0; i < points.Count; i++)
            {
                var currentDistance = (points[i] - position).sqrMagnitude;
               
                if (currentDistance < shortestDistance)
                {
                    print(pathing.name + " " + points[i] + " " +  position  + "  " + (points[i]  - position).magnitude);
                    if (currentDistance <= maxRange)
                        lineActive = true;
                    
                    path = pathing;
                    shortestDistance = currentDistance;
                }
            }
        }
        
        return path;
    }
}
