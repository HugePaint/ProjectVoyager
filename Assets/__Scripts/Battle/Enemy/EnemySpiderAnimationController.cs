using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpiderAnimationController : MonoBehaviour
{
    public AnimancerComponent animancerComponent;
    public AnimationClip idleAnimationClip;
    public AnimationClip walkAnimationClip;
    public AnimationClip dieAnimationClip;

    public EnemyAppearOrbController enemyAppearOrbController;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public EnemySpiderDieAnimationHandler enemySpiderDieAnimationHandler;

    private Tween gitGlow;
    private Tween glowBack;

    public void HitGlow()
    {
        gitGlow?.Kill();
        glowBack?.Kill();
        skinnedMeshRenderer.material.EnableKeyword("_EMISSION");
        var intensity = -10f;
        var color = Color.white;
        gitGlow = DOTween.To(() => intensity, x => intensity = x, 2f, 0.2f).OnUpdate(() =>
        {
            skinnedMeshRenderer.material.SetColor("_EmissionColor", color * intensity);
        }).OnComplete(GlowBack);
    }

    public void GlowBack()
    {
        var intensityN = 2f;
        var colorN = Color.white;
        glowBack = DOTween.To(() => intensityN, x => intensityN = x, -10f, 0.2f).OnUpdate(() =>
        {
            skinnedMeshRenderer.material.SetColor("_EmissionColor", colorN * intensityN);
        }).OnComplete(() =>
        {
            skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        });
    }

    public void PlayerIdleAnimation(float fateDuration = 0.2f)
    {
        animancerComponent.Play(idleAnimationClip, fateDuration);
    }

    // public void PlayerAttackAnimation(float fateDuration = 0.2f)
    // {
    //     animancerComponent.Play(attackAnimationClip, fateDuration);
    // }

    public void PlayerDieAnimation(float fateDuration = 0.2f)
    {
        gitGlow?.Kill();
        glowBack?.Kill();
        skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        var animancerState = animancerComponent.Play(dieAnimationClip, fateDuration);
        animancerState.Speed = 1.5f;
    }

    public void PlayDieInstantAnimation()
    {
        gitGlow?.Kill();
        glowBack?.Kill();
        skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        enemySpiderDieAnimationHandler.DieInstant();
    }

    public void DisableAnimancerComponent()
    {
        animancerComponent.enabled = false;
    }
    
    public void EnableAnimancerComponent()
    {
        animancerComponent.enabled = true;
    }

    public void PlayWalkAnimation(float fateDuration = 0.2f, float playSpeed = 1f)
    {
        var animancerState = animancerComponent.Play(walkAnimationClip, fateDuration);
        animancerState.Speed = playSpeed;
    }

    public void SpiderAppear(Action dissolveCompleteAction)
    {
        PlayerIdleAnimation();
        enemyAppearOrbController.gameObject.SetActive(true);
        transform.DOScale(1f, 0.5f).From(0).SetEase(Ease.OutBack);
        Global.DoTweenWait(0.8f, () => { enemyAppearOrbController.OrbDissolve(dissolveCompleteAction); });
    }
}