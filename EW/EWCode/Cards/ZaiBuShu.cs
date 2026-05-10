using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZaiBuShu() : EWCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 05 zai_bu_shu.png";

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            await PowerCmd.Apply<EWNextTurnEnergyPower>(Owner.Creature, 3m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}
