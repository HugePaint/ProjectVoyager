using static Global;

namespace EffectModifier
{
    public class WaterNatureDamageMultiplier : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.waterElementReactionDataModifier.overrideNatureDamageMultiplier += value / 100f;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.waterElementReactionDataChange.overrideNatureDamageMultiplier += difference / 100f;
        }
    }
}

