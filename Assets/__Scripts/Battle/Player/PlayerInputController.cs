using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerBattleInput playerBattleInput;
    public Vector2 currentInput;
    public bool dashing;

    private void Awake()
    {
        Global.Battle.playerInputController = this;
        playerBattleInput = new PlayerBattleInput();
        playerBattleInput.PlayerBattle.Enable();
        currentInput = new Vector2();
    }

    private void FixedUpdate()
    {
        var moveDirection = new Vector3();
        if (!dashing)
        {
            var input = playerBattleInput.PlayerBattle.Movement.ReadValue<Vector2>();
            currentInput = input;
            moveDirection = new Vector3(input.x, 0f, input.y);
        }
        else
        {
            moveDirection = new Vector3(currentInput.x, 0f, currentInput.y) * 15f;
        }

        if (moveDirection.magnitude > 0)
        {
            if (!Global.Battle.playerBehaviourController.isMoving) EventCenter.GetInstance().EventTrigger(Global.Events.PlayerStartMove);
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayerMoving, moveDirection);
        }
        else
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.PlayerStopMove);
        }
    }

    private void Update()
    {
        if (playerBattleInput.PlayerBattle.Dash.triggered & !dashing)
        {
            EventCenter.GetInstance().EventTrigger(Global.Events.Dash);
            Global.Battle.playerBehaviourController.rigidBody.drag = 15f;
            dashing = true;
            Global.DoTweenWait(0.1f, () =>
            {
                Global.Battle.playerBehaviourController.rigidBody.drag = 5f;
                dashing = false;
            });
        }
    }
}