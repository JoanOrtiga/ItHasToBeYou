using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoorCinematic()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Cinematic/InitalOpenDoor");
    }

    public void CloseDoorCinematic()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Cinematic/InitialCloseDoor");
        
    }
}
