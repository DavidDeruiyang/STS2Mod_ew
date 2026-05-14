using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class DieZhouJi() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL3 04 die_meng_ji.png";
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(12, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CommonActions.CardAttack(
                this,
                cardPlay.Target,
                vfx: "vfx/vfx_attack_slash"
            ).Execute(choiceContext);

            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}
