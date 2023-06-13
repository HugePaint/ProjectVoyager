using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CannonBattleManager : MonoBehaviour
{
    public CannonBattle[] cannonBattles;

    private void Awake()
    {
        Global.Battle.cannonBattleManager = this;
        cannonBattles = GetComponentsInChildren<CannonBattle>();
    }

    private void Start()
    {
        if (!Global.BetweenMenuAndBattle.fromMainMenu)
        {
            CreateTestCannonData();
        }
        LoadInitCannonsData();
        Global.Battle.battlePrefabManager.BuildPool();
        Global.BetweenMenuAndBattle.fromMainMenu = false;
        foreach (var cannon in Global.Battle.cannonBattleManager.cannonBattles)
        {
            cannon.cannonOutfitChanger.UpdateOutfit(cannon.attackType, cannon.elementType);
        }

        if (Global.Battle.battleData.isTesting)
        {
            for (var i = 0; i < cannonBattles.Length; i++)
            {
                cannonBattles[i].active = Global.Battle.battleData.cannonTestActiveList[i];
            }
        }
        else
        {
            for (var i = 0; i < cannonBattles.Length; i++)
            {
                cannonBattles[i].active = false;
            }

            cannonBattles[0].active = true;
        }
    }

    public void LoadInitCannonsData()
    {
        AssignCannonId();
        foreach (var playerInitialCannon in Global.BetweenMenuAndBattle.playerInitialCannons)
        {
            cannonBattles[playerInitialCannon.cannonId].LoadInitData(playerInitialCannon);
        }
    }

    public void AssignCannonId()
    {
        for (var index = 0; index < cannonBattles.Length; index++)
        {
            var cannonBattle = cannonBattles[index];
            cannonBattle.InitSelf(index);
        }
    }
    
    private void CreateTestCannonData()
    {

        Global.BetweenMenuAndBattle.playerInitialCannons = Global.Battle.battleData.testCannons;
    }

    public void UnlockCannon(int id)
    {
        foreach (var cannonBattle in cannonBattles)
        {
            if (cannonBattle.cannonId == id)
            {
                cannonBattle.active = true;
                cannonBattle.TryStayAttack();
            }
        }
        EventCenter.GetInstance().EventTrigger(Global.Events.UnlockCannon);
    }

    public void TryAttack()
    {
        foreach (var cannonBattle in cannonBattles)
        {
            cannonBattle.TryStayAttack();
        }
    }

    public void GenerateInitialCoolDownForBetterAnimation()
    {
        foreach (var cannonBattle in cannonBattles)
        {
            if (!cannonBattle.inCoolDown)
            {
                var coolDown = Random.Range(0.2f, 1.5f);
                cannonBattle.inCoolDown = true;
                Global.DoTweenWait(coolDown, () =>
                {
                    cannonBattle.inCoolDown = false;
                    cannonBattle.TryStayAttack();
                });
            }
        }
    }

    public List<CannonBattle> GetLockedCannons()
    {
        var listReturn = new List<CannonBattle>();
        foreach (var cannon in cannonBattles)
        {
            if (!cannon.active) listReturn.Add(cannon);
        }
        return listReturn;
    }
    
    public List<CannonBattle> GetUnLockedCannons()
    {
        var listReturn = new List<CannonBattle>();
        foreach (var cannon in cannonBattles)
        {
            if (cannon.active) listReturn.Add(cannon);
        }
        return listReturn;
    }

    public void DisableAllCannon()
    {
        foreach (var cannon in cannonBattles)
        {
            cannon.active = false;
        }
    }

    public Global.Misc.ElementType GetCannonElementById(int id)
    {
        foreach (var cannon in cannonBattles)
        {
            if (cannon.cannonId == id)
            {
                return cannon.elementType;
            }
        }
        return Global.Misc.ElementType.Fire;
    }

    public List<Global.Misc.ElementType> GetAllCannonElements()
    {
        var returnList = new List<Global.Misc.ElementType>();
        foreach (var cannon in cannonBattles)
        {
            if (!returnList.Contains(cannon.elementType))
            {
                returnList.Add(cannon.elementType);
            }
        }
        return returnList;
    }
}