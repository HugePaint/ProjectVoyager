using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    public List<WeaponUI> weaponUis;
    private void Awake()
    {
        Global.Battle.weaponUIManager = this;
    }

    private void Start()
    {
        Global.DoTweenWait(0.1f, () =>
        {
            foreach (var cannon in Global.Battle.cannonBattleManager.cannonBattles)
            {
                if (!cannon.active)
                {
                    foreach (var weaponUI in weaponUis)
                    {
                        if (weaponUI.id == cannon.cannonId) weaponUI.UnReady();
                    }
                }
            }
        });
    }

    public void UnlockCannonUIAnimation(int id, Transform startPosition)
    {
        foreach (var weaponUI in weaponUis)
        {
            if (weaponUI.id == id) weaponUI.UnlockChipUIAnimation(startPosition);
        }
    }

    public void UpgradeWeaponUI(int id)
    {
        foreach (var weaponUI in weaponUis)
        {
            if (weaponUI.id == id) weaponUI.PlayerPowerUpParticleSystem();
        }
    }
}
