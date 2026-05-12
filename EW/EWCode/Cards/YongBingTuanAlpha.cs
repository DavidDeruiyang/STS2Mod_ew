using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class YongBingTuanAlpha() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "PL3 03 yong_bing_tuan_α.png";
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWKazdelStrengthPower>(Owner.Creature, IsUpgraded ? 2m : 1m, Owner.Creature, this);
        }
    }
}
