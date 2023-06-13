using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private void Awake()
    {
        Global.Battle.buffManager = this;
    }

    public void Burn(Enemy enemy)
    {
        if (Global.Battle.battleData.fireElementReactionData.burnDamage <= 0f) return;
        var newBuff = new BuffBurn();
        newBuff.ApplyBuff(enemy);
    }
    
    public void WaterSlow(Enemy enemy)
    {
        if (Global.Battle.battleData.waterElementReactionData.speedChangeAmount >= 0f) return;
        var newBuff = new WaterSlowBuff();
        newBuff.ApplyBuff(enemy);
    }

    public void SpeedUp(Enemy enemy)
    {
        var newBuff = new SpeedUpBuff();
        newBuff.ApplyBuff(enemy);
    }
}
