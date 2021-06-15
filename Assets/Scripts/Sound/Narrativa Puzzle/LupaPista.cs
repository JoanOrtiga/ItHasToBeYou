using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LupaPista : MonoBehaviour
{
    public bool pickUpLupa = false;
    float time = 0;
    public float timeForClue = 120;

    private void Update()
    {
        if (pickUpLupa == true)
        {
            time += Time.deltaTime;
            if (time > timeForClue)
            {
                this.gameObject.GetComponent<TextBox>().StartTextPuzzle();
            }
        }
    }
}
