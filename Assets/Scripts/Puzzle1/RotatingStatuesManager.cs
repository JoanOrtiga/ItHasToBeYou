using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStatuesManager : MonoBehaviour , IPuzzleSolver
{
    public Transform secretDoor; 
    [SerializeField] private RotateStatues[] statues;
    public string finishPuzzlePath;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CamaraShake.ShakeOnce(3, 3, new Vector3(0.1f, 0.1f));

        }
    }

    public bool Solved()
    {
        foreach (var statue in statues)
        {
            if (!statue.Solved())
                return false;
        }

        FMODUnity.RuntimeManager.PlayOneShot(finishPuzzlePath, secretDoor.position);
        CamaraShake.ShakeOnce(3, 3);
        return true;
    }
}
