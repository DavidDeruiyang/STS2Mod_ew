using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class MaFangYu() : EWCard(2, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL3 01 ma_fang_yu.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(14, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            }
        }

        protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    }
}
