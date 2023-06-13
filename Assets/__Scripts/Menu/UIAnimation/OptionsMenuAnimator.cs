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

public class OptionsMenuAnimator : MonoBehaviour
{
    public Vector3 startPositionOffset;
    public float animationTime;
    public float delayBetweenEntryAppear;
    [FormerlySerializedAs("logo")] public GameObject title;

    public GameObject panelParentGroup;
    public GameObject optionsMenuCamera;

    private List<GameObject> entries;
    private List<CanvasGroup> canvasGroups;

    private List<Tweener> entryTweeners;
    private List<Tweener> textTweeners;
    private Tweener titleAppear;

    void Awake()
    {
        entries = new List<GameObject>();
        canvasGroups = panelParentGroup.GetComponentsInChildren<CanvasGroup>().ToList();
        
        entryTweeners = new List<Tweener>();
        textTweeners = new List<Tweener>();
        optionsMenuCamera.SetActive(false);
      
        for (int i = 0; i < panelParentGroup.transform.childCount; i++)
        {
            var entry = panelParentGroup.transform.GetChild(i);
            entries.Add(entry.gameObject);
            // entry.DOLocalMove(entry.transform.localPosition + startPositionOffset, 0f);
            // entry.GetComponent<CanvasGroup>().DOFade(0f, 0f);
        }
        
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuOptionsHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuOptionsShow, Show);
        
        SetUpTweenerMove();
        SetUpTweenerAlphaLerp();
        // SetUpTweenerCameraRotate();

        title.GetComponent<CanvasGroup>().alpha = 0;
        
        // mainMenuCameraGroupParent.SetActive(true);
    }
    
    public void Hide()
    {
        // mainMenuCameraGroupParent.SetActive(false);
        // ToggleButtons(false);
        optionsMenuCamera.SetActive(false);
        
        int count = 0;
        for (int i = entryTweeners.Count - 1; i >= 0; i--)
        {
            entryTweeners[i].timeScale = 2;
            entryTweeners[i].isBackwards = true;
            int index = i;   
            DOTween.To(() => 0, x => _ = x, 1f, count * delayBetweenEntryAppear/2f)
                .OnComplete(() => { entryTweeners[index].Play(); });
            
            count++;
        }
        
        foreach (var tweener in textTweeners)
        {
            tweener.timeScale = 2;
            tweener.PlayBackwards();
        }
      
        CanvasGroup titleGroup = title.GetComponent<CanvasGroup>();
        titleAppear.Kill();
        titleAppear = DOTween.To(()=> titleGroup.alpha, x=> titleGroup.alpha = x,
            0, animationTime/2f);
    }

    public void Show()
    {
        // mainMenuCameraGroupParent.SetActive(true);
        optionsMenuCamera.SetActive(true);
        
        int count = 0;
        foreach (var tweener in entryTweeners)
        {
            int index = count;
            tweener.timeScale = 1.5f;
            tweener.isBackwards = false;
            DOTween.To(() => 0, x => _ = x, 1f, count * delayBetweenEntryAppear)
                .OnComplete(() =>
                {
                    tweener.Play();
                });
            count++;
        }

        for (int i = 0; i < textTweeners.Count; i++)
        {
            int index = i;
            textTweeners[i].timeScale = 2;
            textTweeners[i].isBackwards = false;
            DOTween.To(() => 0, x => _ = x, 1f, i * delayBetweenEntryAppear)
                .OnComplete(() =>
                {
                    textTweeners[index].Play().SetDelay(animationTime / textTweeners[index].timeScale);
                });
        }
        
        CanvasGroup titleGroup = title.GetComponent<CanvasGroup>();
        float titleDelay = 1f;
        titleAppear.Kill();
        titleAppear = titleGroup.DOFade(1, animationTime).SetEase(Ease.OutCubic).SetDelay(titleDelay);
    }
    
    private void SetUpTweenerMove()
    {
        int count = 0;
        foreach (GameObject entry in entries)
        {
            var localPosition = entry.transform.localPosition;
            var tweener = entry.transform.DOLocalMove(localPosition, animationTime)
                .SetDelay(count * delayBetweenEntryAppear).From(localPosition + startPositionOffset);
            tweener.SetAutoKill(false).Pause();
            entryTweeners.Add(tweener);
            count++;
        }
    }

    private void SetUpTweenerAlphaLerp()
    {
        for (int i = 0; i < canvasGroups.Count; i++)
        {
            int index = i;
            var tweener = DOTween.To(()=> canvasGroups[index].alpha, x=> canvasGroups[index].alpha = x,
                    1f, animationTime)
                .SetDelay(animationTime/2f + i * delayBetweenEntryAppear/2f).SetEase(Ease.InCubic).From(0f);
            tweener.SetAutoKill(false).Pause();
            textTweeners.Add(tweener);
        }
    }
}
