using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SanErYi() : EWCard(2, CardType.Power, CardRarity.Uncommon, TargetType.None)
    {
        protected override string PortraitFileName => "PL2 02 3_2_1.png";
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar(EWThreeTwoOnePower.DamageBonusKey, 4m)
        ];

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            var power = await PowerCmd.Apply<EWThreeTwoOnePower>(owner, 1m, owner, this);
            if (power != null)
            {
                power.DynamicVars[EWThreeTwoOnePower.DamageBonusKey].BaseValue =
                    DynamicVars[EWThreeTwoOnePower.DamageBonusKey].BaseValue;
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars[EWThreeTwoOnePower.DamageBonusKey].UpgradeValueBy(2m);
        }
    }
}
