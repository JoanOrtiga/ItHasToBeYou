using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbietSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/CreakyWood");
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/Ambiet");
    }

   
}
