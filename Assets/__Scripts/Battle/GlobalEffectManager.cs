using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GlobalEffectManager : MonoBehaviour
{
    public MMF_Player energyHitEffect;
    public MMF_Player playerGetHitEffect;
    public MMF_Player bossLaserEffect;
    public MMF_Player bossBodyAttackEffect;
    public MMF_Player bossAppearColorEffect;
    public MMF_Player bossAppearShakeEffect;
    public MMF_Player goodChipEffect;
    private void Awake()
    {
        Global.Battle.globalEffectManager = this;
        EventCenter.GetInstance().AddEventListener<float>(Global.Events.PlayerGetHit, PlayerGetHitEffect);
    }

    public void StopWhenPause()
    {
        energyHitEffect.StopFeedbacks();
        playerGetHitEffect.StopFeedbacks();
        bossLaserEffect.StopFeedbacks();
        bossBodyAttackEffect.StopFeedbacks();
    }

    public void PlayerEnergyHitEffect()
    {
        energyHitEffect.PlayFeedbacks();
    }

    public void PlayerGetHitEffect(float _)
    {
        playerGetHitEffect.PlayFeedbacks();
    }

    public void BossLaserEffect()
    {
        bossLaserEffect.PlayFeedbacks();
    }

    public void BossBodyAttackEffect()
    {
        bossBodyAttackEffect.PlayFeedbacks();
    }

    public void BossAppearColorChange()
    {
        bossAppearColorEffect.PlayFeedbacks();
        Global.DoTweenWait(1.1f, () =>
        {
            bossAppearColorEffect.PlayFeedbacks();
        });
    }
    
    public void BossAppearShakeEffect()
    {
        bossAppearShakeEffect.PlayFeedbacks();
    }
    
    public void GoodChipEffect()
    {
        goodChipEffect.PlayFeedbacks();
    }
}
