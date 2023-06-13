using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnlockChipArea : MonoBehaviour
{
    public LockedChip lockedChip;
    public CanvasGroup canvasGroup;
    public Vector3 outScreenPosition;
    public Vector3 inScreenPosition;
    public Transform workStationTransform;
    public bool stationReady;
    public bool chipUnlocked;
    private void Awake()
    {
        Global.Battle.unlockChipArea = this;
        workStationTransform.DOLocalMove(outScreenPosition, 0f);
        canvasGroup.alpha = 0f;
        stationReady = true;
        chipUnlocked = true;
    }

    public bool ReadyForNextChip()
    {
        return stationReady && chipUnlocked;
    }
    
    public void StationIsTaken()
    {
        stationReady = false;
        chipUnlocked = false;
    }

    public void UnlockChipForCannon(int id)
    {
        lockedChip.StartUnlocking(2f, id);
        Global.DoTweenWait(3.5f, () =>
        {
            workStationTransform.DOLocalMove(outScreenPosition, 1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                canvasGroup.alpha = 0f;
                stationReady = true;
            });
        });
    }

    public void Appear(int chipId)
    {
        stationReady = false;
        chipUnlocked = false;
        foreach (var chip in Global.Battle.chipInCannonsInBattle)
        {
            if (chip.inBattleID == chipId) lockedChip.chipSpriteController.UpdateSprite(chip.info.elementType, chip.info.attackType, chip.info.modificationEffectRarities);
        }
        lockedChip.Reset();
        canvasGroup.DOFade(1f, 0.5f).SetEase(Ease.Linear);
        workStationTransform.DOLocalMove(inScreenPosition, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Global.DoTweenWait(0.2f, () =>
            {
                UnlockChipForCannon(chipId);
            });
        });
    }
}
