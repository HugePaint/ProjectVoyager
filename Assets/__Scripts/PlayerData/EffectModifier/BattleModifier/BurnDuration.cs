using static Global;

namespace EffectModifier
{
    public class BurnDuration : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.fireElementReactionDataModifier.burnDuration += value;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.fireElementReactionDataChange.burnDuration += difference;
        }
    }
}

