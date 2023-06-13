using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleCollisionEnergy : MonoBehaviour
{
    public Global.Misc.ElementType elementType;
    private List<ObjectPool<EnergyHit>> effectsOnCollision;
    public float destroyTimeDelay = 1;
    public bool useWorldSpacePosition = true;
    public float offset = 0;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool useOnlyRotationOffset = true;
    public bool useFirePointRotation;
    public bool destroyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    public Energy energy;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        effectsOnCollision = new List<ObjectPool<EnergyHit>>();
    }

    private void GetHitEffectList()
    {
        effectsOnCollision = new List<ObjectPool<EnergyHit>>();
        switch (elementType)
        {
            case Global.Misc.ElementType.Fire:
                effectsOnCollision.Add(Global.Battle.battlePrefabManager.energyFireHitPool);
                break;
            case Global.Misc.ElementType.Water:
                effectsOnCollision.Add(Global.Battle.battlePrefabManager.energyWaterHitPool);
                break;
            case Global.Misc.ElementType.Nature:
                effectsOnCollision.Add(Global.Battle.battlePrefabManager.energyNatureHitPool);
                break;
            case Global.Misc.ElementType.Light:
                effectsOnCollision.Add(Global.Battle.battlePrefabManager.energyLightHitPool);
                break;
            case Global.Misc.ElementType.Dark:
                effectsOnCollision.Add(Global.Battle.battlePrefabManager.energyDarkHitPool);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private ObjectPool<Energy> GetEnergyPool()
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

    void OnParticleCollision(GameObject other)
    {
        var numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        GetHitEffectList();
        for (var i = 0; i < numCollisionEvents; i++)
        {
            foreach (var effectPool in effectsOnCollision)
            {
                var instance = effectPool.Get();
                instance.TryHit(energy.elementType, energy.damage, energy.range);
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
            Global.DoTweenWait(0.5f, () => { GetEnergyPool().Release(energy); });
        }
    }
}
