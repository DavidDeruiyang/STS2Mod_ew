using BaseLib.Utils;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class CiSha() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL1 06 ci_sha.png";

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(6, ValueProp.Move),
            new DynamicVar("BonusDamage", 5m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            var target = cardPlay.Target;
            if (owner == null || target == null) return;

            var damage = DynamicVars.Damage.BaseValue + (HasPower<EWCamouflagePower>(owner) ? DynamicVars["BonusDamage"].BaseValue : 0m);
            await DamageCmd.Attack(damage).FromCard(this).Targeting(target).Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, target, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["BonusDamage"].UpgradeValueBy(3m);
        }
    }
}
