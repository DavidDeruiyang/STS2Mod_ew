using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class D6Bomb() : BombCard(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy, 3m, 10m)
    {
        protected override string PortraitFileName => "SL2 01 D6_zha_dan.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
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
            DynamicVars[BombDamageKey].UpgradeValueBy(5m);
        }
    }
}
