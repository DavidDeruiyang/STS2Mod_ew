using EW.EWCode.Keywords;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZhaDanKuangRen() : EWCard(2, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        protected override string PortraitFileName => "SL2 04 bao_zha_tian_cai.png";
        private const string GeneratedCardKey = "GeneratedCard";

        public override IEnumerable<CardKeyword> CanonicalKeywords => [EWKeywords.OriginiumBomb];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => SingleCardPreview<D6Bomb>(IsUpgraded);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar(GeneratedCardKey, "D6炸弹")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null)
            {
                return;
            }

            var owner = Owner;
            var pileTypes = new[] { PileType.Hand, PileType.Draw, PileType.Discard };
            var cards = CardPile.GetCards(owner, pileTypes)
                .Where(card => card != this)
                .ToList();

            var transformations = cards.Select(card =>
            {
                var replacement = owner.Creature.CombatState!.CreateCard<D6Bomb>(owner);
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(replacement, CardPreviewStyle.None);
                }

                return new CardTransformation(card, replacement);
            }).ToList();

            if (transformations.Count == 0)
            {
                return;
            }

            await CardCmd.Transform(transformations, owner.PlayerRng.Transformations, CardPreviewStyle.None);
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardKey]).StringValue = "D6+炸弹";
        }
    }
}
