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
    private float primaryAttackTransitionDelay = 1f;

    //Coroutines
    private Coroutine primaryAttackState0Routine;
    private Coroutine secondaryAttackStateRoutine;
    private Coroutine specialAttackStateRoutine;

    //Coroutine running checks
    private bool primaryRoutineActive = false;
    private bool secondaryRoutineActive = false;
    private bool specialRoutineActive = false;

    //Attack delays
    //Such as cooldowns
    private bool primaryEnabled;
    private bool secondaryEnabled;
    private bool specialEnabled;

    private float primaryCoolDown = 0f;
    private float secondaryCoolDown = 2f;
    private float specialCoolDown = 5f;

    //Attack uses
    //If value is -1, then there is no max uses
    private int primaryMaxUses = -1;
    private int secondaryMaxUses = 2;
    private int specialMaxUses = 1;

    private int primaryCurrentUses;
    private int secondaryCurrentUses;
    private int specialCurrentUses;

    void Awake()
    {
        inputController = GetComponent<InputListener>();
        movementController = GetComponent<PlayerMovementController>();
        animationController = GetComponent<AnimationStateController>();
    }

    void Start()
    {
        primaryEnabled = true;
        secondaryEnabled = true;
        specialEnabled = true;
        primaryCurrentUses = primaryMaxUses;
        secondaryCurrentUses = secondaryMaxUses;
        specialCurrentUses = specialMaxUses;
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = inputController.getAttack();
        specialAttack = inputController.getSpecialAttack() && specialEnabled && specialCurrentUses > 0f;
        secondaryAttack = inputController.getSecondaryAttack() && !specialAttack && secondaryEnabled && secondaryCurrentUses > 0f;
        primaryAttack = inputController.getPrimaryAttack() && !secondaryAttack && !specialAttack;

        //Check if any attack animations are in progress
        primaryAttackState0Animation = animationController.AnimationInPlay("PrimaryState0");
        primaryAttackState1Animation = animationController.AnimationInPlay("PrimaryState1");
        secondaryAttackAnimation = animationController.AnimationInPlay("Secondary");
        specialAttackAnimation = animationController.AnimationInPlay("Special");

        //True if a single attack animation in progress
        attackAnimationInProgress = primaryAttackState0Animation || primaryAttackState1Animation || secondaryAttackAnimation || specialAttackAnimation;

        //Primary Attack
        //Check for primary attack input and if attack animation still in progress
        //Cycle to first attack state when final attack state in completed
        //When final attack state animation is over, reset states
        if (primaryAttack)
        {
            if (!attackAnimationInProgress)
            {
                if(primaryAttackState == 0f)
                {
                    primaryAttackState++;
                }else if(primaryAttackState == 1f)
                {
                    primaryAttackState++;
                }
            }
        }
        //If primary attack state used, start coroutine for reset
        //Done outside if(primaryAttack) so it continues to run when not primary attacking
        if(primaryAttackState == 1f)
        {
            primaryAttackState0Routine = StartCoroutine(primaryAttackStateReset());
        }else if(primaryAttackState == 2f)
        {
            StopCoroutine(primaryAttackStateReset());
            primaryAttackState = 0f;
        }

        //Secondary Attack
        if (secondaryAttack && !attackAnimationInProgress)
        {
            secondaryAttackState = true;
            secondaryEnabled = false;
            secondaryCurrentUses--;
        }   

        //Special Attack
        if(specialAttack && !attackAnimationInProgress)
        {
            specialAttackState = true;
            specialEnabled = false;
            specialCurrentUses--;
        }

        //If current uses for attacks are lower than max uses,
        //Start cooldowns to add uses
        //Primary
        
        //Secondary
        if(secondaryCurrentUses < secondaryMaxUses)
        {
            StartCoroutine(secondaryAttackReset());
        }
        //Special

    }
    
    private void Attack()
    {
        //Detect enemies in range of attack

        //Damage taken
    }

    public bool getAttacking()
    {
        return isAttacking;
    }

    public float getPrimaryAttack()
    {
        return primaryAttackState;
    }

    public bool getSecondaryAttack()
    {
        return secondaryAttackState;
    }

    public bool getSpecialAttack()
    {
        return specialAttackState;
    }

    IEnumerator primaryAttackStateReset()
    {
        primaryRoutineActive = true;
        yield return new WaitForSeconds(primaryAttackTransitionDelay);
        primaryAttackState = 0;
        primaryRoutineActive = false;
    }

    IEnumerator secondaryAttackReset()
    {
        secondaryRoutineActive = true;
        yield return new WaitForSeconds(secondaryCoolDown);
        secondaryEnabled = true;
        secondaryCurrentUses++;
        secondaryRoutineActive = false;
    }

    IEnumerator specialAttackReset()
    {
        specialRoutineActive = true;
        yield return new WaitForSeconds(specialCoolDown);
        specialEnabled = true;
        specialCurrentUses++;
        specialRoutineActive = false;
    }
}
