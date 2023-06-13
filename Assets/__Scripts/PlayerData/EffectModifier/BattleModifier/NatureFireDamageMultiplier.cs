using static Global;

namespace EffectModifier
{
    public class NatureFireDamageMultiplier : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.natureElementReactionDataModifier.overrideFireDamageMultiplier += value / 100f;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.natureElementReactionDataChange.overrideFireDamageMultiplier += difference / 100f;
        }
    }
}

