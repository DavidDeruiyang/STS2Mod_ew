using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class LengCiDunRen() : EWCard(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        private static readonly FieldInfo? AttackDamagePropsField =
            typeof(MegaCrit.Sts2.Core.Commands.Builders.AttackCommand).GetField(
                "<DamageProps>k__BackingField",
                BindingFlags.Instance | BindingFlags.NonPublic
            );

        protected override string PortraitFileName => "AL1 07 leng_ci_dun_ren.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(10, ValueProp.Unblockable)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            var attack = DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(target);
            AttackDamagePropsField?.SetValue(attack, ValueProp.Unblockable);
            await attack.Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, target, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(5m);
        }
    }
}
