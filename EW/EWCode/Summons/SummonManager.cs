using Godot;
using EW.EWCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
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
        private static readonly Dictionary<Creature, Dictionary<int, SummonInstance>> ActiveSummonsByOwner = [];
        private static readonly Dictionary<Creature, int> TotalHLZYAttackCountByOwner = [];

        public const int AnySlot = -1;
        public const int MaxSummons = 3;
        private const int DefaultReadyRetryFrames = 120;
        private const decimal CamouflagePerHLZY = 2m;

        public static IReadOnlyCollection<SummonInstance> Active => ActiveSummons.Values
            .Concat(ActiveSummonsByOwner.Values.SelectMany(summons => summons.Values))
            .ToList();
        public static int TotalHLZYAttackCount => TotalHLZYAttackCountByOwner.Values.Sum();

        public static bool SummonHLZY(SummonSource source = SummonSource.Other, int slotIndex = AnySlot, Creature? summoner = null, CardModel? cardSource = null)
        {
            var summon = ReserveHLZY(source, slotIndex, summoner);
            if (summon == null)
            {
                return false;
            }

            _ = ApplyCamouflageSafely(summoner, cardSource);
            _ = SpawnVisualWhenReadySafely(source, summoner, summon, DefaultReadyRetryFrames);
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
            var summon = ReserveHLZY(source, slotIndex, summoner);
            if (summon == null)
            {
                return false;
            }

            await ApplyCamouflageSafely(summoner, cardSource);
            _ = SpawnVisualWhenReadySafely(source, summoner, summon, retryFrames);
            return true;
        }

        private static async Task SpawnVisualWhenReadySafely(
            SummonSource source,
            Creature? summoner,
            SummonInstance summon,
            int retryFrames
        )
        {
            try
            {
                await SpawnVisualWhenReady(source, summoner, summon, retryFrames);
            }
            catch (Exception ex)
            {
                MainFile.Logger.Info($"HLZY visual spawn skipped from {source} after error: {ex.Message}");
            }
        }

        private static async Task SpawnVisualWhenReady(
            SummonSource source,
            Creature? summoner,
            SummonInstance summon,
            int retryFrames
        )
        {
            for (var attempt = 0; attempt <= retryFrames; attempt++)
            {
                if (!IsActiveSummon(summoner, summon))
                {
                    return;
                }

                if (TryFindCombatVisualMethod(SpawnHlzyMethod, summoner, out var node))
                {
                    try
                    {
                        node.Call(SpawnHlzyMethod, summon.SlotIndex);
                    }
                    catch (Exception ex)
                    {
                        MainFile.Logger.Info($"HLZY visual spawn skipped from {source} after error: {ex.Message}");
                    }

                    return;
                }

                var room = NCombatRoom.Instance;
                if (room?.GetTree() == null)
                {
                    await Task.Delay(16);
                    continue;
                }

                await room.ToSignal(room.GetTree(), SceneTree.SignalName.ProcessFrame);
            }

            MainFile.Logger.Info($"HLZY visual spawn skipped from {source}: combat visual spawn method was not found after waiting.");
        }

        private static async Task ApplyCamouflageSafely(Creature? summoner, CardModel? cardSource)
        {
            try
            {
                await ApplyCamouflage(summoner, cardSource);
            }
            catch (Exception ex)
            {
                MainFile.Logger.Info($"HLZY camouflage application skipped after error: {ex.Message}");
            }
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
            ClearHLZYFor(null, slotIndex);
        }

        public static void ClearHLZY(Creature owner, int slotIndex = AnySlot)
        {
            ClearHLZYFor(owner, slotIndex);
        }

        private static void ClearHLZYFor(Creature? owner, int slotIndex)
        {
            var activeSummons = GetActiveSummons(owner, create: false);
            if (slotIndex == AnySlot)
            {
                activeSummons?.Clear();
                if (owner == null)
                {
                    foreach (var summons in ActiveSummonsByOwner.Values)
                    {
                        summons.Clear();
                    }
                }
            }
            else
            {
                activeSummons?.Remove(slotIndex);
            }

            if (owner == null)
            {
                foreach (var visualNode in FindCombatVisualMethods(ClearHlzyMethod))
                {
                    visualNode.Call(ClearHlzyMethod, slotIndex);
                }

                return;
            }

            if (TryFindCombatVisualMethod(ClearHlzyMethod, owner, out var node))
            {
                node.Call(ClearHlzyMethod, slotIndex);
            }
        }

        public static void ResetForCombatStart()
        {
            ActiveSummons.Clear();
            ActiveSummonsByOwner.Clear();
            TotalHLZYAttackCountByOwner.Clear();
        }

        public static void ResetForCombatStart(Creature owner)
        {
            ActiveSummonsByOwner.Remove(owner);
            TotalHLZYAttackCountByOwner.Remove(owner);
        }

        public static void ClearForCombatEnd(string reason)
        {
            ClearHLZY();
            ActiveSummonsByOwner.Clear();
            TotalHLZYAttackCountByOwner.Clear();
            MainFile.Logger.Info($"HLZY summons cleared: {reason}.");
        }

        public static void RecordHLZYAttacks(int count)
        {
            RecordHLZYAttacks(null, count);
        }

        public static void RecordHLZYAttacks(Creature? owner, int count)
        {
            if (count <= 0)
            {
                return;
            }

            if (owner == null)
            {
                return;
            }

            TotalHLZYAttackCountByOwner[owner] = GetTotalHLZYAttackCount(owner) + count;
        }

        public static bool DismissOneHLZY()
        {
            return DismissOneHLZYFor(null);
        }

        public static bool DismissOneHLZY(Creature owner)
        {
            return DismissOneHLZYFor(owner);
        }

        private static bool DismissOneHLZYFor(Creature? owner)
        {
            var activeSummons = GetActiveSummons(owner, create: false);
            if (activeSummons == null)
            {
                return false;
            }

            var summon = activeSummons.Values
                .Where(summon => summon.Id == SummonInstance.HLZYId && summon.IsAlive)
                .OrderByDescending(summon => summon.SlotIndex)
                .FirstOrDefault();

            if (summon == null)
            {
                return false;
            }

            ClearHLZYFor(owner, summon.SlotIndex);
            return true;
        }

        public static int CountHLZY()
        {
            return CountHLZYFor(null);
        }

        public static int CountHLZY(Creature owner)
        {
            return CountHLZYFor(owner);
        }

        private static int CountHLZYFor(Creature? owner)
        {
            var activeSummons = GetActiveSummons(owner, create: false);
            if (activeSummons == null)
            {
                return 0;
            }

            return activeSummons.Values.Count(summon =>
                summon.Id == SummonInstance.HLZYId &&
                summon.IsAlive
            );
        }

        public static IReadOnlyList<SummonInstance> GetLivingSummons()
        {
            return ActiveSummons.Values
                .Concat(ActiveSummonsByOwner.Values.SelectMany(summons => summons.Values))
                .Where(summon => summon.IsAlive)
                .OrderBy(summon => summon.SlotIndex)
                .ToList();
        }

        public static int GetProvidedEffectAmount(SummonEffectKind kind)
        {
            return ActiveSummons.Values
                .Concat(ActiveSummonsByOwner.Values.SelectMany(summons => summons.Values))
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

        public static int GetTotalHLZYAttackCount(Creature? owner)
        {
            return owner != null && TotalHLZYAttackCountByOwner.TryGetValue(owner, out var count)
                ? count
                : 0;
        }

        private static SummonInstance? ReserveHLZY(SummonSource source, int requestedSlot, Creature? owner)
        {
            var activeSummons = GetActiveSummons(owner, create: true)!;
            var slotIndex = ResolveSlot(activeSummons, requestedSlot);
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

            activeSummons[slotIndex] = summon;
            return summon;
        }

        private static int ResolveSlot(Dictionary<int, SummonInstance> activeSummons, int requestedSlot)
        {
            if (requestedSlot != AnySlot)
            {
                return requestedSlot >= 0 && requestedSlot < MaxSummons && !activeSummons.ContainsKey(requestedSlot)
                    ? requestedSlot
                    : -1;
            }

            for (var slotIndex = 0; slotIndex < MaxSummons; slotIndex++)
            {
                if (!activeSummons.ContainsKey(slotIndex))
                {
                    return slotIndex;
                }
            }

            return -1;
        }

        private static bool IsActiveSummon(Creature? owner, SummonInstance summon)
        {
            return GetActiveSummons(owner, create: false)?.TryGetValue(summon.SlotIndex, out var activeSummon) == true
                && ReferenceEquals(activeSummon, summon);
        }

        private static Dictionary<int, SummonInstance>? GetActiveSummons(Creature? owner, bool create)
        {
            if (owner == null)
            {
                return ActiveSummons;
            }

            if (!ActiveSummonsByOwner.TryGetValue(owner, out var summons) && create)
            {
                summons = [];
                ActiveSummonsByOwner[owner] = summons;
            }

            return summons;
        }

        private static bool TryFindCombatVisualMethod(StringName methodName, Creature? owner, out Node node)
        {
            node = null!;

            var room = NCombatRoom.Instance;
            if (room == null)
            {
                return false;
            }

            if (owner != null)
            {
                var creatureNode = room.GetCreatureNode(owner);
                return creatureNode != null && TryFindMethod(creatureNode, methodName, out node);
            }

            return TryFindMethod(room, methodName, out node);
        }

        private static IEnumerable<Node> FindCombatVisualMethods(StringName methodName)
        {
            var room = NCombatRoom.Instance;
            return room == null
                ? []
                : FindMethodNodes(room, methodName).ToList();
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

        private static IEnumerable<Node> FindMethodNodes(Node current, StringName methodName)
        {
            if (current.HasMethod(methodName))
            {
                yield return current;
            }

            foreach (var child in current.GetChildren())
            {
                if (child is not Node childNode)
                {
                    continue;
                }

                foreach (var node in FindMethodNodes(childNode, methodName))
                {
                    yield return node;
                }
            }
        }
    }
}
