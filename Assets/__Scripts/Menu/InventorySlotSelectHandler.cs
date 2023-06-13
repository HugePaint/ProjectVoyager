using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InventorySlotSelectHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [FormerlySerializedAs("swapButton")] public GameObject chipPanel;
    public CanvasGroup chipPanelCanvasGroup;
    [HideInInspector] public GameObject chipPanelParent; // to make panel not masked by UI
    private Tween fadeTweener;
    private Tween deselectTweener;
    private Vector3 chipPanelLocalPosition;
    
    private void Awake()
    {
        chipPanel.SetActive(true);
        chipPanelCanvasGroup.interactable = false;
        chipPanelCanvasGroup.blocksRaycasts = false;
        chipPanelLocalPosition = chipPanel.transform.localPosition;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        deselectTweener.Kill();
        chipPanel.transform.SetParent(chipPanelParent.transform);
        
        var parent = transform.parent;
        Debug.Log(parent.name + " was selected as sibling " + parent.GetSiblingIndex());
        Global.MainMenu.inventoryDisplayController.SelectSlot(gameObject.transform.parent.GetSiblingIndex());
        ToggleCanvasGroup(true);
    }
    
    // public void Deselect()
    // {
    //     chipPanel.transform.SetParent(transform);
    //     ToggleCanvasGroup(false);
    // }

    private void ToggleCanvasGroup(bool isOn)
    {
        fadeTweener.Kill();
        if (isOn)
            fadeTweener = chipPanelCanvasGroup.DOFade(1f, 0.2f).OnComplete(() =>
            {
                chipPanelCanvasGroup.interactable = true;
                chipPanelCanvasGroup.blocksRaycasts = true;
            });
        else
        {
            fadeTweener = chipPanelCanvasGroup.DOFade(0f, 0.1f).OnComplete(() =>
            {
                chipPanelCanvasGroup.interactable = false;
                chipPanelCanvasGroup.blocksRaycasts = false;
                chipPanel.transform.localPosition = chipPanelLocalPosition;
            });
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        deselectTweener.Kill();
        deselectTweener = Global.DoTweenWait(0.2f, () =>
        {
            chipPanel.transform.SetParent(transform);
            ToggleCanvasGroup(false);
        });
    }
}
