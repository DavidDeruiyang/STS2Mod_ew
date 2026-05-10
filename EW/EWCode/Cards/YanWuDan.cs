using BaseLib.Utils;
using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class YanWuDan() : EWCard(1, CardType.Skill, CardRarity.Basic, TargetType.None)
    {
        protected override string PortraitFileName => "SL1 01 yan_wu_dan.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.Camouflage];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay, false);
            await PowerCmd.Apply<EWCamouflagePower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}
