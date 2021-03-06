using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class StatuesSolver : MonoBehaviour
{
    [SerializeField] private MovingStatue[] movingStatues;

    [SerializeField] private Transform die;

    [SerializeField] private Animator animator;

    [SerializeField] private Transform centralPoint;
    private PlayerController playerController;

    [SerializeField] private float radius;

    [SerializeField] private Transform lookAtMidRoom;
    [SerializeField] private float lookAtSpeed;

    
    private CanvasTutorial canvasTutorial;

    private bool x = true;

     public bool[] narativeStatues;

     public bool isSolved;

    private TextBox narrativaFinalPuzzle;
    private bool narrativeDone = false;

    public FMOD.Studio.EventInstance Sound;

    public GameObject changeEmmisive;
    
    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        canvasTutorial = FindObjectOfType<CanvasTutorial>();
        narrativaFinalPuzzle = GetComponent<TextBox>();
    }

   
    private void Update()
    {
        bool solved = true;
        foreach (var statue in movingStatues)
        {
            if (!statue.Solved())
            {
                solved = false;
            }
        }

        isSolved = solved;
        
        if (solved)
        {
            if (narrativeDone == false)
            {
                narrativeDone = true;
                narrativaFinalPuzzle.StartTextPuzzle();
                Sound = FMODUnity.RuntimeManager.CreateInstance("event:/INGAME/Puzzle 3/EarthQuake");
                Sound.start();
                CamaraShake.ShakeOnce(20, 5f, new Vector3(0.1f, 0.1f));
            }
            
            if ((playerController.transform.position - centralPoint.position).sqrMagnitude > radius * radius)
            {
                if (!x)
                    return;
                animator.SetTrigger("OpenDoor");
                StartCoroutine(LookAt());
                StartCoroutine(Die());
                x = false;
                FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 3/Escaleras Elevandose/Escalera",
                    this.gameObject.transform.position);
                //CamaraShake.ShakeOnce(12, 3, new Vector3(0.35f, 0.35f));
                canvasTutorial.TutorialPuzzle31(false);
                changeEmmisive.SetActive(true);
                
            }
        }
    }

    IEnumerator LookAt()
    {
        playerController.DisableController(true,false,true,false);
        
       /* while (!playerController.cameraController.LookAt(lookAtMidRoom.position, lookAtSpeed))
        {
            yield return null; 
        }*/

        yield return new WaitForSeconds(7f);

        Sound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        
      //  playerController.cameraController.ResetDesires();

        playerController.EnableController(true,false,true,false);
    }

    private void OnDrawGizmosSelected()
    {
        if (centralPoint != null)
            Gizmos.DrawWireSphere(centralPoint.position, radius);
    }

    private IEnumerator Die()
    {
        for (int i = 0; i < 500; i++)
        {
            die.position = die.position - (Vector3.up * Time.deltaTime);
            yield return null;
        }

        this.enabled = false;
    }
}