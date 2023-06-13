using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UpgradeSlotSelectHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject chipPanel;
    public CanvasGroup chipPanelCanvasGroup;
    public KeepUIInsideCanvas keepUIInsideCanvas;
    public Button chipButton;
    [HideInInspector] public GameObject chipPanelParent; // to make panel not masked by UI
    [HideInInspector] public int slotIndex = -1;
    private Tween fadeTweener;
    private Tween deselectTweener;
    private Vector3 chipPanelLocalPosition;
    private bool isDragging = false;
    
    private void Awake()
    {
        chipPanel.SetActive(true);
        chipPanelCanvasGroup.interactable = false;
        chipPanelCanvasGroup.blocksRaycasts = false;
        chipPanelLocalPosition = chipPanel.transform.localPosition;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        if (isDragging) return;
        
        deselectTweener.Kill();
        chipPanel.transform.SetParent(chipPanelParent.transform);
        
        var parent = transform.parent;
        Debug.Log("Upgrade Slot:" + parent.name + " was selected as sibling " + parent.GetSiblingIndex());
        slotIndex = (gameObject.transform.parent.GetSiblingIndex());
        keepUIInsideCanvas.KeepInsideCanvas();
        ToggleCanvasGroup(true);
    }
    
    public void Deselect()
    {
        chipPanel.transform.SetParent(transform);
        chipPanelCanvasGroup.interactable = false;
        chipPanelCanvasGroup.blocksRaycasts = false;
    }

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

    public void OnBeginDrag(BaseEventData eventData)
    {
        Debug.Log("UpgradeSlotSelectHandler: OnBeginDrag");
        isDragging = true;
        Deselect();
    }

    public void OnPointerUp(BaseEventData eventData)
    {
        isDragging = false;
    }
}
