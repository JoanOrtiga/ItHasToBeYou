using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotation : StateMachineBehaviour
{
    [SerializeField] private Vector3 objectiveEulerRot;
    //private Vector3 defaultRot;

    [SerializeField] private Vector3 objectivePosition;
    //private Vector3 defaultPos;

    private AnimationEventHand animationEventHand;

    [SerializeField] private string handTag = "RightHand";
    
    private void Awake()
    {
        animationEventHand = GameObject.FindGameObjectWithTag(handTag).GetComponent<AnimationEventHand>();
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        //defaultRot = animator.transform.localEulerAngles;
        
        animationEventHand.handRotation = animator.transform.localEulerAngles;
        animator.transform.localEulerAngles = (objectiveEulerRot);

        //defaultPos = animator.transform.localPosition;
        
        animationEventHand.handPosition = animator.transform.localPosition;
        animator.transform.localPosition = objectivePosition;
        
        animator.Update(0.0f);
        
        Debug.Log(handTag);
        
        Debug.Log(animator.transform.localPosition);
        Debug.Log(animator.transform.localRotation);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  /*  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.localEulerAngles = defaultRot;
        animator.transform.localPosition = defaultPos;
    }*/

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
