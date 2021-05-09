using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserveObject : MonoBehaviour
{
    [HideInInspector]
    public bool isObserving = false;
    public string text;

    private void Update()
    {
        if (isObserving)
        {
            //RAYCAST
        }
    }
}
