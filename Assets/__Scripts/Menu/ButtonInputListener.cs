using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonInputListener : MonoBehaviour
{
    public Button buttonToClick; // Assign this in the inspector
    public InputActionReference buttonClickAction; // Assign your Input Action Reference here
    public CanvasGroup[] canvasGroups;
    
    public bool IsButtonInteractable()
    {
        if (!buttonToClick.interactable)
        {
            return false; // The button itself is not interactable
        }

        // Check CanvasGroup components in the parent hierarchy
        foreach (CanvasGroup canvasGroup in canvasGroups)
        {
            if (!canvasGroup.interactable)
            {
                return false; // One of the parent CanvasGroups is not interactable
            }
        }

        return true; // The button is interactable
    }
    
    private void OnEnable()
    {
        buttonClickAction.action.Enable();
        buttonClickAction.action.performed += HandleButtonClickAction;
        canvasGroups = buttonToClick.GetComponentsInParent<CanvasGroup>();
    }

    private void OnDisable()
    {
        buttonClickAction.action.Disable();
        buttonClickAction.action.performed -= HandleButtonClickAction;
    }

    private void HandleButtonClickAction(InputAction.CallbackContext context) {
        if (IsButtonInteractable() == false) return;
        // Simulate a button click
        buttonToClick.onClick.Invoke();
    }
}