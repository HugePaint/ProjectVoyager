using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpiderDieAnimationHandler : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public EnemySpider enemyScript;
    public float clipDuration;

    private int dieMoment;
    private void OnEnable()
    {
        AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(skinnedMeshRenderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, 0f);
        dieMoment = Random.Range(0, 5);
    }

    // called in animator event
    public void DieFadeOut(int dieMomentTriggered)
    {
        if (dieMomentTriggered != dieMoment) return;
        if (enemyScript.dead) return;
        enemyScript.dead = true;
        enemyScript.ExpBallAppear();
        enemyScript.enemySpiderAnimationController.DisableAnimancerComponent();
        ResetPosition();
        var clip = 0f;
        DOTween.To(() => clip, x => clip = x, 1f, clipDuration).OnUpdate(() =>
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(skinnedMeshRenderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
        }).OnComplete(() =>
        {
            ResetPosition();
            enemyScript.enemySpiderAnimationController.EnableAnimancerComponent();
            ResetPosition();
            enemyScript.ReleaseSelf();
        });
    }

    public void ResetPosition()
    {
        transform.localRotation = quaternion.identity;
        transform.localPosition = new Vector3(0f, 0f, 0f);
    }

    public void DieInstant()
    {
        enemyScript.dead = true;
        enemyScript.enemySpiderAnimationController.DisableAnimancerComponent();
        ResetPosition();
        var clip = 0f;
        DOTween.To(() => clip, x => clip = x, 1f, clipDuration).OnUpdate(() =>
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(skinnedMeshRenderer.material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
        }).OnComplete(() =>
        {
            ResetPosition();
            enemyScript.enemySpiderAnimationController.EnableAnimancerComponent();
            ResetPosition();
            enemyScript.ReleaseSelf();
        });
    }
}
