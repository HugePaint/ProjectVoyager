using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using static ChipData;

public class CannonDisplayController : MonoBehaviour
{
    private int slotSelected;
    public GameObject cannonSlotParent;
    public GameObject cannonPrefabParent;
    public List<GameObject> cannonSlots;
    public ChipDetailDisplayController chipDetailDisplayController;

    private List<CannonSlotSelectHandler> cannonSlotHandlers;
    private PlayerDataManager playerDataManager;
    private List<CannonOutfitChanger> outfitChangers;

    public void Awake()
    {
        Global.MainMenu.cannonDisplayController = this;
        outfitChangers = cannonPrefabParent.GetComponentsInChildren<CannonOutfitChanger>().ToList();
    }

    public void Start()
    {
        playerDataManager = Global.MainMenu.playerDataManager;
        cannonSlots = new List<GameObject>();
        cannonSlotHandlers = new List<CannonSlotSelectHandler>();
        for (int i = 0; i < playerDataManager.floatingCannonCount; i++)
        {
            GameObject slot = cannonSlotParent.transform.GetChild(i).gameObject;
            cannonSlots.Add(slot);

            CannonSlotSelectHandler handler = slot.GetComponentInChildren<CannonSlotSelectHandler>();
            cannonSlotHandlers.Add(handler);
        }
        
        UpdateAll();
    }

    public void UpdateSlot(int index)
    {
        ChipInfo slotInfo = playerDataManager.GetChipInfoFromCannon(index);
        var chipSpriteController = cannonSlots[index].GetComponentInChildren<ChipSpriteController>();
        chipSpriteController.UpdateSprite(slotInfo.elementType, slotInfo.attackType, slotInfo.modificationEffectRarities);
        var nameLocalizeStringEvent = cannonSlots[index].GetComponentInChildren<LocalizeStringEvent>();
        nameLocalizeStringEvent.StringReference.SetReference("UI Text", slotInfo.name);
        chipDetailDisplayController.UpdateInfo(slotInfo);
        
        outfitChangers[index].UpdateOutfit(slotInfo.attackType, slotInfo.elementType);
    }

    public void UpdateAll()
    {
        for (int i = 0; i < cannonSlots.Count; i++)
        {
            UpdateSlot(i);
        }
    }
    
    public void SelectSlot(int index)
    {
        slotSelected = index;
        
        ChipInfo slotInfo = playerDataManager.GetChipInfoFromCannon(index);
        chipDetailDisplayController.UpdateInfo(slotInfo);

        for (int i = 0; i < cannonSlotHandlers.Count; i++)
        {
            if (i == index) continue;
            cannonSlotHandlers[i].Deselect();
        }
    }

    public int GetSlotSelected()
    {
        return slotSelected;
    }
}
