using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class MainMenuInit : MonoBehaviour
{
    public BattleData battleData;
    public ColorData colorData;
    public Locale defaultLocale;
    
    private void Awake()
    {
        DOTween.Init(useSafeMode:false);
        LocalizationSettings.SelectedLocale = defaultLocale;
        LoadData();
    }

    void LoadData()
    {
        Global.Battle.battleData = battleData;
        Global.Misc.colorData = colorData;
    }
}
