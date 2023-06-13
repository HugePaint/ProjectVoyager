using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using static ChipData;

public class InventoryDisplayController : MonoBehaviour
{
    private int slotSelected;
    public GameObject inventorySlotParent;
    public GameObject inventorySlotPrefab;
    public GameObject noChipMessage;
    public GameObject chipPanelParent;
    private List<GameObject> inventorySlots;
    private List<ChipDetailDisplayController> chipPanelControllers;
    private PlayerDataManager playerDataManager;

    public void Awake()
    {
        Global.MainMenu.inventoryDisplayController = this;
    }

    public void Start()
    {
        playerDataManager = Global.MainMenu.playerDataManager;
        inventorySlots = new List<GameObject>();
        chipPanelControllers = new List<ChipDetailDisplayController>();
        
        noChipMessage.SetActive(0 == playerDataManager.GetTotalItemInInventory());
        for (int i = 0; i < playerDataManager.GetTotalItemInInventory(); i++)
        {
            if (inventorySlotParent.transform.childCount <= i)
            {
                Instantiate(inventorySlotPrefab, inventorySlotParent.transform);
            }
            
            GameObject slot = inventorySlotParent.transform.GetChild(i).gameObject;
            inventorySlots.Add(slot);
            
            InventorySlotSelectHandler handler = slot.GetComponentInChildren<InventorySlotSelectHandler>();
            // handler.chipPanel.transform.SetParent(chipPanelParent.transform);
            handler.chipPanelParent = chipPanelParent;
            chipPanelControllers.Add(handler.chipPanel.GetComponent<ChipDetailDisplayController>());
        }
    }

    public void UpdateSlot(int index)
    {
        ChipInfo slotInfo = playerDataManager.GetChipInfoFromInventory(index);
        var chipSpriteController = inventorySlots[index].GetComponentInChildren<ChipSpriteController>();
        chipSpriteController.UpdateSprite(slotInfo.elementType, slotInfo.attackType, slotInfo.modificationEffectRarities);
        chipPanelControllers[index].UpdateInfo(slotInfo);
    }

    public void UpdateAll()
    {
        noChipMessage.SetActive(0 == playerDataManager.GetTotalItemInInventory());
        chipPanelControllers = new List<ChipDetailDisplayController>();
        
        foreach (var slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots = new List<GameObject>();
        chipPanelControllers = new List<ChipDetailDisplayController>();
        
        for (int i = 0; i < playerDataManager.GetTotalItemInInventory(); i++)
        {
            if (inventorySlots.Count <= i)
            {
                GameObject slot = Instantiate(inventorySlotPrefab, inventorySlotParent.transform);
                inventorySlots.Add(slot);
            }
            
            var handler = inventorySlots[i].GetComponentInChildren<InventorySlotSelectHandler>();
            handler.chipPanelParent = chipPanelParent;
            chipPanelControllers.Add(handler.chipPanel.GetComponent<ChipDetailDisplayController>());
            UpdateSlot(i);
        }

        var scrollPos = inventorySlotParent.transform.localPosition;
        scrollPos.y -= 0.5f * ((RectTransform)inventorySlotParent.transform).rect.height;
        inventorySlotParent.transform.localPosition = scrollPos;
    }

    public void RemoveLast2()
    {
        Destroy(inventorySlots[^1]);
        inventorySlots.RemoveAt(inventorySlots.Count - 1);
        chipPanelControllers.RemoveAt(inventorySlots.Count - 1);
        
        Destroy(inventorySlots[^1]);
        inventorySlots.RemoveAt(inventorySlots.Count - 1);
        chipPanelControllers.RemoveAt(inventorySlots.Count - 1);
    }

    public void SelectSlot(int index)
    {
        slotSelected = index;

        // for (int i = 0; i < inventorySlots.Count; i++)
        // {
        //     if (i == index) continue;
        //     inventorySlots[i].GetComponentInChildren<InventorySlotSelectHandler>().Deselect();
        // }
    }
    
    public int GetSlotSelected()
    {
        return slotSelected;
    }
}
