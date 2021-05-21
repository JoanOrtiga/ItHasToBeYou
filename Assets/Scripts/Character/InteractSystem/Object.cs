using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Vector3 rotation;
    public bool hasBeenPlaced;

    [HideInInspector] public bool isStartPlace = true;

    public Vector3 startPos { get; private set; }
    public Quaternion startRot;



    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void ReLocate()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }

  
}
