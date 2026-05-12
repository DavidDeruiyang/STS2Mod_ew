using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class TouZhiShou() : EWCard(1, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "PL4 02 tou_zhi_shou.png";
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWHLZYSplashPower>(Owner.Creature, IsUpgraded ? 60m : 50m, Owner.Creature, this);
        }
    }
}
