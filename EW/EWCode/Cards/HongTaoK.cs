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

            foreach (var enemy in LivingEnemiesOf(owner))
            {
                await CommonActions.CardAttack(this, enemy, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
                await PlayHLZYAttack(choiceContext, enemy, this);

                if (BombUtils.CountBombs(enemy) > 0)
                {
                    await CommonActions.Apply<TemporaryStrengthPower>(choiceContext, enemy, this, -1m);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
