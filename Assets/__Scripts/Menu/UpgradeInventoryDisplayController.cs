using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeInventoryDisplayController : MonoBehaviour
{
    public GameObject inventorySlotParent;
    public GameObject inventorySlotPrefab;
    public GameObject noChipMessage;
    public GameObject chipPanelParent;
    public List<ChipDetailDisplayController> upgradeSlots;
    public List<ChipPreset> upgradeSlotPlaceholders;
    public GameObject upgradeButton;
    // public MainMenuButtonHoverHandler upgradeButtonHover;
    [HideInInspector] public int targetChipSlot = 0;
    public ClosePanelOnClickOutside closePanelController;
        
    private List<GameObject> inventorySlots;
    private List<ChipDetailDisplayController> chipPanelControllers;
    private PlayerDataManager playerDataManager;

    private List<int> slotSelected;
    private List<int> slotPreviouslySelected;

    public void Awake()
    {
        Global.MainMenu.upgradeInventoryDisplayController = this;
    }

    public void Start()
    {
        playerDataManager = Global.MainMenu.playerDataManager;
        inventorySlots = new List<GameObject>();
        chipPanelControllers = new List<ChipDetailDisplayController>();

        for (int i = 0; i < playerDataManager.GetTotalItemInInventory(); i++)
        {
            if (inventorySlotParent.transform.childCount <= i)
            {
                Instantiate(inventorySlotPrefab, inventorySlotParent.transform);
            }
            
            GameObject slot = inventorySlotParent.transform.GetChild(i).gameObject;
            inventorySlots.Add(slot);
            
            UpgradeSlotSelectHandler handler = slot.GetComponentInChildren<UpgradeSlotSelectHandler>();
            // handler.chipPanel.transform.SetParent(chipPanelParent.transform);
            handler.chipPanelParent = chipPanelParent;
            chipPanelControllers.Add(handler.chipPanel.GetComponent<ChipDetailDisplayController>());
        }
    }

    public void UpdateSlot(int index)
    {
        ChipData.ChipInfo slotInfo = playerDataManager.GetChipInfoFromInventory(index);
        var chipSpriteController = inventorySlots[index].GetComponentInChildren<ChipSpriteController>();
        chipSpriteController.UpdateSprite(slotInfo.elementType, slotInfo.attackType, slotInfo.modificationEffectRarities);
        chipPanelControllers[index].UpdateInfo(slotInfo);
    }

    private void UpdateAll()
    {
        noChipMessage.SetActive(0 == playerDataManager.GetTotalItemInInventory());
        chipPanelControllers = new List<ChipDetailDisplayController>();
        noChipMessage.SetActive(0 == playerDataManager.GetTotalItemInInventory());
        
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
            
            var handler = inventorySlots[i].GetComponentInChildren<UpgradeSlotSelectHandler>();
            handler.chipPanelParent = chipPanelParent;
            chipPanelControllers.Add(handler.chipPanel.GetComponent<ChipDetailDisplayController>());
            UpdateSlot(i);
        }
        
        var scrollPos = inventorySlotParent.transform.localPosition;
        scrollPos.y -= 0.5f * ((RectTransform)inventorySlotParent.transform).rect.height;
        inventorySlotParent.transform.localPosition = scrollPos;
    }

    public void InitializeUpgrade()
    {
        if (slotPreviouslySelected != null)
            foreach (int disabledIndex in slotPreviouslySelected)
            {
                if (disabledIndex != -1)
                    inventorySlots[disabledIndex].GetComponent<CanvasGroup>().interactable = true;
            }

        for (var index = 0; index < upgradeSlots.Count; index++)
        {
            var slot = upgradeSlots[index];
            var placeholder = new ChipData.ChipInfo(upgradeSlotPlaceholders[index]);
            slot.UpdateInfo(placeholder);
        }

        slotSelected = new List<int> { -1, -1 };
        slotPreviouslySelected = new List<int> { -1, -1 };
        upgradeButton.SetActive(false);

        UpdateAll();
    }
    
    public void SelectSlot(int slotIndex, int inventoryIndex)
    {
        slotSelected[slotIndex] = inventoryIndex;
        if (slotPreviouslySelected[slotIndex] != -1)
        {
            inventorySlots[slotPreviouslySelected[slotIndex]].GetComponent<CanvasGroup>().interactable = true;
        }
        inventorySlots[inventoryIndex].GetComponent<CanvasGroup>().interactable = false;
        slotPreviouslySelected[slotIndex] = inventoryIndex;

        var chipInfo = playerDataManager.GetChipInfoFromInventory(inventoryIndex);
        upgradeSlots[slotIndex].UpdateInfo(chipInfo);

        upgradeButton.SetActive(true);
        foreach (int index in slotSelected)
        {
            if (index == -1) upgradeButton.SetActive(false);
        }
    }

    public List<int> GetSlotSelected()
    {
        foreach (int slotIndex in slotSelected)
        {
            if (slotIndex == -1) return null;
        }
        return new List<int>(slotSelected);
    }

    public void SetUpgradeSlot(int index)
    {
        targetChipSlot = index;
    }
}
