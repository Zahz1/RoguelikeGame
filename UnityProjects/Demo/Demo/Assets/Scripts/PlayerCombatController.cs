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
    PlayerMovementControllerImproved movementController;
    AnimationStateController animationController;

    //Attack types
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool primaryAttack = false;
    [SerializeField] private bool secondaryAttack = false;
    [SerializeField] private bool specialAttack = false;

    //Attack states
    [SerializeField] private int primaryAttackState = 0;
    private bool secondaryAttackState = false;
    private bool specialAttackState = false;

    //Animation States
    [SerializeField] private bool attackAnimationInProgress;
    [SerializeField] private bool primaryAttackState0Animation;
    [SerializeField] private bool primaryAttackState1Animation;
    [SerializeField] private bool secondaryAttackAnimation;
    [SerializeField] private bool specialAttackAnimation;

    //Attack state delay
    //Used to decide if attack will transition to next state
    //Or reset 
    private float primaryAttackTransitionDelay = 1f;

    //Coroutines
    private Coroutine primaryAttackState0Routine;
    private Coroutine primaryAttackState1Routine;
    private Coroutine secondaryAttackRoutine;
    private Coroutine specialAttackRoutine;

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
    private float secondaryCoolDown = 5f;
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
        inputController = GameObject.Find("GameManager").GetComponent<InputListener>();
        movementController = GetComponent<PlayerMovementControllerImproved>();
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
        //Input
        //Only get input if game is not paused
        if (!PauseController.GameIsPaused)
        {
            isAttacking = inputController.getAttack();
            specialAttack = inputController.getSpecialAttack() && specialEnabled && specialCurrentUses > 0f;
            secondaryAttack = inputController.getSecondaryAttack() && !specialAttack && secondaryEnabled && secondaryCurrentUses > 0f;
            primaryAttack = inputController.getPrimaryAttack() && !secondaryAttack && !specialAttack && !attackAnimationInProgress;
        }
        
        primaryAttackState0Animation = animationController.AnimationInPlay("PrimaryState0", 0);
        primaryAttackState1Animation = animationController.AnimationInPlay("PrimaryState1", 0);
        secondaryAttackAnimation = animationController.AnimationInPlay("Secondary", 0);
        specialAttackAnimation = animationController.AnimationInPlay("Special", 0);

        //True if a single attack animation in progress
        attackAnimationInProgress = primaryAttackState0Animation || primaryAttackState1Animation || secondaryAttackAnimation || specialAttackAnimation;

        //Primary Attack
        //Check for primary attack input and if attack animation still in progress
        //Cycle to first attack state when final attack state in completed
        //When final attack state animation is over, reset states
        //Continue this ladder for more attack states
        if (primaryAttack)
        {
            if (!attackAnimationInProgress)
            {
                if(primaryAttackState == 0 || primaryAttackState == 2)
                {
                    primaryAttackState = 1;
                }
                else if(primaryAttackState == 1)
                {
                    primaryAttackState = 2;
                }
            }
        }
        //If primary attack state used, start coroutine for reset
        //Done outside if(primaryAttack) so it continues to run when not primary attacking
        //Continue this ladder for more attack states
        if (primaryAttackState == 1)
        {
            primaryAttackState0Routine = StartCoroutine(primaryAttackStateReset());
        }
        else if(primaryAttackState == 2)
        {
            StopCoroutine(primaryAttackStateReset());
        }

        //Secondary Attack
        if (secondaryAttack && !attackAnimationInProgress)
        {
            secondaryAttackState = true;
            secondaryEnabled = false;
            secondaryCurrentUses--;
        }
        if (!secondaryAttack)
        {
            Invoke("secondaryStateReset", 0f);
        }

        //Special Attack
        if(specialAttack && !attackAnimationInProgress)
        {
            specialAttackState = true;
            specialEnabled = false;
            specialCurrentUses--;
        }
        if (!specialAttack)
        {
            Invoke("specialStateReset", 0f);
        }

        //If current uses for attacks are lower than max uses,
        //Start cooldowns to add uses
        //Primary
        
        //Secondary
        //Coroutine to begin secondary attack cooldown will start if current uses
        //is less than max uses, or secondaryEnabled == false, both should be the case
        //unless max uses increases
        //Also to begin cooldown, last cooldown/coroutine must not be active
        if((secondaryCurrentUses < secondaryMaxUses || !secondaryEnabled) && !secondaryRoutineActive)
        {
            secondaryAttackRoutine = StartCoroutine(secondaryAttackReset());
        }
        //Special
        //Coroutine to begin special attack cooldown will start if current uses
        //is less than max uses, or specialEnabled == false, both should be the case
        //unless max uses increases
        if((specialCurrentUses < specialMaxUses || !specialEnabled) && !specialRoutineActive)
        {
            specialAttackRoutine = StartCoroutine(specialAttackReset());
        }

        //Incase current uses of attacks are replenished without the use of cooldowns
        //Through items or some other method, stop cooldown coroutines if active
        //Secondary
        if(secondaryCurrentUses == secondaryMaxUses && secondaryRoutineActive)
        {
            StopCoroutine(secondaryAttackRoutine);
        }
        //Special
        if(specialCurrentUses == specialMaxUses && specialRoutineActive)
        {
            StopCoroutine(specialAttackRoutine);
        }
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

    public bool getPrimary()
    {
        return primaryAttack;
    }

    public int getPrimaryState()
    {
        return primaryAttackState;
    }

    public bool getSecondary()
    {
        return secondaryAttackState;
    }

    public bool getSpecial()
    {
        return specialAttackState;
    }

    private void secondaryStateReset()
    {
        secondaryAttackState = false;
    }

    private void specialStateReset()
    {
        specialAttackState = false;
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
