using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Flexalon;
using Unity.VisualScripting;
using UnityEngine;

public class ChipShredManager : MonoBehaviour
{
    public FlexalonRandomLayout flexalonRandomLayout;
    public List<ChipGainShred> chipGainShreds;
    public ParticleSystem particleSystem;

    private void Awake()
    {
    }

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        foreach (var shred in chipGainShreds)
        {
            shred.ResetScaleToZero();
        }
    }

    public void StartPartTwoAnimation()
    {
        EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioSoundEffect, GameAudios.AudioName.ChipGainRelease);
        particleSystem.Play();
        foreach (var shred in chipGainShreds)
        {
            shred.AnimateScale();
            flexalonRandomLayout.RotationMaxX = 40f;
        }

        transform.DORotate(new Vector3(0f, 0f, 0f), 2.5f).From(new Vector3(0f, 180f, 0f));

        Global.DoTweenWait(1.5f, () =>
        {
            flexalonRandomLayout.RandomizeRotationX = false;
            flexalonRandomLayout.RandomizeRotationY = false;
            flexalonRandomLayout.RandomizeRotationZ = false;
            foreach (var shred in chipGainShreds)
            {
                shred.AnimateInterpolationSpeed(0,2f,2f);
            }
        });
        
        Global.DoTweenWait(5f, () =>
        {
            foreach (var shred in chipGainShreds)
            {
                shred.Dissolve();
            }
            Global.Battle.chipGainUIManager.chipBoxManager.Appear();
        });
    }
}
