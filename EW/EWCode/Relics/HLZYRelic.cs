using EW.EWCode.Powers;
using EW.EWCode.Summons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Relics
{
    public class HLZYRelic : EWRelic
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        private readonly HashSet<Creature> _dealersThatConsumedHLZYThisTurn = [];
        private bool _hasSeenFirstPlayerTurnStart;

        public override string PackedIconPath => "res://EW/images/relics/hlzyRelic_sts2_redraw_transparent.png";
        protected override string PackedIconOutlinePath => "res://EW/images/relics/hlzyRelic_sts2_redraw_transparent.png";
        protected override string BigIconPath => "res://EW/images/relics/big/hlzyRelic_sts2_redraw_transparent.png";

        public override Task BeforeCombatStart()
        {
            _dealersThatConsumedHLZYThisTurn.Clear();
            _hasSeenFirstPlayerTurnStart = false;

            SummonManager.ResetForCombatStart();

            if (SummonManager.CountHLZY() == 0)
            {
                _ = SummonManager.SummonHLZYWhenReady(
                    SummonSource.Relic,
                    slotIndex: 0,
                    summoner: Owner?.Creature
                );
            }

            return Task.CompletedTask;
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            var owner = Owner?.Creature;
            if (owner == null || owner.IsDead || player != Owner)
            {
                return;
            }

            _dealersThatConsumedHLZYThisTurn.Clear();

            if (!_hasSeenFirstPlayerTurnStart)
            {
                _hasSeenFirstPlayerTurnStart = true;
                MainFile.Logger.Info("[EW] HLZY camouflage tick skipped on first player turn start.");
                return;
            }

            var camouflage = owner.GetPowerInstances<EWCamouflagePower>().FirstOrDefault();
            if (camouflage == null)
            {
                return;
            }

            if (camouflage.Amount > 1)
            {
                MainFile.Logger.Info($"[EW] HLZY camouflage decremented at player turn start: {camouflage.Amount} -> {camouflage.Amount - 1}.");
                await PowerCmd.Decrement(camouflage);
                return;
            }

            MainFile.Logger.Info("[EW] HLZY camouflage removed at player turn start.");
            await PowerCmd.Remove(camouflage);
        }

        public override Task AfterDamageReceived(
            PlayerChoiceContext choiceContext,
            Creature target,
            DamageResult result,
            ValueProp props,
            Creature? dealer,
            CardModel? cardSource
        )
        {
            var owner = Owner?.Creature;
            if (owner == null || owner.IsDead || target != owner)
            {
                return Task.CompletedTask;
            }

            if (result.UnblockedDamage <= 0 || dealer == null || _dealersThatConsumedHLZYThisTurn.Contains(dealer))
            {
                return Task.CompletedTask;
            }

            if (SummonManager.DismissOneHLZY())
            {
                _dealersThatConsumedHLZYThisTurn.Add(dealer);
                MainFile.Logger.Info($"[EW] HLZY dismissed after unblocked damage from {dealer.LogName}: {result.UnblockedDamage}.");
            }

            return Task.CompletedTask;
        }
    }
}
