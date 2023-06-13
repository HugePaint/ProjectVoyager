using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class PlayerBehaviourController : MonoBehaviour
{
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Collider playerCollider;
    [HideInInspector] public PlayerModelController playerModel;
    [HideInInspector] public PlayerAnimationController playerAnimationController;

    public bool movingFront;
    public bool isMoving;

    [SerializeField] private float movementSpeed;

    private void Awake()
    {
        Global.Battle.playerBehaviourController = this;

        //Get Components
        rigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        playerModel = GetComponentInChildren<PlayerModelController>();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        //Assign Events
        EventCenter.GetInstance().AddEventListener<Vector3>(Global.Events.PlayerMoving, Moving);
        EventCenter.GetInstance().AddEventListener(Global.Events.PlayerStopMove, StopMove);
        EventCenter.GetInstance().AddEventListener(Global.Events.PlayerChangeMoveDirectionToFront, StartMovingFront);
        EventCenter.GetInstance().AddEventListener(Global.Events.PlayerChangeMoveDirectionToBack, StartMovingBack);
        EventCenter.GetInstance().AddEventListener(Global.Events.StartSpawningEnemy, () =>
        {
            EnablePlayerController(true);
        });
        
        EventCenter.GetInstance().AddEventListener(Global.Events.GameOver, () =>
        {
            GameOverBehaviour();
        });
        
        movementSpeed = 0;
    }

    private void Start()
    {
        playerAnimationController.PlayAnimationIdle();
        Init();
        EnablePlayerController(false);
    }

    public void EnablePlayerController(bool enable)
    {
        Global.Battle.playerInputController.enabled = enable;
    }

    public void UpdateMovementSpeed(float value)
    {
        movementSpeed += value;
    }

    private void Init()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayerChangeMoveDirectionToFront);
        isMoving = false;
    }

    private void Moving(Vector3 inputVector)
    {
        isMoving = true;
        Global.Battle.fakePlayer.SetLookAt(inputVector);
        rigidBody.AddForce(inputVector * movementSpeed, ForceMode.Force);

        if (movingFront)
        {
            if (playerModel.GetRotationDirection() == PlayerModelController.MoveDirection.Back)
            {
                playerModel.LookAtPosition(playerModel.GetPosition() - inputVector, 0f);
                playerAnimationController.PlayAnimationBack();
                if (movingFront) EventCenter.GetInstance().EventTrigger(Global.Events.PlayerChangeMoveDirectionToBack);
            }
            else
            {
                playerModel.LookAtPosition(playerModel.GetPosition() + inputVector);
                playerAnimationController.PlayAnimationFront();
            }
        }
        else
        {
            if (playerModel.GetRotationDirection() == PlayerModelController.MoveDirection.Back)
            {
                playerModel.LookAtPosition(playerModel.GetPosition() + inputVector, 0f);
                playerAnimationController.PlayAnimationFront();
                if (!movingFront)
                    EventCenter.GetInstance().EventTrigger(Global.Events.PlayerChangeMoveDirectionToFront);
            }
            else
            {
                playerModel.LookAtPosition(playerModel.GetPosition() - inputVector);
                playerAnimationController.PlayAnimationBack();
            }
        }
    }

    private void StopMove()
    {
        if (!isMoving) return;
        isMoving = false;
        playerAnimationController.PlayAnimationIdle();
        if (!movingFront) EventCenter.GetInstance().EventTrigger(Global.Events.PlayerChangeMoveDirectionToFront);
    }

    private void StartMovingFront()
    {
        movingFront = true;
    }

    private void StartMovingBack()
    {
        movingFront = false;
    }

    public void GameOverBehaviour()
    {
        if (!Global.Battle.battleTimer.timeEnd)
        {
            playerAnimationController.PlayAnimationDie();
            EnablePlayerController(false);
        }
        else
        {
            Global.DoTweenWait(2f, () =>
            {
                EnablePlayerController(false);
            });
        }

        Global.Battle.cannonBattleManager.DisableAllCannon();
    }
}