using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAP_ParticleSystemController;
using UnityEngine;

public class EnergyHit : MonoBehaviour
{
    public List<Transform> allTransforms;
    public float damage;
    public Global.Misc.ElementType elementType;

    public Collider collider;
    private List<Collider> damagedCollider;

    private void OnEnable()
    {
        Global.DoTweenWait(0.5f, () =>
        {
            collider.enabled = false;
        });
    }

    public void TryHit(Global.Misc.ElementType elementTypeIncoming, float damageIncoming, float range)
    {
        Global.Battle.globalEffectManager.PlayerEnergyHitEffect();
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect,GameAudios.AudioName.EnergyHit);
        foreach (var transform in allTransforms)
        {
            transform.DOScale(new Vector3(1f, 1f, 1f) * range, 0f);
        }
        damage = damageIncoming;
        elementType = elementTypeIncoming;
        damagedCollider = new List<Collider>();
        collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        if (damagedCollider.Contains(other)) return;
        damagedCollider.Add(other);
        other.GetComponent<Enemy>().TakeDamage(elementType, damage);
    }
}
