using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserHitPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayerGetHit, Global.Battle.battleData.bossStats.bossLaserDamage);
    }
}
