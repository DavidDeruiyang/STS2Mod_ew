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
    public class ZhanShuSheJi() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL1 03 zhan_shu_she_ji.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(8, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            if (target == null || Owner == null) return;

            await CommonActions.CardAttack(this, target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, target, this);
            await PowerCmd.Apply<EWNextTurnEnergyPower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
