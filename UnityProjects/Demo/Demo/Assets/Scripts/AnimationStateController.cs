using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    PlayerMovementController movementController;

    void Awake()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementController>();
    }

    void Update()
    {
        animator.SetFloat("Speed", movementController.GetCurrentSpeed());
        animator.SetBool("isIdle", movementController.GetCurrentSpeed() == 0);
        animator.SetBool("isGrounded", movementController.GetGrounded());
        if (movementController.GetJumping())
        {
            animator.SetTrigger("isJump");
        } 
    }

    //Check if animator is playing any information
    public bool AnimatorIsPlaying(int layer)
    {
        return animator.GetCurrentAnimatorStateInfo(layer).length >
               animator.GetCurrentAnimatorStateInfo(layer).normalizedTime;
    }

    //Check if animator is playing specific animation
    public bool AnimationInPlay(string animationName, int layer)
    {
        return AnimatorIsPlaying(layer) && animator.GetCurrentAnimatorStateInfo(layer).IsName(animationName);
    }

    //Check if animation ends
    public bool CheckAnimationEnd(string animationName, int layer) 
    {
        //If animation you check to end still in play, return false
        //If animation you check to end no longer in play, return true
        if (AnimationInPlay(animationName, layer))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
