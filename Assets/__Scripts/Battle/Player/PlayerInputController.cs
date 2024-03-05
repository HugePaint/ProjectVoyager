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

    private Vector2 GetNormalizedPosition(Vector2 position)
    {
        // Convert position to be relative to the center of the screen
        Vector2 centerRelativePosition = new Vector2(position.x - Screen.width / 2, position.y - Screen.height / 2);
        
        // Normalize based on the maximum possible distance from the center to the corner
        float maxDistance = Mathf.Sqrt(Mathf.Pow(Screen.width / 2, 2) + Mathf.Pow(Screen.height / 2, 2));
        Vector2 normalizedPosition = centerRelativePosition / maxDistance;

        // Ensure the values are clamped between -1 and 1
        normalizedPosition.x = Mathf.Clamp(normalizedPosition.x, -1, 1);
        normalizedPosition.y = Mathf.Clamp(normalizedPosition.y, -1, 1);

        // normalized again so the length is 1
        return normalizedPosition.normalized;
    }

    private Vector2 ReadMovementInput() {
        Vector2 input = playerBattleInput.PlayerBattle.Movement.ReadValue<Vector2>();

        // if no keyboard/stick input, read mouse
        if (input.Equals(Vector2.zero) && Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            var normalizedMousePosition = GetNormalizedPosition(Mouse.current.position.ReadValue());
            // Debug.Log($"PlayerInputController: Normalized Mouse Position {normalizedMousePosition}");
            return GetNormalizedPosition(Mouse.current.position.ReadValue());
        }
        
        // if no keyboard/stick & no mouse input, read touchscreen
        if (input.Equals(Vector2.zero) && UpdateActiveTouches() > 0)
        {
            var normalizedTouchPosition = GetNormalizedPosition(activeTouches[^1].position.ReadValue());
            // Debug.Log($"PlayerInputController: Normalized Last Touch Position {normalizedTouchPosition}");
            return normalizedTouchPosition;
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
            currentInput = ReadMovementInput();
            Global.DoTweenWait(0.1f, () =>
            {
                Global.Battle.playerBehaviourController.rigidBody.drag = 5f;
                dashing = false;
            });
        }
    }
}