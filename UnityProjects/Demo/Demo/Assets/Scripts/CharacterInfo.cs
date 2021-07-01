using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public bool isAlive { get; set; }
    public string characterName { get; private set; }

    private CharacterStats baseStats;
    public CharacterStats currentStats;    

    public string primaryName { get; private set; }
    public string secondaryName { get; private set; }
    public string specialName { get; private set; }

    public string primaryDescription { get; private set; }
    public string secondaryDescription { get; private set; }
    public string specialDescription { get; private set; }

    private void Awake()
    {
        InitializeStats();
        isAlive = true;
    }

    private void Start()
    {
        
        
        
    }

    private void InitializeBaseStats(){
        this.baseStats = new CharacterStats
        (
            0,      //Wallet
            100,    //Max Health
            20,     //Primary Damage
            40,     //Secondary Damage
            75,     //Special Damage
            0f,     //Primary Cooldown
            2f,     //Secondary Cooldown
            4f,     //Special Cooldown
            -1,     //Primary Max Uses (-1: No max uses)
            2,      //Secondary Max Uses
            1      //Special Max Uses
        );
    }

    private void InitializeCurrentStats(){
        this.currentStats.DeepCopy(baseStats);
    }

    private void InitializeStats(){
        InitializeBaseStats();
        InitializeCurrentStats();
    }

    public struct CharacterStats
    {
        public int Wallet { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int PrimaryDamage { get; set; }
        public int SecondaryDamage { get; set; }
        public int SpecialDamage { get; set; }
        public float PrimaryCooldown { get; set; }
        public float SecondaryCooldown { get; set; }
        public float SpecialCooldown { get; set; }
        public int PrimaryMaxUses { get; set; }
        public int SecondaryMaxUses { get; set; }
        public int SpecialMaxUses { get; set; }

        public CharacterStats(int wallet, int maxHealth, int primaryDamage, int secondaryDamage, 
                                int specialDamage, float primaryCooldown, float secondaryCooldown, float specialCooldown,
                                int primaryMaxUses, int secondaryMaxUses, int specialMaxUses)
        {
            Wallet = wallet;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            PrimaryDamage = primaryDamage;
            SecondaryDamage = secondaryDamage;
            SpecialDamage = specialDamage;
            PrimaryCooldown = primaryCooldown;
            SecondaryCooldown = secondaryCooldown;
            SpecialCooldown = specialCooldown;
            PrimaryMaxUses = primaryMaxUses;
            SecondaryMaxUses = secondaryMaxUses;
            SpecialMaxUses = specialMaxUses;                                                   
        }

        public void DeepCopy(CharacterStats original)
        {
            Wallet = original.Wallet;
            MaxHealth = original.MaxHealth;
            PrimaryDamage = original.PrimaryDamage;
            SecondaryDamage = original.SecondaryDamage;
            SpecialDamage = original.SpecialDamage;
            PrimaryCooldown = original.PrimaryCooldown;
            SecondaryCooldown = original.SecondaryCooldown;
            SpecialCooldown = original.SpecialCooldown;
            PrimaryMaxUses = original.PrimaryMaxUses;
            SecondaryMaxUses = original.SecondaryDamage;
            SpecialMaxUses = original.SpecialMaxUses;
        }
    }
}