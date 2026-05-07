using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models.Relics;
using System.Threading.Tasks;

namespace EW.EWCode.Relics
{
    public class HLZYRelic : EWRelic
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        public override string PackedIconPath => "res://EW/images/relics/hlzyRelic.png";
        protected override string PackedIconOutlinePath => "res://EW/images/relics/hlzyRelic.png";
        protected override string BigIconPath => "res://EW/images/relics/big/hlzyRelic.png";

        public override Task BeforeCombatStart()
        {
            SummonManager.ResetForCombatStart();

            if (SummonManager.CountHLZY() == 0)
            {
                _ = SummonManager.SummonHLZYWhenReady(SummonSource.Relic, slotIndex: 0);
            }

            return Task.CompletedTask;
        }
    }
}
