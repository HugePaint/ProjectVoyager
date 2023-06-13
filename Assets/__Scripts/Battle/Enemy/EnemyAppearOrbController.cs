using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using UnityEngine;

public class EnemyAppearOrbController : MonoBehaviour
{
    public MeshRenderer renderer;
    public float clipDuration;

    private void Awake()
    {
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(renderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, 0f);
    }

    private void OnEnable()
    {
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(renderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, 0f);
    }

    public void OrbDissolve(Action completeAction)
    {
        DoRotate();
        var clip = 0f;
        DOTween.To(() => clip, x => clip = x, 1f, clipDuration).SetEase(Ease.InSine).OnUpdate(() =>
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(renderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
        }).OnComplete(() =>
        {
            gameObject.SetActive(false);
            completeAction?.Invoke();
        });
    }
    public void DoRotate()
    {
        transform.DORotate(transform.rotation.eulerAngles + new Vector3(0f, 179f, 0), clipDuration);
    }
}