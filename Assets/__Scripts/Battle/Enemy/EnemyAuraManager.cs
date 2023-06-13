using System;
using DG.Tweening;
using UnityEngine;

public class EnemyAuraManager : MonoBehaviour
{
    [SerializeField] private EnemyAura burnAura;
    [SerializeField] private EnemyAura slowDownAura;
    [SerializeField] private EnemyAura speedUpAura;

    // private Tween burnBuffPopTween;
    // private Tween slowDownBuffPopTween;
    // private Tween speedUpBuffPopTween;

    public void DisableAllAura()
    {
        burnAura.gameObject.SetActive(false);
        slowDownAura.gameObject.SetActive(false);
        speedUpAura.gameObject.SetActive(false);
    }

    public void PopBuff(BattleDataManager.BuffReference buff)
    {
        // var buffAura = buff switch
        // {
        //     BattleDataManager.BuffReference.BurnBuff => burnAura,
        //     BattleDataManager.BuffReference.WaterSlowBuff => slowDownAura,
        //     BattleDataManager.BuffReference.SpeedUp => speedUpAura,
        //     _ => throw new ArgumentOutOfRangeException(nameof(buff), buff, null)
        // };
        // var buffAuraTween = buff switch
        // {
        //     BattleDataManager.BuffReference.BurnBuff => burnBuffPopTween,
        //     BattleDataManager.BuffReference.WaterSlowBuff => slowDownBuffPopTween,
        //     BattleDataManager.BuffReference.SpeedUp => speedUpBuffPopTween,
        //     _ => throw new ArgumentOutOfRangeException(nameof(buff), buff, null)
        // };
        //
        // buffAuraTween?.Kill();
        // buffAura.transform.localScale = new Vector3(1f, 1f, 1f);
        // buffAuraTween = buffAura.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f);
        
        BuffSetActive(true, buff);
    }

    public void BuffSetActive(bool active, BattleDataManager.BuffReference buff)
    {
        var buffAura = buff switch
        {
            BattleDataManager.BuffReference.BurnBuff => burnAura,
            BattleDataManager.BuffReference.WaterSlowBuff => slowDownAura,
            BattleDataManager.BuffReference.SpeedUp => speedUpAura,
            _ => throw new ArgumentOutOfRangeException(nameof(buff), buff, null)
        };
        buffAura.transform.localScale = new Vector3(1f, 1f, 1f);
        buffAura.gameObject.SetActive(active);
    }
}