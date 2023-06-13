using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCannonSlot
{
    private int slotIndex;
    private Chip currentChip;

    public FloatingCannonSlot(int cannonIndex)
    {
        slotIndex = cannonIndex;
        // default chip
        currentChip = new Chip(0);
        // TODO: set currentChip when a save file exists
    }
    
    public Chip SetChip(Chip newChip)
    {
        Chip oldChip = currentChip;
        currentChip = newChip;
        return oldChip;
    }

    public void ApplyBattleEffects()
    {
        currentChip.ApplyPreGameBattleEffects(slotIndex);
    }

    public ChipData.ChipInfo GetCurrentChipInfo()
    {
        return currentChip.GetInfo();
    }

    public Chip GetChip()
    {
        return currentChip;
    }
}
