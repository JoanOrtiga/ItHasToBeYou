using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralsProperties : MonoBehaviour
{
    public int materialNumber;
    
   
    void Start()
    {
        
    }

  
    void Update()
    {
        if (this.gameObject.GetComponent<Object>().hasBeenPlaced == true)
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
    }
}
