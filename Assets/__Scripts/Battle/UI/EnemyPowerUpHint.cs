using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyPowerUpHint : MonoBehaviour
{
    public float endX;
    public float startX;
    public Transform hintTransform;
    public GameObject backgroundToShow;

    private Tween showTween;
    private Tween backTween;

    private void Awake()
    {
        var position = hintTransform.localPosition;
        hintTransform.localPosition = new Vector3(startX, position.y, position.z);
        backgroundToShow.SetActive(false);
    }

    public void Show()
    {
        showTween?.Kill();
        backTween?.Kill();
        backTween = null;
        backgroundToShow.SetActive(true);
        showTween = hintTransform.DOLocalMoveX(endX, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            showTween = null;
            Global.DoTweenWait(2f, ()=>
            {
                Hide();
            });
        });
    }

    public void Hide()
    {
        if (showTween != null) return;
        if (backTween != null) return;
        backTween = hintTransform.DOLocalMoveX(startX, 1f).SetEase(Ease.InBack).OnComplete(() =>
        {
            backTween = null;
            backgroundToShow.SetActive(false);
        });
    }
}
