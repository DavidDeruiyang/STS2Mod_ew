using EW.EWCode.Keywords;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SummonHLZY() : EWCard(1, CardType.Skill, CardRarity.None, TargetType.None)
    {
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            EWKeywords.SoulShadow,
            EWKeywords.Camouflage
        ];

        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            _ = SummonManager.SummonHLZYWhenReady(
                SummonSource.Card,
                summoner: Owner?.Creature,
                cardSource: this
            );
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
        }
    }
}
