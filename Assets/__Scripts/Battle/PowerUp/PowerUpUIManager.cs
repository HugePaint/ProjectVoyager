using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerUpUIManager : MonoBehaviour
{
    public List<PowerUpCard> powerUpCards;
    public CanvasGroup shadowCanvasGroup;
    public Transform startPosition;
    public Transform endPosition;
    public GameObject clickCover;

    public void Appear()
    {
        clickCover.SetActive(true);
        AssignPowerUps();
        
        shadowCanvasGroup.DOFade(1, 1f).From(0f).SetEase(Ease.Linear).SetUpdate(isIndependentUpdate:true);
        foreach (var powerUpCard in powerUpCards)
        {
            powerUpCard.Appear(startPosition);
        }

        Global.DoTweenWait(1f, () =>
        {
            clickCover.SetActive(false);
            EventSystem.current.SetSelectedGameObject(powerUpCards[0].gameObject);
        }, true);
    }

    public void AssignPowerUps()
    {
        var upgradingChips = new List<int>();
        foreach (var powerUpCard in powerUpCards)
        {
            var unlockedCannons = Global.Battle.cannonBattleManager.GetUnLockedCannons();
            var upgradableCannon = new List<Chip>();
            foreach (var cannon in unlockedCannons)
            {
                foreach (var chip in Global.Battle.chipInCannonsInBattle)
                {
                    if (chip.inBattleID == cannon.cannonId && chip.upgradable && !upgradingChips.Contains(chip.inBattleID)) upgradableCannon.Add(chip);
                }
            }
            var randomPowerUp = Random.Range(-1, 1);
            if (randomPowerUp >= 0 && upgradableCannon.Count > 0)
            {
                var upgradeChip = Global.GetRandomFromList(upgradableCannon);
                var upgradeId = upgradeChip.inBattleID;
                var startLevel = upgradeChip.upgradeLevel;
                powerUpCard.ActiveUpgradeCannonCard(upgradeId, startLevel);
                upgradingChips.Add(upgradeId);
            }
            else
            {
                powerUpCard.ActiveNewEffectCard();
            }
        }

        var lockedCannon = Global.Battle.cannonBattleManager.GetLockedCannons();
        if (lockedCannon.Count > 0 && Global.Battle.unlockChipArea.ReadyForNextChip())
        {
            var smallestId = lockedCannon[0].cannonId;
            foreach (var cannonBattle in lockedCannon)
            {
                if (cannonBattle.cannonId < smallestId)
                {
                    smallestId = cannonBattle.cannonId;
                }
            }
            powerUpCards[2].ActiveUnlockNewCannonCard(smallestId);
        }
    }

    public void CloseAll()
    {
        clickCover.SetActive(true);
        foreach (var powerUpCard in powerUpCards)
        {
            powerUpCard.Close(endPosition);
        }
        
        EventSystem.current.SetSelectedGameObject(null);

        Global.DoTweenWait(1.5f, () =>
        {
            gameObject.SetActive(false);
            Global.Battle.UnPauseGame();
        }, true);
    }


}
