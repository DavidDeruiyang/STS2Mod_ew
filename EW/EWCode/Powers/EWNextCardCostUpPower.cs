using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWNextCardCostUpPower : EWPower
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override Task BeforeCardPlayed(CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner)
            {
                return Task.CompletedTask;
            }

            _ = MegaCrit.Sts2.Core.Commands.PowerCmd.Remove(this);
            return Task.CompletedTask;
        }
    }
}
