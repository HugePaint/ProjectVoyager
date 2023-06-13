using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossLocations : MonoBehaviour
{
    public List<Transform> locations;
    private int currentPositionIndex;
    private void Awake()
    {
        Global.Battle.bossLocations = this;
        currentPositionIndex = -1;
    }

    public Transform GetBossLocation()
    {
        if (Global.Battle.battleData.isTesting) return locations[0];
        if (currentPositionIndex == -1)
        {
            var randomIndex = Random.Range(0, locations.Count);
            currentPositionIndex = randomIndex;
            return locations[randomIndex];
        }
        else
        {
            var randomIndex = Random.Range(0, locations.Count);
            while (randomIndex == currentPositionIndex)
            {
                randomIndex = Random.Range(0, locations.Count);
            }
            currentPositionIndex = randomIndex;
            return locations[randomIndex];
        }
    }
    
}
