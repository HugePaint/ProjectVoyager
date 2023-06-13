using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeMenuAnimator : MonoBehaviour
{
    public float animationTime;
    public float waitAfterUpgradeToShow = 1f;

    public GameObject hideAfterUpgrade;
    [FormerlySerializedAs("chipPanels")] public GameObject showAfterUpgrade;
    public GameObject menuCamera;
    public GameObject particles;
    
    private CanvasGroup mainCanvasGroup;
    private CanvasGroup showAfterUpgradeCanvasGroup;
    private CanvasGroup hideAfterUpgradeCanvasGroup;

    private Tweener mainCanvasGroupAppear;
    private Tweener hideAfterUpgradeFadeOut;
    private Tweener showAfterUpgradeAppear;
    private Tween waitToAppear;
    private Tween waitToAppear2;

    void Awake()
    {
        mainCanvasGroup = GetComponent<CanvasGroup>();
        showAfterUpgradeCanvasGroup = showAfterUpgrade.GetComponent<CanvasGroup>();
        hideAfterUpgradeCanvasGroup = hideAfterUpgrade.GetComponent<CanvasGroup>();
        menuCamera.SetActive(false);
        particles.SetActive(false);

        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;
        showAfterUpgradeCanvasGroup.interactable = false;
        showAfterUpgradeCanvasGroup.blocksRaycasts = false;
        
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuUpgradeHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuUpgradeShow, Show);
        // EventCenter.GetInstance().AddEventListener(Global.Events.MenuUpgradeProceed, UpgradeProceed);
        // EventCenter.GetInstance().AddEventListener(Global.Events.MenuUpgradeBack, UpgradeBack);
        
        SetUpTweenerAlphaLerp();
    }
    
    public void Hide()
    {
        menuCamera.SetActive(false);
        particles.SetActive(false);
        mainCanvasGroupAppear.timeScale = 3f;
        mainCanvasGroupAppear.PlayBackwards();
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;
        showAfterUpgradeAppear.timeScale = 3f;
        showAfterUpgradeAppear.PlayBackwards();
        showAfterUpgradeCanvasGroup.interactable = false;
        showAfterUpgradeCanvasGroup.blocksRaycasts = false;
        waitToAppear?.Kill();
        waitToAppear2?.Kill();
    }

    public void Show()
    {
        menuCamera.SetActive(true);
        mainCanvasGroupAppear.timeScale = 1f;
        mainCanvasGroupAppear.PlayForward();
        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;
        hideAfterUpgradeCanvasGroup.interactable = true;
        hideAfterUpgradeCanvasGroup.blocksRaycasts = true;
        hideAfterUpgradeFadeOut.Restart();
        hideAfterUpgradeFadeOut.Pause();
    }

    public void UpgradeProceed()
    {
        waitToAppear = Global.DoTweenWait(waitAfterUpgradeToShow, () =>
        {
            particles.SetActive(true);
            showAfterUpgradeAppear.timeScale = 1f;
            showAfterUpgradeAppear.PlayForward();
        });
        
        hideAfterUpgradeFadeOut.PlayForward();
        hideAfterUpgradeCanvasGroup.interactable = false;
        hideAfterUpgradeCanvasGroup.blocksRaycasts = false;
    }

    public void UpgradeBack()
    {
        particles.SetActive(false);
        showAfterUpgradeAppear.timeScale = 3f;
        showAfterUpgradeAppear.PlayBackwards();
        showAfterUpgradeCanvasGroup.interactable = false;
        showAfterUpgradeCanvasGroup.blocksRaycasts = false;

        waitToAppear.Kill();
        waitToAppear = Global.DoTweenWait(waitAfterUpgradeToShow/2f, () =>
        {
            hideAfterUpgradeFadeOut.PlayBackwards();
            waitToAppear2 = Global.DoTweenWait(animationTime / 2f, () =>
            {
                hideAfterUpgradeCanvasGroup.interactable = true;
                hideAfterUpgradeCanvasGroup.blocksRaycasts = true;
            });
        });
    }
    
    private void SetUpTweenerAlphaLerp()
    {
        mainCanvasGroupAppear = mainCanvasGroup.DOFade(1f, animationTime).From(0f).SetEase(Ease.InCubic)
            .OnComplete(() =>
            {
                
            });
        mainCanvasGroupAppear.SetAutoKill(false).Pause();
        
        showAfterUpgradeAppear = showAfterUpgradeCanvasGroup.DOFade(1f, animationTime).From(0f).SetEase(Ease.InCubic)
            .OnComplete((() =>
            {
                showAfterUpgradeCanvasGroup.interactable = true;
                showAfterUpgradeCanvasGroup.blocksRaycasts = true;
            }));
        showAfterUpgradeAppear.SetAutoKill(false).Pause();

        hideAfterUpgradeFadeOut =
            hideAfterUpgradeCanvasGroup.DOFade(0f, animationTime / 2f).From(0f).SetEase(Ease.InCubic).From(1);
        hideAfterUpgradeFadeOut.SetAutoKill(false).Pause();
    }
}
