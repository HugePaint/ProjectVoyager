using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BattleTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public CanvasGroup canvasGroup;
    private float timeRemaining;
    public Tween countDownTween;
    public bool timeEnd;
    private void Awake()
    {
        Global.Battle.battleTimer = this;
        canvasGroup.alpha = 0f;
        timeEnd = false;
        EventCenter.GetInstance().AddEventListener(Global.Events.StartSpawningEnemy, () =>
        {
            StartCountDown(Global.Battle.battleData.gameEndTime, () =>
            {
                timeEnd = true;
                EventCenter.GetInstance().EventTrigger(Global.Events.GameOver);
            });
        });
        EventCenter.GetInstance().AddEventListener(Global.Events.GameOver, () =>
        {
            countDownTween?.Kill();
        });
    }

    public void StartCountDown(float second, Action timerEndAction)
    {
        canvasGroup.DOFade(1f, 1f);
        var currentTimeRemain = second;
        countDownTween = DOTween.To(() => currentTimeRemain, x => currentTimeRemain = x, 0f, second).SetEase(Ease.Linear).OnUpdate(() =>
        {
            timeRemaining = 600f - currentTimeRemain;
            UpdateTimerText(currentTimeRemain);
        }).OnComplete(() =>
        {
            timerEndAction?.Invoke();
        });
    }

    public void UpdateTimerText(float timeRemain)
    {
        var minutes = Mathf.FloorToInt(timeRemain / 60);
        var seconds = Mathf.FloorToInt(timeRemain % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnDestroy()
    {
        if (timeRemaining > 1f)
        {
            if (MetricManagerScript.instance != null) MetricManagerScript.instance.LogString("Survive Time in second", timeRemaining.ToString());
        }
    }
}
