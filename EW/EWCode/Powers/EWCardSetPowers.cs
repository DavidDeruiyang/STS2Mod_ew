using BaseLib.Utils;
using EW.EWCode.Cards;
using EW.EWCode.Powers;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWEndCombatHealPower : EWPower
    {
        private static readonly Dictionary<Creature, decimal> ActiveHealing = [];

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public static void Register(Creature owner, decimal percent)
        {
            ActiveHealing[owner] = percent;
        }

        public static async Task HealRegisteredAtCombatEnd()
        {
            var entries = ActiveHealing.ToList();
            ActiveHealing.Clear();

            foreach (var (owner, percent) in entries)
            {
                if (owner == null || owner.IsDead)
                {
                    continue;
                }

                var missingHp = owner.MaxHp - owner.CurrentHp;
                if (missingHp <= 0)
                {
                    continue;
                }

                var healAmount = decimal.Ceiling(missingHp * percent / 100m);
                if (healAmount > 0m)
                {
                    await CreatureCmd.Heal(owner, healAmount, false);
                }
            }
        }

        public override async Task AfterCombatEnd(CombatRoom room)
        {
            if (Owner == null || Owner.IsDead)
            {
                return;
            }

            var missingHp = Owner.MaxHp - Owner.CurrentHp;
            if (missingHp <= 0m)
            {
                return;
            }

            var healAmount = decimal.Ceiling(missingHp * Amount / 100m);
            if (healAmount > 0m)
            {
                await CreatureCmd.Heal(Owner, healAmount, false);
                MainFile.Logger.Info($"[EW] 给我玩明日方舟 healed {healAmount} at combat end ({Amount}% of missing HP).");
            }
        }
    }

    public class EWKazdelSpeakerPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner) return;
            await KazdelCardUtils.AddRandomKazdelCardToHand(player, false, Amount > 1m ? -1 : 0);
        }
    }

    public class EWKazdelHopePower : EWPower
    {
        private int _kazdelCardsPlayedThisTurn;
        private bool _triggeredThisTurn;

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner != null && player.Creature == Owner)
            {
                _kazdelCardsPlayedThisTurn = 0;
                _triggeredThisTurn = false;
            }

            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner || !KazdelCardUtils.IsKazdelCard(cardPlay.Card))
            {
                return;
            }

            _kazdelCardsPlayedThisTurn++;
            if (_triggeredThisTurn || _kazdelCardsPlayedThisTurn < 3)
            {
                return;
            }

            _triggeredThisTurn = true;
            await PowerCmd.Apply<PlatingPower>(Owner, 5m, Owner, cardPlay.Card);
            await PowerCmd.Apply<StrengthPower>(Owner, 3m, Owner, cardPlay.Card);
        }
    }

    public class EWKazdelStrengthPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner || !KazdelCardUtils.IsKazdelCard(cardPlay.Card)) return;
            await CommonActions.ApplySelf<StrengthPower>(choiceContext, cardPlay.Card, Amount);
        }
    }

    public class EWKazdelDexterityPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner || !KazdelCardUtils.IsKazdelCard(cardPlay.Card)) return;
            await CommonActions.ApplySelf<DexterityPower>(choiceContext, cardPlay.Card, Amount);
        }
    }

    public class EWNotebookPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side) return;

            var player = CombatManager.Instance.DebugOnlyGetState()?.Players
                .FirstOrDefault(player => player.Creature == Owner);
            if (player == null || CardPile.GetCards(player, [PileType.Hand]).Any()) return;

            await PowerCmd.Apply<EWNextTurnEnergyPower>(Owner, Amount, Owner, null);
        }
    }

    public class EWSoulRemainsPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner || SummonManager.CountHLZY() <= 0) return;
            await PowerCmd.Apply<EWCamouflagePower>(Owner, Amount, Owner, null);
        }
    }

    public class EWSoulBlessingPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || player.Creature != Owner || SummonManager.CountHLZY() <= 0) return;
            await PlayerCmd.GainEnergy(Amount, player);
        }
    }

    public class EWAncestorGuardPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side) return;
            var block = Amount * SummonManager.CountHLZY();
            if (block > 0m) await CreatureCmd.GainBlock(Owner, block, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move, null, false);
        }
    }

    public class EWHLZYSplashPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWHLZYRepeatAttackPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWHLZYRoaringHandPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWRemoveStrengthAtTurnEndPower : EWPower
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side)
            {
                return;
            }

            await PowerCmd.Apply<StrengthPower>(Owner, -Amount, Owner, null);
            await PowerCmd.Remove(this);
        }
    }

    public class EWRestoreStrengthAtTurnEndPower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side)
            {
                return;
            }

            await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
            await PowerCmd.Remove(this);
        }
    }

    public class EWAfterimagePower : EWPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
    }

    public class EWAfterimageMarkPower : EWPower
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override decimal ModifyDamageAdditive(
            Creature? target,
            decimal amount,
            MegaCrit.Sts2.Core.ValueProps.ValueProp props,
            Creature? dealer,
            CardModel? cardSource
        )
        {
            if (Owner == null || target != Owner || cardSource?.Owner?.Creature != dealer)
            {
                return 0m;
            }

            return 2m * Amount;
        }
    }
}
