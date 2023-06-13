using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float backgroundAppearTime = 0.3f;
    [FormerlySerializedAs("enabled")] public bool hoverEnabled = true;
    public bool forceDisable = false;
    
    private Image buttonBackground;
    private RectTransform rectTransform;
    private Color originalColor;

    private Tweener buttonTweener;
    private Tweener bgTweener;
    
    public void Awake()
    {
        buttonBackground = transform.GetComponent<Image>();
        rectTransform = transform.GetComponent<RectTransform>();
        originalColor = buttonBackground.color;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (forceDisable) return;
        if (!hoverEnabled) return;
        
        buttonTweener = buttonBackground.DOColor(Color.white, backgroundAppearTime);
        bgTweener = rectTransform.DOScale(new Vector3(1.03f, 1.1f, 1f), backgroundAppearTime);
        
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayMenuSoundEffect, GameAudios.AudioName.MenuEntryButtonHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (forceDisable) return;
        
        buttonTweener.Kill();
        bgTweener.Kill();
        
        buttonBackground.DOColor(originalColor, backgroundAppearTime/2f);
        rectTransform.DOScale(new Vector3(1f, 1f, 1f), backgroundAppearTime/2f);
    }

    public void OnEntryClick()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayMenuSoundEffect, GameAudios.AudioName.MenuEntryButtonClick);
    }
    
    public void OnEntryClickFailed()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayMenuSoundEffect, GameAudios.AudioName.MenuEntryButtonFailed);
    }
}
