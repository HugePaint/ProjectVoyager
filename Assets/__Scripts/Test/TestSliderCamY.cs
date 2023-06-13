using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestSliderCamY : MonoBehaviour
{
    public bool isY;
    public Slider slider;
    public TMP_Text text;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(ChangeText);
    }

    private void Start()
    {
        var transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        var offset = transposer.m_FollowOffset;
        if (isY)
        {
            slider.value = offset.y;
            text.text = "Offset Y: " + offset.y.ToString("0.0");
        }
        else
        {
            slider.value = offset.z;
            text.text = "Offset Z: " + offset.z.ToString("0.0");
        }
    }

    public void ChangeText(float value)
    {
        var transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        var offset = transposer.m_FollowOffset;
        if (isY)
        {
            text.text = "Offset Y: " + value.ToString("0.0");
            transposer.m_FollowOffset = new Vector3(offset.x, value, offset.z);
        }
        else
        {
            transposer.m_FollowOffset = new Vector3(offset.x, offset.y, value);
            text.text = "Offset Z: " + value.ToString("0.0");
        }
    }
}