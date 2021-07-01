using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/EnemyBase")]
public class EnemyBase : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public int BaseHealth;
    public int BaseDamage;
    public int CriticalHitChance;
    public int BaseSpeed;
    public int BaseArmor;
    public float BaseAOERadius;
    public EnemyType[] Type;
    public EnemyDamageClass DamageClass;
    public object[] Drop;
    public int BaseXPGain;
    public List<AbilityBase> Attacks = new List<AbilityBase>();
}
