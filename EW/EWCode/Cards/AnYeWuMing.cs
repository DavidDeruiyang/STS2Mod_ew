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
    public class AnYeWuMing() : EWCard(0, CardType.Skill, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "SL3 02 an_ye_wu_ming.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard, EWKeywords.Camouflage];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("Camouflage", 1m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await PowerCmd.Apply<EWCamouflagePower>(Owner.Creature, DynamicVars["Camouflage"].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade() => DynamicVars["Camouflage"].UpgradeValueBy(1m);
    }
}
