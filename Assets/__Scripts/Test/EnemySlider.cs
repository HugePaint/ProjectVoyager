using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySlider : MonoBehaviour
{
    public Slider slider;
    public List<GameObject> enemies;
    public TMP_Text text;

    private void Awake()
    {
        slider.onValueChanged.RemoveAllListeners();
        slider.onValueChanged.AddListener(SizeChange);
    }

    public void SizeChange(float value)
    {
        text.text = "Scale: " + value.ToString("0.0");
        foreach (var e in enemies)
        {
            e.transform.DOScale(new Vector3(value, value, value), 0);
        }
    }
}