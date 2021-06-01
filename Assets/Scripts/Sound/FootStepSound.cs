using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [Header("Sounds path")]
    public string FootstepWood;
    public string FootstepStairsStone;
    public string FootstepStairsMetal;
 

    public GameObject pivot;
  
    public float walkSpeed = 0.5f;
    private PlayerMovement player;
    private bool playerIsMoving;


    [HideInInspector] public bool walkWood;
    [HideInInspector] public bool stairsStone;
    [HideInInspector] public bool stairsMetal;

    [Header("Raycast")]
    [SerializeField] private float checkDistance = 0.5f;
    [SerializeField] private LayerMask DetectLayerMask;
    Ray ray;
    RaycastHit rayCastHit;
    bool hitted = false;

    private int woodLayer;
    private int stairsLayer;


    void Start()
    {
        walkWood = true;
        player = GetComponent<PlayerMovement>();
        InvokeRepeating("CallFootsteps", 0, walkSpeed);

        woodLayer = LayerMask.NameToLayer("Wood");
        stairsLayer = LayerMask.NameToLayer("Stairs");
    }



    void Update()
    {
        
        PlayerMoving();


        ray = new Ray(pivot.transform.position, Vector3.down);
        hitted = Physics.Raycast(ray, out rayCastHit, checkDistance, DetectLayerMask.value);

        Debug.DrawRay(pivot.transform.position, Vector3.down * checkDistance);


        if (hitted == true)
        {
           
            if (rayCastHit.transform.gameObject.layer == woodLayer)
            {
                walkWood = true;
                stairsMetal = false;
                stairsStone = false;
            }
            else if (rayCastHit.transform.gameObject.layer == stairsLayer)
            {
                if (rayCastHit.transform.gameObject.CompareTag("Stone"))
                {
                    stairsStone = true;
                    walkWood = false;
                    stairsMetal = false;
                }
                else if (rayCastHit.transform.gameObject.CompareTag("Metal"))
                {
                    stairsStone = false;
                    walkWood = false;
                    stairsMetal = true;
                }
               

                
            }
        }
        else
        {
            stairsStone = false;
            walkWood = false;
            stairsMetal = false;
        }
       
      

    }



    void PlayerMoving()
    {
        if (player.inputVector.x > 0 || player.inputVector.y > 0 || player.inputVector.x < 0 || player.inputVector.y < 0)
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
            else if (stairsStone)
            {
                FMODUnity.RuntimeManager.PlayOneShot(FootstepStairsStone);
            }
            else if (stairsMetal)
            {
                FMODUnity.RuntimeManager.PlayOneShot(FootstepWood);

            }

        }
    }


}
