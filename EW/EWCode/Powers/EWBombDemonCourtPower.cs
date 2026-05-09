using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;

namespace EW.EWCode.Powers
{
    public class EWBombDemonCourtPower : EWPower
    {
        public const string CountdownReductionKey = "CountdownReduction";

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public decimal CountdownReduction => DynamicVars[CountdownReductionKey].BaseValue;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(CountdownReductionKey, 1m)
        ];
    }
}
