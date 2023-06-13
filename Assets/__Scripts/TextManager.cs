using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using static Global.Misc;

public static class TextManager
{
    private const string ElementKeyPrefix = "Element-";
    private const string AttacktypeKeyPrefix = "Attacktype-";
    
    public static string GetElementTextKey(ElementType type)
    {
        if (type == ElementType.None) return "None";
        return ElementKeyPrefix + type.ToString();
    }
    
    public static string GetAttackTypeTextKey(CannonAttackType type)
    {
        return AttacktypeKeyPrefix + type.ToString();
    }
    
    public static string GetModificationEffectNameKey(int id)
    {
        return $"ModEffect-{id}-Name";
    }
    
    public static string GetModificationEffectDescriptionKey(int id)
    {
        return $"ModEffect-{id}-Description";
    }

    public static void SetModificationEffectDescriptionToLocalizeEvent(
        LocalizeStringEvent localizeStringEvent, int modificationEffectID, Rarity rarity)
    {
        var localizedString = localizeStringEvent.StringReference;
        
        string key = TextManager.GetModificationEffectDescriptionKey(modificationEffectID);
        localizedString.SetReference("UI Text", key);
        
        var modEffect = ModificationEffectManager.GetModificationEffectByID(modificationEffectID);
        var modifierValueList = modEffect.GetValues((int)rarity);
        localizedString.Arguments = new List<object>();
        for (int i = 0; i < modifierValueList.Count; i++)
        {
            localizedString.Arguments.Add(modifierValueList[i]);
        }
        for (int i = modifierValueList.Count - 1; i < 6; i++)
        {
            localizedString.Arguments.Add(0);
        }
        localizedString.RefreshString();
    }
}
