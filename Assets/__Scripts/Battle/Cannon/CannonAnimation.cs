using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Flexalon;
using UnityEngine;

public class CannonAnimation : MonoBehaviour
{
    public FlexalonObject cannonFlexalonObject;
    public FlexalonLerpAnimator cannonFlexalonLerpAnimator;
    public FlexalonConstraint flexalonConstraint;

    public float currentRotationAngle;
    public float currentOffsetY;

    private void Awake()
    {
        cannonFlexalonObject = GetComponent<FlexalonObject>();
        cannonFlexalonLerpAnimator = GetComponent<FlexalonLerpAnimator>();
        flexalonConstraint = GetComponent<FlexalonConstraint>();
        cannonFlexalonLerpAnimator.InterpolationSpeed = 5f;
        cannonFlexalonLerpAnimator.AnimatePosition = true;
        cannonFlexalonLerpAnimator.AnimateScale = true;
        cannonFlexalonLerpAnimator.AnimateRotation = true;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
    }

    public void ChangeRotationX(float angle)
    {
        currentRotationAngle = angle;
    }

    public void UpdateRotationX()
    {
        cannonFlexalonObject.Rotation = Quaternion.Euler(new Vector3(currentRotationAngle, 0f, 0f));
    }

    public void ClearRotation()
    {
        cannonFlexalonObject.Rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }


    public void ChangeOffsetY(float offset)
    {
        currentOffsetY = offset;
    }

    public void UpdateOffsetY()
    {
        cannonFlexalonObject.Offset = new Vector3(0f, currentOffsetY, 0f);
    }

    public void ClearOffset()
    {
        cannonFlexalonObject.Offset = new Vector3(0f, 0f, 0f);
    }

    public void OutFormation(AttackPoint attackPointStay, float animationTime)
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.CannonPrepare);
        transform.parent = Global.Battle.playerBehaviourController.transform;
        Global.Battle.cannonAnimationManager.cannonsInFormation.Remove(this);
        flexalonConstraint.enabled = true;
        //cannonFlexalonLerpAnimator.AnimatePosition = false;
        cannonFlexalonLerpAnimator.InterpolationSpeed = 8f;
        Global.DoTweenWait(animationTime/2f, () =>
        {
            cannonFlexalonLerpAnimator.InterpolationSpeed = 100f;
        });
        flexalonConstraint.Target = attackPointStay.gameObject;
        ClearOffset();
        ClearRotation();
    }
    
    public void IntoFormation()
    {
        flexalonConstraint.Target = null;
        flexalonConstraint.enabled = false;
        transform.parent = Global.Battle.cannonAnimationManager.transform;
        Global.Battle.cannonAnimationManager.cannonsInFormation.Add(this);
        //cannonFlexalonLerpAnimator.AnimatePosition = true;
        cannonFlexalonLerpAnimator.InterpolationSpeed = 5f;
        UpdateRotationX();
        UpdateOffsetY();
    }
}