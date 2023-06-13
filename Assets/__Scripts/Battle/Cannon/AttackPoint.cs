using System;
using System.Collections;
using System.Collections.Generic;
using Flexalon;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    private bool hasAssignedWeapon;
    public Transform lookAtTarget;

    public float DistanceToPoint(Transform target)
    {
        return hasAssignedWeapon ? 99999f : Vector3.Distance(transform.position, target.position);
    }

    public void AttachWeapon(Enemy enemy)
    {
        hasAssignedWeapon = true;
        ChangeLookAtTarget(enemy);
    }

    public void ChangeLookAtTarget(Enemy enemy)
    {
        if (enemy == null) return;
        lookAtTarget = enemy.transform;
        enemy.beingLookedAtBy = this;
    }

    public void RemoveWeapon()
    {
        hasAssignedWeapon = false;
        lookAtTarget = null;
    }

    private void Update()
    {
        if (lookAtTarget == null) return;
        transform.LookAt(lookAtTarget);
    }

    public Vector3 GetRotationValue()
    {
        return transform.rotation.eulerAngles;
    }
}