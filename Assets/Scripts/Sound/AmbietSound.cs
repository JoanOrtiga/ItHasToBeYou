using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbietSound : MonoBehaviour
{
    [Header("Objects")]
    public GameObject calderaUno;
    public GameObject calderaDos;
    public string SoundPathCaldera = "event:/INGAME/ObjectSounds/Caldera";

    [Header("Ambiet")]
    public GameObject[] windows;
    public string windowSoundPath;

    public GameObject[] torches;
    public string torchesSoundPath;


    // Start is called before the first frame update
    void Start()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/CreakyWood");
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/Ambiet");
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Sonido Ambiente/MusicBackground");

        FMODUnity.RuntimeManager.PlayOneShot(SoundPathCaldera,calderaUno.transform.position);
        FMODUnity.RuntimeManager.PlayOneShot(SoundPathCaldera,calderaDos.transform.position);

       

        for (int i = 0; i < windows.Length; i++)
        {
            FMODUnity.RuntimeManager.PlayOneShot(windowSoundPath, windows[i].transform.position);
        }

        for (int i = 0; i < torches.Length; i++)
        {
            FMODUnity.RuntimeManager.PlayOneShot(torchesSoundPath, torches[i].transform.position);
        }
    }

   
}
