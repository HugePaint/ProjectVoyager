using System.Collections.Generic;
using UnityEngine;
using EffectModifier;
using static Global.Misc;
using static ChipData;

public static class ModificationEffectManager
{
    private static readonly Dictionary<string, ModificationEffectScriptableObject> ModEffectDictByName;
    private static readonly Dictionary<int, ModificationEffectScriptableObject> ModEffectDictByID;
    private static readonly List<ModificationEffectScriptableObject> ModEffectList;

    private static Dictionary<CannonAttackType, List<ModificationEffectScriptableObject>> attackTypeExclusiveModEffect;
    private static Dictionary<ElementType, List<ModificationEffectScriptableObject>> elementTypeExclusiveModEffect;
    private static List<ModificationEffectScriptableObject> attackTypeGenericModEffect;
    private static List<ModificationEffectScriptableObject> elementTypeGenericModEffect;
    private static List<ModificationEffectScriptableObject> allGenericModEffect;

    static ModificationEffectManager()
    {
        Object[] allModEffects = Resources.LoadAll($"", typeof(ModificationEffectScriptableObject));

        ModEffectDictByName = new Dictionary<string, ModificationEffectScriptableObject>();
        ModEffectDictByID = new Dictionary<int, ModificationEffectScriptableObject>();
        ModEffectList = new List<ModificationEffectScriptableObject>();
        
        attackTypeExclusiveModEffect = new Dictionary<CannonAttackType, List<ModificationEffectScriptableObject>>();
        attackTypeGenericModEffect = new List<ModificationEffectScriptableObject>();
        foreach(CannonAttackType attackType in System.Enum.GetValues(typeof(CannonAttackType)))
        {
            attackTypeExclusiveModEffect.Add(attackType, new List<ModificationEffectScriptableObject>());
        }
        
        elementTypeExclusiveModEffect = new Dictionary<ElementType, List<ModificationEffectScriptableObject>>();
        elementTypeGenericModEffect = new List<ModificationEffectScriptableObject>();
        foreach(ElementType elementType in System.Enum.GetValues(typeof(ElementType)))
        {
            elementTypeExclusiveModEffect.Add(elementType, new List<ModificationEffectScriptableObject>());
        }

        allGenericModEffect = new List<ModificationEffectScriptableObject>();
        
        foreach (var o in allModEffects)
        {
            var modEffect = (ModificationEffectScriptableObject)o;
            ModEffectDictByName.Add(modEffect.effectName, modEffect);
            ModEffectDictByID.Add(modEffect.id, modEffect);
            ModEffectList.Add(modEffect);

            if (modEffect.effectType == ModificationEffectType.Placeholder) return;
            if (modEffect.requiredAttackTypes.Count == 0)
            {
                attackTypeGenericModEffect.Add(modEffect);
            }
            else
            {
                foreach (var attackType in modEffect.requiredAttackTypes)
                {
                    attackTypeExclusiveModEffect[attackType].Add(modEffect);
                }
            }

            if (modEffect.requiredElementTypes.Count == 0)
            {
                elementTypeGenericModEffect.Add(modEffect);
            } 
            else
            {
                foreach (var elementType in modEffect.requiredElementTypes)
                {
                    elementTypeExclusiveModEffect[elementType].Add(modEffect);
                }
            }

            if ((modEffect.requiredElementTypes.Count == 0) && (modEffect.requiredAttackTypes.Count == 0))
            {
                allGenericModEffect.Add(modEffect);
            }
        }
    }

    public static ModificationEffectScriptableObject GetModificationEffectByName(string effectName)
    {
        return ModEffectDictByName[effectName];
    }
    
    public static ModificationEffectScriptableObject GetModificationEffectByID(int id)
    {
        return ModEffectDictByID[id];
    }

    public static List<ModificationEffectScriptableObject> GetAll()
    {
        return ModEffectList;
    }

    public static List<ModificationEffectScriptableObject> GetAll(ElementType elementType, CannonAttackType attackType)
    {
        var currentPool = new List<ModificationEffectScriptableObject>();
        foreach (var modEffect in ModEffectList)
        {
            if (modEffect.effectType == ModificationEffectType.Placeholder) continue;
            
            // exclusive
            if (modEffect.requiredAttackTypes.Contains(attackType)
                && modEffect.requiredElementTypes.Contains(elementType))
                currentPool.Add(modEffect);

            if (modEffect.requiredAttackTypes.Contains(attackType)
                && modEffect.requiredElementTypes.Count == 0)
                currentPool.Add(modEffect);
            
            if (modEffect.requiredAttackTypes.Count == 0
                && modEffect.requiredElementTypes.Contains(elementType))
                currentPool.Add(modEffect);
            
            
            // generic
            if (modEffect.requiredAttackTypes.Count == 0
                && modEffect.requiredElementTypes.Count == 0)
            {
                currentPool.Add(modEffect);
            }
        }
        
        return currentPool;
    }
    
    
    public static ModificationEffectScriptableObject GetRandomFromList(List<ModificationEffectScriptableObject> list)
    {
        int index = Random.Range(0, list.Count);
        return list[index];
    }
    
