using System;
using System.Collections.Generic;
using UnityEngine;
using static ChipData;
using static Global.Misc;
using Random = UnityEngine.Random;

public static class BattleRewardManager
{
    public static void AddToInventory(List<ChipInfo> rewardChips)
    {
        Global.Misc.savePath = Application.persistentDataPath + "/PlayerData.json";
        var jsonToLoad = System.IO.File.ReadAllText(Global.Misc.savePath);
        var saveData = JsonUtility.FromJson<PlayerDataManager.PlayerData>(jsonToLoad);

        foreach (var chipInfo in rewardChips)
        {
            saveData.inventory.Add(chipInfo);
        }
        
        var jsonToSave = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(Global.Misc.savePath, jsonToSave);

        Debug.Log("Reward Chips Added:");
        foreach( var x in rewardChips) {
            Debug.Log( x.name);
        }
        Debug.Log("Saved to: " + Global.Misc.savePath);
    }

    public static ChipInfo CreateChip(Rarity rarity)
    {
        ChipInfo newChip = CreateChipInfoWithRandomNameAndDescription(rarity);
        newChip.attackType = (CannonAttackType)Random.Range(0, 3);
        newChip.elementType = (ElementType)Random.Range(0, 3);

        int numberOfModEffect = 1;
        // check rarity within Common and Legendary
        switch (rarity)
        {
            case Rarity.Common:
                numberOfModEffect = Random.Range(1, 3);
                break;
            case Rarity.Uncommon:
                numberOfModEffect = 3;
                break;
            case Rarity.Rare:
                numberOfModEffect = 4;
                break;
            case Rarity.Epic:
                numberOfModEffect = 5;
                break;
            case Rarity.Legendary:
                numberOfModEffect = 6;
                break;
            case Rarity.None:
            case Rarity.Mythical1:
            case Rarity.Mythical2:
            case Rarity.Mythical3:
            case Rarity.Mythical4:
            case Rarity.Mythical5:
            case Rarity.Mythical6:
            default:
                throw new NotSupportedException(
                    $"CreateChip: Rarity should range from Common to Legendary. {nameof(rarity)}, {rarity}");
        }
        
        // int numberOfModEffect = Random.Range(1 + (int)rarity, 3 + (int) rarity);
        
        var availableEffectList = ModificationEffectManager.GetAll(newChip.elementType, newChip.attackType);
        var guaranteedEffect = ModificationEffectManager.GetRandomFromList(availableEffectList);
        newChip.modificationEffectIDs.Add(guaranteedEffect.id);
        newChip.modificationEffectRarities.Add(rarity);
        
        for (int i = 1; i < numberOfModEffect; i++)
        {
            var modEffect = ModificationEffectManager.GetRandomFromList(availableEffectList);
            newChip.modificationEffectIDs.Add(modEffect.id);
            Rarity randomRarity = (Rarity)Random.Range(0, 1 + (int)rarity);
            newChip.modificationEffectRarities.Add(randomRarity);
        }

        return newChip;
    }

    public static ChipInfo CreateChipInfoWithRandomNameAndDescription(Rarity rarity)
    {
        var newChipInfo = new ChipInfo();
        
        switch (rarity)
        {
            case Rarity.Common:
                newChipInfo.name = "ChipInfo-Name-Common-";
                newChipInfo.description = "ChipInfo-Description-Common-";
                break;
            case Rarity.Uncommon:
                newChipInfo.name = "ChipInfo-Name-Uncommon-";
                newChipInfo.description = "ChipInfo-Description-Uncommon-";
                break;
            case Rarity.Rare:
                newChipInfo.name = "ChipInfo-Name-Rare-";
                newChipInfo.description = "ChipInfo-Description-Rare-";
                break;
            case Rarity.Epic:
                newChipInfo.name = "ChipInfo-Name-Epic-";
                newChipInfo.description = "ChipInfo-Description-Epic-";
                break;
            case Rarity.Legendary:
                newChipInfo.name = "ChipInfo-Name-Legendary-";
                newChipInfo.description = "ChipInfo-Description-Legendary-";
                break;
        }

        int index = Random.Range(1, 7);
        newChipInfo.name += index;
        newChipInfo.description += index;
        
        newChipInfo.modificationEffectRarities = new List<Rarity>();
        newChipInfo.modificationEffectIDs = new List<int>();

        return newChipInfo;
    }

    public static ChipInfo CreatePremadeChip()
    {
        return new ChipInfo();
    }
}
