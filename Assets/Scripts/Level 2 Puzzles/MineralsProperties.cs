using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralsProperties : MonoBehaviour
{
    public int materialNumber;
    public float weightMetal;
  
  
    void Update()
    {
        if (this.gameObject.GetComponent<ObjectParameters>().hasBeenPlaced == true)
        {

            if (transform.parent.gameObject.GetComponent<PlaceMaterial>()!= null)
            {
                if (transform.parent.gameObject.GetComponent<PlaceMaterial>().materialNumber == materialNumber)
                {
                    transform.parent.gameObject.GetComponent<PlaceMaterial>().correctMaterial = true;
                }
                else
                {
                    transform.parent.gameObject.GetComponent<PlaceMaterial>().correctMaterial = false;
                }
            }
            else if (transform.parent.gameObject.GetComponent<PlacePlate>() != null)
            {
            }
           
            
        }
    }
}
