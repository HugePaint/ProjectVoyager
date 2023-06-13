using static Global;

namespace EffectModifier
{
    public class BounceRange : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.natureElementReactionDataModifier.bounceRange += value;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.natureElementReactionDataChange.bounceRange += difference;
        }
    }
}

