using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class VoidCenterPickUpManager : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPickUpPrefab;
    [SerializeField] private GameObject chaosBallPickUpPrefab;
    public ObjectPool<GameObject> powerUpPickUpPool;
    public ObjectPool<GameObject> chaosBallPickUpPool;

    public List<VoidCenterPickUp> voidCenterPickUps;

    public enum PickUpTypes
    {
        ChaosBall,
        PowerUp
    }


    private void Awake()
    {
        Global.Battle.voidCenterPickUpManager = this;
        voidCenterPickUps = new List<VoidCenterPickUp>(GetComponentsInChildren<VoidCenterPickUp>());
        BuildPickUpPool();
    }

    private void BuildPickUpPool()
    {
        powerUpPickUpPool = new ObjectPool<GameObject>(() => Instantiate(powerUpPickUpPrefab),
            powerUp => { powerUp.SetActive(true); }, powerUp => { powerUp.SetActive(false); },
            powerUp => { Destroy(powerUp.gameObject); }, false, 16, 32);

        chaosBallPickUpPool = new ObjectPool<GameObject>(() => Instantiate(chaosBallPickUpPrefab),
            chaosBall => { chaosBall.SetActive(true); }, chaosBall => { chaosBall.SetActive(false); },
            chaosBall => { Destroy(chaosBall.gameObject); }, false, 16, 100);
    }

    public VoidCenterPickUp GetPickUpParent()
    {
        if (voidCenterPickUps.Count == 0) return null;
        var returnParent = Global.GetRandomFromList(voidCenterPickUps);
        voidCenterPickUps.Remove(returnParent);
        return returnParent;
    }

    public void GeneratePickUp(PickUpTypes pickUpType)
    {
        switch (pickUpType)
        {
            case PickUpTypes.ChaosBall:
                GenerateChaosBall();
                break;
            case PickUpTypes.PowerUp:
                GeneratePowerUp();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(pickUpType), pickUpType, null);
        }
    }

    private void GenerateChaosBall()
    {

        var chaosBallGenerate = chaosBallPickUpPool.Get();
        var randomCirclePosition = Random.insideUnitCircle * Global.Battle.battleData.enemySpawnRule.arenaRadius;
        var randomPosition = new Vector3(randomCirclePosition.x, 0f, randomCirclePosition.y);
        while (Vector3.Distance(randomPosition, Global.Battle.playerBehaviourController.transform.position) < Global.Battle.battleData.enemySpawnRule.chaosBallSpawnSafeRadiusAroundPlayer 
               || Vector3.Distance(randomPosition, Global.Battle.voidCenter.transform.position) < (Global.Battle.battleData.enemySpawnRule.safeRadiusFromCenter + 2f))
        {
            randomCirclePosition = Random.insideUnitCircle * Global.Battle.battleData.enemySpawnRule.arenaRadius;
            randomPosition = new Vector3(randomCirclePosition.x, 0f, randomCirclePosition.y);
        }

        chaosBallGenerate.transform.position = randomPosition;
    }

    public void GeneratePowerUp(VoidCenterPickUp placeHolder = null)
    {
        if (!placeHolder) placeHolder = GetPickUpParent();
        if (placeHolder == null) return;
        var powerUpGenerate = powerUpPickUpPool.Get();
        placeHolder.Active(PickUpTypes.PowerUp, powerUpGenerate);
        powerUpGenerate.transform.parent = placeHolder.transform;
        powerUpGenerate.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}