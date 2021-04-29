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

    public GameObject textBox;
    public string text;
    public float timeDuration;
    private bool textDone = false;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
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
                time = 0;
            }

           

            if (time > timeDuration)
            {
 
                textBox.gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                textBox.GetComponent<Text>().text = text;
                textBox.gameObject.transform.parent.gameObject.SetActive(true);
                
            }
        }

        if (hasBeenPlaced ==false)
        {
            correctMaterial = false;
            puzzle.GetComponent<InteractPlanetarium>().allClues[materialNumber - 1] = false;
        }
    }
}
