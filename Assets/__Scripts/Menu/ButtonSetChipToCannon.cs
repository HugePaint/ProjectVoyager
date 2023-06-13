using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global.MainMenu;

public class ButtonSetChipToCannon : MonoBehaviour
{
    public void SetChipOnPressed()
    {
        int inventorySlotIndex = inventoryDisplayController.GetSlotSelected();
        int cannonSlotIndex = cannonDisplayController.GetSlotSelected();
        
        playerDataManager.SetChipToCannon(inventorySlotIndex, cannonSlotIndex);
        inventoryDisplayController.UpdateSlot(inventorySlotIndex);
        cannonDisplayController.UpdateSlot(cannonSlotIndex);
    }
}
