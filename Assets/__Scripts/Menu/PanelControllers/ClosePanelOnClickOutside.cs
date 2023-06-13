using UnityEngine;
using UnityEngine.EventSystems;

public class ClosePanelOnClickOutside : MonoBehaviour, IPointerClickHandler
{
    public CanvasGroup panel;

    public void ClosePanel()
    {
        Debug.Log("UIPanelController: Panel Close.");
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ClosePanelOnClickOutside: OnPointerClick.");
        // if (panel.interactable && !RectTransformUtility.RectangleContainsScreenPoint(
        //         panel.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
        // {
        //     ClosePanel();
        // }
        if (panel.interactable && eventData.pointerCurrentRaycast.gameObject == gameObject)
        {
            ClosePanel();
        }
    }
}