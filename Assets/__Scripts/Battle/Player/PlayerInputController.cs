using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInputController : MonoBehaviour
{
    private PlayerBattleInput playerBattleInput;
    public Vector2 currentInput;
    public bool dashing;

    private List<TouchControl> activeTouches = new List<TouchControl>();

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
            // var input = playerBattleInput.PlayerBattle.Movement.ReadValue<Vector2>();
            var input = ReadMovementInput();
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
    
    private int UpdateActiveTouches()
    {
        // Clear the list of active touches each frame and repopulate it
        activeTouches.Clear();
        foreach (var touch in Touchscreen.current.touches)
        {
            if (touch.press.isPressed)
            {
                // Add only the active touches to the list
                activeTouches.Add(touch);
            }
        }

        return activeTouches.Count;
    }

    private Vector2 ReadLatestTouchPosition() {
        Vector2 normalizedTouchPosition = Vector2.zero;
        if (activeTouches.Count > 0)
        {
            // Get the last touch in the list, which is the latest touch
            TouchControl latestTouch = activeTouches[activeTouches.Count - 1];
            var latestTouchPosition = latestTouch.position.ReadValue();
            normalizedTouchPosition = new Vector2(latestTouchPosition.x / Screen.width, latestTouchPosition.y / Screen.height);
            Debug.Log($"Latest Touch Position: {latestTouchPosition}");
        }
        return normalizedTouchPosition;
    }

    private Vector2 ReadMovementInput() {
        Vector2 input = playerBattleInput.PlayerBattle.Movement.ReadValue<Vector2>();
        
        // if no keyboard/stick input, read touchscreen
        if (input.Equals(Vector2.zero) && UpdateActiveTouches() > 0)
        {
            input = ReadLatestTouchPosition();
            
            // compare input to the center
            input -= new Vector2(0.5f, 0.5f);
            if (input.x > 0f) input.x = 1;
            if (input.x < 0f) input.x = -1;
            if (input.y > 0f) input.y = 1;
            if (input.y < 0f) input.y = -1;
        }

        return input;
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