    /// <summary>
    /// Get a random Modification Effect from all loaded scriptable objects.
    /// It may return a laser effect for a bullet cannon.
    /// </summary>
    /// <returns>A random Mod Effect.</returns>
    public static ModificationEffectScriptableObject GetRandom()
    {
        return GetRandomFromList(ModEffectList);
    }
    
    /// <summary>
    /// Get a random Modification Effect which is only suitable for a given attack type
    /// and no requirement for element type.
    /// Generic Mod Effect (don't require a specific attack type) may be returned.
    /// </summary>
    /// <param name="attackType">Cannon Attack Type</param>
    /// <returns>A random Mod Effect for a given Cannon Attack Type.</returns>
    public static ModificationEffectScriptableObject GetRandom(CannonAttackType attackType)
    {
        var currentPool = new List<ModificationEffectScriptableObject>();
        foreach (var modEffect in attackTypeExclusiveModEffect[attackType])
        {
            if (modEffect.requiredElementTypes.Count == 0)
                currentPool.Add(modEffect);
        }
        currentPool.AddRange(allGenericModEffect);
        return GetRandomFromList(currentPool);
    }
    
    /// <summary>
    /// Get a random Modification Effect which is only suitable for a given element type
    /// and no requirement for attack type.
    /// Generic Mod Effect (don't require a specific element) may be returned.
    /// </summary>
    /// <param name="elementType">Element Type</param>
    /// <returns>A random Mod Effect for a given Element Type.</returns>
    public static ModificationEffectScriptableObject GetRandom(ElementType elementType)
    {
        var currentPool = new List<ModificationEffectScriptableObject>();
        foreach (var modEffect in elementTypeExclusiveModEffect[elementType])
        {
            if (modEffect.requiredAttackTypes.Count == 0)
                currentPool.Add(modEffect);
        }
        currentPool.AddRange(allGenericModEffect);
        return GetRandomFromList(currentPool);
    }
    
    public static ModificationEffectScriptableObject GetRandom(ElementType elementType, CannonAttackType attackType)
    {
        var currentPool = GetAll(elementType, attackType);
        return GetRandomFromList(currentPool);
    }

    public static ModificationEffectScriptableObject GetRandomExclusive(CannonAttackType attackType)
    {
        var currentPool = new List<ModificationEffectScriptableObject>();
        foreach (var modEffect in attackTypeExclusiveModEffect[attackType])
        {
            if (modEffect.requiredElementTypes.Count == 0)
                currentPool.Add(modEffect);
        }
        return GetRandomFromList(currentPool);
    }
    
    public static ModificationEffectScriptableObject GetRandomExclusive(ElementType elementType)
    {
        var currentPool = new List<ModificationEffectScriptableObject>();
        foreach (var modEffect in elementTypeExclusiveModEffect[elementType])
        {
            if (modEffect.requiredAttackTypes.Count == 0)
                currentPool.Add(modEffect);
        }
        return GetRandomFromList(currentPool);
    }

    public static void ApplySingleModificationEffectInBattle(ModificationEffectScriptableObject modEffect, Rarity rarity)
    {
        List<EffectModifierID> effectModifierIds = modEffect.effectModifierIDs;
        bool isNewEffect = false;
        List<float> oldValues = new List<float>();
        if ((int)rarity - 1 < 0)
            isNewEffect = true;
        else
            oldValues = modEffect.effectModifierValuesByRank[(int)rarity - 1].value;

        List<float> newValues = modEffect.effectModifierValuesByRank[(int)rarity].value;
            
        for (int i = 0; i < effectModifierIds.Count; i++)
        {
            System.Type type = System.Type.GetType("EffectModifier."+effectModifierIds[i].ToString());
            var modifier = System.Activator.CreateInstance(type);
            
            if (modifier is IBattleModifier battleModifier)
            {
                if (isNewEffect)
                    battleModifier.ApplyInBattle(newValues[i]);
                else
                    battleModifier.ApplyInBattle(newValues[i] - oldValues[i]);
            }
        }
    }
    
    public static void ApplyNewModificationEffectInBattle(ModificationEffectScriptableObject modEffect, Rarity rarity)
    {
        List<EffectModifierID> effectModifierIds = modEffect.effectModifierIDs;
        List<float> newValues = modEffect.effectModifierValuesByRank[(int)rarity].value;
            
        for (int i = 0; i < effectModifierIds.Count; i++)
        {
            System.Type type = System.Type.GetType("EffectModifier."+effectModifierIds[i].ToString());
            var modifier = System.Activator.CreateInstance(type);
            
            if (modifier is IBattleModifier battleModifier)
            {
                battleModifier.ApplyInBattle(newValues[i]);
            }
        }
    }
}
