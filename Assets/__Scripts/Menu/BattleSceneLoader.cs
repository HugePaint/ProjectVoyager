using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.GetInstance().AddEventListener(Global.Events.MenuGameStart, EnterBattleScene);
    }

    public struct PreBattleData
    {
        public List<BattleDataManager.CannonImport> playerInitialCannons;
        public BattleDataManager.PlayerStatsModifier playerStatsModifier;
    }
    
    void EnterBattleScene()
    {
        Global.BetweenMenuAndBattle.playerStatsModifier = new BattleDataManager.PlayerStatsModifier();
        Global.BetweenMenuAndBattle.battleGlobalParametersModifier = new BattleDataManager.BattleGlobalParameters();
        Global.BetweenMenuAndBattle.fireElementReactionDataModifier = new BattleDataManager.FireElementReactionData();
        Global.BetweenMenuAndBattle.waterElementReactionDataModifier = new BattleDataManager.WaterElementReactionData();
        Global.BetweenMenuAndBattle.natureElementReactionDataModifier = new BattleDataManager.NatureElementReactionData();
        
        Global.BetweenMenuAndBattle.playerInitialCannons = new List<BattleDataManager.CannonImport>();
        Global.BetweenMenuAndBattle.chipInCannons = Global.MainMenu.playerDataManager.GetAllChipsEquipped();
        for (int i = 0; i < Global.MainMenu.playerDataManager.floatingCannonCount; i++)
        {
            var currentCannon = new BattleDataManager.CannonImport();
            currentCannon.cannonImportInfo = new BattleDataManager.CannonBasicStats();
            var chipInCannon = Global.MainMenu.playerDataManager.GetChipInfoFromCannon(i);
            
            currentCannon.cannonId = i;
            currentCannon.cannonImportInfo.cannonAttackType = chipInCannon.attackType;
            currentCannon.cannonImportInfo.elementType = chipInCannon.elementType;

            BattleDataManager.CannonBasicStats basicStats;
            switch (chipInCannon.attackType)
            {
                case Global.Misc.CannonAttackType.Bullet:
                    basicStats = Global.Battle.battleData.bulletBasicStats;
                    break;
                case Global.Misc.CannonAttackType.Laser:
                    basicStats = Global.Battle.battleData.laserBasicStats;
                    break;
                case Global.Misc.CannonAttackType.Energy:
                    basicStats = Global.Battle.battleData.energyBasicStats;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            currentCannon.cannonImportInfo.attack = basicStats.attack;
            currentCannon.cannonImportInfo.attackCoolDown = basicStats.attackCoolDown;
            currentCannon.cannonImportInfo.laserDuration = basicStats.laserDuration;
            currentCannon.cannonImportInfo.energyRange = basicStats.energyRange;
            
            Global.BetweenMenuAndBattle.playerInitialCannons.Add(currentCannon);
            Global.BetweenMenuAndBattle.chipInCannons[i].EquippedChipPrepare(currentCannon.cannonId);
        }

        Global.MainMenu.playerDataManager.ApplyModificationEffectsOnPreBattleData();
        Global.BetweenMenuAndBattle.fromMainMenu = true;
        
        DOTween.KillAll();
        SceneManager.LoadScene("Battle");
    }
}
