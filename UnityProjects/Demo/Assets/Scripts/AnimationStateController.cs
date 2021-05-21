using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animatorController;
    PlayerMovementControllerImproved movementController;
    InputListener inputController;

    [SerializeField] private float animatorSpeed;
    [SerializeField] private bool jump;
    [SerializeField] private bool isGrounded;

    void Awake()
    {
        animatorController = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementControllerImproved>();
        inputController = GetComponent<InputListener>();
    }

    // Update is called once per frame
    void Update()
    {
        animatorSpeed = movementController.getCurrentSpeed();
        jump = movementController.getJump();
        isGrounded = movementController.getGrounded();

        animatorController.SetFloat("Speed", movementController.getCurrentSpeed());
        animatorController.SetBool("isJumping", movementController.getJump());
        animatorController.SetBool("isGrounded", movementController.getGrounded());

    }

    //Check if animator is playing any information
    public bool AnimatorIsPlaying()
    {
        return animatorController.GetCurrentAnimatorStateInfo(0).length >
               animatorController.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    //Check if animator is playing specific animation
    public bool AnimationInPlay(string animationName)
    {
        return AnimatorIsPlaying() && animatorController.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    //Check if animation ends
    public bool CheckAnimationEnd(string animationName) 
    {
        //If animation you check to end still in play, return false
        //If animation you check to end no longer in play, return true
        if (AnimationInPlay(animationName))
        {
            return false;
        }
        else
        {
            return true;
        }
    }


}
