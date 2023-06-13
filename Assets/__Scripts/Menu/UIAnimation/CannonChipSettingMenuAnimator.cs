using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CannonChipSettingMenuAnimator : MonoBehaviour
{
    public List<GameObject> chipCameras;
    public CanvasGroup canvasGroup;
    public GameObject characterModel;
    private CannonChipSettingMenuPreviousNextHandler cannonChipSettingMenuPreviousNextHandler;
    private Tweener fadeInTweener;

    private void Awake()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;

        cannonChipSettingMenuPreviousNextHandler = GetComponent<CannonChipSettingMenuPreviousNextHandler>();
        
        EventCenter.GetInstance().AddEventListener<int>(Global.Events.MenuCannonChipSettingShow, OnTransition);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuCannonChipSettingHide, OnHide);
    }

    public void OnTransition(int index)
    {
        cannonChipSettingMenuPreviousNextHandler.Initialize(index);
        
        foreach (var cam in chipCameras)
            cam.SetActive(false);
        chipCameras[index].SetActive(true);

        foreach (var slot in Global.MainMenu.cannonDisplayController.cannonSlots)
            slot.SetActive(false);
        Global.MainMenu.cannonDisplayController.cannonSlots[index].SetActive(true);
        
        fadeInTweener.Kill();
        fadeInTweener = DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 0.5f)
            .SetEase(Ease.OutCirc).SetDelay(1f).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                characterModel.SetActive(false);
            });
    }
    
    public void OnHide()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        characterModel.SetActive(true);
        foreach (var cam in chipCameras)
        {
            cam.SetActive(false);
        }
        
        fadeInTweener.Kill();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 0.5f)
            .SetEase(Ease.InCirc);
    }
}
