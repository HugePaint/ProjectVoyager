using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class VoidCenterProgressBar : MonoBehaviour
{
    public float maxValue;
    public float currentValue;
    public Image image;

    private Tween progressBarTween;
    private bool finishing;
    private int completeNum;
    private void Awake()
    {
        Global.Battle.voidCenterProgressBar = this;
        image.fillAmount = 1;
        progressBarTween = null;
        finishing = false;
        currentValue = 0;
        completeNum = 0;
    }

    private void Start()
    {
        if (Global.Battle.battleData.isGainingChips) gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.parent.transform.LookAt(Camera.main.transform);
    }

    public void UpdateValue(float currentValueInput)
    {
        currentValue += currentValueInput;
    }

    public void UpdateBar()
    {
        if (finishing) return;
        if (currentValue > maxValue)
        {
            currentValue -= maxValue;
            UpdateBarToEnd();
        }
        else
        {
            progressBarTween?.Kill();
            var barValue = image.fillAmount;
            var targetValue = 1f - (currentValue / maxValue);
            var difference = barValue - targetValue;
            DOTween.To(() => barValue, x => barValue = x, targetValue, 5f * difference).OnUpdate(() =>
            {
                image.fillAmount = barValue;
            });
        }
    }

    public void UpdateBarToEnd()
    {
        finishing = true;
        progressBarTween?.Kill();
        var barValue = image.fillAmount;
        var targetValue = 0f;
        var difference =  barValue - targetValue;
        DOTween.To(() => barValue, x => barValue = x, targetValue, 5f * difference).OnUpdate(() =>
        {
            image.fillAmount = barValue;
        }).OnComplete(() =>
        {
            completeNum += 1;
            maxValue += Global.Battle.battleData.pickUpConfig.voidCenterExpPowerUpValueIncreaseAfterEachPick;
            var pickupParent = Global.Battle.voidCenterPickUpManager.GetPickUpParent();
            image.fillAmount = 1f;
            transform.DOScale(0.15f, 0.15f).From(0.1f).OnComplete(() =>
            {
                transform.DOScale(0.1f, 0.15f).OnComplete(() =>
                {
                    finishing = false;
                    UpdateBar();
                });
            });
            ExpBallToPickUp(pickupParent);
        });
    }

    public void ExpBallToPickUp(VoidCenterPickUp pickUp)
    {
        if (!pickUp) return;
        var distance = Vector3.Distance(transform.position, pickUp.transform.position);
        var randomNum = Random.Range(5,8);
        for (var i = 0; i < randomNum; i++)
        {
            var randomX = Global.RandomPositiveAndNegativeFromRage(2f, 3f);
            var randomY = Global.RandomPositiveAndNegativeFromRage(1f, 2f);
            var randomZ = Global.RandomPositiveAndNegativeFromRage(2f, 3f);
            var expBall = Global.Battle.battlePrefabManager.expBallObjectPool.Get();
            var expBallTransform = expBall.transform;
            expBallTransform.position = transform.position;
            var endPosition = expBallTransform.position + new Vector3(randomX,randomY,randomZ);
            expBallTransform.DOScale(0.3f, 0.2f).From(0) .SetEase(Ease.Linear);
            expBall.transform.DOMove(endPosition, 0.2f).OnComplete(() =>
            {
                var randomDelay = Random.Range(0.1f, 0.3f);
                expBallTransform.parent = pickUp.transform;

                expBall.transform.DOLocalMove(new Vector3(0f,0f,0f), 0.05f* distance).SetEase(Ease.InCubic).SetDelay(randomDelay).OnComplete(
                    () =>
                    {
                        expBall.transform.DOScale(1f, 0).OnComplete(() =>
                        {
                            expBall.transform.parent = null;
                            Global.Battle.battlePrefabManager.expBallObjectPool.Release(expBall);
                        });
                    });
            });
            
        }
        Global.DoTweenWait(0.05f* distance + 0.5f, () =>
        {
            Global.Battle.voidCenterPickUpManager.GeneratePowerUp(pickUp);
        });
    }
}
