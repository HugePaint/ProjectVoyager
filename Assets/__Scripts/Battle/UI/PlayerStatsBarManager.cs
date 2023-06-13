using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsBarManager : MonoBehaviour
{
    public TMP_Text healthAmountText;
    public TMP_Text maxAmountText;
    public Image redBar;
    public Image blackBar;
    
    private Tween redBarTween;
    private Tween blackBarTween;
    private Tween textChangeTween;
    private Tween shakeTween;
    
    private float maxHealth;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        EventCenter.GetInstance().AddEventListener(Global.Events.GameStart, StartRealGameAction);
        EventCenter.GetInstance().AddEventListener(Global.Events.LoadInitialUI, () =>
        {
            canvasGroup.DOFade(1, 0.5f).From(0);
        });
    }

    public void StartRealGameAction()
    {
        canvasGroup.DOFade(0f, 0f);
    }

    public void InitSelf(float maxHealthInput)
    {
        redBar.fillAmount = 0;
        blackBar.fillAmount = 0;
        maxHealth = maxHealthInput;
        maxAmountText.text = "/" + Mathf.RoundToInt(maxHealthInput);
        healthAmountText.text = Mathf.RoundToInt(maxHealthInput).ToString();
        redBarTween = null;
        blackBarTween = null;
        textChangeTween = null;
        shakeTween = null;
    }

    public void UpdateMaxHealth(float changeAmount, float currentHealth)
    {
        redBarTween?.Kill();
        redBarTween = null;
        blackBarTween?.Kill();
        blackBarTween = null;
        textChangeTween?.Kill();
        textChangeTween = null;
        shakeTween?.Kill();
        shakeTween = null;

        maxHealth += changeAmount;
        var fillAmount = 1f -(currentHealth / maxHealth);
        redBar.fillAmount = fillAmount;
        blackBar.fillAmount = fillAmount;
        maxAmountText.text = "/" + Mathf.RoundToInt(maxHealth);
        healthAmountText.text = Mathf.RoundToInt(currentHealth).ToString();

    }

    public void UpdateHealth(float currentHealth)
    {
        shakeTween ??= transform.DOShakePosition(0.2f, 20f, 30).SetEase(Ease.Linear).OnComplete(() => { shakeTween = null; });
        redBarTween?.Kill();
        var redFillValue = redBar.fillAmount;
        var redTargetValue = 1f - (currentHealth / maxHealth);
        var valueRedDifference = redTargetValue - redFillValue;
        redBarTween = DOTween.To(() => redFillValue, x => redFillValue = x, redTargetValue, 0.5f * valueRedDifference).OnUpdate(() =>
        {
            redBar.fillAmount = redFillValue;
        });
        
        blackBarTween?.Kill();
        var blackFillValue = blackBar.fillAmount;
        var blackTargetValue = 1f - (currentHealth / maxHealth);
        var valueBlackDifference = blackTargetValue - blackFillValue;
        blackBarTween = DOTween.To(() => blackFillValue, x => blackFillValue = x, blackTargetValue, 0.5f * valueBlackDifference).OnUpdate(() =>
        {
            blackBar.fillAmount = blackFillValue;
        }).SetDelay(0.5f);

        textChangeTween?.Kill();
        var currentHealthText = int.Parse(healthAmountText.text);
        var targetHealthText = currentHealth;
        var healthTextDifference = (currentHealthText - targetHealthText) / maxHealth;
        var textValueHolder = (float)currentHealthText;
        redBarTween = DOTween.To(() => textValueHolder, x => textValueHolder = x, targetHealthText, 0.5f * healthTextDifference).OnUpdate(() =>
        {
            healthAmountText.text = Mathf.RoundToInt(textValueHolder).ToString();
        });
    }
}
