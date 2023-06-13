using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour, IPointerClickHandler
{
    public CanvasGroup panel;
    public Button openButton;

    private void Start()
    {
        // Make sure the panel is not visible at the start
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;

        // Add an onClick listener to the button
        openButton.onClick.AddListener(OpenPanel);
    }

    public void OpenPanel()
    {
        Debug.Log("UIPanelController: Panel Open.");
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }

    public void ClosePanel()
    {
        Debug.Log("UIPanelController: Panel Close.");
        panel.alpha = 0;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UIPanelController: OnPointerClick.");
        // If the panel is active and the click is not on the panel or its children
        if (panel.interactable && !RectTransformUtility.RectangleContainsScreenPoint(panel.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
        {
            ClosePanel();
        }
    }
}