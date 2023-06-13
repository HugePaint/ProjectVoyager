using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public class AttackDamage : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            Global.BetweenMenuAndBattle.playerInitialCannons[targetCannonIndex]
                .cannonImportInfo.attack += value;
        }

        public void ApplyInBattle(float difference)
        {
            Global.Battle.cannonAttributeChange.cannonImportInfo.attack += difference;
        }
    }
}

