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

public class MenuAnimator : MonoBehaviour
{
    public GameObject menuTitle;
    public GameObject entryGroupParent;
    public GameObject currentMenuCamera;
    
    public Vector3 startPositionOffset;
    public float animationTime = 2f;
    public float delayBetweenEntryAppear = 0.15f;
    // Total Time = animationTime + delayBetweenEntryAppear
    public float titleDelay = 1f;

    private CanvasGroup entryParentCanvasGroup;
    private List<GameObject> entries;
    private List<CanvasGroup> entryCanvasGroups;

    private List<Tweener> entryMoveTweeners;
    private List<Tweener> entryFadeTweeners;
    private Tweener titleFadeTweener;

    void Awake()
    {
        entries = new List<GameObject>();
        entryCanvasGroups = new List<CanvasGroup>();
        for (int i = 0; i < entryGroupParent.transform.childCount; i++)
        {
            var entry = entryGroupParent.transform.GetChild(i);
            entries.Add(entry.gameObject);
            entryCanvasGroups.Add(entry.GetComponent<CanvasGroup>());
        }
        
        entryMoveTweeners = new List<Tweener>();
        entryFadeTweeners = new List<Tweener>();
        currentMenuCamera.SetActive(false);
        
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuOptionsHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuOptionsShow, Show);
        
        SetUpTweenersEntriesMove();
        SetUpTweenersEntriesFade();
        SetUpTweenerTitleFade();
    }
    
    public void Hide()
    {
        // TODO: ToggleButtons(false);
        currentMenuCamera.SetActive(false);
        
        int count = 0;
        for (int i = entryMoveTweeners.Count - 1; i >= 0; i--)
        {
            float timeScale = 2f;
            entryMoveTweeners[i].timeScale = timeScale;
            entryMoveTweeners[i].isBackwards = true;
            int index = i;   
            DOTween.To(() => 0, x => _ = x, 1f,
                    count * delayBetweenEntryAppear/timeScale)
                .OnComplete(() =>
                {
                    // Play backwards to hide
                    entryMoveTweeners[index].Play();
                });
            
            count++;
        }
        
        foreach (var tweener in entryFadeTweeners)
        {
            tweener.timeScale = 2;
            tweener.PlayBackwards();
        }
      
        CanvasGroup titleGroup = menuTitle.GetComponent<CanvasGroup>();
        titleFadeTweener.Kill();
        titleFadeTweener = DOTween.To(()=> titleGroup.alpha, x=> titleGroup.alpha = x,
            0, animationTime/2f);
    }

    public void Show()
    {
        // mainMenuCameraGroupParent.SetActive(true);
        currentMenuCamera.SetActive(true);
        
        int count = 0;
        foreach (var tweener in entryMoveTweeners)
        {
            float timeScale = 1f;
            tweener.timeScale = timeScale;
            tweener.isBackwards = false;
            DOTween.To(() => 0, x => _ = x, 1f, 
                    count * delayBetweenEntryAppear/timeScale)
                .OnComplete(() =>
                {
                    // Play forwards to show
                    tweener.Play();
                });
            count++;
        }

        for (int i = 0; i < entryFadeTweeners.Count; i++)
        {
            int index = i;
            entryFadeTweeners[i].timeScale = 2;
            entryFadeTweeners[i].isBackwards = false;
            DOTween.To(() => 0, x => _ = x, 1f, i * delayBetweenEntryAppear)
                .OnComplete(() =>
                {
                    entryFadeTweeners[index].Play()
                        .SetDelay(animationTime / entryFadeTweeners[index].timeScale);
                });
        }
        
        CanvasGroup titleGroup = menuTitle.GetComponent<CanvasGroup>();
        float titleDelay = 1f;
        titleFadeTweener.Kill();
        titleFadeTweener = titleGroup.DOFade(1, animationTime).SetEase(Ease.OutCubic).SetDelay(titleDelay);
    }
    
    private void SetUpTweenersEntriesMove()
    {
        int count = 0;
        foreach (GameObject entry in entries)
        {
            var localPosition = entry.transform.localPosition;
            var tweener = entry.transform.DOLocalMove(localPosition, animationTime)
                .SetDelay(count * delayBetweenEntryAppear).From(localPosition + startPositionOffset);
            tweener.SetAutoKill(false).Pause();
            entryMoveTweeners.Add(tweener);
            count++;
        }
    }

    private void SetUpTweenersEntriesFade()
    {
        for (int i = 0; i < entryCanvasGroups.Count; i++)
        {
            int index = i;
            var tweener = entryCanvasGroups[index].DOFade(1f, animationTime)
                .SetDelay(animationTime/2f + i * delayBetweenEntryAppear/2f).SetEase(Ease.InCubic).From(0f);
            tweener.SetAutoKill(false).Pause();
            entryFadeTweeners.Add(tweener);
        }
    }

    private void SetUpTweenerTitleFade()
    {
        titleFadeTweener = menuTitle.GetComponent<CanvasGroup>()
            .DOFade(1f, animationTime)
            .SetEase(Ease.OutCubic).SetDelay(titleDelay).From(0f);
        titleFadeTweener.SetAutoKill(false).Pause();
    }
}
