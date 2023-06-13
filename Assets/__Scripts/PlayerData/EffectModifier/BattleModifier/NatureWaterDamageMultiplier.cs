using static Global;

namespace EffectModifier
{
    public class NatureWaterDamageMultiplier : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.natureElementReactionDataModifier.overrideWaterDamageMultiplier += value / 100f;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.natureElementReactionDataChange.overrideWaterDamageMultiplier += difference / 100f;
        }
    }
}

