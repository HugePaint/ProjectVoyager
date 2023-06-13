using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChipUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public CanvasGroup elementCanvasGroup;
    public Image elementImage;
    public Image attackImage;
    public List<ChipUIModificationEffect> chipUIModificationEffects;

    public Transform chipLocation;
    public Transform modificationEffectsParent;

    private void Awake()
    {
        canvasGroup.DOFade(0f, 0f);
    }

    public void Init(ChipData.ChipInfo chipInfo)
    {
        switch (chipInfo.elementType)
        {
            case Global.Misc.ElementType.Fire:
                elementImage.sprite = Global.Battle.battlePrefabManager.fireElementSprite;
                elementImage.color = Global.Misc.colorData.Fire;
                break;
            case Global.Misc.ElementType.Water:
                elementImage.sprite = Global.Battle.battlePrefabManager.waterElementSprite;
                elementImage.color = Global.Misc.colorData.Water;
                break;
            case Global.Misc.ElementType.Nature:
                elementImage.sprite = Global.Battle.battlePrefabManager.natureElementSprite;
                elementImage.color = Global.Misc.colorData.Nature;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (chipInfo.attackType)
        {
            case Global.Misc.CannonAttackType.Bullet:
                attackImage.sprite = Global.Battle.battlePrefabManager.bulletSprite;
                break;
            case Global.Misc.CannonAttackType.Laser:
                attackImage.sprite = Global.Battle.battlePrefabManager.laserSprite;
                break;
            case Global.Misc.CannonAttackType.Energy:
                attackImage.sprite = Global.Battle.battlePrefabManager.energySprite;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        for (var i = 0; i < chipInfo.modificationEffectIDs.Count; i++)
        {
            chipUIModificationEffects[i].UpdateInfo(chipInfo.modificationEffectIDs[i], chipInfo.modificationEffectRarities[i]);
        }
        
        for (var i = chipInfo.modificationEffectIDs.Count; i < chipUIModificationEffects.Count; i++)
        {
            chipUIModificationEffects[i].UpdateInfo(-1, Global.Misc.Rarity.None);
        }
        
    }

    public void Appear()
    {
        var attackImageLocalPosition = attackImage.transform.localPosition;
        attackImage.transform.DOLocalMoveX(attackImageLocalPosition.x, 1f).From(attackImageLocalPosition.x-100f);

        var modificationEffectParentLocalPosition = modificationEffectsParent.transform.localPosition;
        modificationEffectsParent.transform.DOLocalMoveX(modificationEffectParentLocalPosition.x, 1f).From(modificationEffectParentLocalPosition.x + 100f);
        canvasGroup.DOFade(1f, 1f);
        Global.DoTweenWait(0.5f, () =>
        {
            elementCanvasGroup.DOFade(1f, 1f).OnComplete(() =>
            {
                Global.Battle.chipGainUIManager.ViewOneChip();
            });
        });
    }
}
