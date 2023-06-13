using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDebugger : MonoBehaviour
{
    private void Awake()
    {
        // EventCenter.GetInstance()
        //     .AddEventListener(Global.Events.PlayerChangeMoveDirectionToFront, () => { Debug.Log("Front"); });
        // EventCenter.GetInstance()
        //     .AddEventListener(Global.Events.PlayerChangeMoveDirectionToBack, () => { Debug.Log("Back"); });

        // EventCenter.GetInstance().AddEventListener(Global.Events.PlayerStartMove, () => { Debug.Log("Start Move"); });
    }
}