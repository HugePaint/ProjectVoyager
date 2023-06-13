using static Global;

namespace EffectModifier
{
    public class BounceDamage : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.natureElementReactionDataModifier.bounceDamage += value;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.natureElementReactionDataChange.bounceDamage += difference;
        }
    }
}

