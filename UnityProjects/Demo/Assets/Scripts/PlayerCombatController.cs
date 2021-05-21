using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    /* The PlayerCombat controller will grab input from InputListener 
     * to determine attack animation and logic to use
     * Things to do:
     * Determine whether it is a primary, secondary, tertiary, etc. attack
     * Determine if attack is cancellable, able to be performed while player-
     * still provides player movement input
     * Determine if an attack has multiple states, what state the attack is in-
     * and not allow transition to the next state until the animation is complete
     * Communicate with Aninmation Controller the attack speed to influence animation speed
     * 
     */

    //Needed components
    InputListener inputController;
    PlayerMovementController movementController;
    AnimationStateController animationController;

    //Attack types
    private bool isAttacking = false;
    private bool primaryAttack = false;
    private bool secondaryAttack = false;

    //Attack states
    private bool primaryAttackState0 = false;
    private bool primaryAttackState1 = false;
    private bool secondaryAttackState = false;

    //Movement state
    private bool isSprinting;
    private bool isWalking;
    private bool isIdle;
    private bool isJumping;

    //Animation States
    private bool attackAnimationInProgress;
    private bool primaryAttackState0Animation;
    private bool primaryAttackState1Animation;
    private bool secondaryAttackAnimation;

    //Attack state delay
    //Used to decide if attack will transition to next state
    //Or reset 
    private float primaryAttackTransitionDelay = 1.2f;

    //Coroutines
    private Coroutine primaryAttackState0Routine;
    private Coroutine primaryAttackState1Routine;

    void Awake()
    {
        inputController = GetComponent<InputListener>();
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponent<AnimationStateController>();
    }

    // Update is called once per frame
    void Update()
    {
        isSprinting = movementController.getSprinting();
        //Player cannot attack while sprinting
        if (!isSprinting)
        {
            isAttacking = inputController.getAttack();
            primaryAttack = inputController.getPrimaryAttack();
            secondaryAttack = inputController.getSecondaryAttack();

            //Check if any attack animations are in progress
            primaryAttackState0Animation = animationController.AnimationInPlay("PrimaryState0");
            primaryAttackState1Animation = animationController.AnimationInPlay("PrimaryState1");
            secondaryAttackAnimation = animationController.AnimationInPlay("Secondary");

            //True if a single attack animation in progress
            attackAnimationInProgress = primaryAttackState0Animation || primaryAttackState1Animation || secondaryAttackAnimation;

            //Primary Attack
            //Check for primary attack input and if attack animation still in progress
            //Cycle to first attack state when final attack state in completed
            //When final attack state animation is over, reset states
            if (!primaryAttackState1Animation)
            {
                primaryAttackState0 = false;
                primaryAttackState1 = false;
                StopCoroutine(primaryAttackState0Routine);
                StopCoroutine(primaryAttackState1Routine);
            }

            if (primaryAttack && !attackAnimationInProgress)
            {
                //Check if first primary attack stage
                //If primaryAttack0 is false, then it has not been
                //Completed, set to true
                if (!primaryAttackState0 && !primaryAttackState1)
                {
                    primaryAttackState0 = true;
                }

                //Check to perform second state of primaryAttack
                //If primaryAttack0 is true and animation is false, then it
                //is completed
                //primaryAttack1 has been used within specified time
                if (primaryAttackState0 && !primaryAttackState0Animation)
                {
                    primaryAttackState0 = false;
                    //Check if primaryAttack1 has been used incase of further states
                    if (!primaryAttackState1)
                    {
                        primaryAttackState1 = true;
                    }
                }


            }

            //Secondary Attack
            if (secondaryAttack && !attackAnimationInProgress)
            {
                secondaryAttackState = true;

            }

            // Attack cooldowns

            //Primary
            //If primaryAttack0 is performed, must recieve input again
            //Within primaryAttackTransitionTime to use the next stage
            //Else next use of primaryAttack will be primaryAttack0
            if (primaryAttackState0)
            {
                primaryAttackState0Routine = StartCoroutine(primaryAttackState0Reset());
            }

            /*
             * If more attack states are added
            if (primaryAttackState1)
            {
                primaryAttackState1Routine = StartCoroutine(primaryAttackState1Reset());
            }
            */

            //Secondary
            if (secondaryAttack)
            {

            }
        }

    }

    private void Attack()
    {
        //Set attack animation

        //Detect enemies in range of attack

        //Damage taken
    }

    public bool getAttacking()
    {
        return isAttacking;
    }

    public bool getPrimaryAttack()
    {
        return primaryAttack;
    }

    public bool getPrimaryAttackState0()
    {
        return primaryAttackState0;
    }

    public bool getPrimaryAttackState1()
    {
        return primaryAttackState1;
    }

    public bool getSecondaryAttack()
    {
        return secondaryAttack;
    }

    IEnumerator primaryAttackState0Reset()
    {
        yield return new WaitForSeconds(primaryAttackTransitionDelay);
        primaryAttackState0 = false;
    }

    IEnumerator primaryAttackState1Reset()
    {
        yield return new WaitForSeconds(primaryAttackTransitionDelay);
        primaryAttackState1 = false;
    }

    float secondaryDelay = 0;

    IEnumerator secondaryAttackReset()
    {
        yield return new WaitForSeconds(secondaryDelay);
        secondaryAttackState = false;
    }
}
