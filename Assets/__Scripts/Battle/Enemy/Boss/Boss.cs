using System;
using System.Collections;
using System.Collections.Generic;
using AmazingAssets.AdvancedDissolve;
using Animancer;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : MonoBehaviour
{
    private AnimancerComponent animancerComponent;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private BossLaserAttackManager bossLaserAttackManager;
    public BossAttackIndicator bossAttackIndicator;
    public BossLaserHitPoint bossLaserHitPoint;

    public AnimationClip bossIdleClip;

    public List<BossBodyAttack> bossBodyAttacks;

    private Tween skillTween;
    private Tween attackTween;

    private void Awake()
    {
        animancerComponent = GetComponent<AnimancerComponent>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        bossLaserAttackManager = GetComponent<BossLaserAttackManager>();
        
        EventCenter.GetInstance().AddEventListener(Global.Events.GameOver, () =>
        {
            skillTween?.Kill();
            attackTween?.Kill();
        });
    }

    private void Start()
    {
        animancerComponent.Play(bossIdleClip);
    }

    public void DisableComponents()
    {
        bossAttackIndicator.gameObject.SetActive(false);
        bossLaserHitPoint.gameObject.SetActive(false);
        foreach (var bossBodyAttack in bossBodyAttacks)
        {
            bossBodyAttack.gameObject.SetActive(false);
        }
    }

    public void Appear()
    {
        var clip = 1f;
        foreach (var material in skinnedMeshRenderer.materials)
        {
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, 1f);
        }

        Global.DoTweenWait(0.5f, () =>
        {
            DOTween.To(() => clip, x => clip = x, 0f, 2f).SetEase(Ease.InCubic).OnUpdate(() =>
            {
                foreach (var material in skinnedMeshRenderer.materials)
                {
                    AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(material, AdvancedDissolveProperties.Cutout.Standard.Property.Clip, clip);
                }
            });
        });
        TryAttack();
        TryBodyAttack();
    }

    public void SetPosition(Transform targetPosition)
    {
        transform.position = targetPosition.position;
        transform.LookAt(Global.Battle.voidCenter.transform);
    }


    public void TryAttack()
    {
        skillTween = Global.DoTweenWait(Global.Battle.battleData.bossStats.bossSkillCoolDown, () =>
        {
            var randomSkill = Random.Range(0, 2);
            switch (randomSkill)
            {
                case 0:
                    SpeedUpSkill();
                    break;
                case 1:
                    LaserAttack();
                    break;
            }
            TryAttack();
        });
        
        
    }

    public void TryBodyAttack()
    {
        attackTween = Global.DoTweenWait(Global.Battle.battleData.bossStats.bossBodyAttackCoolDown, () =>
        {
            BodyAttack();
            TryBodyAttack();
        });
    }

    public void LaserAttack()
    {
        bossAttackIndicator.transform.position = transform.position;
        bossAttackIndicator.gameObject.SetActive(true);
        bossAttackIndicator.Appear();
        Global.DoTweenWait(2f, () =>
        {
            bossAttackIndicator.Disappear();
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.BossLaser);
            Global.DoTweenWait(0.3f, () =>
            {
                Global.Battle.globalEffectManager.BossLaserEffect();
                bossLaserHitPoint.gameObject.SetActive(true);
                bossLaserAttackManager.Shoot(bossLaserHitPoint.transform);
                bossLaserHitPoint.transform.DOMove(bossAttackIndicator.endPosition.position, 1f).From(bossAttackIndicator.startPosition.position).SetEase(Ease.Linear).OnComplete(() =>
                {
                    bossLaserAttackManager.LaserEnd();
                });
                
                bossAttackIndicator.image.material.DisableKeyword("GLOW_ON");
                bossAttackIndicator.gameObject.SetActive(false);
            });
        });
    }

    public void SpeedUpSkill()
    {
        var enemyEffectList = Global.GetRandomItemsFromList(Global.Battle.enemyManager.enemiesOnField, Global.Battle.battleData.bossStats.bossSpeedUpSkillNum);
        foreach (var enemy in enemyEffectList)
        {
            if (enemy.EnemyType != Global.Misc.EnemyType.Boss) Global.Battle.buffManager.SpeedUp(enemy);
        }
    }

    public void BodyAttack()
    {
        foreach (var bossBodyAttack in bossBodyAttacks)
        {
            bossBodyAttack.gameObject.SetActive(true);
            bossBodyAttack.Appear();
        }
    }
}
