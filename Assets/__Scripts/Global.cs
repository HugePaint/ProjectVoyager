using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public static class Global
{
    public struct Battle
    {
        //Data in Battle Scene
        public static BattleData battleData;
        //todo: refactor
        public static BattleDataManager.FireElementReactionData fireElementReactionDataChange;
        public static BattleDataManager.WaterElementReactionData waterElementReactionDataChange;
        public static BattleDataManager.NatureElementReactionData natureElementReactionDataChange;
        public static BattleDataManager.PlayerStatsModifier playerStatsChange;
        public static BattleDataManager.BattleGlobalParameters battleGlobalParametersChange;
        public static BattleDataManager.CannonImport cannonAttributeChange;

        //Components in Battle Scene
        public static PlayerInputController playerInputController;
        public static PlayerBehaviourController playerBehaviourController;
        public static BattleDataManager battleDataManager;
        public static CannonBattleManager cannonBattleManager;
        public static CannonAnimationManager cannonAnimationManager;
        public static AttackPointManager attackPointManager;
        public static BattlePrefabManager battlePrefabManager;
        public static EnemyManager enemyManager;
        public static AttackPointMoveHelperManager attackPointMoveHelperManager;
        public static AudioManager audioManager;
        public static EnemyPrefabManager enemyPrefabManager;
        public static PlayerBattleController playerBattleController;
        public static VoidCenter voidCenter;
        public static GlobalEffectManager globalEffectManager;
        public static BattleUIManager battleUIManager;
        public static ChipGainUIManager chipGainUIManager;
        public static VoidCenterPickUpManager voidCenterPickUpManager;
        public static VoidCenterProgressBar voidCenterProgressBar;
        public static BuffManager buffManager;
        public static BossLocations bossLocations;
        public static List<BattleDataManager.ChipGetting> chipsGettingInTheEnd;
        public static UnlockChipArea unlockChipArea;
        public static WeaponUIManager weaponUIManager;
        public static PowerUpManager powerUpManager;
        public static BattleBgmManager battleBgmManager;
        public static BattleTimer battleTimer;
        public static BattleEnergyDisplay battleEnergyDisplay;
        public static PlayerGetHitController playerGetHitController;

        // for rotation
        public static FakePlayer fakePlayer;

        //environment
        public static float chaos;

        public static List<Chip> chipInCannonsInBattle;

        public static float energyGainWhenGainingChips;

        public static void ApplyPowerUp()
        {
            //fire reactions
            battleData.fireElementReactionData = new BattleDataManager.FireElementReactionData
            {
                overrideWaterDamageMultiplier = battleData.fireElementReactionData.overrideWaterDamageMultiplier + fireElementReactionDataChange.overrideWaterDamageMultiplier,
                overrideNatureDamageMultiplier = battleData.fireElementReactionData.overrideNatureDamageMultiplier + fireElementReactionDataChange.overrideNatureDamageMultiplier,
                burnDamage = battleData.fireElementReactionData.burnDamage + fireElementReactionDataChange.burnDamage,
                burnDuration = battleData.fireElementReactionData.burnDuration + fireElementReactionDataChange.burnDuration,
                burnBuffStackable = battleData.fireElementReactionData.burnBuffStackable || fireElementReactionDataChange.burnBuffStackable,
                stackDamage = battleData.fireElementReactionData.stackDamage + fireElementReactionDataChange.stackDamage
            };

            //water reactions
            battleData.waterElementReactionData = new BattleDataManager.WaterElementReactionData
            {
                overrideFireDamageMultiplier = battleData.waterElementReactionData.overrideFireDamageMultiplier + waterElementReactionDataChange.overrideFireDamageMultiplier,
                overrideNatureDamageMultiplier = battleData.waterElementReactionData.overrideNatureDamageMultiplier + waterElementReactionDataChange.overrideNatureDamageMultiplier,
                speedChangeAmount = battleData.waterElementReactionData.speedChangeAmount + waterElementReactionDataChange.speedChangeAmount,
                slowDuration = battleData.waterElementReactionData.slowDuration + waterElementReactionDataChange.slowDuration
            };
            
            //nature reactions
            battleData.natureElementReactionData = new BattleDataManager.NatureElementReactionData
            {
                overrideWaterDamageMultiplier = battleData.natureElementReactionData.overrideWaterDamageMultiplier + natureElementReactionDataChange.overrideWaterDamageMultiplier,
                overrideFireDamageMultiplier = battleData.natureElementReactionData.overrideFireDamageMultiplier + natureElementReactionDataChange.overrideFireDamageMultiplier,
                bounceDamage = battleData.natureElementReactionData.bounceDamage + natureElementReactionDataChange.bounceDamage,
                bounceTargetNumber = battleData.natureElementReactionData.bounceTargetNumber + natureElementReactionDataChange.bounceTargetNumber,
                bounceRange = battleData.natureElementReactionData.bounceRange + natureElementReactionDataChange.bounceRange
            };

            //playerStats
            playerBattleController.UpdateMaxHealth(playerStatsChange.healthModifier);
            playerBattleController.UpdateDefense(playerStatsChange.defenseModifier);
            playerBehaviourController.UpdateMovementSpeed(playerStatsChange.speedModifier);

            //environment
            EventCenter.GetInstance().EventTrigger(Events.UpdateChaos, battleGlobalParametersChange.chaos);
            ResetPowerUps();

        }

        public static void ResetPowerUps()
        {
            fireElementReactionDataChange = new BattleDataManager.FireElementReactionData
            {
                overrideWaterDamageMultiplier = 0,
                overrideNatureDamageMultiplier = 0,
                burnDamage = 0,
                burnDuration = 0,
                burnBuffStackable = false,
                stackDamage = 0
            };
            waterElementReactionDataChange = new BattleDataManager.WaterElementReactionData
            {
                overrideFireDamageMultiplier = 0,
                overrideNatureDamageMultiplier = 0,
                speedChangeAmount = 0,
                slowDuration = 0
            };
            natureElementReactionDataChange = new BattleDataManager.NatureElementReactionData
            {
                overrideWaterDamageMultiplier = 0,
                overrideFireDamageMultiplier = 0,
                bounceDamage = 0,
                bounceTargetNumber = 0,
                bounceRange = 0
            };
            playerStatsChange = new BattleDataManager.PlayerStatsModifier
            {
                healthModifier = 0,
                defenseModifier = 0,
                speedModifier = 0
            };
            battleGlobalParametersChange = new BattleDataManager.BattleGlobalParameters
            {
                chaos = 0
            };
        }

        public static void ApplyCannonUpgrade(int id)
        {
            foreach (var cannon in cannonBattleManager.cannonBattles)
            {
                if (cannon.cannonId == id)
                {
                    cannon.attack += cannonAttributeChange.cannonImportInfo.attack;
                    cannon.attackCoolDown += cannonAttributeChange.cannonImportInfo.attackCoolDown;
                    cannon.laserDuration += cannonAttributeChange.cannonImportInfo.laserDuration;
                    cannon.energyRange += cannonAttributeChange.cannonImportInfo.energyRange;
                    cannon.Validate();
                }
            }

            ResetCannonUpgrade();
        }

        public static void ResetCannonUpgrade()
        {
            cannonAttributeChange = new BattleDataManager.CannonImport
            {
                cannonId = 0,
                cannonImportInfo = new BattleDataManager.CannonBasicStats
                {
                    cannonAttackType = Misc.CannonAttackType.Bullet,
                    elementType = Misc.ElementType.Fire,
                    attack = 0,
                    attackCoolDown = 0,
                    laserDuration = 0,
                    energyRange = 0
                }
            };
        }

        public static void PauseGame()
        {
            Time.timeScale = 0;
            globalEffectManager.StopWhenPause();
            battleBgmManager.GlobalVolumeDown(0.5f);
        }
        
        public static void UnPauseGame()
        {
            Time.timeScale = 1;
            battleBgmManager.GlobalVolumeBack(0.5f);
        }

    }

    public struct MainMenu
    {
        public static PlayerDataManager playerDataManager;
        public static CannonDisplayController cannonDisplayController;
        public static InventoryDisplayController inventoryDisplayController;
        public static MenuController menuController;

        public static UpgradeInventoryDisplayController upgradeInventoryDisplayController;

        public static MenuBgmManager menuBgmManager;
    }

    public enum Events
    {
        PlayerStartMove,
        PlayerMoving,
        PlayerStopMove,
        PlayerChangeMoveDirectionToFront,
        PlayerChangeMoveDirectionToBack,
        UpdateChaos,
        PlayerGetHit,
        KillAllEnemies,
        Dash,
        
        //boss
        BossAppear,
        BossGetHit,
        
        //game flow
        GameOver,
        GameStart,
        EnterBattleAnimation,
        BattleStart,
        StartSpawningEnemy,
        LoadInitialUI,

        //Menu
        MenuGameStart,
        MenuMainFirstShow,
        MenuMainShow,
        MenuMainHide,
        MenuPreGameShow,
        MenuPreGameHide,
        MenuCannonChipSettingShow,
        MenuCannonChipSettingHide,
        MenuOptionsShow,
        MenuOptionsHide,
        MenuCannonOverviewShow,
        MenuCannonOverviewHide,
        MenuCreditsShow,
        MenuCreditsHide,
        MenuCreditsAccelerate,
        MenuUpgradeShow,
        MenuUpgradeHide,
        MenuUpgradeProceed,
        MenuUpgradeBack,

        //Audio
        PlayAudioBattleBgm,
        PlayAudioSoundEffect,
        PlayMenuSoundEffect,
        FadeOutBgm,
        
        //cannons
        CannonZeroFire,
        CannonOneFire,
        CannonTwoFire,
        CannonThreeFire,
        CannonFourFire,
        CannonFiveFire,
        UnlockCannon
    }

    public struct Misc
    {
        //audio settings
        public static float bgmVolume;
        public static float soundEffectVolume;

        public static ColorData colorData;

        public static string savePath;
        
        public enum CannonAttackType
        {
            Bullet,
            Laser,
            Energy
        }

        public enum ElementType
        {
            Fire,
            Water,
            Nature,
            Light,
            Dark,
            None
        }
        
        public enum EnemyType
        {
            CreepMelee,
            CreepRange,
            Elite,
            Boss
        }

        public enum CannonMode
        {
            Stay,
            Search
        }

        public enum Rarity
        {
            None = -1,       //grey
            Common = 0,     //white
            Uncommon = 1,   //green
            Rare = 2,       //blue
            Epic = 3,       //purple
            Legendary = 4,  //orange
            
            Mythical1 = 5,
            Mythical2 = 6,
            Mythical3 = 7,
            Mythical4 = 8,
            Mythical5 = 9,
            Mythical6 = 10,
            // 1 Poor (gray)
            // 2 Common (white)
            // 3 Uncommon (green)
            // 4 Rare (blue)
            // 5 Epic (purple)
            // 6 Legendary (orange)
            // 7 Artifact (light gold)
            // 8 Heirloom (Blizzard blue)
        }
    }

    public struct BetweenMenuAndBattle
    {
        public static List<BattleDataManager.CannonImport> playerInitialCannons;
        public static bool fromMainMenu;
        public static BattleDataManager.PlayerStatsModifier playerStatsModifier;
        public static BattleDataManager.BattleGlobalParameters battleGlobalParametersModifier;
        public static BattleDataManager.FireElementReactionData fireElementReactionDataModifier;
        public static BattleDataManager.WaterElementReactionData waterElementReactionDataModifier;
        public static BattleDataManager.NatureElementReactionData natureElementReactionDataModifier;

        public static List<Chip> chipInCannons;
    }

    public static Tween DoTweenWait(float time, Action action, bool isIndependent = false)
    {
        float _ = 0;
        var returnTween = DOTween.To(() => _, x => _ = x, 1f, time).OnComplete(() => { action(); }).SetUpdate(isIndependentUpdate:isIndependent);
        return returnTween;
    }
    
    public static Tween DoTweenRepeat(float time, bool actWhenCall, int repeatTime, Action action)
    {
        if (actWhenCall)
        {
            action();
            repeatTime -= 1;
        }

        if (repeatTime == 0) return null;
        float _ = 0;
        var tweenReturn = DOTween.To(() => _, x => _ = x, 1f, time).OnStepComplete(() =>
        {
            action();
        }).SetLoops(repeatTime);
        return tweenReturn;
    }


    public static T GetRandomFromList<T>(List<T> inputList)
    {
        var randomIndex = Random.Range(0, inputList.Count);
        return inputList[randomIndex];
    }

    public static List<T> GetRandomItemsFromList<T>(List<T> inputList, int itemNum)
    {
        var tempList = new List<T>(inputList);
        var returnList = new List<T>();

        if (tempList.Count <= 0) return returnList;

        for (var i = 0; i < itemNum; i++)
        {
            if (tempList.Count <= 0) continue;
            var item = GetRandomFromList(tempList);
            tempList.Remove(item);
            returnList.Add(item);
        }
        return returnList;
    }

    public static float RandomPositiveAndNegativeFromRage(float min, float max)
    {
        var randomDirection = Random.Range(0, 2);
        var returnValue = Random.Range(min, max);
        if (randomDirection == 0)
        {
            return -returnValue;
        }
        return returnValue;
    }

    public static Misc.Rarity GetARandomRarity(float score)
    {
        var randomValue = Random.Range(0, 100f);
        return randomValue switch
        {
            > 95f => Misc.Rarity.Legendary,
            > 85f => Misc.Rarity.Epic,
            > 70f => Misc.Rarity.Rare,
            > 40f => Misc.Rarity.Uncommon,
            _ => Misc.Rarity.Common
        };
    }
    
}