using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class HunLingBiYou() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "PL4 03 hun_lin_bi_you.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWSoulBlessingPower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade() => AddKeyword(CardKeyword.Retain);
    }
}
