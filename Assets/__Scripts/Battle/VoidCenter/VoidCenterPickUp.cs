using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class VoidCenterPickUp : MonoBehaviour
{
    public VoidCenterPickUpManager.PickUpTypes pickUpType;
    public GameObject pickup;
    private SphereCollider sphereCollider;

    public Tween countDownTween;
    public float timer;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
    }

    public void Active(VoidCenterPickUpManager.PickUpTypes pickupTypeInput, GameObject pickupInput)
    {
        pickUpType = pickupTypeInput;
        pickup = pickupInput;
        sphereCollider.enabled = true;
        countDownTween?.Kill();
        countDownTween = DOTween.To(() => timer, x => timer = x, 600f, 600f).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        if (sphereCollider.enabled)
        {
            if (MetricManagerScript.instance != null) MetricManagerScript.instance.LogString("Power Up Item", "Ignored: " + timer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        switch (pickUpType)
        {
            case VoidCenterPickUpManager.PickUpTypes.ChaosBall:
                //ChaosBallTrigger();
                sphereCollider.enabled = false;
                break;
            case VoidCenterPickUpManager.PickUpTypes.PowerUp:
                PowerUpPlayer();
                sphereCollider.enabled = false;
                Global.Battle.voidCenterPickUpManager.powerUpPickUpPool.Release(pickup);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Global.Battle.voidCenterPickUpManager.voidCenterPickUps.Add(this);
    }

    private void PowerUpPlayer()
    {
        Global.Battle.powerUpManager.ShowPowerUpUI();
        

        //analytics
        countDownTween?.Kill();
        if (MetricManagerScript.instance != null) MetricManagerScript.instance.LogString("Power Up Item", "Collected: " + timer);
    }

    // private void ChaosBallTrigger()
    // {
    //     EventCenter.GetInstance().EventTrigger(Global.Events.UpdateChaos, Global.Battle.battleData.pickUpConfig.chaosBallIncreaseChaosAmount);
    //     var item = pickup;
    //     item.transform.parent = null;
    //     var spawnPosition = Global.Battle.enemyManager.FindGeneralSpawnLocation(10f, 20f);
    //     var distance = Vector3.Distance(spawnPosition, item.transform.position);
    //     item.transform.DOMove(spawnPosition, 0.1f * distance).OnComplete(() =>
    //     {
    //         Global.Battle.enemyManager.SpawnGroupEnemySpider(Global.Battle.battleData.pickUpConfig.chaosBallSpawnEnemyNumber, spawnPosition);
    //         Global.Battle.voidCenterPickUpManager.chaosBallPickUpPool.Release(item);
    //     });
    // }
}