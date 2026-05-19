using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class KaZiDaiErDeXiWang() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "PL3 02 ka_zi_dai_er_de_xi_wang.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await PowerCmd.Apply<EWKazdelHopePower>(Owner.Creature, 1m, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade() => AddKeyword(CardKeyword.Innate);
    }
}
