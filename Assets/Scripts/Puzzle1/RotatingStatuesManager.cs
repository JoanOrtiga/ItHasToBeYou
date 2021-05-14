using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStatuesManager : MonoBehaviour , IPuzzleSolver
{
    [SerializeField] private RotateStatues[] statues;
    
    public bool Solved()
    {
        foreach (var statue in statues)
        {
            if (!statue.Solved())
                return false;
        }

        return true;
    }
}
