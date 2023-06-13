using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Flexalon;
using UnityEngine;

public class CannonAnimationManager : MonoBehaviour
{
    public CannonAnimation[] cannonsTemp;
    public List<CannonAnimation> cannonsInFormation;
    public List<CannonAnimation> allCannons;
    private FlexalonCircleLayout cannonsFlexalonCircleLayout;
    private float cannonRotationAngleWhileMoving;
    private float gunLoopOffset;

    public float cannonRotationAngleMovingFront;
    public float cannonRotationAngleMovingBack;
    public float cannonRotationAngleIdle;

    public float gunLoopOffsetMovingFront;
    public float gunLoopOffsetMovingBack;
    public float gunLoopOffsetIdle;

    public bool inMovingFormation;


    private void Awake()
    {
        cannonsTemp = GetComponentsInChildren<CannonAnimation>();
        cannonsInFormation = new List<CannonAnimation>(cannonsTemp);
        allCannons = new List<CannonAnimation>(cannonsTemp);
        cannonsFlexalonCircleLayout = GetComponent<FlexalonCircleLayout>();
        Global.Battle.cannonAnimationManager = this;

        EventCenter.GetInstance().AddEventListener<Vector3>(Global.Events.PlayerMoving, ChangeToMovingFormation);
        EventCenter.GetInstance().AddEventListener(Global.Events.PlayerStopMove, ChangeToStopFormation);

        EventCenter.GetInstance().AddEventListener(Global.Events.PlayerChangeMoveDirectionToFront, PlayerMoveFront);
        EventCenter.GetInstance().AddEventListener(Global.Events.PlayerChangeMoveDirectionToBack, PlayerMoveBack);
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        cannonRotationAngleMovingFront = -45f;
        cannonRotationAngleIdle = -15f;
        cannonRotationAngleMovingBack = 45f;

        gunLoopOffsetMovingFront = 0.4f;
        gunLoopOffsetMovingBack = -0.5f;
        gunLoopOffsetIdle = 0;

        PlayerMoveFront();
        inMovingFormation = false;
    }

    private void ChangeToMovingFormation(Vector3 _)
    {
        if (inMovingFormation) return;
        cannonsFlexalonCircleLayout.Rotate = FlexalonCircleLayout.RotateOptions.In;
        foreach (var cannonObject in allCannons)
        {
            cannonObject.ChangeRotationX(cannonRotationAngleWhileMoving);
            cannonObject.ChangeOffsetY(gunLoopOffset);
        }

        foreach (var cannonObject in cannonsInFormation)
        {
            cannonObject.UpdateOffsetY();
            cannonObject.UpdateRotationX();
        }

        inMovingFormation = true;
    }

    private void ChangeToStopFormation()
    {
        cannonsFlexalonCircleLayout.Rotate = FlexalonCircleLayout.RotateOptions.In;
        foreach (var cannonObject in allCannons)
        {
            cannonObject.ChangeRotationX(cannonRotationAngleIdle);
            cannonObject.ChangeOffsetY(gunLoopOffsetIdle);
        }

        foreach (var cannonObject in cannonsInFormation)
        {
            cannonObject.UpdateRotationX();
            cannonObject.UpdateOffsetY();
        }

        inMovingFormation = false;
    }

    private void PlayerMoveFront()
    {
        cannonRotationAngleWhileMoving = cannonRotationAngleMovingFront;
        gunLoopOffset = gunLoopOffsetMovingFront;
        inMovingFormation = false;
    }

    private void PlayerMoveBack()
    {
        cannonRotationAngleWhileMoving = cannonRotationAngleMovingBack;
        gunLoopOffset = gunLoopOffsetMovingBack;
        inMovingFormation = false;
    }
}