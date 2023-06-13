using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ChipData;
using Random = UnityEngine.Random;

public class PlayerDataManager : MonoBehaviour
{
    public int floatingCannonCount = 6;
    
    private List<FloatingCannonSlot> floatingCannonSlots;
    private List<Chip> inventory;
    private List<int> unlockedMemories; //placeholder

    private string saveDataPath = "/PlayerData.json";
    
    [Serializable]
    public class PlayerData
    {
        public List<ChipInfo> inventory;
        public List<ChipInfo> chipInCannons;
    }
    
    private void Awake()
    {
        Global.MainMenu.playerDataManager = this;
        saveDataPath = Application.persistentDataPath + saveDataPath;
        Global.Misc.savePath = saveDataPath;
        Initialize();
        LoadFromDisk();
    }

    private void Start()
    {
        
    }

    public void Initialize()
    {
        inventory = new List<Chip>();
        
        floatingCannonSlots = new List<FloatingCannonSlot>();
        for (int i = 0; i < floatingCannonCount; i++)
        {
            floatingCannonSlots.Add(new FloatingCannonSlot(i));
        }
    }

    public void Test()
    {
        inventory.Add(new Chip(9));
        inventory.Add(new Chip(4));
        inventory.Add(new Chip(5));
        inventory.Add(new Chip(6));
        inventory.Add(new Chip(10));
    }

    public void SetChipToCannon(int inventoryIndex, int cannonIndex)
    {
        Chip chipFromInventory = inventory[inventoryIndex];
        inventory.RemoveAt(inventoryIndex);
        
        Chip chipFromCannon = floatingCannonSlots[cannonIndex].SetChip(chipFromInventory);
        inventory.Insert(inventoryIndex, chipFromCannon);
    }

    public ChipInfo GetChipInfoFromInventory(int index)
    {
        return inventory[index].GetInfo();
    }
    
    public ChipInfo GetChipInfoFromCannon(int index)
    {
        return floatingCannonSlots[index].GetCurrentChipInfo();
    }

    public List<Chip> GetAllChipsEquipped()
    {
        var chipList = new List<Chip>();
        foreach (var cannonSlot in floatingCannonSlots)
        {
            chipList.Add(cannonSlot.GetChip());
        }

        return chipList;
    }

    public int GetTotalItemInInventory()
    {
        return inventory.Count;
    }

    public void ApplyModificationEffectsOnPreBattleData()
    {
        for (int i = 0; i < floatingCannonCount; i++)
        {
            floatingCannonSlots[i].ApplyBattleEffects();
        }
        
    }
    
    public void AddItem(ChipInfo info)
    {
        inventory.Add(new Chip(info));
    }

    private ChipInfo RemoveItem(int index)
    {
        ChipInfo removedInfo = inventory[index].GetInfo();
        inventory.RemoveAt(index);
        return removedInfo;
    }
    
    private ChipInfo RemoveLast()
    {
        ChipInfo removedInfo = inventory[^1].GetInfo();
        inventory.RemoveAt(inventory.Count - 1);
        return removedInfo;
    }

    public ChipInfo Upgrade2Chips(int index1, int index2)
    {
        var chip1 = GetChipInfoFromInventory(index1);
        var chip2 = GetChipInfoFromInventory(index2);
        if (index1 > index2)
        {
            RemoveItem(index1);
            RemoveItem(index2);
        }
        else
        {
            RemoveItem(index2);
            RemoveItem(index1);
        }
        
        var newChipInfo = ChipUpgradeManager.Upgrade(chip1, chip2);
        AddItem(newChipInfo);
        return newChipInfo;
    }

    public void SaveToDisk()
    {
        var saveData = new PlayerData
        {
            chipInCannons = floatingCannonSlots.Select(cannonSlot => cannonSlot.GetCurrentChipInfo()).ToList(),
            inventory = inventory.Select(chip => chip.GetInfo()).ToList()
        };
        
        string json = JsonUtility.ToJson(saveData);
        System.IO.File.WriteAllText(saveDataPath, json);
        
        Debug.Log("Saved to: " + saveDataPath);
    }

    private void LoadFromDisk()
    {
        if (System.IO.File.Exists(saveDataPath) == false)
            return;
        
        string json = System.IO.File.ReadAllText(saveDataPath);
        var saveData = JsonUtility.FromJson<PlayerData>(json);

        foreach (var chipInfo in saveData.inventory)
        {
            inventory.Add(new Chip(chipInfo));
        }

        for (int i = 0; i < floatingCannonCount; i++)
        {
            floatingCannonSlots[i].SetChip(new Chip(saveData.chipInCannons[i]));
        }
        
        Debug.Log(String.Format("PlayerData Loaded: {0} Chips in inventory; {1} Chips in Cannon Slots.",
            saveData.inventory.Count, saveData.chipInCannons.Count));
    }
}
