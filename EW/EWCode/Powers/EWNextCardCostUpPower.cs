using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Powers
{
    public class EWNextCardCostUpPower : EWPower
    {
        private sealed class CostUpState
        {
            public CardModel? SourceCard { get; set; }
            public HashSet<CardModel> Cards { get; } = [];
            public bool SkippedSourceAfterPlay { get; set; }
        }

        private static readonly Dictionary<Creature, CostUpState> ActiveStates = [];

        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public static void Register(Creature owner, CardModel sourceCard, IEnumerable<CardModel> cards)
        {
            if (!ActiveStates.TryGetValue(owner, out var state))
            {
                state = new CostUpState();
                ActiveStates[owner] = state;
            }

            state.SourceCard = sourceCard;
            state.SkippedSourceAfterPlay = false;

            foreach (var card in cards)
            {
                card.EnergyCost.AddThisTurn(1);
                state.Cards.Add(card);
            }
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Card.Owner?.Creature != Owner)
            {
                return;
            }

            if (ActiveStates.TryGetValue(Owner, out var state) &&
                !state.SkippedSourceAfterPlay &&
                ReferenceEquals(cardPlay.Card, state.SourceCard))
            {
                state.SkippedSourceAfterPlay = true;
                return;
            }

            await RevertAndRemove();
        }

        public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (Owner == null || side != Owner.Side)
            {
                return;
            }

            await RevertAndRemove();
        }

        private async Task RevertAndRemove()
        {
            if (Owner != null && ActiveStates.Remove(Owner, out var state))
            {
                foreach (var card in state.Cards.Where(card => card.Owner != null))
                {
                    card.EnergyCost.AddThisTurn(-1);
                }
            }

            await PowerCmd.Remove(this);
        }

        public static void Clear(Creature owner)
        {
            ActiveStates.Remove(owner);
        }

        public override Task AfterCombatEnd(CombatRoom room)
        {
            if (Owner != null)
            {
                Clear(Owner);
            }

            return Task.CompletedTask;
        }
    }
}
