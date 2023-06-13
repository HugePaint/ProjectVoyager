using DG.Tweening;
using UnityEngine;

public abstract class Buff
{
    public float buffLastDuration;
    public BattleDataManager.BuffReference buffReference;
    public Tween durationCountDownTween;
    public abstract void ApplyBuff(Enemy enemy);
    public abstract void UpdateBuffIfExist(Enemy enemy, Buff buff);
    public abstract void ResetCountDown(Enemy enemy, float newLastDuration);
    public abstract void BuffEnd(Enemy enemy);
    public abstract void ForceRemoveBuff(Enemy enemy);

    public Buff FindSameBuff(Enemy enemy, BattleDataManager.BuffReference buffReferenceCompare)
    {
        if (enemy.buffOnEnemy == null || enemy.buffOnEnemy.Count == 0) return null;
        foreach (var buff in enemy.buffOnEnemy)
        {
            if (buff.buffReference == buffReferenceCompare) return buff;
        }

        return null;
    }
}