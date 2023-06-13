using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameOverPage : MonoBehaviour
{
    public CanvasGroup greyCoverCanvasGroup;
    public CanvasGroup textCanvasGroup;
    
    public void Appear()
    {
        greyCoverCanvasGroup.DOFade(1, 1f).From(0f).SetEase(Ease.Linear).OnComplete(() =>
        {
            textCanvasGroup.DOFade(1, 1.5f).From(0f).SetEase(Ease.Linear);
            var textLocalY = textCanvasGroup.transform.localPosition.y;
            textCanvasGroup.transform.DOLocalMoveY(textLocalY, 1.5f).From(textLocalY - 100f);
        });
    }
}
