using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global.MainMenu;

public class ButtonSetChipToUpgradeSlot : MonoBehaviour
{
    public UpgradeSlotSelectHandler upgradeSlotSelectHandler;
    public void SetChipOnPressed()
    {
        upgradeInventoryDisplayController.SelectSlot(
            upgradeInventoryDisplayController.targetChipSlot, upgradeSlotSelectHandler.slotIndex);
        upgradeInventoryDisplayController.closePanelController.ClosePanel();
    }
}
