using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SplashAnimator : MonoBehaviour
{
    public GameObject logoPanel;
    public CanvasGroup canvasGroup;
    public float fadeTime;
    public float enlargeScale;
    
    private Tweener fadeInTweener;
    private Tweener enlargeTweener;

    public void Animate()
    {
        fadeInTweener = canvasGroup.DOFade(1f, fadeTime).From(0f);
        fadeInTweener.OnComplete(() => { canvasGroup.DOFade(0f, fadeTime * 0.5f).From(1f).OnComplete(() =>
            {
                Global.MainMenu.menuController.ShowMainMenu();;
                gameObject.SetActive(false);
            });
        });
        
        var localScale = logoPanel.transform.localScale;
        enlargeTweener = logoPanel.transform.DOScale(enlargeScale * localScale, fadeTime * 3f);
    }
}
