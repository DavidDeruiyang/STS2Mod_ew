using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class DianXiaQiDeMingZi() : EWCard(1, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL3 05 dian_xia_qi_de_ming_zi.png";
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(12, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            }
        }

        protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(4m);
    }
}
