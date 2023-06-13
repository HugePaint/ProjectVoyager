using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Flexalon;
using UnityEngine;

public class ChipGainShred : MonoBehaviour
{
    private FlexalonObject flexalonObject;
    private FlexalonLerpAnimator flexalonLerpAnimator;
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        flexalonObject = GetComponent<FlexalonObject>();
        flexalonLerpAnimator = GetComponent<FlexalonLerpAnimator>();
    }

    public void ResetScaleToZero()
    {
        flexalonLerpAnimator.AnimateScale = false;
        flexalonObject.Scale = new Vector3(0f, 0f, 0f);
        flexalonObject.Rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void AnimateScale()
    {
        flexalonLerpAnimator.AnimateScale = true;
        AnimateInterpolationSpeed(4f, 1f, 1f);
        flexalonObject.Scale = new Vector3(1f, 1f, 1f);
    }

    public void Dissolve()
    {
        var clip = 0f;
        DOTween.To(() => clip, x => clip = x, 1f, 1f).OnUpdate(() =>
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(meshRenderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
        });
    }

    public void AnimateInterpolationSpeed(float startValue, float endValue, float duration)
    {
        var speed = startValue;
        DOTween.To(() => speed, x => speed = x, endValue, duration).SetEase(Ease.Linear).OnUpdate(() =>
        {
            flexalonLerpAnimator.InterpolationSpeed = speed;
        });
        
    }
}
