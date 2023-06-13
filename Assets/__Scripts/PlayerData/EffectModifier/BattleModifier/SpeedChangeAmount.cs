using static Global;

namespace EffectModifier
{
    public class SpeedChangeAmount : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.waterElementReactionDataModifier.speedChangeAmount += value;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.waterElementReactionDataChange.speedChangeAmount += difference;
        }
    }
}

