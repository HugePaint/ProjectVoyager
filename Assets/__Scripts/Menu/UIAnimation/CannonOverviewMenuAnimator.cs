using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class CannonOverviewMenuAnimator : MonoBehaviour
{
    public Vector3 startPositionOffset;
    public float animationTime;
    public float delayBetweenEntryAppear;

    public GameObject menuTitle;
    public GameObject entryParent;
    public GameObject cannonSelectParent;

    public GameObject cannonOverviewCamera;
    public GameObject cannonDisplayLightParent;
    
    private bool buttonState = false;
    private List<GameObject> entries;
    private List<CanvasGroup> groups;
    private List<float> oldAlphas;
    
    private List<Tweener> entryTweeners;
    private List<Tweener> textTweeners;
    private Tweener titleAppear;
    
    private List<Light> cannonLights;
    private List<float> cannonLightsIntensities;
    private List<Tweener> cannonLightTweeners;


    void Awake()
    {
        entries = new List<GameObject>();
        groups = entryParent.GetComponentsInChildren<CanvasGroup>().ToList();
        oldAlphas = new List<float>();
        entryTweeners = new List<Tweener>();
        textTweeners = new List<Tweener>();
        cannonDisplayLightParent.SetActive(true);
        cannonLights = cannonDisplayLightParent.GetComponentsInChildren<Light>().ToList();
        cannonLightsIntensities = new List<float>();
        cannonLightTweeners = new List<Tweener>();
        
        foreach (var light in cannonLights)
        {
            light.gameObject.SetActive(true);
            cannonLightsIntensities.Add(light.intensity);
            light.intensity = 0;
        }
        
        for (int i = 0; i < entryParent.transform.childCount; i++)
        { 
            entries.Add(entryParent.transform.GetChild(i).gameObject);
            entryParent.transform.GetChild(i).position += startPositionOffset;
            var button = entryParent.transform.GetChild(i).GetComponent<Button>();
            button.interactable = false;
        }
        
        foreach (CanvasGroup canvasGroup in groups)
        {
            oldAlphas.Add(canvasGroup.alpha);
            canvasGroup.alpha = 0;
        }

        menuTitle.GetComponent<CanvasGroup>().alpha = 0;
        
        cannonSelectParent.SetActive(false);
        cannonOverviewCamera.SetActive(false);
        
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuCannonOverviewHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuCannonOverviewShow, Show);
        
        SetUpTweenerMove();
        SetUpTweenerAlphaLerp();
        SetUpTweenerCannonLightAppear();
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
        cannonSelectParent.SetActive(false);
        //menuCannonAnimator.StopHighlight();
        cannonOverviewCamera.SetActive(false);
        ToggleButtons(false);

        foreach (var tweener in cannonLightTweeners)
        {
            tweener.timeScale = 2;
            tweener.PlayBackwards();
        }
        
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
        cannonOverviewCamera.SetActive(true);
        
        foreach (var tweener in cannonLightTweeners)
        {
            tweener.timeScale = 0.8f;
            tweener.Restart();
        }
        
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
                        
                        cannonSelectParent.SetActive(true);
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
            var tweener = entry.transform.DOMove(entry.transform.position - startPositionOffset, animationTime)
                .SetDelay(count * delayBetweenEntryAppear).OnComplete((() =>
                {
                    var button = entry.GetComponent<Button>();
                    button.interactable = true;
                    cannonSelectParent.SetActive(true);
                    //menuCannonAnimator.HightlightAllCannons();;
                }));
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
    
    private void SetUpTweenerCannonLightAppear()
    {
        int count = 0;
        foreach (var light in cannonLights)
        {
            var tweener = light.DOIntensity(cannonLightsIntensities[count], animationTime).SetEase(Ease.InOutSine);
            tweener.SetAutoKill(false).Pause();
            cannonLightTweeners.Add(tweener);
        }
    }
}
