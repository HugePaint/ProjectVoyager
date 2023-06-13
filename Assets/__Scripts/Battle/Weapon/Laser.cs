using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;

public class Laser : MonoBehaviour
{
    public Global.Misc.ElementType elementType;
    public float damage;

    public float hitOffset = 0;
    public bool useLaserRotation = false;

    public float maxLength;
    private LineRenderer laser;

    public float mainTextureLength = 1f;
    public float noiseTextureLength = 1f;
    private Vector4 length = new Vector4(1, 1, 1, 1);
    public bool updateSaver;

    private ParticleSystem[] effects;
    public List<LaserHitEffect> hitEffectsGenerated;
    public List<Enemy> enemiesHit;

    private bool discharging;


    private void Awake()
    {
        hitEffectsGenerated = new List<LaserHitEffect>();
        laser = GetComponent<LineRenderer>();
        effects = GetComponentsInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        enemiesHit = new List<Enemy>();
        discharging = false;
        laser.material.DOFade(0.1f, 0f);
        laser.widthMultiplier = 0.2f;
    }

    public void Discharge()
    {
        discharging = true;
        var width = 0.2f;
        DOTween.To(() => width, x => width = x, 0.6f, 0.2f).OnUpdate(() => { laser.widthMultiplier = width; });
        laser.material.DOFade(1f, 0.2f).OnComplete(() =>
        {
            foreach (var hitEffect in hitEffectsGenerated)
            {
                foreach (var ps in hitEffect.particleSystems)
                {
                    if (ps.isPlaying) ps.Stop();
                }
            }

            foreach (var ps in effects)
            {
                if (ps.isPlaying) ps.Stop();
            }

            laser.material.DOFade(0, 0.3f).OnComplete(() =>
            {
                DisablePrepare();
            });
        });
    }

    private void Update()
    {
        laser.material.SetTextureScale("_MainTex", new Vector2(length[0], length[1]));
        laser.material.SetTextureScale("_Noise", new Vector2(length[2], length[3]));
        if (laser != null && updateSaver == false)
        {
            laser.enabled = true;
            laser.SetPosition(0, transform.position);
            var endPos = transform.position + transform.forward * maxLength;
            laser.SetPosition(1, endPos);

            if (!discharging)
            {
                foreach (var ps in effects)
                {
                    if (ps.isPlaying) ps.Stop();
                }
                RaycastHit hitPoint;
                Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitPoint,
                    maxLength, LayerMask.GetMask("Enemy"));
                // if (hitEffectsGenerated.Count == 0)
                // {
                //     var hitEffectPool = GetHitEffectPool();
                //     var hitEffectGen = hitEffectPool.Get();
                //     hitEffectsGenerated.Add(hitEffectGen);
                // }
                // hitEffectsGenerated[0].transform.position = hitPoint.point + hitPoint.normal * hitOffset;
                // if (useLaserRotation)
                //     hitEffectsGenerated[0].transform.rotation = transform.rotation;
                // else
                //     hitEffectsGenerated[0].transform.LookAt(hitPoint.point + hitPoint.normal);
                // foreach (var ps in hitEffectsGenerated[0].particleSystems)
                // {
                //     if (!ps.isPlaying) ps.Play();
                // }
                length[0] = mainTextureLength * (Vector3.Distance(transform.position, endPos));
                length[2] = noiseTextureLength * (Vector3.Distance(transform.position, endPos));
            }
            else
            {
                foreach (var ps in effects)
                {
                    if (!ps.isPlaying) ps.Play();
                }
                var hitPoints = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward),
                    maxLength, LayerMask.GetMask("Enemy"));
                foreach (var hitPoint in hitPoints)
                {
                    DamageEnemy(hitPoint.collider.GetComponent<Enemy>());
                }
                updateSaver = true;
                // if (hitEffectsGenerated.Count < hitPoints.Length)
                // {
                //     for (var i = 0; i < hitPoints.Length - hitEffectsGenerated.Count + 1; i++)
                //     {
                //         var hitEffectPool = GetHitEffectPool();
                //         var hitEffectGen = hitEffectPool.Get();
                //         hitEffectsGenerated.Add(hitEffectGen);
                //     }
                // }
                for (var i = 0; i < hitPoints.Length; i++)
                {
                    var hitEffectPool = GetHitEffectPool();
                    var hitEffectGen = hitEffectPool.Get();
                    hitEffectsGenerated.Add(hitEffectGen);
                }
                for (var i = 0; i < hitPoints.Length; i++)
                {
                    hitEffectsGenerated[i].transform.position =
                        hitPoints[i].point + hitPoints[i].normal * hitOffset;
                    if (useLaserRotation)
                        hitEffectsGenerated[i].transform.rotation = transform.rotation;
                    else
                        hitEffectsGenerated[i].transform.LookAt(hitPoints[i].point + hitPoints[i].normal);
                    foreach (var ps in hitEffectsGenerated[i].particleSystems)
                    {
                        if (!ps.isPlaying) ps.Play();
                    }
                }

                for (var i = hitPoints.Length; i < hitEffectsGenerated.Count; i++)
                {
                    foreach (var ps in hitEffectsGenerated[i].particleSystems)
                    {
                        if (ps.isPlaying) ps.Stop();
                    }
                }

                length[0] = mainTextureLength * (Vector3.Distance(transform.position, endPos));
                length[2] = noiseTextureLength * (Vector3.Distance(transform.position, endPos));
            }
        }
    }

    public void DisablePrepare()
    {
        if (laser != null)
        {
            laser.enabled = false;
        }

        updateSaver = true;

        if (hitEffectsGenerated != null)
        {
            foreach (var ps in effects)
            {
                if (ps.isPlaying) ps.Stop();
            }

            foreach (var hitEff in hitEffectsGenerated)
            {
                GetHitEffectPool().Release(hitEff);
            }

            hitEffectsGenerated = new List<LaserHitEffect>();
        }
    }

    private ObjectPool<LaserHitEffect> GetHitEffectPool()
    {
        return elementType switch
        {
            Global.Misc.ElementType.Fire => Global.Battle.battlePrefabManager.laserFireHitPool,
            Global.Misc.ElementType.Water => Global.Battle.battlePrefabManager.laserWaterHitPool,
            Global.Misc.ElementType.Nature => Global.Battle.battlePrefabManager.laserNatureHitPool,
            Global.Misc.ElementType.Light => Global.Battle.battlePrefabManager.laserLightHitPool,
            Global.Misc.ElementType.Dark => Global.Battle.battlePrefabManager.laserDarkHitPool,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void DamageEnemy(Enemy enemy)
    {
        enemy.TakeDamage(elementType,damage);
    }
}