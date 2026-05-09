using EW.EWCode.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;

namespace EW.EWCode.Powers
{
    public class EWThreeTwoOnePower : EWPower
    {
        public const string DamageBonusKey = "DamageBonus";

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public decimal DamageBonus => DynamicVars[DamageBonusKey].BaseValue;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(DamageBonusKey, 4m)
        ];
    }
}
