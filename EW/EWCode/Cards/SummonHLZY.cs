using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SummonHLZY() : EWCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.None)
    {
        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            _ = SummonManager.SummonHLZYWhenReady(SummonSource.Card);
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
        }
    }
}
