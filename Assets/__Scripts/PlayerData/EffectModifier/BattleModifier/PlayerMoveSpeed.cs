using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public class PlayerMoveSpeed : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            Global.BetweenMenuAndBattle.playerStatsModifier.speedModifier += value;
        }

        public void ApplyInBattle(float difference)
        {
            Global.Battle.playerStatsChange.speedModifier += difference;
        }
    }
}

