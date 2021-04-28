using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceMaterial : MonoBehaviour
{
    public GameObject puzzle;
    public bool hasBeenPlaced;
    public int materialNumber;
    public bool correctMaterial;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hasBeenPlaced && correctMaterial == true)
        {
            puzzle.GetComponent<InteractPlanetarium>().allClues[materialNumber - 1] = true;
        }

        if (hasBeenPlaced ==false)
        {
            correctMaterial = false;
            puzzle.GetComponent<InteractPlanetarium>().allClues[materialNumber - 1] = false;
        }
    }
}
