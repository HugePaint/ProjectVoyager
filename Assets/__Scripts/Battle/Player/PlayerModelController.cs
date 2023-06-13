using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    public enum MoveDirection
    {
        Front,
        Back
    }

    public void LookAtPosition(Vector3 position, float duration = 0.2f, Action turningEndAction = null)
    {
        transform.DOKill();
        transform.DOLookAt(position, duration).SetEase(Ease.Linear).OnComplete(() => { turningEndAction?.Invoke(); });
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public MoveDirection GetRotationDirection()
    {
        var movingDirection = MoveDirection.Front;
        var angleDiff = math.abs(Global.Battle.fakePlayer.GetLookAtDirectionY() - GetRotationY());
        if (angleDiff > 130 && angleDiff < 230)
        {
            movingDirection = MoveDirection.Back;
        }

        return movingDirection;
    }

    private float GetRotationY()
    {
        var rotataionY = 0f;
        if (Global.Battle.playerBehaviourController.movingFront)
        {
            rotataionY = transform.rotation.eulerAngles.y;
        }
        else
        {
            rotataionY = transform.rotation.eulerAngles.y + 180f;
            if (rotataionY >= 360f)
            {
                rotataionY = rotataionY - 360;
            }
        }

        return rotataionY
            ;
    }
}