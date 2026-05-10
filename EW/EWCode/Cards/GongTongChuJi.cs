using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class GongTongChuJi() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL4 02 gong_tong_chu_ji.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            var count = SummonManager.CountHLZY();
            if (count > 0)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(target).WithHitCount(count).Execute(choiceContext);
            }

            await PlayHLZYAttack(choiceContext, target, this);
        }

        protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
    }
}
