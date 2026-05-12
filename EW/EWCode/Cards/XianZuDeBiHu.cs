using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class XianZuDeBiHu() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "PL4 05 xian_zu_de_bi_hu.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWAncestorGuardPower>(Owner.Creature, IsUpgraded ? 8m : 6m, Owner.Creature, this);
        }
    }
}
