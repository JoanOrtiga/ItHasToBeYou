using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{

    public string FootstepWood ;
    public string FootstepStairsStone;
    public string FootstepStairsMetal;

    private PlayerMovement player;

    private bool playerIsMoving;
    public float walkSpeed;

    
    [HideInInspector] public bool walkWood;
    [HideInInspector] public bool upStearsWood;
    [HideInInspector] public bool upStearsStone;
    [HideInInspector] public bool upStearsMetal;

    // Start is called before the first frame update
    void Start()
    {
        walkWood = true;
        player = GetComponent<PlayerMovement>();
        InvokeRepeating("CallFootsteps", 0, walkSpeed);
    }



    void Update()
    {
        if (player.inputVector.x > 0 || player.inputVector.y > 0)
        {

            playerIsMoving = true;
        }
        else
        {
            playerIsMoving = false;
        }
    }




    void CallFootsteps()
    {
        if (playerIsMoving)
        {
            if (walkWood)
            {
                FMODUnity.RuntimeManager.PlayOneShot(FootstepWood);
            }
            else if (upStearsWood)
            {
                FMODUnity.RuntimeManager.PlayOneShot(FootstepWood);
            }
            else if (upStearsStone)
            {
                FMODUnity.RuntimeManager.PlayOneShot(FootstepStairsStone);
            }
            else if (upStearsMetal)
            {
                FMODUnity.RuntimeManager.PlayOneShot(FootstepWood);

            }

        }
    }

}
