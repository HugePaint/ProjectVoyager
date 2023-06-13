using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ChipUIModificationEffect : MonoBehaviour
{
    public Image background;
    public LocalizeStringEvent localizeStringEvent;
    public TMP_Text tmpText;
    public CanvasGroup canvasGroup;

    public void UpdateInfo(int id, Global.Misc.Rarity rarity)
    {
        switch (rarity)
        {
            case Global.Misc.Rarity.None:
                background.color = Global.Misc.colorData.None;
                break;
            case Global.Misc.Rarity.Common:
                background.color = Global.Misc.colorData.Common;
                break;
            case Global.Misc.Rarity.Uncommon:
                background.color = Global.Misc.colorData.Uncommon;
                break;
            case Global.Misc.Rarity.Rare:
                background.color = Global.Misc.colorData.Rare;
                break;
            case Global.Misc.Rarity.Epic:
                background.color = Global.Misc.colorData.Epic;
                break;
            case Global.Misc.Rarity.Legendary:
                background.color = Global.Misc.colorData.Legendary;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
        }

        if (rarity != Global.Misc.Rarity.None)
        {
            var nameKey = TextManager.GetModificationEffectNameKey(id);
            localizeStringEvent.StringReference.SetReference("UI Text", nameKey);
        }
        else
        {
            tmpText.text = "";
        }
    }
}
