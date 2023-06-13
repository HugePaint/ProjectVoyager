using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ModificationEffectTooltipHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public int slotIndex;
    public List<ModificationEffectTooltipHoverHandler> tooltipHandlers;
    public Vector3 tooltipPositionOffset;
    public ChipDetailDisplayController chipDetailDisplayController;

    public bool isPointerInside = false;
    private LocalizeStringEvent localizedStringEvent;
    
    void Awake()
    {
        tooltip.SetActive(false);
        localizedStringEvent = tooltip.GetComponentInChildren<LocalizeStringEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPointerInside) return;
        Vector3 position = Mouse.current.position.ReadValue();
        position += tooltipPositionOffset;
        tooltip.transform.position = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotIndex >= chipDetailDisplayController.modEffectIDs.Count) return;
        
        isPointerInside = true;
        tooltip.SetActive(true);
        
        var idRarityTuple = chipDetailDisplayController.modEffectIDs[slotIndex];
        TextManager.SetModificationEffectDescriptionToLocalizeEvent(
            localizedStringEvent, idRarityTuple.Item1, idRarityTuple.Item2);
        
        Debug.Log("Cursor Entering " + name + " GameObject");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPointerInside)
        {
            isPointerInside = false;
            tooltip.SetActive(false);
            Debug.Log("Cursor Exiting " + name + " GameObject");
        }
    }
}
