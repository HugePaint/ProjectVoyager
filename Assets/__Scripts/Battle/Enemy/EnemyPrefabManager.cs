using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyPrefabManager : MonoBehaviour
{
    public EnemySpider enemySpiderPrefab;
    public Transform spiderParentHolder;
    public ObjectPool<EnemySpider> enemySpiderPool;

    private void Awake()
    {
        Global.Battle.enemyPrefabManager = this;
    }

    private void Start()
    {
        BuildEnemyPool();
    }

    public void BuildEnemyPool()
    {
        enemySpiderPool = new ObjectPool<EnemySpider>(
            () => { return Instantiate(enemySpiderPrefab, spiderParentHolder); },
            enemySpider => { enemySpider.gameObject.SetActive(true); }, enemySpider => { enemySpider.gameObject.SetActive(false); },
            enemySpider => { Destroy(enemySpider.gameObject); }, false, 100, 500);
    }
}
