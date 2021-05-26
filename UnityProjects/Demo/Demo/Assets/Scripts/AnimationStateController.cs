using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animatorController;
    PlayerMovementControllerImproved movementController;
    PlayerCombatController combatController;
    InputListener inputController;

    void Awake()
    {
        animatorController = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementControllerImproved>();
        combatController = GetComponent<PlayerCombatController>();
        inputController = GameObject.Find("GameManager").GetComponent<InputListener>();
    }

    // Update is called once per frame
    void Update()
    {  
        animatorController.SetFloat("Speed", movementController.getCurrentSpeed());
        animatorController.SetBool("isIdle", movementController.getCurrentSpeed() == 0);  
        if (movementController.getJumping())
        {
            animatorController.SetTrigger("isJump");
        } 
        animatorController.SetBool("isGrounded", movementController.getGrounded());
        animatorController.SetBool("isPrimary", combatController.getPrimary());
        animatorController.SetBool("isSecondary", combatController.getSecondary());
        animatorController.SetBool("isSpecial", combatController.getSpecial());
    }

    //Check if animator is playing any information
    public bool AnimatorIsPlaying(int layer)
    {
        return animatorController.GetCurrentAnimatorStateInfo(layer).length >
               animatorController.GetCurrentAnimatorStateInfo(layer).normalizedTime;
    }

    //Check if animator is playing specific animation
    public bool AnimationInPlay(string animationName, int layer)
    {
        return /*AnimatorIsPlaying(layer) &&*/ animatorController.GetCurrentAnimatorStateInfo(layer).IsName(animationName);
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
    /*
     * Potentially for blending animations
    if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("TallPerson"))
    {
        personAnimation.Blend(anmName, targetweight, fadeLength);
    }
    */
}
