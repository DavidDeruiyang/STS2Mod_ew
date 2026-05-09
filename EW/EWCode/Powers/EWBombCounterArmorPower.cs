using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWBombCounterArmorPower : EWPower
    {
        public const string BlockKey = "Block";

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public decimal BlockAmount => DynamicVars[BlockKey].BaseValue;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(BlockKey, 3m)
        ];

        public async Task GainBombBlock(
            PlayerChoiceContext choiceContext,
            CardModel? cardSource,
            Creature? target,
            CardPlay? cardPlay = null
        )
        {
            if (Owner == null || Owner.IsDead)
            {
                return;
            }

            if (cardPlay == null && cardSource != null)
            {
                cardPlay = new CardPlay
                {
                    Card = cardSource,
                    Target = target ?? Owner,
                    ResultPile = PileType.None,
                    Resources = new ResourceInfo
                    {
                        EnergySpent = 0,
                        EnergyValue = 0,
                        StarsSpent = 0,
                        StarValue = 0
                    },
                    IsAutoPlay = true,
                    PlayIndex = 0,
                    PlayCount = 1
                };
            }

            if (cardPlay == null)
            {
                return;
            }

            await CreatureCmd.GainBlock(Owner, BlockAmount, ValueProp.Move, cardPlay, false);
        }
    }
}
