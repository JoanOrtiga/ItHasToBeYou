using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnAnimation : MonoBehaviour
{

    public string soundsPath;

    private VoiceSoundManager voiceSoundManager;

    private void Awake()
    {
        voiceSoundManager = FindObjectOfType<VoiceSoundManager>();
    }

    public void OpenDoorCinematic()
    {
        
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Cinematic/InitalOpenDoor");
    }

    public void CloseDoorCinematic()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Cinematic/InitialCloseDoor");
        
    }

    public void playSounds()
    {
        voiceSoundManager.Sound = FMODUnity.RuntimeManager.CreateInstance(soundsPath);
        voiceSoundManager.Sound.start();
        //FMODUnity.RuntimeManager.PlayOneShot(soundsPath);
    }

    public void StartFootSteps()
    {
       // InvokeRepeating("CallFootsteps", 0, 1f);
    }
    public void StopFootSteps()
    {
        CancelInvoke();
    }

    void CallFootsteps()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/Character/Foostep/FootWood");
      
    }

}
