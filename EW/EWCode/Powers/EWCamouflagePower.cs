using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace EW.EWCode.Powers
{
    public class EWCamouflagePower : EWPower
    {
        public const int MaxStacks = 10;
        private const decimal DamageReductionPerStack = 0.1m;

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override string CustomPackedIconPath => "res://EW/images/powers/micai.png";
        public override string CustomBigIconPath => "res://EW/images/powers/big/micai.png";

        public override bool TryModifyPowerAmountReceived(
            PowerModel canonicalPower,
            Creature target,
            decimal amount,
            Creature? applier,
            out decimal modifiedAmount
        )
        {
            if (canonicalPower is not EWCamouflagePower || target != Owner)
            {
                modifiedAmount = amount;
                return false;
            }

            if (amount <= 0m)
            {
                modifiedAmount = amount;
                return false;
            }

            modifiedAmount = decimal.Max(0m, decimal.Min(amount, MaxStacks - Amount));
            return true;
        }

        public override decimal ModifyDamageMultiplicative(
            Creature? target,
            decimal amount,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource
        )
        {
            if (target != Owner || amount <= 0)
            {
                return 1m;
            }

            var reduction = decimal.Min(Amount, MaxStacks) * DamageReductionPerStack;
            return decimal.Max(0m, 1m - reduction);
        }

    }
}
