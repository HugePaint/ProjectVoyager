using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMeshManager : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        NavMesh.avoidancePredictionTime = 0.5f;
    }

    private void OnEnable()
    {
        navMeshAgent.enabled = true;
    }

    private void OnDisable()
    {
        navMeshAgent.enabled = false;
    }

    private void Update()
    {
        navMeshAgent.SetDestination(Global.Battle.playerBehaviourController.transform.position);
    }

    public void UpdateMoveSpeed(float newSpeed)
    {
        navMeshAgent.speed = newSpeed;
    }
}
