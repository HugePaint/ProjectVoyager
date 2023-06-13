using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private PlayerBattleInput playerBattleInput;
    public Vector2 currentInput;

    private void Awake()
    {
        Global.Battle.playerInputController = this;
        playerBattleInput = new PlayerBattleInput();
        playerBattleInput.PlayerBattle.Enable();
        currentInput = new Vector2();
    }

    private void FixedUpdate()
    {
        var input = playerBattleInput.PlayerBattle.Movement.ReadValue<Vector2>();
        currentInput = input;
        var moveDirection = new Vector3(input.x, 0f, input.y);

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
}