using System;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleEnergyDisplay : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TMP_Text energyCountText;
    public float energyCount;
    public ParticleImage particleImage;

    private Tween energyIncreaseTween;

    private void Awake()
    {
        Global.Battle.battleEnergyDisplay = this;
        energyCount = 0;
        energyCountText.text = "0";
        canvasGroup.alpha = 0f;
        EventCenter.GetInstance().AddEventListener(Global.Events.LoadInitialUI, Appear);
        EnterChipGain();
        if (particleImage) particleImage.enabled = false;
    }

    public void Appear()
    {
        canvasGroup.DOFade(1f, 1f);
    }

    public void UpdateCount(float increase)
    {
        energyCount += increase;
        energyCountText.text = ((int)energyCount).ToString();
        
        if (energyIncreaseTween == null)
        {
            energyIncreaseTween = transform.DOScale(1.4f, 0.1f).OnComplete(() =>
            {
                transform.DOScale(1f, 0.1f).OnComplete(() =>
                {
                    energyIncreaseTween = null;
                });
            });
        }
    }

    private void OnDestroy()
    {
        Global.Battle.energyGainWhenGainingChips = energyCount;
    }

    public void EnterChipGain()
    {
        if (Global.Battle.battleData.isGainingChips)
        {
            canvasGroup.alpha = 1f;
            energyCount = Global.Battle.energyGainWhenGainingChips;
            energyCountText.text = ((int)energyCount).ToString();
        }
    }

    public void Release()
    {
        particleImage.rateOverTime = (energyCount / 4f);
        particleImage.enabled = true;
        var valueText = energyCount;
        DOTween.To(() => valueText, x => valueText = x, 0f, 4f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            energyCountText.text = ((int)valueText).ToString();
        }).OnComplete(() =>
        {
            canvasGroup.DOFade(0, 1f);
        });
    }
}
