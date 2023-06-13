using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EffectModifier
{
    public interface IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value);
        public void ApplyInBattle(float difference);
    }
}
