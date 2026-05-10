using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class LinShiYanTi() : EWCard(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 02 lin_shi_yan_ti.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(16, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            await PowerCmd.Apply<EWNextCardCostUpPower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
