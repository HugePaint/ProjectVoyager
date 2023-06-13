using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleController : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public float defense;

    private void Awake()
    {
        Global.Battle.playerBattleController = this;
        health = 0;
        defense = 0;
        //BetweenSceneTest();
        EventCenter.GetInstance().AddEventListener<float>(Global.Events.PlayerGetHit,PlayerGetHit);
    }

    public void BetweenSceneTest()
    {
        Global.BetweenMenuAndBattle.playerStatsModifier = new BattleDataManager.PlayerStatsModifier
        {
            healthModifier = 10,
            defenseModifier = 10,
            speedModifier = 10
        };
    }

    private void PlayerGetHit(float damage)
    {
        UpdateHealth(-damage);
        Global.Battle.battleUIManager.playerStatsBarManager.UpdateHealth(health);
        CheckPlayerDie();
    }

    public void CheckPlayerDie()
    {
        if (health <= 0)
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.GameOver);
        }
    }

    public void UpdateHealth(float value)
    {
        health += value;
    }

    public void UpdateMaxHealth(float changeAmount)
    {
        health += changeAmount;
        Global.Battle.battleUIManager.playerStatsBarManager.UpdateMaxHealth(changeAmount, health);
    }

    public void UpdateDefense(float value)
    {
        defense += value;
    }
}