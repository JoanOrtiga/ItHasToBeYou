using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbietSound : MonoBehaviour
{

    public GameObject calderaUno;
    public GameObject calderaDos;
    public string SoundPathCaldera = "event:/INGAME/ObjectSounds/Caldera";
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/CreakyWood");
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/Ambiet");

        FMODUnity.RuntimeManager.PlayOneShot(SoundPathCaldera,calderaUno.transform.position);
        FMODUnity.RuntimeManager.PlayOneShot(SoundPathCaldera,calderaDos.transform.position);

        
    }

   
}
