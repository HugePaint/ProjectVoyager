using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public class EnergyRange : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            Global.BetweenMenuAndBattle.playerInitialCannons[targetCannonIndex]
                .cannonImportInfo.energyRange += value;
        }
        
        public void ApplyInBattle(float difference)
        {
            Global.Battle.cannonAttributeChange.cannonImportInfo.energyRange += difference;
        }
    }
}

