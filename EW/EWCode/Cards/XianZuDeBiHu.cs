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
    public class XianZuDeBiHu() : EWCard(2, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        private const string GuardBlockKey = "GuardBlock";

        protected override string PortraitFileName => "PL4 05 xian_zu_de_bi_hu.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(GuardBlockKey, 6m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWAncestorGuardPower>(Owner.Creature, DynamicVars[GuardBlockKey].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade() => DynamicVars[GuardBlockKey].UpgradeValueBy(2m);
    }
}
