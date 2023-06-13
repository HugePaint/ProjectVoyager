using System;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class CannonBattle : MonoBehaviour
{
    public bool active;
    public int cannonId;
    public Global.Misc.CannonAttackType attackType;
    public Global.Misc.ElementType elementType;
    public float attack;
    public float attackCoolDown;
    public float laserDuration;
    public float energyRange;


    public Transform dischargePointBullet;
    public Transform dischargePointLaser;
    public Transform dischargePointEnergy;
    private Transform dischargePoint;
    public bool inCoolDown;
    private CannonAnimation cannonAnimation;
    public CannonOutfitChanger cannonOutfitChanger;
    private CannonLocalMotionManager cannonLocalMotionManager;
    private Tween leaveTween;
    
    //mmf effects
    public MMF_Player bulletShootMmfPlayer;
    public MMF_Player laserShootMmfPlayer;
    public MMF_Player energyShootMmfPlayer;
    
    private bool attacking;

    private void Awake()
    {
        cannonAnimation = GetComponent<CannonAnimation>();
        cannonOutfitChanger = GetComponent<CannonOutfitChanger>();
        cannonLocalMotionManager = GetComponentInChildren<CannonLocalMotionManager>();
        dischargePoint = dischargePointBullet;
        inCoolDown = false;
        attacking = false;
    }

    private void Start()
    {
        //cannonOutfitChanger.UpdateOutfit(attackType, elementType);
    }

    public void LoadInitData(BattleDataManager.CannonImport importData)
    {
        attackType = importData.cannonImportInfo.cannonAttackType;
        elementType = importData.cannonImportInfo.elementType;
        attack = importData.cannonImportInfo.attack;
        attackCoolDown = importData.cannonImportInfo.attackCoolDown;
        laserDuration = importData.cannonImportInfo.laserDuration;
        energyRange = importData.cannonImportInfo.energyRange;
        AssignDischargePoint(attackType);
        Validate();
    }

    public void AssignDischargePoint(Global.Misc.CannonAttackType attackType)
    {
        switch (attackType)
        {
            case Global.Misc.CannonAttackType.Bullet:
                dischargePoint = dischargePointBullet;
                break;
            case Global.Misc.CannonAttackType.Laser:
                dischargePoint = dischargePointLaser;
                break;
            case Global.Misc.CannonAttackType.Energy:
                dischargePoint = dischargePointEnergy;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(attackType), attackType, null);
        }
    }

    public void Validate()
    {
        switch (attackType)
        {
            case Global.Misc.CannonAttackType.Bullet:
                attackCoolDown = MathF.Max(attackCoolDown, 0.2f);
                break;
            case Global.Misc.CannonAttackType.Laser:
                attackCoolDown = MathF.Max(attackCoolDown, 2f);
                laserDuration = MathF.Max(laserDuration, 0.5f);
                break;
            case Global.Misc.CannonAttackType.Energy:
                attackCoolDown = MathF.Max(attackCoolDown, 2f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void InitSelf(int id)
    {
        cannonId = id;
    }

    public void TryStayAttack()
    {
        var randomDelay = Random.Range(0f, 0.1f);
        Global.DoTweenWait(attackCoolDown * randomDelay, () =>
        {
            if (inCoolDown) return;
            if (attacking) return;
            if (!active) return;
            switch (attackType)
            {
                case Global.Misc.CannonAttackType.Bullet:
                    StayAttackBullet();
                    break;
                case Global.Misc.CannonAttackType.Laser:
                    StayAttackLaser();
                    break;
                case Global.Misc.CannonAttackType.Energy:
                    StayAttackEnergy();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
        
        // if (inCoolDown) return;
        // if (attacking) return;
        // if (!active) return;
        // switch (attackType)
        // {
        //     case Global.Misc.CannonAttackType.Bullet:
        //         StayAttackBullet();
        //         break;
        //     case Global.Misc.CannonAttackType.Laser:
        //         StayAttackLaser();
        //         break;
        //     case Global.Misc.CannonAttackType.Energy:
        //         StayAttackEnergy();
        //         break;
        //     default:
        //         throw new ArgumentOutOfRangeException();
        // }
    }

    public void StayAttackBullet()
    {
        var enemyTarget = Global.Battle.enemyManager.GetTarget(Global.Misc.CannonMode.Stay);
        if (!enemyTarget)
        {
            return;
        }
        attacking = true;
        var attackPoint = Global.Battle.attackPointManager.GetAttackPoint(enemyTarget.transform);
        var animationTime = (attackCoolDown >= 0.5f) ? 0.4f : 0.1f;
        AttachToAttackPoint(attackPoint, enemyTarget, animationTime);
        Global.DoTweenWait(animationTime, () => { ShootBullet(); });
        leaveTween?.Kill();
        leaveTween = Global.DoTweenWait(1f, () =>
        {
            LeaveAttackPoint(attackPoint);
        });
        leaveTween.OnKill(() =>
        {
            attackPoint.RemoveWeapon();
        });
        Global.DoTweenWait(attackCoolDown, () =>
        {
            //leaveTween?.Kill();
            inCoolDown = false;
            TryStayAttack();
        });
    }

    public void StayAttackLaser()
    {
        var enemyTarget = Global.Battle.enemyManager.GetTarget(Global.Misc.CannonMode.Stay);
        if (!enemyTarget) return;
        attacking = true;
        var attackPoint = Global.Battle.attackPointManager.GetAttackPoint(enemyTarget.transform);
        AttachToAttackPoint(attackPoint, enemyTarget);
        Global.DoTweenWait(0.4f, () => { ShootLaser(); });
        Global.DoTweenWait(0.8f + laserDuration, () => { LeaveAttackPoint(attackPoint); });
        Global.DoTweenWait(attackCoolDown + laserDuration, () =>
        {
            inCoolDown = false;
            TryStayAttack();
        });
    }
    
    public void StayAttackEnergy()
    {
        var enemyTarget = Global.Battle.enemyManager.GetTarget(Global.Misc.CannonMode.Stay);
        if (!enemyTarget) return;
        attacking = true;
        var attackPoint = Global.Battle.attackPointManager.GetAttackPoint(enemyTarget.transform);
        AttachToAttackPoint(attackPoint, enemyTarget);
        Global.DoTweenWait(0.4f, () => { ShootEnergy(); });
        Global.DoTweenWait(1f, () => { LeaveAttackPoint(attackPoint); });
        Global.DoTweenWait(attackCoolDown, () =>
        {
            inCoolDown = false;
            TryStayAttack();
        });
    }

    public void AttachToAttackPoint(AttackPoint attackPoint, Enemy enemyTarget, float animationTime = 0.4f)
    {
        cannonAnimation.OutFormation(attackPoint, animationTime);
        //var positionDifference = attackPoint.transform.position - transform.position;
        //cannonAnimation.transform.DOLocalMove(transform.localPosition + positionDifference, 0.2f);
        attackPoint.AttachWeapon(enemyTarget);
    }

    public void LeaveAttackPoint(AttackPoint attackPoint)
    {
        attackPoint.RemoveWeapon();
        cannonAnimation.IntoFormation();
    }

    public void ShootBullet()
    {
        var gunKickTime = attackCoolDown < 0.5f ? attackCoolDown / 2f : 0.3f;
        cannonLocalMotionManager.DoGunKick(gunKickTime);
        if (bulletShootMmfPlayer!= null) bulletShootMmfPlayer.PlayFeedbacks();
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.BulletShoot);
        var dischargeObject = GetAttackPrefabBullet().Get();
        dischargeObject.damage = attack;
        dischargeObject.elementType = elementType;
        var dischargeObjectTransform = dischargeObject.transform;
        dischargeObjectTransform.position = dischargePoint.position;
        dischargeObjectTransform.rotation = dischargePoint.rotation;
        inCoolDown = true;
        attacking = false;
        TriggerFireEvent();
    }
    
    public void ShootLaser()
    {
        var dischargePool = GetAttackPrefabLaser();
        var dischargeObject = dischargePool.Get();
        dischargeObject.damage = attack;
        dischargeObject.updateSaver = false;
        var dischargeObjectTransform = dischargeObject.transform;
        dischargeObjectTransform.position = dischargePoint.position;
        dischargeObjectTransform.rotation = dischargePoint.rotation;
        dischargeObjectTransform.parent = transform;
        TriggerFireEvent();
        attacking = false;
        inCoolDown = true;

        cannonAnimation.cannonFlexalonLerpAnimator.AnimatePosition = false;
        cannonAnimation.cannonFlexalonLerpAnimator.InterpolationSpeed = 20f;

        Global.DoTweenWait(laserDuration, () =>
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.LaserDischarge);
            cannonLocalMotionManager.DoGunKick(0.3f);
            if (laserShootMmfPlayer!= null) laserShootMmfPlayer.PlayFeedbacks();
            dischargeObject.Discharge();
            cannonAnimation.cannonFlexalonLerpAnimator.AnimatePosition = true;
            cannonAnimation.cannonFlexalonLerpAnimator.InterpolationSpeed = 5f;
            Global.DoTweenWait(1f, () =>
            {
                dischargePool.Release(dischargeObject);
            });
        });
    }

    public void ShootEnergy()
    {
        var dischargeObject = GetAttackPrefabEnergy().Get();
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect,GameAudios.AudioName.EnergyShoot);
        cannonLocalMotionManager.DoGunKick(0.3f);
        energyShootMmfPlayer.PlayFeedbacks();
        dischargeObject.damage = attack;
        dischargeObject.range = energyRange;
        dischargeObject.elementType = elementType;
        var dischargeObjectTransform = dischargeObject.transform;
        dischargeObjectTransform.position = dischargePoint.position;
        dischargeObjectTransform.rotation = dischargePoint.rotation;
        TriggerFireEvent();
        attacking = false;
        inCoolDown = true;
    }

    public ObjectPool<Bullet> GetAttackPrefabBullet()
    {
        return elementType switch
        {
            Global.Misc.ElementType.Fire => Global.Battle.battlePrefabManager.bulletFirePool,
            Global.Misc.ElementType.Water => Global.Battle.battlePrefabManager.bulletWaterPool,
            Global.Misc.ElementType.Nature => Global.Battle.battlePrefabManager.bulletNaturePool,
            Global.Misc.ElementType.Light => Global.Battle.battlePrefabManager.bulletLightPool,
            Global.Misc.ElementType.Dark => Global.Battle.battlePrefabManager.bulletDarkPool,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public ObjectPool<Laser> GetAttackPrefabLaser()
    {
        return elementType switch
        {
            Global.Misc.ElementType.Fire => Global.Battle.battlePrefabManager.laserFirePool,
            Global.Misc.ElementType.Water => Global.Battle.battlePrefabManager.laserWaterPool,
            Global.Misc.ElementType.Nature => Global.Battle.battlePrefabManager.laserNaturePool,
            Global.Misc.ElementType.Light => Global.Battle.battlePrefabManager.laserLightPool,
            Global.Misc.ElementType.Dark => Global.Battle.battlePrefabManager.laserDarkPool,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public ObjectPool<Energy> GetAttackPrefabEnergy()
    {
        return elementType switch
        {
            Global.Misc.ElementType.Fire => Global.Battle.battlePrefabManager.energyFirePool,
            Global.Misc.ElementType.Water => Global.Battle.battlePrefabManager.energyWaterPool,
            Global.Misc.ElementType.Nature => Global.Battle.battlePrefabManager.energyNaturePool,
            Global.Misc.ElementType.Light => Global.Battle.battlePrefabManager.energyLightPool,
            Global.Misc.ElementType.Dark => Global.Battle.battlePrefabManager.energyDarkPool,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public void TriggerFireEvent()
    {
        switch (cannonId)
        {
            case 0:
                EventCenter.GetInstance().EventTrigger(Global.Events.CannonZeroFire);
                break;
            case 1:
                EventCenter.GetInstance().EventTrigger(Global.Events.CannonOneFire);
                break;
            case 2:
                EventCenter.GetInstance().EventTrigger(Global.Events.CannonTwoFire);
                break;
            case 3:
                EventCenter.GetInstance().EventTrigger(Global.Events.CannonThreeFire);
                break;
            case 4:
                EventCenter.GetInstance().EventTrigger(Global.Events.CannonFourFire);
                break;
            case 5:
                EventCenter.GetInstance().EventTrigger(Global.Events.CannonFiveFire);
                break;
        }
    }
}