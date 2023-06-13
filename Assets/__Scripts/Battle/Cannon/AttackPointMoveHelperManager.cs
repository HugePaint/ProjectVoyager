using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Flexalon;
using UnityEngine;

public class AttackPointMoveHelperManager : MonoBehaviour
{
    private AttackPointMoveHelper[] attackPointMoveHelpersTemp;
    public List<AttackPointMoveHelper> attackPointMoveHelpers;
    public AttackPoint assignedAttackPoint;

    private void Awake()
    {
        attackPointMoveHelpersTemp = GetComponentsInChildren<AttackPointMoveHelper>();
        attackPointMoveHelpers = new List<AttackPointMoveHelper>(attackPointMoveHelpersTemp);
        assignedAttackPoint = null;
        Global.Battle.attackPointMoveHelperManager = this;
    }

    public void GetMoveHelper(AttackPoint attackPoint, CannonBattle cannonBattle, FlexalonConstraint flexalonConstraint,
        float duration)
    {
        var helper = Global.GetRandomFromList(attackPointMoveHelpers);
        attackPointMoveHelpers.Remove(helper);
        flexalonConstraint.Target = helper.gameObject;
        assignedAttackPoint = attackPoint;
        helper.transform.position = cannonBattle.transform.position;
        var positionDifference = attackPoint.transform.position - helper.transform.position;
        helper.transform.DOLocalMove(helper.transform.localPosition + positionDifference, duration).OnComplete(() =>
        {
            flexalonConstraint.Target = attackPoint.gameObject;
            attackPointMoveHelpers.Add(helper);
        });
    }

    private void Update()
    {
        if (assignedAttackPoint == null) return;
        transform.rotation = assignedAttackPoint.transform.rotation;
    }
}