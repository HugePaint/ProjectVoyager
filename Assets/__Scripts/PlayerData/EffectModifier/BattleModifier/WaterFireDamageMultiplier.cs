using static Global;

namespace EffectModifier
{
    public class WaterFireDamageMultiplier : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.waterElementReactionDataModifier.overrideFireDamageMultiplier += value / 100f;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.waterElementReactionDataChange.overrideFireDamageMultiplier += difference / 100f;
        }
    }
}

