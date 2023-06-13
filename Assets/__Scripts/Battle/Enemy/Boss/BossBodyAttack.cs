using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossBodyAttack : Enemy
{
    private Tween gitGlow;
    private Tween glowBack;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public CanvasGroup indicatorCanvasGroup;
    public Image indicatorImage;
    //public Collider hitBox;
    public Collider moveCollider;
    public Transform attackTransform;
    public ParticleSystem caterParticleSystem;
    public ParticleSystem smokeParticleSystem;

    public Transform startPosition;
    public Transform endPosition;

    public bool alreadyHit;

    public Tween appearTween;
    public Tween lfeCountDown;

    protected override void InitSelf()
    {
        EnemyType = Global.Misc.EnemyType.Boss;
        Defense = Global.Battle.battleData.bossStats.bossDefense;
        Attack = Global.Battle.battleData.bossStats.bossBodyAttack;
        Health = 10f;
        alreadyHit = false;
        appearTween = null;
    }

    public void Appear()
    {
        InitSelf();
        attackTransform.DOMove(startPosition.position, 0f);
        //hitBox.gameObject.SetActive(false);
        moveCollider.enabled = false;
        var attackPosition = FindAttackLocation();
        transform.position = attackPosition;
        indicatorCanvasGroup.DOFade(1f, 0.3f).From(0);
        indicatorCanvasGroup.transform.DOScale(1f,0.5f).From(3f);
        Global.DoTweenWait(0.5f, () =>
        {
            indicatorImage.material.EnableKeyword("GLOW_ON");
            var glow = 1f;
            DOTween.To(() => glow, x => glow = x, 20f, 0.2f).OnUpdate(() =>
            {
                indicatorImage.material.SetFloat("_Glow", glow);
            });
        });
        Global.DoTweenWait(0.8f, () =>
        {
            caterParticleSystem.Play();
        });
        Global.DoTweenWait(0.9f, () =>
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect,GameAudios.AudioName.BossBodyAttack);
            Global.Battle.globalEffectManager.BossBodyAttackEffect();
            smokeParticleSystem.Play();
            caterParticleSystem.Pause();
            indicatorCanvasGroup.DOFade(0f, 0f);
            indicatorImage.material.DisableKeyword("GLOW_ON");
            TransformAppear();
        });
    }

    public void LifeCountDown()
    {
        lfeCountDown?.Kill();
        lfeCountDown = Global.DoTweenWait(Global.Battle.battleData.bossStats.bossBodyAttackLifeTime, () =>
        {
            Die();
        });
    }

    public void TransformAppear()
    {
        appearTween = attackTransform.DOMove(endPosition.position, 0.2f).From(startPosition.position).OnComplete(() =>
        {
            Global.Battle.enemyManager.AddEnemyToList(this);
            appearTween = null;
            if (alreadyHit)
            {
                Die();
            }
            else
            {
                LifeCountDown();
            }
        });
        //hitBox.gameObject.SetActive(true);
        moveCollider.enabled = true;
    }

    public void Disappear()
    {
        foreach (var material in skinnedMeshRenderer.materials)
        {
            material.DisableKeyword("_EMISSION");
        }
        attackTransform.DOMove(startPosition.position, 0.5f);
        caterParticleSystem.Play();
    }

    public Vector3 FindAttackLocation()
    {
        var randomLocation = Random.insideUnitCircle * 5;
        var playerPosition = Global.Battle.playerBehaviourController.transform.position;
        var inputValue = Global.Battle.playerInputController.currentInput * 3;
        var returnLocation = new Vector3(playerPosition.x + randomLocation.x + inputValue.x, 1f,playerPosition.z + randomLocation.y+ inputValue.y);
        return returnLocation;
    }

    protected override void HealthChangeEvent()
    {
        
    }

    protected override void DefenseChangeEvent()
    {
        
    }

    protected override void AttackChangeEvent()
    {
        
    }

    protected override void AttackCoolDownChangeEvent()
    {
        
    }

    protected override void MoveSpeedChangeEvent()
    {
        
    }

    protected override void EnemyTypeChangeEvent()
    {
        
    }

    protected override void CannonLockedChangeEvent()
    {
        
    }

    protected override void OnSpawnMoreAction()
    {
        
    }

    protected override void OnHitMoreAction()
    {
        HitGlow();
    }

    protected override void DieMoreAction()
    {
        moveCollider.enabled = false;
        lfeCountDown?.Kill();
        Global.Battle.enemyManager.enemiesOnField.Remove(this);
        gitGlow?.Kill();
        glowBack?.Kill();
        skinnedMeshRenderer.material.DisableKeyword("_EMISSION");
        Disappear();
    }

    public override void DieInstant()
    {
        alreadyHit = true;
        if (appearTween == null) Die();
    }
    
    public void HitGlow()
    {
        gitGlow?.Kill();
        glowBack?.Kill();
        foreach (var material in skinnedMeshRenderer.materials)
        {
            material.EnableKeyword("_EMISSION");
        }
        var intensity = -10f;
        var color = Color.white;
        gitGlow = DOTween.To(() => intensity, x => intensity = x, 2f, 0.2f).OnUpdate(() =>
        {
            foreach (var material in skinnedMeshRenderer.materials)
            {
                material.SetColor("_EmissionColor", color * intensity);
            }
        }).OnComplete(GlowBack);
    }

    public void GlowBack()
    {
        var intensityN = 2f;
        var colorN = Color.white;
        glowBack = DOTween.To(() => intensityN, x => intensityN = x, -10f, 0.2f).OnUpdate(() =>
        {
            foreach (var material in skinnedMeshRenderer.materials)
            {
                material.SetColor("_EmissionColor", colorN * intensityN);
            }
        }).OnComplete(() =>
        {
            foreach (var material in skinnedMeshRenderer.materials)
            {
                material.DisableKeyword("_EMISSION");
            }
        });
    }
}
