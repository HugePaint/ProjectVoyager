using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ElementSymbolOnEnemy : MonoBehaviour
{
    public Image image;
    public Material material;

    private Tween glowTween;

    private void Awake()
    {
        image.material = Instantiate(image.material);
        material = image.material;
    }

    private void OnEnable()
    {
        material = image.material;
        SwitchElementIcon(Global.Misc.ElementType.None);
    }

    public void ElementImageAppear(Global.Misc.ElementType elementType)
    {
        //temp
        if (elementType == Global.Misc.ElementType.Dark || elementType == Global.Misc.ElementType.Light) return;
        //temp
        
        SwitchElementIcon(elementType);
        var glowValue = 10f;
        glowTween?.Kill();
        glowTween = DOTween.To(() => glowValue, x => glowValue = x, 1f, 0.3f).OnUpdate(() =>
        {
            material.SetFloat("_Glow",glowValue);
        }).SetEase(Ease.InQuint);
        transform.DOKill();
        transform.DOScale(1f, 0.3f).From(2f).SetEase(Ease.InQuint);
    }

    public void SwitchElementIcon(Global.Misc.ElementType elementType)
    {
        switch (elementType)
        {
            case Global.Misc.ElementType.Fire:
                image.sprite = Global.Battle.battlePrefabManager.fireElementSprite;
                material.SetColor("_Color",Global.Misc.colorData.Fire);
                break;
            case Global.Misc.ElementType.Water:
                image.sprite = Global.Battle.battlePrefabManager.waterElementSprite;
                material.SetColor("_Color",Global.Misc.colorData.Water);
                break;
            case Global.Misc.ElementType.Nature:
                image.sprite = Global.Battle.battlePrefabManager.natureElementSprite;
                material.SetColor("_Color",Global.Misc.colorData.Nature);
                break;
            case Global.Misc.ElementType.Light:
                break;
            case Global.Misc.ElementType.Dark:
                break;
            case Global.Misc.ElementType.None:
                image.sprite = null;
                material.SetColor("_Color",new Color(0f,0f,0f,0f));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
        }
        material.SetFloat("_Glow",1f);
    }
}
