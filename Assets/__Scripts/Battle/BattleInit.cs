using DG.Tweening;
using UnityEngine;

public class BattleInit : MonoBehaviour
{
    public BattleData battleData;
    public ColorData colorData;

    private void Awake()
    {
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 240;
        DOTween.Init(useSafeMode: true);
        LoadData();
    }

    private void Start()
    {
        UpdateDataInScene();
        if (!Global.Battle.battleData.isGainingChips)
        {
            //EventCenter.GetInstance().EventTrigger(Global.Events.PlayAudioBattleBgm, GameAudios.AudioName.BattleBgm);
            Global.Battle.battleBgmManager.StartPlayBGM();
        }
        else
        {
            Global.DoTweenWait(2f, () =>
            {
                Global.Battle.battleBgmManager.StartPlayBGMChipGain();
            });
        }
    }

    private void LoadData()
    {
        LoadSettings();
        Global.Battle.ResetPowerUps();
        Global.Battle.ResetCannonUpgrade();
    }

    private void LoadSettings()
    {
        if (Global.Battle.battleData != null && Global.Battle.battleData.isGainingChips) return;
        Global.Battle.battleData = Instantiate(battleData);
        Global.Misc.colorData = colorData;
        Global.Battle.chipInCannonsInBattle = Global.BetweenMenuAndBattle.chipInCannons;
        if (Global.BetweenMenuAndBattle.fromMainMenu)
        {
            Global.Battle.battleData.fireElementReactionData = new BattleDataManager.FireElementReactionData
            {
                overrideWaterDamageMultiplier =
                    Global.Battle.battleData.fireElementReactionData.overrideWaterDamageMultiplier +
                    Global.BetweenMenuAndBattle.fireElementReactionDataModifier.overrideWaterDamageMultiplier,
                overrideNatureDamageMultiplier =
                    Global.Battle.battleData.fireElementReactionData.overrideNatureDamageMultiplier +
                    Global.BetweenMenuAndBattle.fireElementReactionDataModifier.overrideNatureDamageMultiplier,
                burnDamage = Global.Battle.battleData.fireElementReactionData.burnDamage +
                             Global.BetweenMenuAndBattle.fireElementReactionDataModifier.burnDamage,
                burnDuration = Global.Battle.battleData.fireElementReactionData.burnDuration +
                               Global.BetweenMenuAndBattle.fireElementReactionDataModifier.burnDuration,
                burnBuffStackable = Global.Battle.battleData.fireElementReactionData.burnBuffStackable ||
                                    Global.BetweenMenuAndBattle.fireElementReactionDataModifier.burnBuffStackable,
                stackDamage = Global.Battle.battleData.fireElementReactionData.stackDamage +
                              Global.BetweenMenuAndBattle.fireElementReactionDataModifier.stackDamage
            };

            Global.Battle.battleData.waterElementReactionData = new BattleDataManager.WaterElementReactionData
            {
                overrideFireDamageMultiplier =
                    Global.Battle.battleData.waterElementReactionData.overrideFireDamageMultiplier +
                    Global.BetweenMenuAndBattle.waterElementReactionDataModifier.overrideFireDamageMultiplier,
                overrideNatureDamageMultiplier =
                    Global.Battle.battleData.waterElementReactionData.overrideNatureDamageMultiplier +
                    Global.BetweenMenuAndBattle.waterElementReactionDataModifier.overrideNatureDamageMultiplier,
                speedChangeAmount = Global.Battle.battleData.waterElementReactionData.speedChangeAmount +
                             Global.BetweenMenuAndBattle.waterElementReactionDataModifier.speedChangeAmount,
                slowDuration = Global.Battle.battleData.waterElementReactionData.slowDuration +
                               Global.BetweenMenuAndBattle.waterElementReactionDataModifier.slowDuration
            };

            Global.Battle.battleData.natureElementReactionData = new BattleDataManager.NatureElementReactionData
            {
                overrideWaterDamageMultiplier =
                    Global.Battle.battleData.natureElementReactionData.overrideWaterDamageMultiplier +
                    Global.BetweenMenuAndBattle.natureElementReactionDataModifier.overrideWaterDamageMultiplier,
                overrideFireDamageMultiplier =
                    Global.Battle.battleData.natureElementReactionData.overrideFireDamageMultiplier +
                    Global.BetweenMenuAndBattle.natureElementReactionDataModifier.overrideFireDamageMultiplier,
                bounceDamage = Global.Battle.battleData.natureElementReactionData.bounceDamage +
                               Global.BetweenMenuAndBattle.natureElementReactionDataModifier.bounceDamage,
                bounceTargetNumber = Global.Battle.battleData.natureElementReactionData.bounceTargetNumber +
                                     Global.BetweenMenuAndBattle.natureElementReactionDataModifier.bounceTargetNumber,
                bounceRange = Global.Battle.battleData.natureElementReactionData.bounceRange +
                              Global.BetweenMenuAndBattle.natureElementReactionDataModifier.bounceRange
            };
        }
        else
        {
            // Global.Battle.fireElementReactionData = Global.Battle.battleData.fireElementReactionBasicData;
            // Global.Battle.waterElementReactionData = Global.Battle.battleData.waterElementReactionBasicData;
            // Global.Battle.natureElementReactionData = Global.Battle.battleData.natureElementReactionBasicData;
        }
    }

    private void UpdateDataInScene()
    {
        Global.Battle.chaos = 0f;
        EventCenter.GetInstance().EventTrigger(Global.Events.UpdateChaos, battleData.enemySpawnRule.startChaosValue);
        Global.Battle.playerBehaviourController.UpdateMovementSpeed(battleData.playerInitMoveSpeed + Global.BetweenMenuAndBattle.playerStatsModifier.speedModifier);
        Global.Battle.playerBattleController.UpdateHealth(battleData.playerInitHealth + Global.BetweenMenuAndBattle.playerStatsModifier.healthModifier);
        Global.Battle.playerBattleController.UpdateDefense(battleData.playerInitDefense + Global.BetweenMenuAndBattle.playerStatsModifier.defenseModifier);
        if (Global.Battle.battleUIManager) Global.Battle.battleUIManager.playerStatsBarManager.InitSelf(Global.Battle.playerBattleController.health);
        Global.Battle.playerBattleController.maxHealth = Global.Battle.playerBattleController.health;
    }
}