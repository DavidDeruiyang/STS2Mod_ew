using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class SuperBigBoom() : BombCard(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy, 3m, 30m)
    {
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var owner = Owner?.Creature;
            var target = cardPlay.Target;
            if (owner == null || target == null)
            {
                return;
            }

            await BombUtils.ApplyBomb(choiceContext, target, owner, this, BombTurns, BombDamage, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars[TurnsKey].UpgradeValueBy(-1m);
        }
    }
}
