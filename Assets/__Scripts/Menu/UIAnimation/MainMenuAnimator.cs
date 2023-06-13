using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class MainMenuAnimator : MonoBehaviour
{
    public Vector3 startPositionOffset;
    public float animationTime;
    public float delayBetweenEntryAppear;
    public GameObject logo;

    public GameObject mainMenuCameraGroupParent;
    public GameObject playerLightParent;

    private List<GameObject> entries;
    private List<TMP_Text> texts;
    private List<float> oldAlphas;

    private List<Tweener> entryTweeners;
    private List<Tweener> textTweeners;
    private Tweener cameraGroupRotate;
    private Tweener logoAppear;
    private Tweener enableButtonWait;

    private List<Light> playerLights;

    void Awake()
    {
        entries = new List<GameObject>();
        texts = GetComponentsInChildren<TMP_Text>().ToList();
        oldAlphas = new List<float>();
        
        entryTweeners = new List<Tweener>();
        textTweeners = new List<Tweener>();

        playerLights = playerLightParent.GetComponentsInChildren<Light>().ToList();
        foreach (var light in playerLights)
            light.enabled = false;

        for (int i = 0; i < transform.childCount; i++)
        { 
            entries.Add(transform.GetChild(i).gameObject);
            transform.GetChild(i).GetComponent<CanvasGroup>().DOFade(0f, 0f);
        }
        
        foreach (TMP_Text text in texts)
        {
            Color color = text.color;
            oldAlphas.Add(color.a);
            color.a = 0;
            text.color = color;
        }
        
        ToggleButtons(false);
        
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuMainFirstShow, FirstShow);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuMainHide, Hide);
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuMainShow, Show);
        
        SetUpTweenerMove();
        SetUpTweenerAlphaLerp();
        SetUpTweenerCameraRotate();

        mainMenuCameraGroupParent.SetActive(false);
    }

    public void ToggleButtons(bool isEnabled)
    {
        enableButtonWait.Kill();
        foreach (var buttonEntry in entries)
        {
            var button = buttonEntry.GetComponent<Button>();
            button.interactable = isEnabled;
            var handler = buttonEntry.GetComponent<MainMenuButtonHoverHandler>();
            handler.hoverEnabled = isEnabled;
        }
    }
    
    public void SetUpTweenerCameraRotate()
    {
        mainMenuCameraGroupParent.SetActive(true);
        Vector3 targetRotate = mainMenuCameraGroupParent.transform.rotation.eulerAngles;
        targetRotate.y += 4;
        cameraGroupRotate = mainMenuCameraGroupParent.transform.DORotate(targetRotate, 20f).SetEase(Ease.Linear);
        cameraGroupRotate.SetLoops(-1, LoopType.Yoyo).Pause();
    }
    
    public void FirstShow()
    {
        ToggleButtons(false);
        
        Sequence seq = DOTween.Sequence();
        List<Tweener> mainMenuLightsAppear = new List<Tweener>();
        foreach (Light light in playerLights)
        {
            light.enabled = true;
            float finalIntensity = light.intensity;
            mainMenuLightsAppear.Add(light.DOIntensity(finalIntensity, 3f)
                .From(0).SetEase(Ease.InSine));
        }

        float delayAfterCurtain = 0f;
        seq.Append(mainMenuLightsAppear[0]).SetDelay(delayAfterCurtain);
        for (int i = 1; i < mainMenuLightsAppear.Count; i++)
            seq.Insert(delayAfterCurtain, mainMenuLightsAppear[i]);

        Global.DoTweenWait(delayAfterCurtain, Show);
    }
    

    public void Hide()
    {
        mainMenuCameraGroupParent.SetActive(false);
        ToggleButtons(false);
        cameraGroupRotate.Pause();

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

        var logoText = logo.GetComponent<TMP_Text>();
        logoAppear.Kill();
        logoAppear = DOTween.ToAlpha(()=> logoText.color, x=> logoText.color = x,
            0, animationTime/2f);
    }

    public void Show()
    {
        mainMenuCameraGroupParent.SetActive(true);
        cameraGroupRotate.Restart();

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
        enableButtonWait = DOTween.To(() => 0, x => _ = x, 1f, 1.5f)
            .OnComplete(() =>
            {
                ToggleButtons(true);
            });

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
        
        var logoText = logo.GetComponent<TMP_Text>();
        float logoDelay = 1f;
        logoAppear.Kill();
        logoAppear = DOTween.ToAlpha(()=> logoText.color, x=> logoText.color = x,
            1f, animationTime * 2).SetEase(Ease.OutCubic).SetDelay(logoDelay);
    }
    
    private void SetUpTweenerMove()
    {
        int count = 0;
        foreach (GameObject entry in entries)
        {
            entry.GetComponent<CanvasGroup>().DOFade(1f, 0f);
            var tweener = entry.transform.DOLocalMoveX(0 , animationTime)
                .SetDelay(count * delayBetweenEntryAppear).OnComplete((() =>
                {
                    var button = entry.GetComponent<Button>();
                    button.interactable = true;
                })).From(- startPositionOffset.x);
            tweener.SetAutoKill(false).Pause();
            entryTweeners.Add(tweener);
            count++;
        }
    }

    private void SetUpTweenerAlphaLerp()
    {
        for (int i = 0; i < texts.Count; i++)
        {
            int index = i;
            var tweener = DOTween.ToAlpha(()=> texts[index].color, x=> texts[index].color = x,
                oldAlphas[index], animationTime)
                .SetDelay(animationTime/2f + i * delayBetweenEntryAppear/2f).SetEase(Ease.InCubic);
            tweener.SetAutoKill(false).Pause();
            textTweeners.Add(tweener);
        }
    }
}
