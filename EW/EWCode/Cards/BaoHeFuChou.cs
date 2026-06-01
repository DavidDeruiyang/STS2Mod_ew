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
    public class BaoHeFuChou() : EWCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL4 04 bao_he_fu_chou.png";
        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null) return;

            var owner = Owner?.Creature;
            if (owner == null) return;

            var x = GetResolvedEnergyXValue(cardPlay);
            var hits = x * SummonManager.CountHLZY(owner);
            if (hits > 0)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(target).WithHitCount(hits).Execute(choiceContext);
            }

            await PlayHLZYAttack(choiceContext, target, this, hits);
        }

        protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
