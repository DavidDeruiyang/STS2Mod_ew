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
    public class BiJiBen() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        private const string EnergyKey = "Energy";

        protected override string PortraitFileName => "PL3 05 bi_ji_ben.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.KazdelCard];


        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(EnergyKey, 1m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWNotebookPower>(Owner.Creature, DynamicVars[EnergyKey].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade() => DynamicVars[EnergyKey].UpgradeValueBy(1m);
    }
}
