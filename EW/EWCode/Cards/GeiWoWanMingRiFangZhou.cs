using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class GeiWoWanMingRiFangZhou() : EWCard(1, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        private const string HealPercentKey = "HealPercent";

        protected override string PortraitFileName => "PL1 02 gei_wo_wan_mrfz.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(HealPercentKey, 15m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null)
            {
                await PowerCmd.Apply<EWEndCombatHealPower>(Owner.Creature, DynamicVars[HealPercentKey].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade() => DynamicVars[HealPercentKey].UpgradeValueBy(5m);
    }
}
