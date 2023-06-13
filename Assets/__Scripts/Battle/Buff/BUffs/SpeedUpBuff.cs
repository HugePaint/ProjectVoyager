using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpeedUpBuff : Buff
{
    public float speedChangeAmount;
    
    public override void ApplyBuff(Enemy enemy)
    {
        var sameBuffOnEnemy = FindSameBuff(enemy, buffReference);
        if (sameBuffOnEnemy == null)
        {
            if (enemy.speedUpBuffText != null)
            {
                enemy.speedUpBuffText.PlayFeedbacks(enemy.transform.position, 0);
            }
            enemy.enemyAuraManager.PopBuff(BattleDataManager.BuffReference.SpeedUp);
            enemy.buffOnEnemy.Add(this);
            
            enemy.MoveSpeed += speedChangeAmount;
            
            durationCountDownTween = Global.DoTweenWait(buffLastDuration, () =>
            {
                BuffEnd(enemy);
            });
        }
        else
        {
            UpdateBuffIfExist(enemy, sameBuffOnEnemy);
        }
    }

    public override void UpdateBuffIfExist(Enemy enemy, Buff buff)
    {
        var sameSpeedUpBuff = (SpeedUpBuff)buff;
        enemy.enemyAuraManager.PopBuff(BattleDataManager.BuffReference.SpeedUp);
        if (Math.Abs(speedChangeAmount - sameSpeedUpBuff.speedChangeAmount) > 0.01f)
        {
            enemy.MoveSpeed -= sameSpeedUpBuff.speedChangeAmount;
            enemy.MoveSpeed += speedChangeAmount;
            sameSpeedUpBuff.speedChangeAmount = speedChangeAmount;
        }
        sameSpeedUpBuff.ResetCountDown(enemy, buffLastDuration);
    }

    public override void ResetCountDown(Enemy enemy, float newLastDuration)
    {
        durationCountDownTween?.Kill();
        durationCountDownTween = Global.DoTweenWait(newLastDuration, () =>
        {
            BuffEnd(enemy);
        });
    }

    public override void BuffEnd(Enemy enemy)
    {
        enemy.MoveSpeed -= speedChangeAmount;
        enemy.buffOnEnemy.Remove(this);
        enemy.enemyAuraManager.BuffSetActive(false, BattleDataManager.BuffReference.SpeedUp);
    }

    public override void ForceRemoveBuff(Enemy enemy)
    {
        durationCountDownTween?.Kill();
        enemy.MoveSpeed -= speedChangeAmount;
    }
    
    public SpeedUpBuff()
    {
        buffReference = BattleDataManager.BuffReference.SpeedUp;
        buffLastDuration = Global.Battle.battleData.bossStats.bossSpeedUpSkillDuration;
        speedChangeAmount = Global.Battle.battleData.bossStats.bossSpeedUpSkillAmount;
    }
}
