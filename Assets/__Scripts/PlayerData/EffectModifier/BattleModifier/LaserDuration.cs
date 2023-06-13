using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public class LaserDuration : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            Global.BetweenMenuAndBattle.playerInitialCannons[targetCannonIndex]
                            .cannonImportInfo.laserDuration += value;
        }

        public void ApplyInBattle(float difference)
        {
            Global.Battle.cannonAttributeChange.cannonImportInfo.laserDuration += difference;
        }
    }
}

