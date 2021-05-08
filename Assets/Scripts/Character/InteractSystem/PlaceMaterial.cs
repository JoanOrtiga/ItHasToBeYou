using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceMaterial : MonoBehaviour
{
    public GameObject puzzle;
    public bool hasBeenPlaced;
    public int materialNumber;
    public bool correctMaterial;

 
    private bool textDone = false;
    private float time;
    private TextBox textBox;
    // Start is called before the first frame update
    void Start()
    {
        textBox = gameObject.GetComponent<TextBox>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (hasBeenPlaced && correctMaterial == true)
        {
            puzzle.GetComponent<InteractPlanetarium>().allClues[materialNumber - 1] = true;

            if (textDone == false)
            {
                textDone = true;
                textBox.StartText();
            }

       
        }

        if (hasBeenPlaced ==false)
        {
            correctMaterial = false;
            puzzle.GetComponent<InteractPlanetarium>().allClues[materialNumber - 1] = false;
        }
    }
}
