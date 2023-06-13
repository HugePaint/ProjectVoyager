using static Global;

namespace EffectModifier
{
    public class SlowDuration : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.waterElementReactionDataModifier.slowDuration += value;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.waterElementReactionDataChange.slowDuration += difference;
        }
    }
}

