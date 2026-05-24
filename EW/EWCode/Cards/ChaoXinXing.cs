using EW.EWCode.Keywords;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ChaoXinXing() : EWCard(1, CardType.Skill, CardRarity.Ancient, TargetType.None)
    {
        protected override string PortraitFileName => "超新星.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.SoulShadow];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null)
            {
                return;
            }

            for (var i = 0; i < 3; i++)
            {
                await SummonManager.SummonHLZYWhenReady(
                    SummonSource.Card,
                    summoner: Owner.Creature,
                    cardSource: this
                );
            }
        }

        protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    }
}
