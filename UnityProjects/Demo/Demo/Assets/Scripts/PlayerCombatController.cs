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
    private bool specialAttack = false;

    //Attack states
    private float primaryAttackState = 0f;
    private bool secondaryAttackState = false;
    private bool specialAttackState = false;

    //Animation States
    private bool attackAnimationInProgress;
    private bool primaryAttackState0Animation;
    private bool primaryAttackState1Animation;
    private bool secondaryAttackAnimation;
    private bool specialAttackAnimation;

    //Attack state delay
    //Used to decide if attack will transition to next state
    //Or reset 
    private float primaryAttackTransitionDelay = 0.5f;

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
        isAttacking = inputController.getAttack();
        secondaryAttack = inputController.getSecondaryAttack();
        specialAttack = inputController.getSpecialAttack();
        primaryAttack = inputController.getPrimaryAttack() && !secondaryAttack && !specialAttack;

        //Check if any attack animations are in progress
        primaryAttackState0Animation = animationController.AnimationInPlay("PrimaryState0");
        primaryAttackState1Animation = animationController.AnimationInPlay("PrimaryState1");
        secondaryAttackAnimation = animationController.AnimationInPlay("Secondary");
        specialAttackAnimation = animationController.AnimationInPlay("Special");

        //True if a single attack animation in progress
        attackAnimationInProgress = primaryAttackState0Animation || primaryAttackState1Animation || secondaryAttackAnimation;

        //Primary Attack
        //Check for primary attack input and if attack animation still in progress
        //Cycle to first attack state when final attack state in completed
        //When final attack state animation is over, reset states
        /*if (primaryAttack)
        {
            if (!attackAnimationInProgress)
            {
                break;
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

        
         * If more attack states are added
        if (primaryAttackState1)
        {
            primaryAttackState1Routine = StartCoroutine(primaryAttackState1Reset());
        }
        
        
        //Secondary
        if (secondaryAttack)
        {

        }*/

    }
    /*
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

    public bool getSecondaryAttack()
    {
        return secondaryAttack;
    }

    private void startPrimaryDelay()
    {
        StartCoroutine(startPrimaryDelay());
    }

    IEnumerator primaryAttackStateReset()
    {
        yield return new WaitForSeconds(primaryAttackTransitionDelay);
        primaryAttackState = 0;
    }

    float secondaryDelay = 0;

    IEnumerator secondaryAttackReset()
    {
        yield return new WaitForSeconds(secondaryDelay);
        secondaryAttackState = false;
    }*/
}
