using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class JiFengErShi() : EWCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL3 05 ji_feng_er_shi.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(12, ValueProp.Move), new DynamicVar("BonusDamage", 6m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            var hasDebuff = target.GetPowerInstances<MegaCrit.Sts2.Core.Models.Powers.WeakPower>().Any()
                || target.GetPowerInstances<MegaCrit.Sts2.Core.Models.Powers.VulnerablePower>().Any()
                || BombUtils.CountBombs(target) > 0;
            var damage = DynamicVars.Damage.BaseValue + (hasDebuff ? DynamicVars["BonusDamage"].BaseValue : 0m);
            await DamageCmd.Attack(damage).FromCard(this).Targeting(target).Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, target, this);
        }

        protected override void OnUpgrade() => DynamicVars["BonusDamage"].UpgradeValueBy(6m);
    }
}
