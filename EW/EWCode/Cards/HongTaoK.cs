using BaseLib.Utils;
using EW.EWCode.Keywords;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class HongTaoK() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "AL1 01 hong_tao_k.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null) return;

            var targets = LivingEnemiesOf(owner).ToList();
            foreach (var enemy in targets)
            {
                if (!enemy.IsAlive || !enemy.IsHittable)
                {
                    continue;
                }

                var hadBomb = BombUtils.CountBombs(enemy) > 0;

                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(enemy)
                    .WithNoAttackerAnim()
                    .Execute(choiceContext);

                if (enemy.IsAlive && enemy.IsHittable)
                {
                    await PlayHLZYAttack(choiceContext, enemy, this);
                }

                if (hadBomb && enemy.IsAlive)
                {
                    await PowerCmd.Apply<StrengthPower>(
                        enemy,
                        -1m,
                        owner,
                        this
                    );
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
