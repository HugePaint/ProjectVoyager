using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCannonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float emissionAppearTime = 0.3f;
    public float originalIntensity = 2.2f;
    public float targetIntensity = 7.5f;
    public GameObject targetGunModel;
    private MeshRenderer meshRenderer;
    private Color oldColor;

    private Tweener materialTweener;

    public void Awake()
    {
        meshRenderer = targetGunModel.transform.GetComponent<MeshRenderer>();
        meshRenderer.material.DisableKeyword("_EMISSION");
        meshRenderer.material.EnableKeyword("_EMISSION");
        oldColor = meshRenderer.material.GetColor("_EmissionColor");
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        materialTweener?.Kill();
        meshRenderer.material.DisableKeyword("_EMISSION");
        meshRenderer.material.EnableKeyword("_EMISSION");
        var intensity = originalIntensity;
        var color = oldColor;
        materialTweener = DOTween.To(() => intensity, x => intensity = x, targetIntensity, emissionAppearTime).OnUpdate(() =>
        {
            meshRenderer.material.SetColor("_EmissionColor", color * intensity);
        });
        
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayMenuSoundEffect, GameAudios.AudioName.MenuCannonHover);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        materialTweener?.Kill();
        meshRenderer.material.DisableKeyword("_EMISSION");
        meshRenderer.material.EnableKeyword("_EMISSION");
        var intensity = targetIntensity;
        var color = oldColor;
        materialTweener = DOTween.To(() => intensity, x => intensity = x, originalIntensity, emissionAppearTime).OnUpdate(() =>
        {
            meshRenderer.material.SetColor("_EmissionColor", color * intensity);
        });
    }

    void OnDisable()
    {
        materialTweener.Kill();
        meshRenderer.material.SetColor("_EmissionColor", oldColor);
    }
}
