using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private void Awake()
    {
        Global.Battle.powerUpManager = this;
    }

    public void ShowPowerUpUI()
    {
        Global.Battle.battleUIManager.powerUpUIManager.gameObject.SetActive(true);
        Global.Battle.battleUIManager.powerUpUIManager.Appear();
        Global.Battle.PauseGame();
    }

    public void CannonPowerUp(int id)
    {
        foreach (var chip in Global.Battle.chipInCannonsInBattle)
        {
            if (chip.inBattleID == id)chip.UpgradeInBattle();
        }

        Global.Battle.ApplyPowerUp();
        Global.Battle.ApplyCannonUpgrade(id);
        Global.DoTweenWait(1.5f, () =>
        {
            Global.Battle.weaponUIManager.UpgradeWeaponUI(id);
        }, true);
    }

    public void ApplyNewEffect(ModificationEffectScriptableObject modificationEffectScriptableObject, Global.Misc.Rarity rarity)
    {
        ModificationEffectManager.ApplyNewModificationEffectInBattle(modificationEffectScriptableObject, rarity);
        var unlockedCannon = Global.Battle.cannonBattleManager.GetUnLockedCannons();
        var powerUpCannon = Global.GetRandomFromList(unlockedCannon);
        Global.Battle.ApplyPowerUp();
        Global.Battle.ApplyCannonUpgrade(powerUpCannon.cannonId);
    }
}
