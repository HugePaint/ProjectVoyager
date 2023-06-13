using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public class AttackCooldown : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            Global.BetweenMenuAndBattle.playerInitialCannons[targetCannonIndex]
                .cannonImportInfo.attackCoolDown += value;
        }

        public void ApplyInBattle(float difference)
        {
            Global.Battle.cannonAttributeChange.cannonImportInfo.attackCoolDown += difference;
        }
    }
}

