using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    #region properties

    [SerializeField] private float health;
    [SerializeField] private float defense;
    [SerializeField] private float attack;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float animationMoveSpeed;
    [SerializeField] private Global.Misc.EnemyType enemyType;
    [SerializeField] private Global.Misc.ElementType mainElementAttached;
    [SerializeField] private Global.Misc.ElementType secondElementAttached;

    [SerializeField] private int cannonLocked;

    public float Health
    {
        get => health;
        set
        {
            health = value;
            HealthChangeEvent();
        }
    }

    public float Defense
    {
        get => defense;
        set
        {
            defense = value;
            DefenseChangeEvent();
        }
    }

    public float Attack
    {
        get => attack;
        set
        {
            attack = value;
            AttackChangeEvent();
        }
    }

    public float AttackCoolDown
    {
        get => attackCoolDown;
        set
        {
            attackCoolDown = value;
            AttackCoolDownChangeEvent();
        }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        set
        {
            moveSpeed = Mathf.Max(value, Global.Battle.battleData.enemySpiderBasicStats.enemySpiderMinMoveSpeed);
            MoveSpeedChangeEvent();
        }
    }

    public float AnimationMoveSpeed
    {
        get => animationMoveSpeed;
        set => animationMoveSpeed = value;
    }

    public Global.Misc.EnemyType EnemyType
    {
        get => enemyType;
        set
        {
            enemyType = value;
            EnemyTypeChangeEvent();
        }
    }

    public int CannonLocked
    {
        get => cannonLocked;
        set
        {
            cannonLocked = value;
            CannonLockedChangeEvent();
        }
    }

    public Global.Misc.ElementType MainElementAttached
    {
        get => mainElementAttached;
        set
        {
            mainElementAttached = value;
        }
    }

    public Global.Misc.ElementType SecondElementAttached
    {
        get => secondElementAttached;
        set
        {
            secondElementAttached = value;
        }
    }

    #endregion

    public List<Buff> buffOnEnemy;
    public MMF_Player normalDamageText;
    public MMF_Player mediumDamageText;
    public MMF_Player largeDamageText;
    public MMF_Player burnDamageText;
    public MMF_Player burnBuffText;
    public MMF_Player waterSlowBuffText;
    public MMF_Player bounceText;
    public MMF_Player speedUpBuffText;
    public EnemyUIManager enemyUIManager;
    public EnemyAuraManager enemyAuraManager;
    public AttackPoint beingLookedAtBy;

    private float nextHitMultiplier;

    private void Awake()
    {
        EventCenter.GetInstance().AddEventListener(Global.Events.KillAllEnemies, DieInstant);
    }

    private void OnEnable()
    {
        buffOnEnemy = new List<Buff>();
        mainElementAttached = Global.Misc.ElementType.None;
        secondElementAttached = Global.Misc.ElementType.None;
        nextHitMultiplier = 1f;
        enemyAuraManager.DisableAllAura();
        beingLookedAtBy = null;
    }

    private void OnDisable()
    {
        
    }

    public void OnSpawnAction()
    {
        InitSelf();
        OnSpawnMoreAction();
    }

    public float GetSelfPriority(float distance, Global.Misc.CannonMode cannonMode)
    {
        var weight = cannonMode switch
        {
            Global.Misc.CannonMode.Stay => Global.Battle.battleData.weightForStayCannon,
            Global.Misc.CannonMode.Search => Global.Battle.battleData.weightForSearchCannon,
            _ => throw new ArgumentOutOfRangeException(nameof(cannonMode), cannonMode, null)
        };

        var priority = distance * weight.distance + cannonLocked + weight.cannonLocked;

        priority += EnemyType switch
        {
            Global.Misc.EnemyType.CreepMelee => weight.enemyTypeCreepMelee,
            Global.Misc.EnemyType.CreepRange => weight.enemyTypeCreepRange,
            Global.Misc.EnemyType.Elite => weight.enemyTypeElite,
            Global.Misc.EnemyType.Boss => weight.enemyTypeBoss,
            _ => throw new ArgumentOutOfRangeException()
        };
        return priority;
    }

    public void TakeDamage(Global.Misc.ElementType elementType, float damage, BattleDataManager.DamageTextType damageTextType = BattleDataManager.DamageTextType.Normal)
    {
        if (Health <= 0) return;
        if (elementType != Global.Misc.ElementType.None) AttachElement(elementType);
        var norDamageTextDrop = normalDamageText;
        if (nextHitMultiplier >= 1.1f)
        {
            norDamageTextDrop = mediumDamageText;
        }

        if (nextHitMultiplier >= 1.5f)
        {
            norDamageTextDrop = largeDamageText;
        }

        var realDamage = damage * nextHitMultiplier;
        if (enemyType == Global.Misc.EnemyType.Boss)
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.BossGetHit, realDamage);
        }
        else
        {
            Health -= realDamage;
        }
        nextHitMultiplier = 1f;
        switch (damageTextType)
        {
            case BattleDataManager.DamageTextType.Normal:
                if (norDamageTextDrop != null)
                {
                    norDamageTextDrop.PlayFeedbacks(transform.position, realDamage);
                }
                break;
            case BattleDataManager.DamageTextType.Burn:
                if (burnDamageText != null)
                {
                    burnDamageText.PlayFeedbacks(transform.position, realDamage);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(damageTextType), damageTextType, null);
        }
        OnHitMoreAction();
        if (enemyType != Global.Misc.EnemyType.Boss) CheckDeath();
    }

    public void AttachElement(Global.Misc.ElementType elementType)
    {
        //temp 
        if (elementType == Global.Misc.ElementType.Dark || elementType == Global.Misc.ElementType.Light) return;
        //temp
        if (mainElementAttached == Global.Misc.ElementType.None && secondElementAttached == Global.Misc.ElementType.None)
        {
            if (enemyUIManager) enemyUIManager.elementSymbol.ElementImageAppear(elementType);
            switch (elementType)
            {
                case Global.Misc.ElementType.Fire:
                    FireReaction();
                    mainElementAttached = elementType;
                    break;
                case Global.Misc.ElementType.Water:
                    WaterReaction();
                    mainElementAttached = elementType;
                    break;
                case Global.Misc.ElementType.Nature:
                    NatureReaction();
                    mainElementAttached = elementType;
                    break;
                case Global.Misc.ElementType.Light:
                    secondElementAttached = elementType;
                    break;
                case Global.Misc.ElementType.Dark:
                    secondElementAttached = elementType;
                    break;
                case Global.Misc.ElementType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
            }
        }
        else
        {
            switch (elementType)
            {
                case Global.Misc.ElementType.Fire:
                    FireReaction();
                    break;
                case Global.Misc.ElementType.Water:
                    WaterReaction();
                    break;
                case Global.Misc.ElementType.Nature:
                    NatureReaction();
                    break;
                case Global.Misc.ElementType.Light:
                    break;
                case Global.Misc.ElementType.Dark:
                    break;
                case Global.Misc.ElementType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(elementType), elementType, null);
            }
        }
    }

    public void FireReaction()
    {
        switch (mainElementAttached)
        {
            case Global.Misc.ElementType.Fire:
                Global.Battle.buffManager.Burn(this);
                break;
            case Global.Misc.ElementType.Water:
                OverrideElement(Global.Misc.ElementType.Fire);
                nextHitMultiplier = Global.Battle.battleData.fireElementReactionData.overrideWaterDamageMultiplier;
                break;
            case Global.Misc.ElementType.Nature:
                OverrideElement(Global.Misc.ElementType.Fire);
                nextHitMultiplier = Global.Battle.battleData.fireElementReactionData.overrideNatureDamageMultiplier;
                break;
            case Global.Misc.ElementType.None:
                Global.Battle.buffManager.Burn(this);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void WaterReaction()
    {
        switch (mainElementAttached)
        {
            case Global.Misc.ElementType.Fire:
                OverrideElement(Global.Misc.ElementType.Water);
                nextHitMultiplier = Global.Battle.battleData.waterElementReactionData.overrideFireDamageMultiplier;
                break;
            case Global.Misc.ElementType.Water:
                Global.Battle.buffManager.WaterSlow(this);
                break;
            case Global.Misc.ElementType.Nature:
                OverrideElement(Global.Misc.ElementType.Water);
                nextHitMultiplier = Global.Battle.battleData.waterElementReactionData.overrideNatureDamageMultiplier;
                break;
            case Global.Misc.ElementType.None:
                Global.Battle.buffManager.WaterSlow(this);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void NatureReaction()
    {
        switch (mainElementAttached)
        {
            case Global.Misc.ElementType.Fire:
                OverrideElement(Global.Misc.ElementType.Nature);
                nextHitMultiplier = Global.Battle.battleData.natureElementReactionData.overrideFireDamageMultiplier;
                break;
            case Global.Misc.ElementType.Water:
                OverrideElement(Global.Misc.ElementType.Nature);
                nextHitMultiplier = Global.Battle.battleData.natureElementReactionData.overrideWaterDamageMultiplier;
                break;
            case Global.Misc.ElementType.Nature:
                NatureBounceReaction();
                break;
            case Global.Misc.ElementType.None:
                NatureBounceReaction();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void NatureBounceReaction()
    {
        if (Global.Battle.battleData.natureElementReactionData.bounceDamage <= 0) return;
        if (bounceText != null)
        {
            bounceText.PlayFeedbacks(transform.position, 0);
        }

        var target = Global.Battle.enemyManager.GetEnemyInRange(this, Global.Battle.battleData.natureElementReactionData.bounceRange);
        if (target == null) return;
        var dischargeObject = Global.Battle.battlePrefabManager.bulletNaturePool.Get();
        dischargeObject.damage = Global.Battle.battleData.natureElementReactionData.bounceDamage;
        dischargeObject.elementType = Global.Misc.ElementType.None;
        dischargeObject.sourceEnemy = this;
        var dischargeObjectTransform = dischargeObject.transform;
        var offset = (target.transform.position - transform.position).normalized;
        dischargeObjectTransform.position = transform.position + offset;
        dischargeObjectTransform.LookAt(target.transform);
    }

    public void OverrideElement(Global.Misc.ElementType newElement)
    {
        mainElementAttached = newElement;
        if (enemyUIManager) enemyUIManager.elementSymbol.ElementImageAppear(newElement);
    }
    
    

    public void CheckDeath()
    {
        if (Health > 0) return;
        Die();
    }

    public void Die()
    {
        foreach (var buff in buffOnEnemy)
        {
            buff.ForceRemoveBuff(this);
        }
        buffOnEnemy = new List<Buff>();
        enemyAuraManager.DisableAllAura();

        if (beingLookedAtBy != null)
        {
            beingLookedAtBy.ChangeLookAtTarget(Global.Battle.enemyManager.GetEnemyInRange(this, 100f));
        }
        
        DieMoreAction();
    }
    

    protected abstract void InitSelf();
    protected abstract void HealthChangeEvent();
    protected abstract void DefenseChangeEvent();
    protected abstract void AttackChangeEvent();
    protected abstract void AttackCoolDownChangeEvent();
    protected abstract void MoveSpeedChangeEvent();
    protected abstract void EnemyTypeChangeEvent();
    protected abstract void CannonLockedChangeEvent();
    protected abstract void OnSpawnMoreAction();
    protected abstract void OnHitMoreAction();
    protected abstract void DieMoreAction();
    public abstract void DieInstant();
}