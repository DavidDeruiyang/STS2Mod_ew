using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Cards
{
    public static class BombUtils
    {
        public static async Task<EWTimedBombPower?> ApplyBomb(
            PlayerChoiceContext choiceContext,
            Creature target,
            Creature applier,
            CardModel cardSource,
            decimal turns,
            decimal damage,
            CardPlay? cardPlay = null
        )
        {
            turns = AdjustTurns(applier, turns);
            damage = AdjustDamage(applier, damage);

            var bomb = await PowerCmd.Apply<EWTimedBombPower>(target, turns, applier, cardSource);
            bomb?.SetBomb(damage, cardSource);

            await TriggerBombArmor(choiceContext, applier, cardSource, target, cardPlay);
            return bomb;
        }

        public static async Task<int> DetonateBombs(
            PlayerChoiceContext choiceContext,
            Creature target,
            CardModel? cardSource = null
        )
        {
            var bombs = target.GetPowerInstances<EWTimedBombPower>().ToList();
            foreach (var bomb in bombs)
            {
                await bomb.Detonate(choiceContext, cardSource);
            }

            return bombs.Count;
        }

        public static int CountBombs(Creature target)
        {
            return target.GetPowerInstances<EWTimedBombPower>().Count();
        }

        public static async Task AddBombCardToHand<T>(
            Player player,
            bool upgraded = false,
            int amount = 1
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

            await CardPileCmd.AddGeneratedCardsToCombat(
                cards,
                PileType.Hand,
                true,
                CardPilePosition.Top
            );
        }

        public static IEnumerable<Creature> LivingEnemiesOf(Creature owner)
        {
            return owner.CombatState!.GetOpponentsOf(owner).Where(c => c.IsAlive && c.IsHittable);
        }

        public static Creature? RandomLivingEnemyOf(Creature owner)
        {
            var enemies = LivingEnemiesOf(owner).ToList();
            return enemies.Count == 0
                ? null
                : owner.CombatState!.RunState.Rng.CombatTargets.NextItem(enemies);
        }

        public static bool IsBombCard(CardModel card)
        {
            return card is BombCard;
        }

        public static decimal GetBombCardDamage(CardModel card)
        {
            return card is BombCard bombCard ? bombCard.BombDamage : 0m;
        }

        private static decimal AdjustTurns(Creature applier, decimal turns)
        {
            return applier.GetPowerInstances<EWBombDemonCourtPower>().Any()
                ? 1m
                : turns;
        }

        private static decimal AdjustDamage(Creature applier, decimal damage)
        {
            var bonus = applier.GetPowerInstances<EWThreeTwoOnePower>().Sum(power => power.DamageBonus);
            return damage + bonus;
        }

        private static async Task TriggerBombArmor(
            PlayerChoiceContext choiceContext,
            Creature owner,
            CardModel cardSource,
            Creature target,
            CardPlay? cardPlay
        )
        {
            foreach (var power in owner.GetPowerInstances<EWBombCounterArmorPower>())
            {
                await power.GainBombBlock(choiceContext, cardSource, target, cardPlay);
            }
        }
    }
}
