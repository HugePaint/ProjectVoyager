using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "ScriptableObjects/BattleData")]
public class BattleData : ScriptableObject
{
    [Header("Player data at the beginning of battle")]
    public float playerInitMoveSpeed;

    public float playerInitHealth;
    public float playerInitDefense;

    [Header("cannon basic stats")] 
    public BattleDataManager.CannonBasicStats bulletBasicStats;
    public BattleDataManager.CannonBasicStats laserBasicStats;
    public BattleDataManager.CannonBasicStats energyBasicStats;

    [Header("Element reactions basic stats")]
    public BattleDataManager.FireElementReactionData fireElementReactionData;
    public BattleDataManager.WaterElementReactionData waterElementReactionData;
    public BattleDataManager.NatureElementReactionData natureElementReactionData;

    [Header("Enemy basic stats")] 
    public BattleDataManager.EnemyBasicStats enemySpiderBasicStats;
    public BattleDataManager.EnemyBasicStats enemyShootSpiderBasicStats;

    [Header("Boss")] public BattleDataManager.BossStats bossStats;

    [Header("Find Enemy Target Priority weight")]
    public BattleDataManager.EnemyPriorityWeight weightForStayCannon;

    public BattleDataManager.EnemyPriorityWeight weightForSearchCannon;

    [Header("Enemy Spawn Rule")] public BattleDataManager.EnemySpawnRule enemySpawnRule;
    public BattleDataManager.EnemyStatsChaosMultiplier enemySpiderChaosMultiplier;
    public BattleDataManager.EnemyStatsChaosMultiplier enemySpiderShootChaosMultiplier;

    [Header("Pickup Config")] 
    public BattleDataManager.PickUpConfig pickUpConfig;

    [Header("Chip Gain")] 
    public bool isGainingChips;
    public float dashCooldown = 1f;
    
    public float chaosIncreasePerTenSeconds;
    public float bossAppearTime;
    public float gameEndTime;


    //testing settings
    [Header("Testing"), Space(20)] 
    public bool isTesting;
    public BattleDataManager.EnemyBasicStats testEnemyInformation;
    public List<bool> cannonTestActiveList;
    public List<BattleDataManager.CannonImport> testCannons;
}