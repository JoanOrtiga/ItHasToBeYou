using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueSides : MonoBehaviour , IInteractable
{

    public MoveableStatue.Sides side;

    [SerializeField] private MoveableStatue statue;

    private Transform playerTransform;
    private PlayerController playerController;
    private CharacterController characterController;

    private bool ActiveSide;

    [SerializeField] private Transform positonChild;

    [SerializeField] private GameObject otherSide1;
    [SerializeField] private GameObject otherSide2;
    [SerializeField] private GameObject otherSide3;

    private bool deActivated = true;

    private bool cooldowned = false;
    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        characterController = FindObjectOfType<CharacterController>();
        playerTransform = playerController.transform;
    }

    public void Interact()
    {
        if(ActiveSide)
            return;

        if (deActivated == false)
            return;

        cooldowned = false;
        
        statue.ChangeSide(side);

        deActivated = false;
        ActiveSide = true;
        characterController.enabled = false;
        playerTransform.position = positonChild.position;
        playerTransform.forward = positonChild.forward;
        playerTransform.parent = positonChild;
        playerController.enabled = false;
        
        otherSide1.SetActive(false);
        otherSide2.SetActive(false);
        otherSide3.SetActive(false);

        StartCoroutine(Cooldown());

    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.01f);
        cooldowned = true;
    }

    private void Update()
    {
        if (ActiveSide)
        {
            if (Input.GetButtonDown("Interact") && cooldowned)
            {
                deActivated = false;
                
                ActiveSide = false;
                characterController.enabled = true;
                playerTransform.parent = null;
                playerController.enabled = true;
                
                otherSide1.SetActive(true);
                otherSide2.SetActive(true);
                otherSide3.SetActive(true);
            }
        }
        else
        {
            deActivated = true;
        }
    }
}
