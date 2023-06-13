using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public Boss boss;

    private void Awake()
    {
        boss.DisableComponents();
        boss.gameObject.SetActive(false);
        EventCenter.GetInstance().AddEventListener(Global.Events.BossAppear, BossAppear);
    }

    public void BossAppear()
    {
        Global.DoTweenWait(3f, () =>
        {
            boss.gameObject.SetActive(true);
            boss.Appear();
            boss.SetPosition(Global.Battle.bossLocations.GetBossLocation());
        });
    }
}
