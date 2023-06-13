using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class PreGameMenuAnimator : MonoBehaviour
{
    public Vector3 startPositionOffset;
    public float animationTime;
    public float delayBetweenEntryAppear;

    public GameObject menuTitle;
    public GameObject entryParent;

    public GameObject preGameCamera;
    public GameObject frontLight;
    
    private bool buttonState = false;
    private List<GameObject> entries;
    private List<CanvasGroup> groups;
    private List<float> oldAlphas;
    
    private List<Tweener> entryTweeners;
    private List<Tweener> textTweeners;
    private Tweener titleAppear;

    private Light frontLightComponent;
    private float frontLightIntensity;
    

    void Awake()
    {
        entries = new List<GameObject>();
        groups = entryParent.GetComponentsInChildren<CanvasGroup>().ToList();
        oldAlphas = new List<float>();
        entryTweeners = new List<Tweener>();
        textTweeners = new List<Tweener>();
        frontLightComponent = frontLight.GetComponent<Light>();
        frontLightIntensity = frontLightComponent.intensity;
        
        for (int i = 0; i < entryParent.transform.childCount; i++)
        { 
            entries.Add(entryParent.transform.GetChild(i).gameObject);
            var button = entryParent.transform.GetChild(i).GetComponent<Button>();
            button.interactable = false;
        }
        
        foreach (CanvasGroup canvasGroup in groups)
        {
            oldAlphas.Add(canvasGroup.alpha);
            canvasGroup.alpha = 0;
        }

        menuTitle.GetComponent<CanvasGroup>().alpha = 0;
        
        preGameCamera.SetActive(false);
        
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuPreGameHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuPreGameShow, Show);
        
        SetUpTweenerMove();
        SetUpTweenerAlphaLerp();
    }
    
    public void ToggleButtons(bool isEnabled)
    {
        if (buttonState != isEnabled)
        {
            buttonState = isEnabled;
            foreach (var buttonEntry in entries)
            {
                var button = buttonEntry.GetComponent<Button>();
                button.interactable = isEnabled;
                var handler = buttonEntry.GetComponent<MainMenuButtonHoverHandler>();
                handler.hoverEnabled = isEnabled;
            }
        }
    }
    
    public void Hide()
    {
        //menuCannonAnimator.StopHighlight();
        frontLightComponent.DOIntensity(frontLightIntensity, 0.5f).SetEase(Ease.InSine);
        preGameCamera.SetActive(false);
        ToggleButtons(false);

        int count = 0;
        foreach (var tweener in entryTweeners)
        {
            tweener.timeScale = 2;
            tweener.isBackwards = true;
            
            DOTween.To(() => 0, x => _ = x, 1f, count * delayBetweenEntryAppear/2f)
                .OnComplete(() => { tweener.Play(); });
            
            count++;
        }
        foreach (var tweener in textTweeners)
        {
            tweener.timeScale = 2;
            tweener.PlayBackwards();
        }

        CanvasGroup titleGroup = menuTitle.GetComponent<CanvasGroup>();
        titleAppear.Kill();
        titleAppear = DOTween.To(()=> titleGroup.alpha, x=> titleGroup.alpha = x,
            0, animationTime/2f);
    }

    public void Show()
    {
        frontLightComponent.DOIntensity(0f, 0.5f).SetEase(Ease.InSine);
        preGameCamera.SetActive(true);
        
        for (int i = 0; i < entryTweeners.Count; i++)
        {
            int index = i;
            entryTweeners[i].timeScale = 1.5f;
            entryTweeners[i].isBackwards = false;
            DOTween.To(() => 0, x => _ = x, 1f, i * delayBetweenEntryAppear)
                .OnComplete(() =>
                {
                    entryTweeners[index].Play().OnComplete(() =>
                    {
                        ToggleButtons(true);
                        //menuCannonAnimator.HightlightAllCannons();
                    });
                });
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
        
        CanvasGroup titleGroup = menuTitle.GetComponent<CanvasGroup>();
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
                .SetDelay(count * delayBetweenEntryAppear).OnComplete((() =>
                {
                    var button = entry.GetComponent<Button>();
                    button.interactable = true;
                    //menuCannonAnimator.HightlightAllCannons();;
                })).From(localPosition + startPositionOffset);
            tweener.SetAutoKill(false).Pause();
            entryTweeners.Add(tweener);
            count++;
        }
    }

    private void SetUpTweenerAlphaLerp()
    {
        for (int i = 0; i < groups.Count; i++)
        {
            int index = i;
            var tweener = DOTween.To(()=> groups[index].alpha, x=> groups[index].alpha = x,
                oldAlphas[index], animationTime)
                .SetDelay(animationTime/2f + i * delayBetweenEntryAppear/2f).SetEase(Ease.InCubic);
            tweener.SetAutoKill(false).Pause();
            textTweeners.Add(tweener);
        }
    }
}
