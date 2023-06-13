using System;
using Animancer;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpider : Enemy
{
    public EnemyNavMeshManager enemyNavMeshManager;
    public EnemySpiderAnimationController enemySpiderAnimationController;
    public Collider enemySpiderCollider;
    public Transform expBallAppearLocation;
    public bool dead;

    protected override void InitSelf()
    {
        Health = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderHealth + Global.Battle.battleData.enemySpiderChaosMultiplier.enemySpiderHealthMultiplier * Global.Battle.chaos;
        Defense = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderDefense + Global.Battle.battleData.enemySpiderChaosMultiplier.enemySpiderDefenseMultiplier * Global.Battle.chaos;
        Attack = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderAttack  + Global.Battle.battleData.enemySpiderChaosMultiplier.enemySpiderAttackMultiplier * Global.Battle.chaos;
        AttackCoolDown = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderAttackCoolDown + Global.Battle.battleData.enemySpiderChaosMultiplier.enemySpiderAttackCoolDownMultiplier * Global.Battle.chaos;
        AnimationMoveSpeed = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderAnimationMoveSpeed;
        MoveSpeed = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderMoveSpeed + Global.Battle.battleData.enemySpiderChaosMultiplier.enemySpiderMoveSpeedMultiplier * Global.Battle.chaos;;
        EnemyType = Global.Battle.battleData.enemySpiderBasicStats.enemySpiderEnemyType;
        UpdateMovementSpeedRelatedAnimation(0f);
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
        UpdateMovementSpeedRelatedAnimation(MoveSpeed);
    }

    protected override void EnemyTypeChangeEvent()
    {
    }

    protected override void CannonLockedChangeEvent()
    {
    }

    protected override void OnSpawnMoreAction()
    {
        transform.LookAt(Global.Battle.playerBehaviourController.transform);
        Appear();
    }

    protected override void OnHitMoreAction()
    {
        enemySpiderAnimationController.HitGlow();
    }

    protected override void DieMoreAction()
    {
        DieBehaviourOtherThanAnimationAndRelease();
        enemySpiderAnimationController.PlayerDieAnimation();
    }

    public override void DieInstant()
    {
        DieBehaviourOtherThanAnimationAndRelease();
        enemySpiderAnimationController.PlayDieInstantAnimation();
    }

    public void ExpBallAppear()
    {
        var expBall = Global.Battle.battlePrefabManager.expBallObjectPool.Get();
        expBall.transform.position = expBallAppearLocation.position;
        expBall.Collect(Global.Battle.battleData.pickUpConfig.voidCenterExpIncreaseValueEnemySpider);
    }

    public void ReleaseSelf()
    {
        Global.Battle.enemyPrefabManager.enemySpiderPool.Release(this);
    }

    private void DieBehaviourOtherThanAnimationAndRelease()
    {
        enemyNavMeshManager.enabled = false;
        enemySpiderCollider.enabled = false;
        Global.Battle.enemyManager.enemiesOnField.Remove(this);
    }

    private void Appear()
    {
        enemyNavMeshManager.enabled = false;
        enemySpiderCollider.enabled = false;
        enemySpiderAnimationController.SpiderAppear(() =>
        {
            dead = false;
            enemySpiderCollider.enabled = true;
            enemyNavMeshManager.enabled = true;
            UpdateMovementSpeedRelatedAnimation(MoveSpeed);
            Global.Battle.enemyManager.AddEnemyToList(this);
        });
    }

    private void UpdateMovementSpeedRelatedAnimation(float newSpeed)
    {
        enemyNavMeshManager.UpdateMoveSpeed(newSpeed);
        var walkPlaySpeed = newSpeed / AnimationMoveSpeed;
        enemySpiderAnimationController.PlayWalkAnimation(playSpeed: walkPlaySpeed);
    }
}