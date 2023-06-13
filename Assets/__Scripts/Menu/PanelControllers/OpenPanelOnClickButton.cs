using UnityEngine;
using UnityEngine.UI;

public class OpenPanelOnClickButton : MonoBehaviour
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
        Debug.Log("OpenPanelOnClickButton: Panel Open.");
        panel.alpha = 1;
        panel.interactable = true;
        panel.blocksRaycasts = true;
    }
}