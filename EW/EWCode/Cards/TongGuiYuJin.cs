using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class TongGuiYuJin() : BombCard(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy, 2m, 20m)
    {
        protected override string PortraitFileName => "AL2 04 tong_gui_yu_jin.png";
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(27, ValueProp.Move),
            new DynamicVar(TurnsKey, 2m),
            new DynamicVar(BombDamageKey, 20m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            if (owner == null || cardPlay.Target == null)
            {
                return;
            }

            await CommonActions.CardAttack(this, cardPlay.Target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, cardPlay.Target, this);
            await BombUtils.ApplyBomb(choiceContext, owner, owner, this, BombTurns, BombDamage, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars[TurnsKey].UpgradeValueBy(1m);
        }
    }
}
