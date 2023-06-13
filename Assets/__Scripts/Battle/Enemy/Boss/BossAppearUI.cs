using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossAppearUI : MonoBehaviour
{
    public Image bossIconImage;
    public CanvasGroup bossIconCanvasGroup;

    public Image frameOne;
    public Image frameTwo1;
    public Image frameTwo2;
    public Image frameTwo3;

    public CanvasGroup textCanvasGroup;

    public CanvasGroup mainCanvasGroup;

    private void Awake()
    {
        bossIconImage.gameObject.SetActive(false);
        frameOne.fillAmount = 0f;
        frameOne.gameObject.SetActive(false);
        frameTwo1.gameObject.SetActive(false);
        frameTwo2.gameObject.SetActive(false);
        frameTwo3.gameObject.SetActive(false);
        textCanvasGroup.gameObject.SetActive(false);
        EventCenter.GetInstance().AddEventListener(Global.Events.BossAppear, BossAppearAnimation);
    }

    public void BossAppearAnimation()
    {
        bossIconImage.gameObject.SetActive(true);
        Global.Battle.globalEffectManager.BossAppearColorChange();
        ImageShine();
        Global.DoTweenWait(1.1f, () =>
        {
            ImageShine();
        });
        Global.DoTweenWait(2f, () =>
        {
            StageTwoAnimation();
        });
    }

    public void ImageShine()
    {
        bossIconCanvasGroup.DOFade(0.6f, 0.5f).SetEase(Ease.InCubic).From(0).OnComplete(() =>
        {
            bossIconCanvasGroup.DOFade(0f, 0.5f).From(0.6f).SetEase(Ease.OutCubic);
        });
    }

    public void StageTwoAnimation()
    {
        frameOne.gameObject.SetActive(true);
        float fill = 0;
        DOTween.To(() => fill, x => fill = x, 1f, 0.6f).SetEase(Ease.Linear).OnUpdate(() =>
        {
            frameOne.fillAmount = fill;
        });
        Global.DoTweenWait(0.2f, () =>
        {
            frameTwo1.gameObject.SetActive(true);
            frameTwo1.transform.DOScale(1f,0.6f).From(0).OnComplete(() =>
            {
                frameTwo1.transform.DOLocalRotate(new Vector3(0f, 0f, 45f),0.4f).OnComplete(() =>
                {
                    StageThreeAnimation();
                });
            });
            Global.DoTweenWait(0.1f, () =>
            {
                frameTwo2.gameObject.SetActive(true);
                frameTwo2.transform.DOScale(1f, 0.6f).From(0).OnComplete(() =>
                {
                    frameTwo2.transform.DOLocalRotate(new Vector3(0f, 0f, 45f),0.4f);
                });
            });
            Global.DoTweenWait(0.2f, () =>
            {
                frameTwo3.gameObject.SetActive(true);
                frameTwo3.transform.DOScale(1f, 0.6f).From(0).OnComplete(() =>
                {
                    frameTwo3.transform.DOLocalRotate(new Vector3(0f, 0f, 45f),0.4f);
                });
            });
        });
    }


    public void StageThreeAnimation()
    {
        bossIconImage.transform.DOScale(1f, 0.5f).From(1.5f).SetEase(Ease.InQuint);
        bossIconCanvasGroup.DOFade(1f,0.5f).From(0f).SetEase(Ease.InQuint);
        textCanvasGroup.gameObject.SetActive(true);
        textCanvasGroup.DOFade(1f,0.5f).From(0f).SetEase(Ease.InQuint).OnComplete(() =>
        {
            Global.Battle.globalEffectManager.BossAppearShakeEffect();
        });
        Global.DoTweenWait(2f, () =>
        {
            StageFourAnimation();
        });
    }

    public void StageFourAnimation()
    {
        mainCanvasGroup.DOFade(0f, 2f).From(1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
