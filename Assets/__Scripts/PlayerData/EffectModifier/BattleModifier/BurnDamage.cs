using static Global;

namespace EffectModifier
{
    public class BurnDamage : IBattleModifier
    {
        public void Apply(int targetCannonIndex, float value)
        {
            BetweenMenuAndBattle.fireElementReactionDataModifier.burnDamage += value;
        }

        public void ApplyInBattle(float difference)
        {
            Battle.fireElementReactionDataChange.burnDamage += difference;
        }
    }
}

