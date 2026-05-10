using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class FanWeiDaJi() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "AL1 04 fan_wei_da_ji.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(14, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null) return;

            if (IsUpgraded)
            {
                await DamageCmd.Attack(7m).FromCard(this).Targeting(owner).Execute(choiceContext);
                foreach (var enemy in LivingEnemiesOf(owner))
                {
                    await DamageCmd.Attack(14m).FromCard(this).Targeting(enemy).Execute(choiceContext);
                    await PlayHLZYAttack(choiceContext, enemy, this);
                }

                return;
            }

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(owner).Execute(choiceContext);
            foreach (var enemy in LivingEnemiesOf(owner))
            {
                await CommonActions.CardAttack(this, enemy, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
                await PlayHLZYAttack(choiceContext, enemy, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(-7m);
        }
    }
}
