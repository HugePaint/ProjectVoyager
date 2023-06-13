
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static ChipData;
using static Global.Misc;
using Random = UnityEngine.Random;

public class ChipUpgradeManager : MonoBehaviour
{
    [FormerlySerializedAs("newChip")] public ChipDetailDisplayController newChipPanel;

    public void OnUpgrade()
    {
        var indexSelected = Global.MainMenu.upgradeInventoryDisplayController.GetSlotSelected();
        var newChipInfo = Global.MainMenu.playerDataManager.Upgrade2Chips(indexSelected[0], indexSelected[1]);
        newChipPanel.UpdateInfo(newChipInfo);
        
        Global.MainMenu.playerDataManager.SaveToDisk();
    }
    
    public static ChipInfo Upgrade(ChipInfo chip1, ChipInfo chip2)
    {
        bool elementTypeFrom1 = (Random.value > 0.5f);
        bool attackTypeFrom1 = (Random.value > 0.5f);
        List<int> modEffectIDPool = chip1.modificationEffectIDs;
        modEffectIDPool.AddRange(chip2.modificationEffectIDs);

        int newModEffectTotalInHalf = Mathf.CeilToInt(modEffectIDPool.Count / 2f);
        int newModEffectTotal = Random.Range(1, newModEffectTotalInHalf + 2);
        if (newModEffectTotal < newModEffectTotalInHalf)
            newModEffectTotal = newModEffectTotalInHalf;
        if (newModEffectTotal > 6)
            newModEffectTotal = 6;
        
        List<int> newModEffectIDs = new List<int>();
        for (int i = 0; i < newModEffectTotal; i++)
        {
            int index = Random.Range(0, modEffectIDPool.Count);
            newModEffectIDs.Add(modEffectIDPool[index]);
            modEffectIDPool.Remove(index);
        }

        
        Rarity rarest = Rarity.Common;
        foreach (var r in chip1.modificationEffectRarities)
            if (rarest < r) rarest = r;
        foreach (var r in chip2.modificationEffectRarities)
            if (rarest < r) rarest = r;
        while (chip1.modificationEffectRarities.Count < chip2.modificationEffectRarities.Count)
            chip1.modificationEffectRarities.Add((Rarity)Random.Range(0, (int)rarest));
        List<Rarity> newRarities = new List<Rarity>();
        for (int i = 0; i < newModEffectTotal; i++)
        {
            if (i < chip1.modificationEffectRarities.Count)
            {
                int lowerLimit = (int)chip1.modificationEffectRarities[i];
                int upperLimit = (int)rarest;
                newRarities.Add((Rarity)Random.Range(lowerLimit, upperLimit + 1));
            }
            else
            {
                newRarities.Add((Rarity)Random.Range(0, (int)rarest + 1));
            }

        }

        Rarity newRarity = (Rarity)(newModEffectTotal - 2);
        if ((int)newRarity == -1) newRarity = Rarity.Common;
        ChipInfo newChip =
            BattleRewardManager.CreateChipInfoWithRandomNameAndDescription(newRarity);
        newChip.elementType = elementTypeFrom1 ? chip1.elementType : chip2.elementType;
        newChip.attackType = elementTypeFrom1 ? chip1.attackType : chip2.attackType;
        newChip.modificationEffectIDs = newModEffectIDs;
        newChip.modificationEffectRarities = newRarities;
        return newChip;
    }
}
