using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class LanguageMenuAnimator : MonoBehaviour
{
    public float disappearTime;
    
    public void Hide()
    {
        PlayerPrefs.SetInt(MenuController.FirstStartupKey, 1);
        
        var canvasGroup = GetComponent<CanvasGroup>();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, disappearTime)
            .From(1f).OnComplete(() =>
            {
                Global.DoTweenWait(0.5f, () =>
                {
                    Global.MainMenu.menuController.ShowSplash();;
                });
                gameObject.SetActive(false);
            });
    }
    
}
