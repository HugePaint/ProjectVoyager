using System;
using System.Collections.Generic;
using EffectModifier;
using UnityEngine;
using static Global.Misc;
using static ChipData;


public class Chip
{
    public ChipInfo info;
    private List<ModificationEffectScriptableObject> modificationEffects;
    
    /// <summary>
    /// Only used in battle. Reset it upon entering the battle.
    /// Range: int[0, 6]
    /// </summary>
    public int upgradeLevel;
    public bool upgradable;
    public int inBattleID;

    public Chip()
    {
        modificationEffects = new List<ModificationEffectScriptableObject>();

        info.name = "NONAMECHIP";
        info.description = "This is a uninitialized Chip with no type.";

        info.modificationEffectIDs = new List<int>();
        info.modificationEffectRarities = new List<Rarity>();
    }
    
    public Chip(int testChipNumber):this()
    {
        SetTestChip(testChipNumber);
    }

    public Chip(ChipInfo newChipInfo)
    {
        info = newChipInfo;
        modificationEffects = new List<ModificationEffectScriptableObject>();
        Initialize();
    }
    
    public void Initialize()
    {
        foreach (var modEffectID in info.modificationEffectIDs)
        {
            var modEffect = ModificationEffectManager.GetModificationEffectByID(modEffectID);
            modificationEffects.Add(modEffect);
        }
    }

    public void ApplyPreGameBattleEffects(int currentCannonIndex)
    {
        //foreach (ModificationEffectScriptableObject modEffect in modificationEffects)
        for (int modEffectIndex = 0; modEffectIndex < modificationEffects.Count; modEffectIndex++)
        {
            List<EffectModifierID> effectModifierIds = modificationEffects[modEffectIndex].effectModifierIDs;
            List<float> values = modificationEffects[modEffectIndex].GetValues((int)info.modificationEffectRarities[modEffectIndex]);
            
            for (int i = 0; i < effectModifierIds.Count; i++)
            {
                Type type = Type.GetType("EffectModifier."+effectModifierIds[i].ToString());
                var modifier = Activator.CreateInstance(type);

                if (modifier is IBattleModifier battleModifier)
                {
                    battleModifier.Apply(currentCannonIndex, values[i]);
                }
            }
        }
    }

    public void EquippedChipPrepare(int id)
    {
        upgradeLevel = 0;
        upgradable = true;
        inBattleID = id;
    }
    
    /// <summary>
    /// Upgrade and apply battle effects to Global.Battle.
    /// </summary>
    /// <returns>current upgradeLevel</returns>
    /// <exception cref="InvalidOperationException">Don't upgrade a level 6 Chip!</exception>
    public int UpgradeInBattle()
    {
        if (upgradable == false)
            throw new InvalidOperationException($"Upgrading a chip with upgradeLevel={upgradeLevel}");
        
        upgradeLevel++;
        if (upgradeLevel >= 6)
            upgradable = false;
        
        for (int modEffectIndex = 0; modEffectIndex < modificationEffects.Count; modEffectIndex++)
        {
            List<EffectModifierID> effectModifierIds = modificationEffects[modEffectIndex].effectModifierIDs;
            List<float> oldValues = modificationEffects[modEffectIndex].GetValues((int)info.modificationEffectRarities[modEffectIndex]);
            List<float> upgradedValues = modificationEffects[modEffectIndex].GetValues(
                (int)info.modificationEffectRarities[modEffectIndex] + 1);
            
            for (int i = 0; i < effectModifierIds.Count; i++)
            {
                Type type = Type.GetType("EffectModifier."+effectModifierIds[i].ToString());
                var modifier = Activator.CreateInstance(type);

                if (modifier is IBattleModifier battleModifier)
                {
                    battleModifier.ApplyInBattle(upgradedValues[i] - oldValues[i]);
                }
            }
        }

        return upgradeLevel;
    }
    
    /*public void ApplySynthesisEffects(SynthesisManager.SynthesisImport synthesisImportStruct, 
        List<EffectModifierID> effectModifierIds, List<float> effectModifierValues)
    {
        for (int i = 0; i < effectModifierIds.Count; i++)
        {
            Type type = Type.GetType("EffectModifier."+effectModifierIds[i].ToString());
            var modifier = Activator.CreateInstance(type);

            if (modifier is ISynthesisModifier synthesisModifier)
                synthesisModifier.Apply(synthesisImportStruct, effectModifierValues[i]);
        }
    }*/

    public ChipInfo GetInfo()
    {
        return info;
    }
    
    public List<string> GetAllModEffectNameKeys()
    {
        List<string> modEffectNameKeys = new List<string>();
        foreach (var modEffects in modificationEffects)
        {
            var key = TextManager.GetModificationEffectNameKey(modEffects.id);
            modEffectNameKeys.Add(key);
        }
        return modEffectNameKeys;
    }
    
