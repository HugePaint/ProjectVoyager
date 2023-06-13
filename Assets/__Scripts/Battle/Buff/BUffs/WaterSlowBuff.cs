using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WaterSlowBuff : Buff
{
    public float speedChangeAmount;

    public override void ApplyBuff(Enemy enemy)
    {
        var sameBuffOnEnemy = FindSameBuff(enemy, buffReference);
        if (sameBuffOnEnemy == null)
        {
            if (enemy.waterSlowBuffText != null)
            {
                enemy.waterSlowBuffText.PlayFeedbacks(enemy.transform.position, 0);
            }
            enemy.enemyAuraManager.PopBuff(BattleDataManager.BuffReference.WaterSlowBuff);
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
        var sameWaterSlowBuff = (WaterSlowBuff)buff;
        enemy.enemyAuraManager.PopBuff(BattleDataManager.BuffReference.WaterSlowBuff);
        if (Math.Abs(speedChangeAmount - sameWaterSlowBuff.speedChangeAmount) > 0.01f)
        {
            enemy.MoveSpeed -= sameWaterSlowBuff.speedChangeAmount;
            enemy.MoveSpeed += speedChangeAmount;
            sameWaterSlowBuff.speedChangeAmount = speedChangeAmount;
        }
        sameWaterSlowBuff.ResetCountDown(enemy, buffLastDuration);
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
        enemy.enemyAuraManager.BuffSetActive(false, BattleDataManager.BuffReference.WaterSlowBuff);
    }

    public override void ForceRemoveBuff(Enemy enemy)
    {
        durationCountDownTween?.Kill();
        enemy.MoveSpeed -= speedChangeAmount;
    }

    public WaterSlowBuff()
    {
        buffReference = BattleDataManager.BuffReference.WaterSlowBuff;
        buffLastDuration = Global.Battle.battleData.waterElementReactionData.slowDuration;
        speedChangeAmount = Global.Battle.battleData.waterElementReactionData.speedChangeAmount;
    }
}
