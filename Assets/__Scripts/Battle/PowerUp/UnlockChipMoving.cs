using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UnlockChipMoving : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public Image image;

    public void FlyToChip(Transform startPosition, Transform endPosition, int id)
    {
        foreach (var cannon in Global.Battle.cannonBattleManager.cannonBattles)
        {
            if (cannon.cannonId == id)
            {
                switch (cannon.elementType)
                {
                    case Global.Misc.ElementType.Fire:
                        image.color = Global.Misc.colorData.Fire;
                        break;
                    case Global.Misc.ElementType.Water:
                        image.color = Global.Misc.colorData.Water;
                        break;
                    case Global.Misc.ElementType.Nature:
                        image.color = Global.Misc.colorData.Nature;
                        break;
                    case Global.Misc.ElementType.Light:
                        image.color = Global.Misc.colorData.Light;
                        break;
                    case Global.Misc.ElementType.Dark:
                        image.color = Global.Misc.colorData.Dark;
                        break;
                    case Global.Misc.ElementType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        transform.DOScale(1f, 0f);
        transform.position = startPosition.position;
        canvasGroup.DOFade(1f, 0.5f).From(0f).OnComplete(() =>
        {
            transform.DOLocalMove(endPosition.localPosition, 1f).SetEase(Ease.InOutCubic).OnComplete(() =>
            {
                foreach (var weaponUi in Global.Battle.weaponUIManager.weaponUis)
                {
                    if (weaponUi.id == id) weaponUi.BecomeReady();
                }
                gameObject.SetActive(false);
            });
            transform.DOScale(0.3f, 1f).SetEase(Ease.Linear);
            Global.DoTweenWait(0.5f, () =>
            {
                canvasGroup.DOFade(0f, 0.5f).From(1f).SetEase(Ease.Linear);
            });
        });
    }
}
