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
    private string characterName;

    private int maxHealth;

    private int primaryDamage;
    private int secondaryDamage;
    private int specialDamage;

    private float primaryCooldown;
    private float secondaryCooldown;
    private float specialCooldown;

    private int primaryMaxUses;
    private int secondaryMaxUses;
    private int specialMaxUses;

    private string primaryName;
    private string secondaryName;
    private string specialName;

    private string primaryDescription;
    private string secondaryDescription;
    private string specialDescription;

    //Getters
    public string getName()
    {
        return characterName;
    }

    public int getMaxHealth()
    {
        return maxHealth;
    }

    public int getPrimaryDamage()
    {
        return primaryDamage;
    }

    public int getSecondaryDamage()
    {
        return secondaryDamage;
    }

    public int getSpecialDamage()
    {
        return specialDamage;
    }

    public float getPrimaryCooldown()
    {
        return primaryCooldown;
    }

    public float getSecondarCooldown()
    {
        return secondaryCooldown;
    }

    public float getSpecialCooldown()
    {
        return specialCooldown;
    }

    public string getPrimaryName()
    {
        return primaryName;
    }

    public string getSecondaryName()
    {
        return secondaryName;
    }

    public string getSpecialName()
    {
        return specialName;
    }

    public string getPrimaryDescription()
    {
        return primaryDescription;
    }

    public string getSecondaryDescription()
    {
        return secondaryDescription;
    }

    public string getSpecialDescription()
    {
        return specialDescription;
    }
}
