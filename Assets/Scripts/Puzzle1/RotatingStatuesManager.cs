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


        CamaraShake.ShakeOnce(3, 3);
        return true;
    }
}
