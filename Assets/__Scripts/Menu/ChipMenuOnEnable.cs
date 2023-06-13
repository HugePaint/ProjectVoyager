using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipMenuOnEnable : MonoBehaviour
{
    public GameObject selectedCannonSlotOnEnable;
    public GameObject selectIndicator;
    
    public void EnableChipMenu()
    {
        selectIndicator.SetActive(true);
        var handler = selectedCannonSlotOnEnable.transform.GetComponentInChildren<CannonSlotSelectHandler>();
        handler.OnSelect(null);
    }
}
