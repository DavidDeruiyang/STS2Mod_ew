using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class DismissHLZY() : EWCard(0, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            SummonManager.DismissOneHLZY();
            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
        }
    }
}
