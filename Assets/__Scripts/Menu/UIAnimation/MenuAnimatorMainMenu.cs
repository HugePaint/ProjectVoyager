using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MenuAnimatorMainMenu : MenuAnimator
{
    public GameObject playerLightParent;
    [Range(0, 30)]
    public float cameraRotateAngle = 5;
    
    private Tweener cameraGroupRotate;
    private List<Light> playerLights;

    new void Awake()
    {
        base.Awake();
        playerLights = playerLightParent.GetComponentsInChildren<Light>().ToList();
        foreach (var light in playerLights) light.enabled = false;
        
        SetUpTweenerCameraRotate();
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuMainFirstShow, FirstShow);
    }
    
    public void FirstShow()
    {
        generalCanvasGroup.interactable = false;
        generalCanvasGroup.blocksRaycasts = false;
        titleFadeTweener.PlayForward();
        
        Sequence seq = DOTween.Sequence();
        List<Tweener> mainMenuLightsAppear = new List<Tweener>();
        foreach (Light light in playerLights)
        {
            light.enabled = true;
            float finalIntensity = light.intensity;
            mainMenuLightsAppear.Add(light.DOIntensity(finalIntensity, 3f)
                .From(0).SetEase(Ease.InSine));
        }

        seq.Append(mainMenuLightsAppear[0]);
        for (int i = 1; i < mainMenuLightsAppear.Count; i++)
            seq.Insert(0, mainMenuLightsAppear[i]);

        Show();
    }

    public void SetUpTweenerCameraRotate()
    {
        currentMenuCamera.SetActive(true);
        Vector3 targetRotate = currentMenuCamera.transform.rotation.eulerAngles;
        targetRotate.y += cameraRotateAngle;
        cameraGroupRotate = currentMenuCamera.transform.DORotate(targetRotate, 20f).SetEase(Ease.Linear);
        cameraGroupRotate.SetLoops(-1, LoopType.Yoyo);
    }
}
