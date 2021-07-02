using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    //Required Components
    private PlayerController playerController;
    private CharacterInfo playerStats;

    //Player Weapons
    Item primaryWeapon { get; set; }

    //Attacks
    private bool isPrimary = false;
    private bool isSecondary = false;
    private bool isSpecial = false;
    
    //Attack damage
    private int primaryDamage;
    private int secondaryDamage;
    private int specialDamage;

    private void Awake(){
        playerController = GetComponent<PlayerController>();
        playerStats = playerController.playerStats;
        GameEvents.Instance.OnPlayerAttackMaxUseChangeEnter += UpdateMaxUses;

        GameEvents.Instance.OnPlayerAttackCooldownCancelEnter += CancelAllCooldowns;
        GameEvents.Instance.OnPlayerAttackCooldownTimeChangeEnter += UpdateCooldown;
    }

    private void Start(){
        UpdateDamage();
        UpdateCooldown();
        UpdateMaxUses();
        UpdateCurrentUses();
    }

    private void Update(){
        ActivateCooldowns();
    }

    #region - Combat -

    private delegate void PrimaryAttack();
    private delegate void SecondaryAttack();
    private delegate void SpecialAttack();

    PrimaryAttack primaryAttack;
    SecondaryAttack secondaryAttack;
    SpecialAttack specialAttack;

    
    private void PrimaryAttack1(){
        
    }

    private void PrimaryAttack2(){

    }

    private void SecondaryAttack1(){

    }

    private void SecondaryAttack2(){

    }

    private void SpecialAttack1(){

    }

    private void SpecialAttack2(){

    }

    private void InitializeAttacks(){
        primaryAttack = PrimaryAttack1;
        secondaryAttack = SecondaryAttack1;
        specialAttack = SpecialAttack1;
    }

    private void UsePrimary(){
        if(PrimaryCurrentUses == 0){ return; } //No uses remaining
        primaryAttack();
        if(PrimaryMaxUses == -1){ return; } //No max uses
        PrimaryCurrentUses--;
    }

    private void UseSecondary(){
        if(SecondaryCurrentUses == 0){ return; }
        secondaryAttack();
        if(SecondaryMaxUses == -1){ return; }
        SecondaryCurrentUses--;
    }

    private void UseSpecial(){
        if(SpecialCurrentUses == 0){ return; }
        specialAttack();
        if(SpecialMaxUses == -1){ return; }
        SpecialCurrentUses--;
    }

    #endregion

    #region - Attack Damage -

    private void UpdateDamage(){
        primaryDamage = playerStats.currentStats.PrimaryDamage;
        secondaryDamage = playerStats.currentStats.SecondaryDamage;
        specialDamage = playerStats.currentStats.SecondaryDamage;
    }

    #endregion

    #region - Attack Uses -
    // Max uses
    public int PrimaryMaxUses { get; private set; }
    public int SecondaryMaxUses { get; private set; }
    public int SpecialMaxUses { get; private set; }
    // Current uses
    public int PrimaryCurrentUses { get; private set; }
    public int SecondaryCurrentUses { get; private set; }
    public int SpecialCurrentUses { get; private set; }

    private void UpdateMaxUses(){
        PrimaryMaxUses = playerStats.currentStats.PrimaryMaxUses;
        SecondaryMaxUses = playerStats.currentStats.SecondaryMaxUses;
        SpecialMaxUses = playerStats.currentStats.SpecialMaxUses;
    }

    private void UpdateCurrentUses(){
        PrimaryCurrentUses = PrimaryMaxUses;
        SecondaryCurrentUses = SecondaryMaxUses;
        SpecialCurrentUses = SpecialMaxUses;
    }

    #endregion

    #region - Cooldowns -

    private Coroutine primaryAttackCooldownRoutine;
    private Coroutine secondaryAttackCooldownRoutine;
    private Coroutine specialAttackCooldownRoutine;
    private bool primaryAttackCooldownIsActive = false;
    private bool secondaryAttackCooldownIsActive = false;
    private bool specialAttackCooldownIsActive = false;
    private float primaryAttackCooldown;
    private float secondaryAttackCooldown;
    private float specialAttackCooldown;

    private void ActivatePrimaryCooldown(){
        if(primaryAttackCooldownIsActive){ return; } //If cooldown is active, don't start new cooldown
        if(PrimaryCurrentUses == PrimaryMaxUses) { return; } //Ability already at max uses
        primaryAttackCooldownRoutine = StartCoroutine(PrimaryAttackCooldown());
    }

    private void ActivateSecondaryCooldown(){
        if(secondaryAttackCooldownIsActive){ return; }
        if(SecondaryCurrentUses == SecondaryMaxUses){ return; }
        secondaryAttackCooldownRoutine = StartCoroutine(SecondaryAttackCooldown());
    }

    private void ActivateSpecialCooldown(){
        if(specialAttackCooldownIsActive){ return; }
        if(SpecialCurrentUses == SpecialMaxUses){ return; }
        specialAttackCooldownRoutine = StartCoroutine(SpecialAttackCooldown());
    }

    private void ActivateCooldowns(){
        ActivatePrimaryCooldown();
        ActivateSecondaryCooldown();
        ActivateSpecialCooldown();
    }

    public void StopPrimaryCooldown(){
        if(primaryAttackCooldownIsActive){
            StopCoroutine(primaryAttackCooldownRoutine);
            PrimaryCurrentUses++;
            primaryAttackCooldownIsActive = false;
        }
    }

    public void StopSecondaryCooldown(){
        if(secondaryAttackCooldownIsActive){
            StopCoroutine(secondaryAttackCooldownRoutine);
            SecondaryCurrentUses++;
            secondaryAttackCooldownIsActive = false;
        }
    }

    public void StopSpecialCooldown(){
        if(specialAttackCooldownIsActive){
            StopCoroutine(specialAttackCooldownRoutine);
            SpecialCurrentUses++;
            specialAttackCooldownIsActive = false;
        }
    }

    public void CancelAllCooldowns(){
        StopPrimaryCooldown();
        StopSpecialCooldown();
        StopSpecialCooldown();
    }

    private void UpdateCooldown(){
        primaryAttackCooldown = playerStats.currentStats.PrimaryCooldown;
        secondaryAttackCooldown = playerStats.currentStats.SecondaryCooldown;
        specialAttackCooldown = playerStats.currentStats.SpecialCooldown;
    }

    private IEnumerator PrimaryAttackCooldown(){
        primaryAttackCooldownIsActive = true;
        yield return new WaitForSeconds(primaryAttackCooldown);
        PrimaryCurrentUses++;
        primaryAttackCooldownIsActive = false;
    }

    private IEnumerator SecondaryAttackCooldown(){
        secondaryAttackCooldownIsActive = true;
        yield return new WaitForSeconds(secondaryAttackCooldown);
        SecondaryCurrentUses++;
        secondaryAttackCooldownIsActive = false;
    }

    private IEnumerator SpecialAttackCooldown(){
        specialAttackCooldownIsActive = true;
        yield return new WaitForSeconds(specialAttackCooldown);
        SpecialCurrentUses++;
        specialAttackCooldownIsActive= false;
    }

    #endregion
}
