using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitController : MonoBehaviour
{
    public Collider playerGetHitCollider;
    private void Awake()
    {
        Global.Battle.playerGetHitController = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy")) return;
        var enemy = other.GetComponent<Enemy>();
        enemy.DieInstant();
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayerGetHit, enemy.Attack);
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.GetHit);
    }
}
