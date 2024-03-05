using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour
{
    public BattleUIManager battleUIManager;
    public ChipGainUIManager chipGainUIManager;
    private void Start()
    {
        DisplayInitialUI();
    }
    
    public void DisplayInitialUI()
    {
        if (Global.Battle.battleData.isGainingChips)
        {
            ShowChipGainUI();
        }
        else
        {
            ShowBattleUI();
        }
    }

    public void ShowBattleUI()
    {
        battleUIManager.gameObject.SetActive(true);
        chipGainUIManager.gameObject.SetActive(false);
    }

    public void ShowChipGainUI()
    {
        battleUIManager.gameObject.SetActive(false);
        chipGainUIManager.gameObject.SetActive(true);
    }
}
