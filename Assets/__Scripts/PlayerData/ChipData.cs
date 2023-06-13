using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Global.Misc;

public static class ChipData
{
    public enum ModificationEffectType
    {
        Battle = 1,
        Synthesis = 2,
        Shop = 3,
        Placeholder = 999
    }
    public enum EffectModifierID
    {
        PlayerMoveSpeed = 11002,
        PlayerHealth = 11003,
        AttackDamage = 12001,
        AttackCooldown = 12002,
        EnergyRange = 12004,
        LaserDuration = 12003,
        SpeedChangeAmount = 10001,
        SlowDuration = 10002,
        BounceRange = 10007,
        BounceDamage = 10008,
        BurnDamage = 10014,
        BurnDuration = 10015,
        FireNatureDamageMultiplier = 10023,
        FireWaterDamageMultiplier = 10024,
        NatureWaterDamageMultiplier = 10025,
        NatureFireDamageMultiplier = 10026,
        WaterFireDamageMultiplier = 10027,
        WaterNatureDamageMultiplier = 10028
    }

    [Serializable]
    public struct ChipInfo
    {
        public string name;
        public string description;
        public ElementType elementType;
        public CannonAttackType attackType;
        public List<int> modificationEffectIDs;
        public List<Rarity> modificationEffectRarities;
        
        public ChipInfo(ChipPreset preset)
        {
            name = preset.nameKey;
            description = preset.descriptionKey;
            elementType = preset.elementType;
            attackType = preset.attackType;
            modificationEffectIDs = new List<int>();
            modificationEffectRarities = new List<Rarity>();
            foreach (var modEffect in preset.modificationEffects)
            {
                modificationEffectIDs.Add(modEffect.id);
                modificationEffectRarities.Add(modEffect.rarity);
            }
        }
    }
}
