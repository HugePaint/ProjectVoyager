using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public List<Enemy> enemiesOnField;
    public bool stopSpawningEnemy;
    public bool startAttacking;

    private void Awake()
    {
        enemiesOnField = new List<Enemy>();
        Global.Battle.enemyManager = this;
        EventCenter.GetInstance().AddEventListener<float>(Global.Events.UpdateChaos, UpdateChaos);
        EventCenter.GetInstance().AddEventListener(Global.Events.StartSpawningEnemy, StartSpawnCreepEnemies);
        startAttacking = false;
    }

    void Start()
    {
        
    }
    

    public void UpdateChaos(float value)
    {
        Global.Battle.chaos += value;
        Debug.Log("Chaos Changed: " + Global.Battle.chaos);
    }

    public void StartSpawnCreepEnemies()
    {
        if (!Global.Battle.battleData.isTesting)
        {
            KeepSpawnCreepEnemies();   
        }
        else
        {
            SpawnTestEnemy();
        }
    }

    public void SpawnTestEnemy()
    {
        var enemySpider = Global.Battle.enemyPrefabManager.enemySpiderPool.Get();
        enemySpider.transform.position = new Vector3(0f, 1f, -10f);
        enemySpider.Health = Global.Battle.battleData.testEnemyInformation.enemySpiderHealth;
        enemySpider.Defense = Global.Battle.battleData.testEnemyInformation.enemySpiderDefense;
        enemySpider.Attack = Global.Battle.battleData.testEnemyInformation.enemySpiderAttack;
        enemySpider.AttackCoolDown = Global.Battle.battleData.testEnemyInformation.enemySpiderAttackCoolDown;
        enemySpider.AnimationMoveSpeed = Global.Battle.battleData.testEnemyInformation.enemySpiderAnimationMoveSpeed;
        enemySpider.MoveSpeed = Global.Battle.battleData.testEnemyInformation.enemySpiderMoveSpeed;
        enemySpider.EnemyType = Global.Battle.battleData.testEnemyInformation.enemySpiderEnemyType;
        AddEnemyToList(enemySpider);
        
        var enemySpider1 = Global.Battle.enemyPrefabManager.enemySpiderPool.Get();
        enemySpider1.transform.position = new Vector3(5f, 1f, -10f);
        enemySpider1.Health = Global.Battle.battleData.testEnemyInformation.enemySpiderHealth;
        enemySpider1.Defense = Global.Battle.battleData.testEnemyInformation.enemySpiderDefense;
        enemySpider1.Attack = Global.Battle.battleData.testEnemyInformation.enemySpiderAttack;
        enemySpider1.AttackCoolDown = Global.Battle.battleData.testEnemyInformation.enemySpiderAttackCoolDown;
        enemySpider1.AnimationMoveSpeed = Global.Battle.battleData.testEnemyInformation.enemySpiderAnimationMoveSpeed;
        enemySpider1.MoveSpeed = Global.Battle.battleData.testEnemyInformation.enemySpiderMoveSpeed;
        enemySpider1.EnemyType = Global.Battle.battleData.testEnemyInformation.enemySpiderEnemyType;
        AddEnemyToList(enemySpider1);
        
        var enemySpider2 = Global.Battle.enemyPrefabManager.enemySpiderPool.Get();
        enemySpider2.transform.position = new Vector3(-10f, 1f, -10f);
        enemySpider2.Health = Global.Battle.battleData.testEnemyInformation.enemySpiderHealth;
        enemySpider2.Defense = Global.Battle.battleData.testEnemyInformation.enemySpiderDefense;
        enemySpider2.Attack = Global.Battle.battleData.testEnemyInformation.enemySpiderAttack;
        enemySpider2.AttackCoolDown = Global.Battle.battleData.testEnemyInformation.enemySpiderAttackCoolDown;
        enemySpider2.AnimationMoveSpeed = Global.Battle.battleData.testEnemyInformation.enemySpiderAnimationMoveSpeed;
        enemySpider2.MoveSpeed = Global.Battle.battleData.testEnemyInformation.enemySpiderMoveSpeed;
        enemySpider2.EnemyType = Global.Battle.battleData.testEnemyInformation.enemySpiderEnemyType;
        AddEnemyToList(enemySpider2);
    }

    public void KeepSpawnCreepEnemies()
    {
        var enemyPerSecond = Global.Battle.battleData.enemySpawnRule.startEnemyPerSecond + Global.Battle.battleData.enemySpawnRule.enemyPerSecondChaosMultiplier *  MathF.Pow(Global.Battle.chaos / 100f, 1f/3f) * Global.Battle.chaos;
        var waitTime = 1f / enemyPerSecond;
        if (waitTime < 0.1f) waitTime = 0.1f;
        Global.DoTweenWait(waitTime, () =>
        {
            if (!stopSpawningEnemy) SpawnEnemySpider();
            if (!stopSpawningEnemy) KeepSpawnCreepEnemies();
        });
    }

    public void SpawnEnemySpider()
    {
        var enemySpider = Global.Battle.enemyPrefabManager.enemySpiderPool.Get();
        enemySpider.transform.position = FindGeneralSpawnLocation(Global.Battle.battleData.enemySpawnRule.enemySpawnSafeRadiusAroundPlayer, Global.Battle.battleData.enemySpawnRule.enemyMaxSpawnRadiusAroundPlayer);
        enemySpider.OnSpawnAction();
    }

    public void SpawnGroupEnemySpider(int num, Vector3 spawnPosition)
    {
        for (var i = 0; i < num; i++)
        {
            Global.DoTweenWait(i * 0.2f, () =>
            {
                var randomLocationX = Random.Range(-4f, 4f);
                var randomLocationZ = Random.Range(-4f, 4f);
                var enemySpider = Global.Battle.enemyPrefabManager.enemySpiderPool.Get();
                enemySpider.transform.position = spawnPosition + new Vector3(randomLocationX, 0f, randomLocationZ);
                enemySpider.OnSpawnAction();
            });
        }
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemiesOnField.Add(enemy);
        if (!startAttacking)
        {
            Global.Battle.cannonBattleManager.GenerateInitialCoolDownForBetterAnimation();
            startAttacking = true;
        }
        else
        {
            Global.Battle.cannonBattleManager.TryAttack();
        }
    }

    public Enemy GetTarget(Global.Misc.CannonMode cannonMode)
    {
        if (enemiesOnField.Count == 0)
        {
            return null;
        }

        var highestPriority = -99999f;
        var enemyTarget = enemiesOnField[0];
        foreach (var enemy in enemiesOnField)
        {
            var distance = Vector3.Distance(enemy.gameObject.transform.position,
                Global.Battle.playerBehaviourController.transform.position);
            var priority = enemy.GetSelfPriority(distance, cannonMode);
            if (priority > highestPriority)
            {
                highestPriority = priority;
                enemyTarget = enemy;
            }
        }

        return enemyTarget;
    }

    public Vector3 FindGeneralSpawnLocation(float minDistance, float maxDistance)
    {
        var randomPosition = Random.insideUnitCircle * Global.Battle.battleData.enemySpawnRule.arenaRadius;
        var returnPosition = new Vector3(randomPosition.x, 1f, randomPosition.y);
        var distance = Vector3.Distance(Global.Battle.playerBehaviourController.transform.position, returnPosition);
        var distanceFromOrigin = Vector3.Distance(new Vector3(0f,1f,0f), returnPosition);

        while (distance < minDistance || distance > maxDistance || distanceFromOrigin < Global.Battle.battleData.enemySpawnRule.safeRadiusFromCenter)
        {
            randomPosition = Random.insideUnitCircle * Global.Battle.battleData.enemySpawnRule.arenaRadius;
            returnPosition = new Vector3(randomPosition.x, 1f, randomPosition.y);
            distance = Vector3.Distance(Global.Battle.playerBehaviourController.transform.position, returnPosition);
            distanceFromOrigin = Vector3.Distance(new Vector3(0f,1f,0f), returnPosition);
        }

        return returnPosition;
    }

    public Enemy GetEnemyInRange(Enemy sourceEnemy, float range)
    {
        if (enemiesOnField == null) return null;
        Enemy enemyInRange = null;
        var minDistance = 99999f;
        foreach (var enemyOnField in enemiesOnField)
        {
            if (enemyOnField != sourceEnemy)
            {
                var distance = Vector3.Distance(sourceEnemy.transform.position, enemyOnField.transform.position);
                if (distance < range && distance < minDistance)
                {
                    minDistance = distance;
                    enemyInRange = enemyOnField;
                }
            }
        }
        return enemyInRange;
    }
}