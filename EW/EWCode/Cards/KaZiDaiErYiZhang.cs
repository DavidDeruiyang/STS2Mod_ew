using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class KaZiDaiErYiZhang() : EWCard(2, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "PL3 01 ka_zi_dai_er_yi_zhang.png";
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await PowerCmd.Apply<EWKazdelSpeakerPower>(Owner.Creature, IsUpgraded ? 1m : 0m, Owner.Creature, this);
            }
        }
    }
}
