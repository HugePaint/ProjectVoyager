using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPresetChip : MonoBehaviour
{
    public ChipPreset targetPreset;
    public string modEffectTableURL = "https://docs.google.com/spreadsheets/d/1BDRTSztU4WLAE9Cc6wiBw8hm_4OpMXMWLdCHdBwxp-I/edit#gid=1240493115";
    
    public void GetChip()
    {
        var chipInfo = new ChipData.ChipInfo(targetPreset);
        Global.MainMenu.playerDataManager.AddItem(chipInfo);
        
        Debug.Log($"DebugGetPresetChip: {chipInfo.name} Chip Added. {chipInfo.modificationEffectIDs.Count} Mod Effects in total.");

        Global.MainMenu.cannonDisplayController.UpdateAll();
        Global.MainMenu.inventoryDisplayController.UpdateAll();
    }
}
