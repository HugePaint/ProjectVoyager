using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using UnityEngine;
using static ChipData;
using Random = UnityEngine.Random;

public class PlayerDataManager : MonoBehaviour
{
    public int floatingCannonCount = 6;
    
    private List<FloatingCannonSlot> floatingCannonSlots;
    private List<Chip> inventory;
    private List<int> unlockedMemories; //placeholder

    private string saveDataPath;
    private const string SaveDataFileName = "/PlayerData.json";
    private const string BackupDataFileName = "/SaveBackup.txt";
    
    [Serializable]
    public class PlayerData
    {
        public List<ChipInfo> inventory;
        public List<ChipInfo> chipInCannons;
    }
    
    private void Awake()
    {
        Global.MainMenu.playerDataManager = this;
        saveDataPath = Application.persistentDataPath + SaveDataFileName;
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
        PlayerPrefs.Save();
        
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

    public string CreateSaveString() {
        string json = System.IO.File.ReadAllText(saveDataPath);
        return Zip(json);
        // return json;
    }

    public int LoadSaveFromString(string saveString) {
        if (saveString == "") return 0;
        
        try
        {
            PlayerData saveData = null;
            if (IsBase64String(saveString))
            {
                Debug.Log("Base64String");
                saveString = Unzip(saveString);
                saveData = JsonUtility.FromJson<PlayerData>(saveString);
            }
            else
            {
                saveData = JsonUtility.FromJson<PlayerData>(saveString);
            }
            
            // extra validation
            if (saveData == null) return 0;

            SaveBackup();

            File.WriteAllText(saveDataPath, saveString);
            PlayerPrefs.Save();
            Debug.Log("Imported Save: " + saveDataPath);
            return 1;
        }
        catch (System.Exception)
        {
            Debug.Log("Save String Invalid. Check again.");
            return 0;
        }
    }

    public void SaveBackup() {
        if (File.Exists(saveDataPath) == false) return;
        
        string oldSave = File.ReadAllText(saveDataPath);
        string backupSavePath = Application.persistentDataPath + BackupDataFileName;
        File.WriteAllText(backupSavePath, Zip(oldSave));
    }
    

    public static void CopyTo(Stream src, Stream dest) {
        byte[] bytes = new byte[4096];

        int cnt;

        while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
            dest.Write(bytes, 0, cnt);
        }
    }
    
    public static string Zip(string str) {
    
        var bytes = Encoding.UTF8.GetBytes(str);
    
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream()) {
            using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                //msi.CopyTo(gs);
                CopyTo(msi, gs);
            }
    
            return Convert.ToBase64String(mso.ToArray());
        }
    }
    
    public static string Unzip(string str) {
        byte[] bytes = Convert.FromBase64String(str);
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream()) {
            using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                //gs.CopyTo(mso);
                CopyTo(gs, mso);
            }
            return Encoding.UTF8.GetString(mso.ToArray());
        }
    }
    
    public static bool IsBase64String(string base64)
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer , out int bytesParsed);
    }
}
