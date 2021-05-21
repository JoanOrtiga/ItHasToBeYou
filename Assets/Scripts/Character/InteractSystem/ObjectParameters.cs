using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameters : MonoBehaviour
{
    
    public bool hasBeenPlaced;

    [HideInInspector] public bool isStartPlace = true;

    //public string callEventWhenPickedUp;

   
    public Vector3 startPos { get; private set; }
    [HideInInspector] public Quaternion startRot;

    //public bool falseWhenDropped = true;

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
