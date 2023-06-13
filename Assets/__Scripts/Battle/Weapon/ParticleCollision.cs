using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;

public class ParticleCollision : MonoBehaviour
{
    public Global.Misc.CannonAttackType cannonAttackType;
    public Global.Misc.ElementType elementType;
    private List<ObjectPool<GameObject>> effectsOnCollision;
    public float destroyTimeDelay = 1;
    public bool useWorldSpacePosition = true;
    public float offset = 0;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool useOnlyRotationOffset = true;
    public bool useFirePointRotation;
    public bool destroyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    public Bullet bullet;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        effectsOnCollision = new List<ObjectPool<GameObject>>();
    }

    private void OnEnable()
    {
        bullet.sourceEnemy = null;
    }

    private void GetHitEffectList()
    {
        effectsOnCollision = new List<ObjectPool<GameObject>>();
        switch (cannonAttackType)
        {
            case Global.Misc.CannonAttackType.Bullet:
                switch (elementType)
                {
                    case Global.Misc.ElementType.Fire:
                        effectsOnCollision.Add(Global.Battle.battlePrefabManager.bulletFireHitPool);
                        break;
                    case Global.Misc.ElementType.Water:
                        effectsOnCollision.Add(Global.Battle.battlePrefabManager.bulletWaterHitPool);
                        break;
                    case Global.Misc.ElementType.Nature:
                        effectsOnCollision.Add(Global.Battle.battlePrefabManager.bulletNatureHitPool);
                        break;
                    case Global.Misc.ElementType.Light:
                        effectsOnCollision.Add(Global.Battle.battlePrefabManager.bulletLightHitPool);
                        break;
                    case Global.Misc.ElementType.Dark:
                        effectsOnCollision.Add(Global.Battle.battlePrefabManager.bulletDarkHitPool);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;
            case Global.Misc.CannonAttackType.Laser:
                break;
            case Global.Misc.CannonAttackType.Energy:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private ObjectPool<Bullet> GetBulletPool()
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

    void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("Enemy"))
        {
            GetBulletPool().Release(bullet);
            return;
        }
        var enemy = other.GetComponent<Enemy>();
        if (bullet.sourceEnemy != null && enemy == bullet.sourceEnemy) return;
        enemy.TakeDamage(bullet.elementType, bullet.damage);
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.BulletHit);
        var numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        GetHitEffectList();
        for (var i = 0; i < numCollisionEvents; i++)
        {
            foreach (var effectPool in effectsOnCollision)
            {
                var instance = effectPool.Get();
                instance.transform.position = collisionEvents[i].intersection + collisionEvents[i].normal * offset;
                if (!useWorldSpacePosition) instance.transform.parent = transform;
                if (useFirePointRotation)
                {
                    instance.transform.LookAt(transform.position);
                }
                else if (rotationOffset != Vector3.zero && useOnlyRotationOffset)
                {
                    instance.transform.rotation = Quaternion.Euler(rotationOffset);
                }
                else
                {
                    instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                    instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                }

                Global.DoTweenWait(destroyTimeDelay, () => { effectPool.Release(instance); });
            }
        }

        if (destroyMainEffect)
        {
            Global.DoTweenWait(0.5f, () => { GetBulletPool().Release(bullet); });
        }
    }
}