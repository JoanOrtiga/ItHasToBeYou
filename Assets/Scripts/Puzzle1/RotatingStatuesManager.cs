using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStatuesManager : MonoBehaviour , IPuzzleSolver
{
    public Transform secretDoor; 
    [SerializeField] private RotateStatues[] statues;
    public string finishPuzzlePath;


    public bool Solved()
    {
        foreach (var statue in statues)
        {
            if (!statue.Solved())
                return false;
        }

        FMODUnity.RuntimeManager.PlayOneShot(finishPuzzlePath, secretDoor.position);

        this.gameObject.GetComponent<TextBox>().StartTextPuzzle();

        CamaraShake.ShakeOnce(3, 3, new Vector3(0.1f, 0.1f));

        statues[0].gameObject.tag = "Untagged";
        statues[1].gameObject.tag = "Untagged";
        statues[2].gameObject.tag = "Untagged";
        return true;
    }
}
