using System;
using UnityEngine;

public class BattleDataManager : MonoBehaviour
{
    [Serializable]
    public struct CannonImport
    {
        public int cannonId;
        public CannonBasicStats cannonImportInfo;
        //in case have more import
    }

    public struct PlayerStatsModifier
    {
        public float healthModifier;
        public float defenseModifier;
        public float speedModifier;
    }

    [Serializable]
    public struct EnemyBasicStats
    {
        public float enemySpiderHealth;
        public float enemySpiderDefense;
        public float enemySpiderAttack;
        public float enemySpiderAttackCoolDown;
        public float enemySpiderMoveSpeed;
        public float enemySpiderAnimationMoveSpeed;
        public float enemySpiderMinMoveSpeed;
        public Global.Misc.EnemyType enemySpiderEnemyType;
    }
    
    [Serializable]
    public struct BossStats
    {
        public float bossHealth;
        public float bossDefense;
        public float bossSpeedUpSkillAmount;
        public float bossSpeedUpSkillDuration;
        public int bossSpeedUpSkillNum;
        public float bossLaserDamage;
        public float bossSkillCoolDown;
        public float bossBodyAttack;
        public float bossBodyAttackLifeTime;
        public float bossBodyAttackCoolDown;
    }
    
    [Serializable]
    public struct EnemyStatsChaosMultiplier
    {
        public float enemySpiderHealthMultiplier;
        public float enemySpiderDefenseMultiplier;
        public float enemySpiderAttackMultiplier;
        public float enemySpiderAttackCoolDownMultiplier;
        public float enemySpiderMoveSpeedMultiplier;
    }
    
    [Serializable]
    public struct EnemyPriorityWeight
    {
        public float distance;
        public float cannonLocked;
        public float enemyTypeCreepMelee;
        public float enemyTypeCreepRange;
        public float enemyTypeElite;
        public float enemyTypeBoss;
    }
    
    [Serializable]
    public struct EnemySpawnRule
    {
        public float startChaosValue;
        public float energyBallIncreaseChaosAmount;
        public float frenzyEnergyBallIncreaseChaosAmount;
        public float startEnemySpawnCoolDown;
        public float enemySpawnCoolDownChaosMultiplier;
        public float enemySpawnSafeRadiusAroundPlayer;
        public float chaosBallSpawnSafeRadiusAroundPlayer;
        public float enemyMaxSpawnRadiusAroundPlayer;
        public float safeRadiusFromCenter;
        public float arenaRadius;
        public float startEnemyPerSecond;
        public float enemyPerSecondChaosMultiplier;
    }
    
    [Serializable]
    public class CannonBasicStats
    {
        public Global.Misc.CannonAttackType cannonAttackType;
        public Global.Misc.ElementType elementType;
        public float attack;
        public float attackCoolDown;
        public float laserDuration;
        public float energyRange;
    }
    
    [Serializable]
    public struct BattleGlobalParameters
    {
        public float chaos;
    }
    
    [Serializable]
    public struct PickUpConfig
    {
        public float chaosBallIncreaseChaosAmount;
        public int chaosBallSpawnEnemyNumber;
        public float chaosBallExpValueEach;
        public float voidCenterExpIncreaseValueEnemySpider;
        public float voidCenterExpPowerUpValue;
        public float voidCenterExpPowerUpValueIncreaseAfterEachPick;
    }
    
    
    [Serializable]
    public struct FireElementReactionData
    {
        public float overrideWaterDamageMultiplier;
        public float overrideNatureDamageMultiplier;
        public float burnDamage;
        public float burnDuration;
        public bool burnBuffStackable;
        public float stackDamage;
    }
    
    [Serializable]
    public struct NatureElementReactionData
    {
        public float overrideWaterDamageMultiplier;
        public float overrideFireDamageMultiplier;
        public float bounceDamage;
        public int bounceTargetNumber;
        public float bounceRange;
    }
    
    [Serializable]
    public struct WaterElementReactionData
    {
        public float overrideFireDamageMultiplier;
        public float overrideNatureDamageMultiplier;
        public float speedChangeAmount;
        public float slowDuration;
    }

    public enum BuffReference
    {
        BurnBuff,
        WaterSlowBuff,
        SpeedUp
    }

    public enum DamageTextType
    {
        Normal,
        Burn
    }
    
    public struct ChipGetting
    {
        public ChipData.ChipInfo chipInfo;
        public Global.Misc.Rarity rarity;
    }

    private void Awake()
    {
        Global.Battle.battleDataManager = this;
    }
}