    public void SetTestChip(int index)
    {
        var playerHealthUp = ModificationEffectManager.GetModificationEffectByID(100003);
        var playerSpeedUp = ModificationEffectManager.GetModificationEffectByID(100004);
        var attackSpeedUp = ModificationEffectManager.GetModificationEffectByID(100002);
        var attackDamageUp = ModificationEffectManager.GetModificationEffectByID(100001);
        var betterLaser = ModificationEffectManager.GetModificationEffectByID(100005);
        var strongerEnergyAttack = ModificationEffectManager.GetModificationEffectByID(100006);
        
        switch (index)
        {
            case 0:
                info.name = "ChipInfo-Name-Starter";
                info.description = "ChipInfo-Description-Starter";
                info.elementType = ElementType.Fire;
                info.attackType = CannonAttackType.Bullet;
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                break;
            
            case 1:
                info.name = "ChipInfo-Name-Advanced-Light";
                info.description = "ChipInfo-Description-Advanced-Light";
                info.elementType = ElementType.Light;
                info.attackType = CannonAttackType.Bullet;
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Epic);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Legendary);
                
                break;
            
            case 2:
                info.name = "ChipInfo-Name-LightBullet";
                info.description = "ChipInfo-Description-LightBullet";
                info.elementType = ElementType.Light;
                info.attackType = CannonAttackType.Bullet;

                modificationEffects.Add(attackDamageUp);
                info.modificationEffectIDs.Add(attackDamageUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                break;
            
            case 3:
                info.name = "ChipInfo-Name-DarkBullet";
                info.description = "ChipInfo-Description-DarkBullet";
                info.elementType = ElementType.Dark;
                info.attackType = CannonAttackType.Bullet;
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Epic);
                
                modificationEffects.Add(attackDamageUp);
                info.modificationEffectIDs.Add(attackDamageUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(attackDamageUp);
                info.modificationEffectIDs.Add(attackDamageUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);

                break;
            
            case 4:
                info.name = "ChipInfo-Name-FireLaser";
                info.description = "ChipInfo-Description-FireLaser";
                info.elementType = ElementType.Fire;
                info.attackType = CannonAttackType.Laser;

                modificationEffects.Add(betterLaser);
                info.modificationEffectIDs.Add(betterLaser.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(betterLaser);
                info.modificationEffectIDs.Add(betterLaser.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackDamageUp);
                info.modificationEffectIDs.Add(attackDamageUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(attackDamageUp);
                info.modificationEffectIDs.Add(attackDamageUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                break;
            
            case 5:
                info.name = "ChipInfo-Name-WaterEnergy";
                info.description = "ChipInfo-Description-WaterEnergy";
                info.elementType = ElementType.Water;
                info.attackType = CannonAttackType.Energy;
                
                modificationEffects.Add(strongerEnergyAttack);
                info.modificationEffectIDs.Add(strongerEnergyAttack.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                    
                modificationEffects.Add(strongerEnergyAttack);
                info.modificationEffectIDs.Add(strongerEnergyAttack.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                break;
            
            case 6:
                info.name = "ChipInfo-Name-NatureLaser";
                info.description = "ChipInfo-Description-NatureLaser";
                info.elementType = ElementType.Nature;
                info.attackType = CannonAttackType.Laser;
                
                modificationEffects.Add(betterLaser);
                info.modificationEffectIDs.Add(betterLaser.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                    
                modificationEffects.Add(betterLaser);
                info.modificationEffectIDs.Add(betterLaser.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                break;
            
            case 7:
                info.name = "ChipInfo-Name-LightEnergy";
                info.description = "ChipInfo-Description-LightEnergy";
                info.elementType = ElementType.Light;
                info.attackType = CannonAttackType.Energy;
                
                modificationEffects.Add(strongerEnergyAttack);
                info.modificationEffectIDs.Add(strongerEnergyAttack.id);
                info.modificationEffectRarities.Add(Rarity.Epic);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Legendary);
                
                break;

            case 8:
                info.name = "ChipInfo-Name-DarkLaser";
                info.description = "ChipInfo-Description-DarkLaser";
                info.elementType = ElementType.Dark;
                info.attackType = CannonAttackType.Laser;
                
                modificationEffects.Add(betterLaser);
                info.modificationEffectIDs.Add(betterLaser.id);
                info.modificationEffectRarities.Add(Rarity.Epic);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerSpeedUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Common);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Legendary);
                
                break;
            
            case 9:
                info.name = "ChipInfo-Name-Advanced-Water";
                info.description = "ChipInfo-Description-Advanced-Water";
                info.elementType = ElementType.Water;
                info.attackType = CannonAttackType.Bullet;
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Epic);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Legendary);
                
                break;
            
            case 10:
                info.name = "ChipInfo-Name-Advanced-Nature";
                info.description = "ChipInfo-Description-Advanced-Nature";
                info.elementType = ElementType.Nature;
                info.attackType = CannonAttackType.Bullet;
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(attackSpeedUp);
                info.modificationEffectIDs.Add(attackSpeedUp.id);
                info.modificationEffectRarities.Add(Rarity.Uncommon);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Rare);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Epic);
                
                modificationEffects.Add(playerHealthUp);
                info.modificationEffectIDs.Add(playerHealthUp.id);
                info.modificationEffectRarities.Add(Rarity.Legendary);
                
                break;
            
            default:
                break;
        }
    }
}
