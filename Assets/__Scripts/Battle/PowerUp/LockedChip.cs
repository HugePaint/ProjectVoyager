using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LockedChip : MonoBehaviour
{
    public ChipSpriteController chipSpriteController;
    public Image cover;
    public Transform workingTransform;
    public CanvasGroup workingImageCanvasGroup;
    public Transform startPosition;
    public Transform endPosition;
    public ParticleSystem particleSystem;

    private void Awake()
    {
        cover.fillAmount = 1f;
        workingTransform.gameObject.SetActive(false);
    }

    public void Reset()
    {
        cover.fillAmount = 1f;
        chipSpriteController.gameObject.SetActive(true);
    }

    public void StartUnlocking(float duration, int id)
    {
        workingTransform.gameObject.SetActive(true);
        workingTransform.localPosition = startPosition.localPosition;
        particleSystem.Play();
        workingImageCanvasGroup.DOFade(1, 0.2f).SetEase(Ease.Linear).From(0f).OnComplete(() =>
        {
            var fillAmount = 1f;
            DOTween.To(() => fillAmount, x => fillAmount = x, 0f, duration).SetEase(Ease.Linear).OnUpdate(() =>
            {
                cover.fillAmount = fillAmount;
            });
            workingTransform.DOLocalMove(endPosition.localPosition, duration).From(startPosition.localPosition).SetEase(Ease.Linear).OnComplete(() =>
            {
                workingImageCanvasGroup.DOFade(0f, 0.2f).SetEase(Ease.Linear);
                particleSystem.Stop();
                Global.DoTweenWait(0.5f, () =>
                {
                    Global.Battle.weaponUIManager.UnlockCannonUIAnimation(id, cover.transform);
                });
                Global.DoTweenWait(1f, () =>
                {
                    chipSpriteController.gameObject.SetActive(false);
                    workingTransform.gameObject.SetActive(false);
                });
            });
        });

    }
}
