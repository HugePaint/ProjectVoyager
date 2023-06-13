using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonChipSettingMenuPreviousNextHandler : MonoBehaviour
{
    public Button previousButton;
    public Button nextButton;
    public int currentIndex;
    private CannonChipSettingMenuAnimator cannonChipSettingMenuAnimator;
    private int totalSlots;
    private MainMenuButtonHoverHandler previousHoverHandler;
    private MainMenuButtonHoverHandler nextHoverHandler;

    private void Awake()
    {
        cannonChipSettingMenuAnimator = GetComponent<CannonChipSettingMenuAnimator>();
        previousHoverHandler = previousButton.GetComponent<MainMenuButtonHoverHandler>();
        nextHoverHandler = nextButton.GetComponent<MainMenuButtonHoverHandler>();
    }

    public void Initialize(int index)
    {
        totalSlots = Global.MainMenu.cannonDisplayController.cannonSlots.Count;
        currentIndex = index;

        previousButton.interactable = (index > 0);
        previousHoverHandler.hoverEnabled = (index > 0);
        nextButton.interactable = (index < totalSlots - 1);
        nextHoverHandler.hoverEnabled = (index < totalSlots - 1);
    }
    
    public void OnClickPrevious()
    {
        Initialize(currentIndex - 1);
        Global.MainMenu.cannonDisplayController.SelectSlot(currentIndex);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonChipSettingShow, currentIndex);
    }

    public void OnClickNext()
    {
        Initialize(currentIndex + 1);
        Global.MainMenu.cannonDisplayController.SelectSlot(currentIndex);
        EventCenter.GetInstance().EventTrigger(Global.Events.MenuCannonChipSettingShow, currentIndex);
    }
}
