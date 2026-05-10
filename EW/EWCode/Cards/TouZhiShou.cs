using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class TouZhiShou() : EWCard(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWHLZYSplashPower>(Owner.Creature, IsUpgraded ? 60m : 50m, Owner.Creature, this);
        }
    }
}
