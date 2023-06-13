using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public class PlayerHealth : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            Global.BetweenMenuAndBattle.playerStatsModifier.healthModifier += value;
        }

        public void ApplyInBattle(float difference)
        {
            Global.Battle.playerStatsChange.healthModifier += difference;
        }
    }
}

