using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using EW.EWCode.Character;
using EW.EWCode.Extensions;
using EW.EWCode.Powers;
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

        protected async Task ChooseCardFromEWPoolToHand(
            PlayerChoiceContext choiceContext,
            CardType cardType,
            bool upgraded = false,
            int optionCount = 3
        )
        {
            if (Owner == null || Owner.Creature.CombatState == null)
            {
                return;
            }

            var options = ModelDb.CardPool<EWCardPool>()
                .AllCards
                .Where(card => card.Type == cardType && card.GetType() != GetType())
                .OrderBy(_ => Guid.NewGuid())
                .Take(optionCount)
                .Select(card =>
                {
                    var generated = Owner.Creature.CombatState.CreateCard(card, Owner);
                    if (upgraded)
                    {
                        CardCmd.Upgrade(generated, CardPreviewStyle.None);
                    }

                    return generated;
                })
                .ToList();

            if (options.Count == 0)
            {
                return;
            }

            var selected = await CardSelectCmd.FromChooseACardScreen(choiceContext, options, Owner, false);
            if (selected != null)
            {
                await CardPileCmd.AddGeneratedCardsToCombat([selected], PileType.Hand, true, CardPilePosition.Top);
            }
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

                if (costDeltaThisTurn != 0)
                {
                    card.EnergyCost.AddThisTurn(costDeltaThisTurn);
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

            var mainDamage = GetCardDamage(cardSource);
            var repeatPower = cardSource.Owner?.Creature.GetPowerInstances<EWHLZYRepeatAttackPower>().FirstOrDefault();
            var hlzyDamage = repeatPower == null
                ? 1m
                : decimal.Max(1m, decimal.Ceiling(mainDamage * repeatPower.Amount / 100m));

            await DamageCmd.Attack(hlzyDamage)
                .FromCard(cardSource)
                .Targeting(target)
                .Unpowered()
                .WithNoAttackerAnim()
                .WithHitCount(hitCount)
                .Execute(choiceContext);

            SummonManager.RecordHLZYAttacks(hitCount);
            if (cardSource.Owner != null)
            {
                ZuZongLeiJi.RefreshDamageForPlayer(cardSource.Owner);
            }

            var owner = cardSource.Owner?.Creature;
            if (owner == null)
            {
                return;
            }

            var roaringAmount = owner.GetPowerInstances<EWHLZYRoaringHandPower>().Sum(power => power.Amount);
            if (roaringAmount > 0m)
            {
                var strengthGain = roaringAmount * hitCount;
                await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.StrengthPower>(owner, strengthGain, owner, cardSource);
                await PowerCmd.Apply<EWRemoveStrengthAtTurnEndPower>(owner, strengthGain, owner, cardSource);
            }

            if (owner.GetPowerInstances<EWAfterimagePower>().Any())
            {
                await PowerCmd.Apply<EWAfterimageMarkPower>(target, 1m, owner, cardSource);
            }

            if (cardSource.TargetType == TargetType.AnyEnemy &&
                owner.GetPowerInstances<EWHLZYSplashPower>().Any() &&
                mainDamage > 0m)
            {
                var splashPercent = owner.GetPowerInstances<EWHLZYSplashPower>().Max(power => power.Amount);
                var splashDamage = decimal.Max(1m, decimal.Ceiling(mainDamage * splashPercent / 100m));
                foreach (var enemy in LivingEnemiesOf(owner).Where(enemy => enemy != target))
                {
                    await DamageCmd.Attack(splashDamage)
                        .FromCard(cardSource)
                        .Targeting(enemy)
                        .Unpowered()
                        .WithNoAttackerAnim()
                        .Execute(choiceContext);
                }
            }
        }

        private static decimal GetCardDamage(CardModel card)
        {
            try
            {
                return card.DynamicVars.Damage.BaseValue;
            }
            catch
            {
                return 1m;
            }
        }
    }
}
