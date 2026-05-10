using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SuiYiKaiHuo() : EWCard(1, CardType.Attack, CardRarity.Common, TargetType.None)
    {
        protected override string PortraitFileName => "AL1 09 sui_yi_kai_huo.png";

        protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(7, ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            var target = BombUtils.RandomLivingEnemyOf(Owner.Creature);
            if (target != null)
            {
                await CommonActions.CardAttack(this, target, vfx: "vfx/vfx_attack_slash").Execute(choiceContext);
                await PlayHLZYAttack(choiceContext, target, this);
            }

            var cardToExhaust = CardPile.GetCards(Owner, [PileType.Hand]).FirstOrDefault(card => card != this);
            if (cardToExhaust != null)
            {
                await CardCmd.Exhaust(choiceContext, cardToExhaust, true, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
