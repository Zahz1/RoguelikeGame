using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    /// <summary>
    /// This script is a blueprint for setting character names,
    /// attack types, cooldowns, names, etc.
    ///      
    /// If characters have different attack type variants, 
    /// potentially alternate animators can be set here
    /// </summary>


    //All values are private so no values can be set from outside
    //this script, values can only be retrieved through getters

    [SerializeField] private bool isAlive;

    private string characterName = "Prototype Character";
    private int characterWallet;

    private int currentHealth;
    private int baseMaxHealth = 100;
    private int basePrimaryDamage = 20;
    private int baseSecondaryDamage = 50;
    private int baseSpecialDamage = 100;
    private float basePrimaryCooldown = 0f;
    private float baseSecondaryCooldown = 2f;
    private float baseSpecialCooldown = 5f;
    private int basePrimaryMaxUses = -1; // If base max values set to -1, then there is unlimited uses
    private int baseSecondaryMaxUses = 2;
    private int baseSpecialMaxUses = 1;

    private int currentMaxHealth;
    private int currentPrimaryDamage;
    private int currentSecondaryDamage;
    private int currentSpecialDamage;
    private float currentPrimaryCooldown;
    private float currentSecondaryCooldown;
    private float currentSpecialCooldown;
    private int currentPrimaryMaxUses;
    private int currentSecondaryMaxUses;
    private int currentSpecialMaxUses;

    private string primaryName;
    private string secondaryName;
    private string specialName;

    private string primaryDescription;
    private string secondaryDescription;
    private string specialDescription;

    private void Awake()
    {
        
    }

    private void Start()
    {
        isAlive = true;
        characterWallet = 0;
        currentMaxHealth = baseMaxHealth;
        currentHealth = currentMaxHealth;
        currentPrimaryDamage = basePrimaryDamage;
        currentSecondaryDamage = baseSecondaryDamage;
        currentSpecialDamage = baseSpecialDamage;
        currentPrimaryCooldown = basePrimaryCooldown;
        currentSecondaryCooldown = baseSecondaryCooldown;
        currentSpecialCooldown = baseSpecialCooldown;
        currentPrimaryMaxUses = basePrimaryMaxUses;
        currentSecondaryMaxUses = baseSecondaryMaxUses;
        currentSpecialMaxUses = baseSpecialMaxUses;
    }

    private void Update()
    {
        if (CheckPlayerDeath() && isAlive)
        {
            isAlive = false;
            GameEvents.Instance.PlayerDiedTriggerEnter();
        }
        if (InputListener.Instance.GetDamageSelf())
        {
            Damage(25);
            Debug.Log(this.currentHealth);
        }
    }

    private bool CheckPlayerDeath()
    {
        if(this.currentHealth <= 0)
        {
            return true;
        }
        return false;
    }

    public void IncreaseMaxHealth(int healthIncrease)
    {
        this.currentMaxHealth += healthIncrease;
        GameEvents.Instance.PlayerMaxHealthIncreaseEnter();
    }

    public void DecreaseMaxHealth(int healthDecrease)
    {
        this.currentMaxHealth -= healthDecrease;
        GameEvents.Instance.PlayerMaxHealthDecreaseEnter();
    }

    public void Heal(int healing) //Heal
    {
        this.currentHealth += healing;
        GameEvents.Instance.PlayerHealingTriggerEnter();
    }

    public void Damage(int damage)
    {
        this.currentHealth -= damage;
        GameEvents.Instance.PlayerDamageTriggerEnter();
    }

    #region Getters
    public bool GetAlive() { return this.isAlive; }
    
    public string GetCharacterName() { return this.characterName; }
    
    public int GetCharacterWallet() { return this.characterWallet; }

    public int GetCurrentHealth() { return this.currentHealth; }
    public int GetCurrentMaxHealth() { return this.currentMaxHealth; }
    public int GetBaseMaxHealth() { return this.baseMaxHealth; }
    
    public int GetBasePrimaryDamage() { return this.basePrimaryDamage; }
    public int GetBaseSecondaryDamage() { return this.baseSecondaryDamage; }
    public int GetBaseSpecialDamage() { return this.baseSpecialDamage; }
    
    public float GetBasePrimaryCooldown() { return this.basePrimaryCooldown; }
    public float GetBaseSecondaryCooldown() { return this.baseSecondaryCooldown; }
    public float GetBaseSpecialCooldown() { return this.baseSpecialCooldown; }
    
    public int GetBasePrimaryMaxUses() { return this.basePrimaryMaxUses; }
    public int GetBaseSecondaryMaxUses() { return this.baseSecondaryMaxUses; }
    public int GetBaseSpecialMaxUses() { return this.baseSpecialMaxUses; }
    
    public int GetCurrentPrimaryDamange() { return this.currentPrimaryDamage; }
    public int GetCurrentSecondaryDamage() { return this.currentSecondaryDamage; }
    public int GetCurrentSpecialDamage() { return this.currentSpecialDamage; }

    public float GetCurrentPrimaryCooldown() { return this.currentPrimaryCooldown; }
    public float GetCurrentSecondaryCooldown() { return this.currentSecondaryCooldown; }
    public float GetCurrentSpecialCooldown() { return this.currentSpecialCooldown; }

    public int GetCurrentPrimaryMaxUses() { return this.currentPrimaryMaxUses; }
    public int GetCurrentSecondaryMaxUses() { return this.currentSecondaryMaxUses; }
    public int GetCurrentSpecialMaxUses() { return this.currentSpecialMaxUses; }

    public string GetPrimaryName() { return this.primaryName; }
    public string GetSecondaryName() { return this.secondaryName; }
    public string GetSpecialName() { return this.specialName; }

    public string GetPrimaryDescription() { return this.primaryDescription; }
    public string GetSecondaryDescription() { return this.secondaryDescription; }
    public string GetSpecialDescription() { return this.specialDescription; }
    #endregion

    #region Setters
    public void SetCurrentHealth(int health) { this.currentHealth = health; }
    #endregion
}
