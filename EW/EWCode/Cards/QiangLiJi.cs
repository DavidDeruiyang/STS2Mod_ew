using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class QiangLiJi() : EWCard(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        private int HitCount => (int)DynamicVars["HitCount"].BaseValue;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(3, ValueProp.Move),
            new IntVar("HitCount", 3)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null)
            {
                return;
            }

            await CommonActions.CardAttack(
                this,
                target,
                HitCount,
                vfx: "vfx/vfx_attack_slash"
            ).Execute(choiceContext);

            await PlayHLZYAttack(choiceContext, target, this);

            await CommonActions.Apply<WeakPower>(
                choiceContext,
                target,
                this,
                1m
            );
        }

        protected override void OnUpgrade()
        {
            DynamicVars["HitCount"].UpgradeValueBy(1m);
        }
    }
}
