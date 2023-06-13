using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChipBoxManager : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public List<ChipBox> chipBoxes;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.DOFade(0f, 0f);
    }

    public void Appear()
    {
        foreach (var chipBox in chipBoxes)
        {
            chipBox.Init();
        }
        canvasGroup.DOFade(1f, 1f);
    }
}
