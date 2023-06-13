using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChaosBallPickup : MonoBehaviour
{
    private SphereCollider sphereCollider;

    public Tween countDownTween;
    public float timer;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        sphereCollider.enabled = true;
        timer = 0;
        countDownTween?.Kill();
        countDownTween = DOTween.To(() => timer, x => timer = x, 600f, 600f).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        //analytics
        countDownTween?.Kill();
        if (MetricManagerScript.instance != null) MetricManagerScript.instance.LogString("Chaos ball item", "Collected: " + timer);
        
        sphereCollider.enabled = false;
        EventCenter.GetInstance().EventTrigger(Global.Events.UpdateChaos, Global.Battle.battleData.pickUpConfig.chaosBallIncreaseChaosAmount);
        Global.Battle.battleUIManager.enemyPowerUpHint.Show();

        var randomExp = Random.Range(6, 11);
        for (var i = 0; i < randomExp; i++)
        {
            Global.DoTweenWait(i*0.15f, () =>
            {
                var exp = Global.Battle.battlePrefabManager.expBallObjectPool.Get();
                var randomX = Global.RandomPositiveAndNegativeFromRage(1f, 2f);
                var randomY = Random.Range(1f, 2f);
                var randomZ = Global.RandomPositiveAndNegativeFromRage(1f, 2f);
                exp.transform.position = transform.position + new Vector3(randomX, randomY, randomZ);
                exp.Collect(Global.Battle.battleData.pickUpConfig.chaosBallExpValueEach);
            });
        }

        transform.DOShakePosition(1.5f, 0.5f, 30).OnComplete(() =>
        {
            Global.Battle.enemyManager.SpawnGroupEnemySpider(Global.Battle.battleData.pickUpConfig.chaosBallSpawnEnemyNumber,
                new Vector3(transform.position.x, 1f,transform.position.z ));
            Global.Battle.voidCenterPickUpManager.chaosBallPickUpPool.Release(gameObject);
        });
    }

    private void OnDestroy()
    {
        if (MetricManagerScript.instance != null) MetricManagerScript.instance.LogString("Chaos ball item", "Ignored: " + timer);
    }
}
