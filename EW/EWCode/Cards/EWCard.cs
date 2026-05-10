using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EW.EWCode.Character;
using EW.EWCode.Extensions;
using EW.EWCode.Summons;
using EW.EWCode.Vfx;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    [Pool(typeof(EWCardPool))]
    public abstract class EWCard(int cost, CardType type, CardRarity rarity, TargetType target) :
        CustomCardModel(cost, type, rarity, target)
    {
        //Image size:
        //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
        //Full art: 606x852
        protected virtual string PortraitFileName => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png";

        public override string CustomPortraitPath => PortraitFileName.BigCardImagePath();

        //Smaller variants of card images for efficiency:
        //Smaller variant of fullart: 250x350
        //Smaller variant of normalart: 250x190

        //Uses card_portraits/card_name.png as image path. These should be smaller images.
        public override string PortraitPath => PortraitFileName.CardImagePath();
        public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();

        protected static IHoverTip CardPreview<T>(bool upgraded = false) where T : CardModel
        {
            var card = ModelDb.Card<T>();
            if (upgraded)
            {
                card = (T)card.ToMutable();
                CardCmd.Upgrade(card, CardPreviewStyle.None);
            }

            return new CardHoverTip(card);
        }

        protected static IEnumerable<IHoverTip> SingleCardPreview<T>(bool upgraded = false) where T : CardModel
        {
            yield return CardPreview<T>(upgraded);
        }

        protected static IEnumerable<Creature> LivingEnemiesOf(Creature owner)
        {
            return owner.CombatState!.GetOpponentsOf(owner).Where(c => c.IsAlive && c.IsHittable);
        }

        protected static bool HasPower<T>(Creature owner) where T : PowerModel
        {
            return owner.GetPowerInstances<T>().Any();
        }

        protected static async Task AddCardsToHand<T>(Player player, int amount, bool upgraded = false, int costDeltaThisTurn = 0)
            where T : CardModel
        {
            await AddGeneratedCards<T>(player, PileType.Hand, amount, upgraded, costDeltaThisTurn);
        }

        protected static async Task AddCardsToDrawPile<T>(Player player, int amount, bool upgraded = false)
            where T : CardModel
        {
            await AddGeneratedCards<T>(player, PileType.Draw, amount, upgraded);
        }

        public static async Task AddGeneratedCards<T>(
            Player player,
            PileType pileType,
            int amount,
            bool upgraded = false,
            int costDeltaThisTurn = 0
        ) where T : CardModel
        {
            var cards = new List<CardModel>();
            for (var i = 0; i < amount; i++)
            {
                var card = player.Creature.CombatState!.CreateCard<T>(player);
                if (upgraded)
                {
                    CardCmd.Upgrade(card, CardPreviewStyle.None);
                }

                cards.Add(card);
            }

            await CardPileCmd.AddGeneratedCardsToCombat(cards, pileType, true, CardPilePosition.Top);
        }

        protected static async Task PlayHLZYAttack(PlayerChoiceContext choiceContext, Creature? target, CardModel cardSource)
        {
            var hitCount = SummonManager.CountHLZY();
            if (target == null || target.IsDead || hitCount <= 0)
            {
                return;
            }

            HLZYAttackVfx.PlayFromAllHLZYTo(target);

            await DamageCmd.Attack(1m)
                .FromCard(cardSource)
                .Targeting(target)
                .Unpowered()
                .WithNoAttackerAnim()
                .WithHitCount(hitCount)
                .Execute(choiceContext);
        }
    }
}
