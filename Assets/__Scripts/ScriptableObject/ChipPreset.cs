using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global.Misc;
using static ChipData;

[CreateAssetMenu(fileName = "ChipPreset", menuName = "ScriptableObjects/ChipPreset")]

public class ChipPreset : ScriptableObject
{
    public string nameKey;
    public string descriptionKey;
    public ElementType elementType;
    public CannonAttackType attackType;
    public List<modEffect> modificationEffects;
    
    [System.Serializable]
    public class modEffect
    {
        public int id;
        public Rarity rarity;
    }
}
