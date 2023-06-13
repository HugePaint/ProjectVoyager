using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPointManager : MonoBehaviour
{
    public AttackPoint[] attackPoints;

    private void Awake()
    {
        Global.Battle.attackPointManager = this;
        attackPoints = GetComponentsInChildren<AttackPoint>();
    }

    public AttackPoint GetAttackPoint(Transform target)
    {
        var shortestDistance = 99999f;
        var returnAttackPoint = attackPoints[0];
        foreach (var ap in attackPoints)
        {
            var distance = ap.DistanceToPoint(target);
            if (distance < shortestDistance)
            {
                returnAttackPoint = ap;
                shortestDistance = distance;
            }
        }

        return returnAttackPoint;
    }
}