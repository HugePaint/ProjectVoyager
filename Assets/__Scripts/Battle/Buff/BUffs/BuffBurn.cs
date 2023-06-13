using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BuffBurn : Buff
{
    public float burnDamage;
    public bool burnBuffStackable;
    public float stackDamage;
    public Tween damageTween;
    public override void ApplyBuff(Enemy enemy)
    {
        var sameBuffOnEnemy = FindSameBuff(enemy, buffReference);
        if (sameBuffOnEnemy == null)
        {
            if (enemy.burnBuffText != null)
            {
                enemy.burnBuffText.PlayFeedbacks(enemy.transform.position, 0);
            }
            enemy.enemyAuraManager.PopBuff(BattleDataManager.BuffReference.BurnBuff);
            
            enemy.buffOnEnemy.Add(this);
            damageTween = Global.DoTweenRepeat(1f, false, -1, () =>
            {
                enemy.TakeDamage(Global.Misc.ElementType.None, burnDamage, BattleDataManager.DamageTextType.Burn);
            });
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
        enemy.enemyAuraManager.PopBuff(BattleDataManager.BuffReference.BurnBuff);
        buff.ResetCountDown(enemy, buffLastDuration);

        // todo: stack damage
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
        damageTween?.Kill();
        enemy.buffOnEnemy.Remove(this);
        enemy.enemyAuraManager.BuffSetActive(false, BattleDataManager.BuffReference.BurnBuff);
    }

    public override void ForceRemoveBuff(Enemy enemy)
    {
        durationCountDownTween?.Kill();
        damageTween?.Kill();
    }

    public BuffBurn()
    {
        buffReference = BattleDataManager.BuffReference.BurnBuff;
        buffLastDuration = Global.Battle.battleData.fireElementReactionData.burnDuration;
        burnDamage = Global.Battle.battleData.fireElementReactionData.burnDamage;
    }
}
