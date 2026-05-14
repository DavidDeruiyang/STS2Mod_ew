using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class YongBingTuanAlpha() : EWCard(2, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        private const string PowerAmountKey = "PowerAmount";

        protected override string PortraitFileName => "PL3 03 yong_bing_tuan_α.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(PowerAmountKey, 1m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWKazdelStrengthPower>(Owner.Creature, DynamicVars[PowerAmountKey].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade() => DynamicVars[PowerAmountKey].UpgradeValueBy(1m);
    }
}
