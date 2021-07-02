using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    private Animator enemyAnimator;

    [SerializeField]
    private EnemyBase enemyBase;

    public EnemyStats enemyStats;
    private void Awake(){
        OnSpawn();
    }

    private void Start(){
        GameDifficulty gameDifficulty = GameManager.Instance.DifficultyModifier;
        Debug.Log(gameDifficulty);
    }

    #region - OnKill -

    private void OnKill(){
        
    }

    private void OnDestroy(){

    }

    #endregion

    #region - Enemy Stats -

    private void OnSpawn(){
        int difficultyLevel = GameManager.Instance.GameDifficultyLevel;
        int gameStage = GameManager.Instance.GameStage;
        GameDifficulty gameDifficulty = GameManager.Instance.DifficultyModifier;

        enemyStats = new EnemyStats(CalculateHealth(difficultyLevel, gameStage, gameDifficulty),
                                    CalculateDamage(difficultyLevel, gameStage, gameDifficulty),
                                    enemyBase.BaseSpeed,
                                    CalculateArmor(difficultyLevel, gameStage, gameDifficulty),
                                    enemyBase.BaseAOERadius,
                                    enemyBase.Drop,
                                    CalculateXPGain(difficultyLevel, gameStage, gameDifficulty));
    }

    private int CalculateHealth(int DifficultyLevel, int GameStage, GameDifficulty DifficultyModifier){
        int health = enemyBase.BaseHealth;
        float difficultyLevelModifer = DifficultyLevel / 15;
        float gameDifficultyHealthModifier = GetGameDifficultyModifier(1f, 1.3f, 1.5f, 2f, DifficultyModifier);
        health = (int)(health * gameDifficultyHealthModifier);
        health = (int)(health * difficultyLevelModifer);
        return health;
    }

    private int CalculateDamage(int DifficultyLevel, int GameStage, GameDifficulty DifficultyModifier){
        int damage = enemyBase.BaseDamage;
        float difficultyLevelModifier = DifficultyLevel / 15;
        float gameDifficultyDamageModifier = GetGameDifficultyModifier(1f, 1.2f, 1.5f, 2.2f, DifficultyModifier);
        damage = (int)(damage * gameDifficultyDamageModifier);
        damage = (int)(damage * difficultyLevelModifier);
        return damage;
    }

    private int CalculateArmor(int DifficultyLevel, int GameStage, GameDifficulty DifficultyModifier){
        int armor = enemyBase.BaseArmor;
        float difficultyLevelModifier = DifficultyLevel / 15;
        float gameDifficultyArmorModifier = GetGameDifficultyModifier(1f, 1.2f, 1.5f, 2f, DifficultyModifier);
        armor = (int)(armor * gameDifficultyArmorModifier);
        armor = (int)(armor * difficultyLevelModifier);
        return armor;
    }

    private void CalculateDrop(int DifficultyLevel, int GameStage, GameDifficulty DifficultyModifier){
        object[] drop = enemyBase.Drop;

    }

    private int CalculateXPGain(int DifficultyLevel, int GameStage, GameDifficulty DifficultyModifier){
        int xpGain = enemyBase.BaseXPGain;
        float difficultyLevelModifier = DifficultyLevel / 15;
        float gameDifficultyXPGainModifier = GetGameDifficultyModifier(1f, 1.5f, 2f, 3.5f, DifficultyModifier);
        xpGain = (int)(xpGain * gameDifficultyXPGainModifier);
        xpGain = (int)(xpGain * difficultyLevelModifier);
        return xpGain;
    }

    private float GetGameDifficultyModifier(float Modifier1, float Modifier2, float Modifier3,
                                                float Modifier4, GameDifficulty DifficultyModifier){ 
        switch(DifficultyModifier){
            case GameDifficulty.Normal:
                return Modifier1;
            case GameDifficulty.Hard:
                return Modifier2;
            case GameDifficulty.Nightmare:
                return Modifier3;
            case GameDifficulty.Regret:
                return Modifier4;
            default:
                return 0f;
        }
    }

    public struct EnemyStats{
        public int Health { get; private set; }
        public int Damage { get; set; }
        public int Speed { get; set; }
        public int Armor { get; set; }
        public float AOERadius { get; private set; }
        public object[] Drop { get; private set; }
        public int XPGain { get; private set; }

        public EnemyStats(int Health, int Damage, int Speed, int Armor, float AOERadius, object[] Drop, int XPGain){
            this.Health = Health;
            this.Damage = Damage;
            this.Speed = Speed;
            this.Armor = Armor;
            this.AOERadius = AOERadius;
            this.Drop = Drop;
            this.XPGain = XPGain; 
        }
    }

    #endregion

    #region -  Attacks -
    
    public List<AbilityBase> Attacks;

    private void InitializeAttacks(){
        Attacks = enemyBase.Attacks;
    }

    #region - Combat -

    public void Damage(int DamageDelt){

    }

    private void DamageReductionFromArmor(int DamageDelt){
        
    }

    #endregion
    

    #endregion
}
