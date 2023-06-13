using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectOnScreenSpace : MonoBehaviour
{
    public GameObject worldObject;
    public GameObject canvas;
    public Camera camera;
    private RectTransform UI_Element;
    private RectTransform CanvasRect;
    

    private void Awake()
    {
        UI_Element = GetComponent<RectTransform>();
        CanvasRect = canvas.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ViewportPosition=camera.WorldToViewportPoint(worldObject.transform.position);
        Vector2 WorldObject_ScreenPosition=new Vector2(
            ((ViewportPosition.x*CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x*0.5f)),
            ((ViewportPosition.y*CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y*0.5f)));
 
        //now you can set the position of the ui element
        UI_Element.anchoredPosition=WorldObject_ScreenPosition;

    }
}
