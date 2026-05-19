using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class YongBingTuanBeta() : EWCard(1, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        private const string PowerAmountKey = "PowerAmount";

        protected override string PortraitFileName => "PL3 04 yong_bing_tuan_β.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(PowerAmountKey, 4m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWKazdelDexterityPower>(Owner.Creature, DynamicVars[PowerAmountKey].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade() => DynamicVars[PowerAmountKey].UpgradeValueBy(2m);
    }
}
