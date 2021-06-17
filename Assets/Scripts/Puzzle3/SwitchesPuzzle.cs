using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchesPuzzle : MonoBehaviour , IPuzzleSolver
{
    [SerializeField] private Switches[] allSwitchesMid;

    [SerializeField] private Animator animator;
    private Animator myAnimator;
    private bool x = true;
    public string GearLoopSolvedPath = "event:/INGAME/Puzzle 3/Mecanismos/MecanismoLoop";
    public GameObject soundPointGear;
    public GameObject soundPointTower;

    [SerializeField] private EndGame endGame;

    private PlayerController playerController;
    [SerializeField] private Transform lookAtMidRoom;

    private TextBox textboxNarrative;
    
    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
        textboxNarrative = GetComponent<TextBox>();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Solved() || Input.GetKeyDown(KeyCode.B))
        {
            if (!x)
                return;

            endGame.enabled = true;
            myAnimator.enabled = true;
            myAnimator.SetTrigger("Rotate");
            animator.SetTrigger("AlignToStars");
            this.enabled = false;
            FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 3/Escaleras Elevandose/Escalera", soundPointTower.transform.position);
            FMODUnity.RuntimeManager.PlayOneShot(GearLoopSolvedPath, soundPointGear.transform.position);
            textboxNarrative.StartTextPuzzle();


        }
    }

    public bool Solved()
    {
        for (int i = 0; i < allSwitchesMid.Length; i++)
        {
            if (!allSwitchesMid[i].Solved())
                return false;
        }
        
        return true;
    }
}