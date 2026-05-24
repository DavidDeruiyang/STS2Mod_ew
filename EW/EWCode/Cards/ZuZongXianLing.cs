using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public class ZuZongXianLing() : EWCard(3, CardType.Skill, CardRarity.Rare, TargetType.None)
    {
        private const string GeneratedCardsKey = "GeneratedCards";

        protected override string PortraitFileName => "SL4 06 zu_zong_xian_ling.png";
        public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            CardPreview<SiHunLingDeYuXi>(IsUpgraded),
            CardPreview<TouZhiShou>(IsUpgraded),
            CardPreview<HunLingBiYou>(IsUpgraded),
            CardPreview<HaoLi>(IsUpgraded),
            CardPreview<XianZuDeBiHu>(IsUpgraded),
            CardPreview<HongMingZhiShou>(IsUpgraded),
            CardPreview<CanYing>(IsUpgraded)
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new StringVar(GeneratedCardsKey, "魂灵之影能力牌")
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;
            var cards = new CardModel[]
            {
                Owner.Creature.CombatState!.CreateCard<SiHunLingDeYuXi>(Owner),
                Owner.Creature.CombatState!.CreateCard<TouZhiShou>(Owner),
                Owner.Creature.CombatState!.CreateCard<HunLingBiYou>(Owner),
                Owner.Creature.CombatState!.CreateCard<HaoLi>(Owner),
                Owner.Creature.CombatState!.CreateCard<XianZuDeBiHu>(Owner),
                Owner.Creature.CombatState!.CreateCard<HongMingZhiShou>(Owner),
                Owner.Creature.CombatState!.CreateCard<CanYing>(Owner)
            }.ToList();
            Owner.RunState.Rng.CombatCardSelection.Shuffle(cards);
            cards = cards.Take(2).ToList();

            foreach (var card in cards)
            {
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(card, CardPreviewStyle.None);
                }

                card.EnergyCost.SetThisTurn(0, false);
            }

            await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, true, CardPilePosition.Top);
        }

        protected override void OnUpgrade()
        {
            ((StringVar)DynamicVars[GeneratedCardsKey]).StringValue = "魂灵之影能力牌+";
        }
    }
}
