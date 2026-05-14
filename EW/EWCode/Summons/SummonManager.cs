using Godot;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EW.EWCode.Summons
{
    public enum SummonSource
    {
        Relic,
        Card,
        Other
    }

    public static class SummonManager
    {
        private static readonly StringName SpawnHlzyMethod = "ew_spawn_hlzy";
        private static readonly StringName ClearHlzyMethod = "ew_clear_hlzy";
        private static readonly StringName HlzyCountMethod = "ew_hlzy_count";
        private static readonly Dictionary<int, SummonInstance> ActiveSummons = [];

        public const int AnySlot = -1;
        public const int MaxSummons = 3;
        private const int DefaultReadyRetryFrames = 120;
        private const decimal CamouflagePerHLZY = 2m;

        public static IReadOnlyCollection<SummonInstance> Active => ActiveSummons.Values;
        public static int TotalHLZYAttackCount { get; private set; }

        public static bool SummonHLZY(SummonSource source = SummonSource.Other, int slotIndex = AnySlot, Creature? summoner = null, CardModel? cardSource = null)
        {
            var summon = ReserveHLZY(source, slotIndex);
            if (summon == null)
            {
                return false;
            }

            if (!TryFindCombatVisualMethod(SpawnHlzyMethod, out var node))
            {
                ActiveSummons.Remove(summon.SlotIndex);
                MainFile.Logger.Info($"HLZY summon skipped from {source}: combat visual spawn method was not found.");
                return false;
            }

            node.Call(SpawnHlzyMethod, summon.SlotIndex);
            _ = ApplyCamouflage(summoner, cardSource);
            return true;
        }

        public static async Task<bool> SummonHLZYWhenReady(
            SummonSource source = SummonSource.Other,
            int slotIndex = AnySlot,
            Creature? summoner = null,
            CardModel? cardSource = null,
            int retryFrames = DefaultReadyRetryFrames
        )
        {
            var summon = ReserveHLZY(source, slotIndex);
            if (summon == null)
            {
                return false;
            }

            for (var attempt = 0; attempt <= retryFrames; attempt++)
            {
                if (TryFindCombatVisualMethod(SpawnHlzyMethod, out var node))
                {
                    node.Call(SpawnHlzyMethod, summon.SlotIndex);
                    await ApplyCamouflage(summoner, cardSource);
                    return true;
                }

                var room = NCombatRoom.Instance;
                if (room?.GetTree() == null)
                {
                    await Task.Delay(16);
                    continue;
                }

                await room.ToSignal(room.GetTree(), SceneTree.SignalName.ProcessFrame);
            }

            ActiveSummons.Remove(summon.SlotIndex);
            MainFile.Logger.Info($"HLZY summon skipped from {source}: combat visual spawn method was not found after waiting.");
            return false;
        }

        private static async Task ApplyCamouflage(Creature? summoner, CardModel? cardSource)
        {
            if (summoner == null || summoner.IsDead)
            {
                return;
            }

            await PowerCmd.Apply<EWCamouflagePower>(
                summoner,
                CamouflagePerHLZY,
                summoner,
                cardSource
            );
        }

        public static void ClearHLZY(int slotIndex = AnySlot)
        {
            if (slotIndex == AnySlot)
            {
                ActiveSummons.Clear();
            }
            else
            {
                ActiveSummons.Remove(slotIndex);
            }

            if (TryFindCombatVisualMethod(ClearHlzyMethod, out var node))
            {
                node.Call(ClearHlzyMethod, slotIndex);
            }
        }

        public static void ResetForCombatStart()
        {
            ActiveSummons.Clear();
            TotalHLZYAttackCount = 0;
        }

        public static void ClearForCombatEnd(string reason)
        {
            ClearHLZY();
            TotalHLZYAttackCount = 0;
            MainFile.Logger.Info($"HLZY summons cleared: {reason}.");
        }

        public static void RecordHLZYAttacks(int count)
        {
            if (count <= 0)
            {
                return;
            }

            TotalHLZYAttackCount += count;
        }

        public static bool DismissOneHLZY()
        {
            var summon = ActiveSummons.Values
                .Where(summon => summon.Id == SummonInstance.HLZYId && summon.IsAlive)
                .OrderByDescending(summon => summon.SlotIndex)
                .FirstOrDefault();

            if (summon == null)
            {
                return false;
            }

            ClearHLZY(summon.SlotIndex);
            return true;
        }

        public static int CountHLZY()
        {
            return ActiveSummons.Values.Count(summon =>
                summon.Id == SummonInstance.HLZYId &&
                summon.IsAlive
            );
        }

        public static IReadOnlyList<SummonInstance> GetLivingSummons()
        {
            return ActiveSummons.Values
                .Where(summon => summon.IsAlive)
                .OrderBy(summon => summon.SlotIndex)
                .ToList();
        }

        public static int GetProvidedEffectAmount(SummonEffectKind kind)
        {
            return ActiveSummons.Values
                .Where(summon => summon.IsAlive)
                .SelectMany(summon => summon.ProvidedEffects)
                .Where(effect => effect.Kind == kind)
                .Sum(effect => effect.Amount);
        }

        public static bool DamageSummon(int slotIndex, int amount)
        {
            if (!ActiveSummons.TryGetValue(slotIndex, out var summon) || amount <= 0)
            {
                return false;
            }

            summon.Blood = int.Max(0, summon.Blood - amount);
            if (summon.IsAlive)
            {
                return true;
            }

            ClearHLZY(slotIndex);
            return true;
        }

        private static SummonInstance? ReserveHLZY(SummonSource source, int requestedSlot)
        {
            var slotIndex = ResolveSlot(requestedSlot);
            if (slotIndex < 0)
            {
                MainFile.Logger.Info($"HLZY summon skipped from {source}: no free summon slot.");
                return null;
            }

            var summon = new SummonInstance
            {
                SlotIndex = slotIndex,
                Blood = 1,
                MaxBlood = 1
            };

            ActiveSummons[slotIndex] = summon;
            return summon;
        }

        private static int ResolveSlot(int requestedSlot)
        {
            if (requestedSlot != AnySlot)
            {
                return requestedSlot >= 0 && requestedSlot < MaxSummons && !ActiveSummons.ContainsKey(requestedSlot)
                    ? requestedSlot
                    : -1;
            }

            for (var slotIndex = 0; slotIndex < MaxSummons; slotIndex++)
            {
                if (!ActiveSummons.ContainsKey(slotIndex))
                {
                    return slotIndex;
                }
            }

            return -1;
        }

        private static bool TryFindCombatVisualMethod(StringName methodName, out Node node)
        {
            node = null!;

            var room = NCombatRoom.Instance;
            if (room == null)
            {
                return false;
            }

            return TryFindMethod(room, methodName, out node);
        }

        private static bool TryFindMethod(Node current, StringName methodName, out Node node)
        {
            if (current.HasMethod(methodName))
            {
                node = current;
                return true;
            }

            foreach (var child in current.GetChildren())
            {
                if (child is Node childNode && TryFindMethod(childNode, methodName, out node))
                {
                    return true;
                }
            }

            node = null!;
            return false;
        }
    }
}
