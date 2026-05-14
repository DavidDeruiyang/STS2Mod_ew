using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class HaoLi() : EWCard(3, CardType.Power, CardRarity.Rare, TargetType.None)
    {
        private const string RepeatPercentKey = "RepeatPercent";

        protected override string PortraitFileName => "PL4 04 hao_li.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar(RepeatPercentKey, 50m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner != null) await PowerCmd.Apply<EWHLZYRepeatAttackPower>(Owner.Creature, DynamicVars[RepeatPercentKey].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade() => DynamicVars[RepeatPercentKey].UpgradeValueBy(20m);
    }
}
