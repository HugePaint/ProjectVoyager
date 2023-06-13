using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static Global.Misc;
using static ChipData;

[CreateAssetMenu(fileName = "ModificationEffectScriptableObject", menuName = "ScriptableObjects/ModificationEffectScriptableObject")]
public class ModificationEffectScriptableObject: ScriptableObject
{
    [Header("Identifiers")]
    [Tooltip("IMPORTANT: ID should be the only identifier for a Mod Effect.")]
    public int id;
    [Tooltip("This effectName is used to be an identifier. The display of name and description is handled by Localization Text Table.")]
    public string effectName;
    [Tooltip("Not used as an identifier or in anywhere of the game.")]
    [FormerlySerializedAs("description")] public string memo;
    
    [Header("Type and Modifiers")]
    public ModificationEffectType effectType;
    public List<ElementType> requiredElementTypes;
    public List<CannonAttackType> requiredAttackTypes;
    public List<EffectModifierID> effectModifierIDs;
    public List<ValuesByRarity> effectModifierValuesByRank;
    
    [System.Serializable]
    public class ValuesByRarity
    {
        public List<float> value;
    }

    public List<float> GetValues(int rarity)
    {
        return effectModifierValuesByRank[rarity].value;
    }

    void OnValidate()
    {
        //
        // {
        //     Debug.LogWarning("Make sure the counts of EffectModifiers match!!");
        // }
    }
}
