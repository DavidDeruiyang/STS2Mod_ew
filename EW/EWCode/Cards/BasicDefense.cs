using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class BasicDefense() : EWCard(1, CardType.Skill, CardRarity.Basic, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 00 fang_yu.png";
        protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(6, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            }
        }

        protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
    }
}
