using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CannonSlotSelectHandler : MonoBehaviour, ISelectHandler
{
    public GameObject selectIndicator;
    
    private void Awake()
    {
        //selectIndicator.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(transform.parent.name + " was selected as sibling " + transform.parent.GetSiblingIndex());
        Global.MainMenu.cannonDisplayController.SelectSlot(gameObject.transform.GetSiblingIndex());
        //selectIndicator.SetActive(true);
    }

    public void Deselect()
    {
        //selectIndicator.SetActive(false);
    }
}
