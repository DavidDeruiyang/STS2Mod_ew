using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class KaZiDaiErDeXiWang() : EWCard(2, CardType.Power, CardRarity.Rare, TargetType.None)
    {
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
