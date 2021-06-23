using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerMovementController))]
[RequireComponent(typeof(PlayerCombatController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AnimationStateController))]
[RequireComponent(typeof(CharacterInfo))]
[RequireComponent(typeof(PlayerInteractController))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    private int id;

    public Transform player { get; private set;}
    public CharacterController characterController { get; private set; }
    public PlayerMovementController playerMovementController { get; private set; }
    public PlayerCombatController playerCombatController { get; private set; }
    public Animator animator { get; private set; }
    public AnimationStateController animationStateController { get; private set; }
    public CharacterInfo playerStats { get; private set; }
    public PlayerInteractController playerInteractController { get; private set; }
    public Inventory playerInv { get; private set; }

    void Awake()
    {
        player = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
        playerMovementController = GetComponent<PlayerMovementController>();
        playerCombatController = GetComponent<PlayerCombatController>();
        animator = GetComponent<Animator>();
        animationStateController = GetComponent<AnimationStateController>();
        playerStats = GetComponent<CharacterInfo>();
        playerInteractController = GetComponent<PlayerInteractController>();
        playerInv = GetComponent<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController.radius = 0.4f;
        characterController.height = 1.8f;
        characterController.center = new Vector3(0f, 0.9f, 0f);
        characterController.skinWidth = 0.01f;
        characterController.minMoveDistance = 0f;
        characterController.stepOffset = 0.3f;
        characterController.slopeLimit = 45f;
    }

    void Update()
    {
        //Check every update for if player died
        CheckPlayerDeath();
        //Check if player goes over max health
        OverHeal();
        //If coroutine to decrease health that is over max limit is active, and player goes to or under
        //Max health, stop the coroutine
        if(dischargeMaxHealthIsActive){
            if(playerStats.currentStats.CurrentHealth <= playerStats.currentStats.MaxHealth){
                StopCoroutine(dischargeMaxHealth);
            }
        }

        //Debug input to kill player
        if (InputListener.Instance.kill && playerStats.isAlive)
        {
            Damage(playerStats.currentStats.CurrentHealth);
            Debug.Log("Player Killed Self!");
            playerMovementController.enabled = false;
            playerCombatController.enabled = false;
            animationStateController.enabled = false;
        }  


    } 

    #region - Stats -

    private Coroutine dischargeMaxHealth;
    private bool dischargeMaxHealthIsActive = false;
    private void CheckPlayerDeath(){
        if(!playerStats.isAlive){ return; }
        if(playerStats.currentStats.CurrentHealth <= 0){
            playerStats.isAlive = false;
            GameEvents.Instance.PlayerDiedTriggerEnter();
        }
    }    

    public void ModifyMaxHealth(int maxHealthChange){
        playerStats.currentStats.MaxHealth += maxHealthChange;
        GameEvents.Instance.PlayerMaxHealthChangeEnter();
    }

    public void Heal(int healing){
        playerStats.currentStats.CurrentHealth += healing;
        GameEvents.Instance.PlayerHealingTriggerEnter();
    }

    public void Damage(int damage){
        playerStats.currentStats.CurrentHealth -= damage;
        CheckPlayerDeath();
        GameEvents.Instance.PlayerDamageTriggerEnter();
    }

    //If player health over max health
    public void OverHeal(){
        if(!(playerStats.currentStats.CurrentHealth > playerStats.currentStats.MaxHealth)){ return; }
        
        int healthDischargeRate = 2;
        float healthDischargeTime = 0.75f;

        dischargeMaxHealth = StartCoroutine(DecrementHealth(healthDischargeRate, healthDischargeTime));
    }

    IEnumerator DecrementHealth(int healthDischargeRate, float timeToDecrement){
        dischargeMaxHealthIsActive = true;
        yield return new WaitForSeconds(timeToDecrement);
        playerStats.currentStats.CurrentHealth -= healthDischargeRate;
        dischargeMaxHealthIsActive = false;
    }

    #endregion

}
