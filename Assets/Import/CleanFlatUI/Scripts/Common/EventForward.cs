using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
public class EventForward : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject targetGameObject;
    public void OnPointerDown(PointerEventData eventData)
    {
        if(targetGameObject != null)
        {
            ExecuteEvents.Execute(targetGameObject, eventData, ExecuteEvents.pointerDownHandler);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetGameObject != null)
        {
            ExecuteEvents.Execute(targetGameObject, eventData, ExecuteEvents.pointerUpHandler);
        }
    }
}
