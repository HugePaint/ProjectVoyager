using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGetPresetChip : MonoBehaviour
{
    public ChipPreset targetPreset;

    public string modEffectTableURL;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetChip()
    {
        var chipInfo = new ChipData.ChipInfo(targetPreset);
        Global.MainMenu.playerDataManager.AddItem(chipInfo);
        
        Debug.Log($"DebugGetPresetChip: {chipInfo.name} Chip Added. {chipInfo.modificationEffectIDs.Count} Mod Effects in total.");

        Global.MainMenu.cannonDisplayController.UpdateAll();
        Global.MainMenu.inventoryDisplayController.UpdateAll();
    }
}
