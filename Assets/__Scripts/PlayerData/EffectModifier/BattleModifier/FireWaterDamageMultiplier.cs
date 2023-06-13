using static Global;

namespace EffectModifier
{
    public class FireWaterDamageMultiplier : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.fireElementReactionDataModifier.overrideWaterDamageMultiplier += value / 100f;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.fireElementReactionDataChange.overrideWaterDamageMultiplier += difference / 100f;
        }
    }
}

