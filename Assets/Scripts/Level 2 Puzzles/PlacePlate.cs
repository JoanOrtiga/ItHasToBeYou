using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlate : MonoBehaviour
{
    public bool hasBeenPlaced;
    public string placeSoundPath;

    private SphereCollider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = gameObject.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenPlaced)
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }
    }
}
