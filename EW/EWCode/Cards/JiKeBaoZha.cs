using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Cards;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class JiKeBaoZha() : EWCard(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        protected override string PortraitFileName => "AL2 05 ji_ke_bao_zha.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var target = cardPlay.Target;
            var bombCard = Owner?.Piles
                .FirstOrDefault(pile => pile.Type == PileType.Hand)?
                .Cards
                .FirstOrDefault(card => card != this && BombUtils.IsBombCard(card));

            if (target == null || bombCard == null)
            {
                return;
            }

            await CardCmd.Exhaust(choiceContext, bombCard, true, false);
            await DamageCmd.Attack(BombUtils.GetBombCardDamage(bombCard))
                .FromCard(this)
                .Targeting(target)
                .Unpowered()
                .Execute(choiceContext);
            await PlayHLZYAttack(choiceContext, target, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
