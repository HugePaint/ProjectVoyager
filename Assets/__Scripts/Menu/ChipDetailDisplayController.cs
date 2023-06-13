using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using static ChipData;
using static Global.Misc;

public class ChipDetailDisplayController : MonoBehaviour
{
    public GameObject chipDetailPanel;
    public GameObject icon;
    public GameObject chipName;
    public GameObject description;
    public GameObject elementType;
    public GameObject attackType;
    public GameObject effectParent;
    public List<Tuple<int, Rarity>> modEffectIDs;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateInfo(ChipInfo info)
    {
        var chipSpriteController = icon.GetComponentInChildren<ChipSpriteController>();
        if (chipSpriteController != null)
            chipSpriteController.UpdateSprite(info.elementType, info.attackType, info.modificationEffectRarities);

        var imageLocalizeEvent = chipName.GetComponent<LocalizeStringEvent>();
        imageLocalizeEvent.StringReference.SetReference(
            "UI Text", info.name);

        var descriptionLocalizeEvent = description.GetComponent<LocalizeStringEvent>();
        descriptionLocalizeEvent.StringReference.SetReference(
            "UI Text", info.description);

        var elementTypeLocalizeEvent = elementType.GetComponent<LocalizeStringEvent>();
        elementTypeLocalizeEvent.StringReference.SetReference(
            "UI Text", TextManager.GetElementTextKey(info.elementType));
        var elementTypeText = elementType.GetComponent<TMP_Text>();
        switch (info.elementType)
        {
            case Global.Misc.ElementType.Fire:
                elementTypeText.color = Global.Misc.colorData.Fire;
                break;
            case Global.Misc.ElementType.Water:
                elementTypeText.color = Global.Misc.colorData.Water;
                break;
            case Global.Misc.ElementType.Nature:
                elementTypeText.color = Global.Misc.colorData.Nature;
                break;
            case Global.Misc.ElementType.Light:
                elementTypeText.color = Global.Misc.colorData.Light;
                break;
            case Global.Misc.ElementType.Dark:
                elementTypeText.color = Global.Misc.colorData.Dark;
                break;
            case ElementType.None: break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var attackTypeLocalizeEvent = attackType.GetComponent<LocalizeStringEvent>();
        attackTypeLocalizeEvent.StringReference.SetReference(
            "UI Text",
            info.elementType != ElementType.None ? TextManager.GetAttackTypeTextKey(info.attackType) : "None");

        var effectLocalizedStrings = effectParent.GetComponentsInChildren<LocalizeStringEvent>().ToList();
        foreach (var localizedString in effectLocalizedStrings)
        {
            localizedString.StringReference.SetReference("UI Text", "None");
        }

        modEffectIDs = new List<Tuple<int, Rarity>>();
        var effectTexts = effectParent.GetComponentsInChildren<TMP_Text>().ToList();
        for (int i = 0; i < info.modificationEffectIDs.Count; i++)
        {
            var modEffect = ModificationEffectManager.GetModificationEffectByID(info.modificationEffectIDs[i]);
            modEffectIDs.Add(new Tuple<int, Rarity>(modEffect.id, info.modificationEffectRarities[i]));
            // effectTexts[i].text = modEffect.effectName;
            string nameKey = TextManager.GetModificationEffectNameKey(modEffect.id);
            effectLocalizedStrings[i].StringReference.SetReference("UI Text", nameKey);
            
            switch (info.modificationEffectRarities[i])
            {
                case Global.Misc.Rarity.None:
                    effectTexts[i].color = Global.Misc.colorData.None;
                    break;
                case Global.Misc.Rarity.Common:
                    effectTexts[i].color = Global.Misc.colorData.Common;
                    break;
                case Global.Misc.Rarity.Uncommon:
                    effectTexts[i].color = Global.Misc.colorData.Uncommon;
                    break;
                case Global.Misc.Rarity.Rare:
                    effectTexts[i].color = Global.Misc.colorData.Rare;
                    break;
                case Global.Misc.Rarity.Epic:
                    effectTexts[i].color = Global.Misc.colorData.Epic;
                    break;
                case Global.Misc.Rarity.Legendary:
                    effectTexts[i].color = Global.Misc.colorData.Legendary;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
