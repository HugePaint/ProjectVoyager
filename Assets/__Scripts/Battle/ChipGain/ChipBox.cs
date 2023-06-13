using System;
using Cinemachine.Utility;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ChipBox : MonoBehaviour
{
    public Transform gateTopTransform;
    public Vector3 gateTopStartPosition;
    public Vector3 gateTopIdlePosition;
    public Vector3 gateTopEndPosition;
    public Vector3 gateTopHoverPosition;
    
    public Transform gateDownTransform;
    public Vector3 gateDownStartPosition;
    public Vector3 gateDownIdlePosition;
    public Vector3 gateDownEndPosition;
    public Vector3 gateDownHoverPosition;

    public ChipSpriteController chipSpriteController;

    public CanvasGroup lightCanvasGroup;
    public Image lightImage;
    public Button clickButton;
    public CanvasGroup canvasGroup;
    public ChipUI chipUI;
    public ParticleSystem particleSystem;

    public int id;
    public ChipData.ChipInfo chipInfo;
    public Global.Misc.Rarity rarity;

    public void Init()
    {
        if (id < 0) return;
        
        chipInfo = Global.Battle.chipsGettingInTheEnd[id].chipInfo;
        rarity = Global.Battle.chipsGettingInTheEnd[id].rarity;
        gateTopTransform.localPosition = gateTopStartPosition;
        gateDownTransform.localPosition = gateDownStartPosition;
        lightCanvasGroup.DOFade(0f, 0f);
        chipSpriteController.UpdateSprite(chipInfo.elementType, chipInfo.attackType, chipInfo.modificationEffectRarities);
        clickButton.gameObject.SetActive(true);
        clickButton.onClick.RemoveAllListeners();
        clickButton.onClick.AddListener(Open);
        chipUI = Global.Battle.chipGainUIManager.chipUIs[id];
        chipUI.Init(chipInfo);
    }

    public void Idle()
    {
        gateTopTransform.DOLocalMove(gateTopIdlePosition,0.5f);
        gateDownTransform.DOLocalMove(gateDownIdlePosition, 0.5f);
        lightCanvasGroup.DOFade(1f, 0.5f);
        switch (rarity)
        {
            case Global.Misc.Rarity.None:
                break;
            case Global.Misc.Rarity.Common:
                lightImage.color = Global.Misc.colorData.Common;
                break;
            case Global.Misc.Rarity.Uncommon:
                lightImage.color = Global.Misc.colorData.Uncommon;
                break;
            case Global.Misc.Rarity.Rare:
                lightImage.color = Global.Misc.colorData.Rare;
                break;
            case Global.Misc.Rarity.Epic:
                lightImage.color = Global.Misc.colorData.Epic;
                break;
            case Global.Misc.Rarity.Legendary:
                lightImage.color = Global.Misc.colorData.Legendary;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void PointerEnter()
    {
        gateTopTransform.DOLocalMove(gateTopHoverPosition,0.5f);
        gateDownTransform.DOLocalMove(gateDownHoverPosition, 0.5f);
    }

    public void PointerExit()
    {
        gateTopTransform.DOLocalMove(gateTopIdlePosition,0.5f);
        gateDownTransform.DOLocalMove(gateDownIdlePosition, 0.5f);
    }

    public void Open()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.ChipGainClick);
        clickButton.gameObject.SetActive(false);
        gateTopTransform.DOLocalMove(gateTopEndPosition,1f).SetEase(Ease.InCubic);
        gateDownTransform.DOLocalMove(gateDownEndPosition, 1f).SetEase(Ease.InCubic);
        Global.DoTweenWait(0.5f, () =>
        {
            if (rarity == Global.Misc.Rarity.Legendary)
            {
                particleSystem.Play();
                Global.Battle.globalEffectManager.GoodChipEffect();
            }
        });
        lightCanvasGroup.DOFade(0f, 1.2f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            Global.DoTweenWait(0.2f, () =>
            {
                chipSpriteController.transform.parent = Global.Battle.chipGainUIManager.transform;

                transform.DOLocalMoveY(transform.localPosition.y - 50f, 1f).SetEase(Ease.InBack);
                canvasGroup.DOFade(0, 1f).SetEase(Ease.Linear);

                chipSpriteController.transform.DOMove(chipUI.chipLocation.position, 1f).OnComplete(() =>
                {
                    Global.DoTweenWait(0.3f, () =>
                    {
                        chipUI.Appear();
                    });
                });
            });
        });
        
    }
}
