using static Global;

namespace EffectModifier
{
    public class FireNatureDamageMultiplier : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.fireElementReactionDataModifier.overrideNatureDamageMultiplier += value / 100f;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.fireElementReactionDataChange.overrideNatureDamageMultiplier += difference / 100f;
        }
    }
}

