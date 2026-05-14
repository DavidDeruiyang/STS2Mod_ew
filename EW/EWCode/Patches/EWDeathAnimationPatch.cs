using HarmonyLib;
using EW.EWCode.Summons;
using Godot;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EW.EWCode.Patches
{
    [HarmonyPatch(typeof(Creature), nameof(Creature.InvokeDiedEvent))]
    public static class EWDeathAnimationPatch
    {
        private static readonly StringName PlayDeathMethod = "ew_play_death";
        private const string PendingAbandonDeathUntilTicksKey = "ew/pending_abandon_death_until_ticks";
        private const ulong PendingAbandonDeathWindowMs = 12_000;

        public static void Prefix(Creature __instance)
        {
            if (!__instance.IsPlayer)
            {
                return;
            }

            var player = __instance.Player;
            if (player == null || player.Character is not Character.EW)
            {
                return;
            }

            SummonManager.ClearForCombatEnd("player death");
            StartDeathAnimation(__instance);
        }

        private static void StartDeathAnimation(Creature creature)
        {
            var room = NCombatRoom.Instance;
            if (room == null)
            {
                TryStartDeathAnimationFromCurrentScene("player death without combat room");
                return;
            }

            var playerNode = room.GetCreatureNode(creature);
            if (playerNode == null)
            {
                MainFile.Logger.Info("EW death animation skipped: player node is not available.");
                return;
            }

            if (TryPlayGodotDeathAnimation(playerNode))
            {
                return;
            }

            var length = playerNode.StartDeathAnim(shouldRemove: false);
            MainFile.Logger.Info($"EW fell back to NCreature death animation, length={length}");
        }

        private static bool TryPlayGodotDeathAnimation(NCreature playerNode)
        {
            var visuals = playerNode.Visuals;
            if (visuals == null)
            {
                MainFile.Logger.Info("EW death animation skipped: player visuals are not available.");
                return false;
            }

            if (TryCallDeathMethod(visuals.GetCurrentBody()))
            {
                MainFile.Logger.Info("EW triggered Godot death animation from current body.");
                return true;
            }

            if (TryCallDeathMethod(visuals))
            {
                MainFile.Logger.Info("EW triggered Godot death animation from visuals.");
                return true;
            }

            MainFile.Logger.Info("EW death animation skipped: ew_play_death was not found.");
            return false;
        }

        public static bool TryStartDeathAnimationFromCurrentScene(string reason)
        {
            if (Engine.GetMainLoop() is not SceneTree tree || tree.Root == null)
            {
                MainFile.Logger.Info($"EW death animation skipped for {reason}: scene tree is not available.");
                return false;
            }

            if (TryCallDeathMethod(tree.Root))
            {
                MainFile.Logger.Info($"EW triggered Godot death animation from current scene: {reason}.");
                return true;
            }

            MainFile.Logger.Info($"EW death animation skipped for {reason}: ew_play_death was not found in current scene.");
            return false;
        }

        public static void MarkPendingAbandonDeath()
        {
            ProjectSettings.SetSetting(PendingAbandonDeathUntilTicksKey, (long)(Time.GetTicksMsec() + PendingAbandonDeathWindowMs));
        }

        private static bool TryCallDeathMethod(Node? node)
        {
            if (node == null)
            {
                return false;
            }

            if (node.HasMethod(PlayDeathMethod))
            {
                node.Call(PlayDeathMethod);
                return true;
            }

            foreach (var child in node.GetChildren())
            {
                if (child is Node childNode && TryCallDeathMethod(childNode))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [HarmonyPatch]
    public static class EWAbandonDeathAnimationPatch
    {
        private static readonly string[] AbandonMethodNameParts =
        [
            "Abandon",
            "RunAbandoned",
            "ConfirmAbandon"
        ];

        public static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var method in AppDomain.CurrentDomain.GetAssemblies()
                         .Where(assembly => assembly.GetName().Name == "sts2")
                         .SelectMany(GetLoadableTypes)
                         .SelectMany(type => AccessTools.GetDeclaredMethods(type))
                         .Where(IsAbandonMethod))
            {
                MainFile.Logger.Info($"EW abandon death animation patch target: {method.DeclaringType?.FullName}.{method.Name}");
                yield return method;
            }
        }

        public static void Prefix(MethodBase __originalMethod)
        {
            EWDeathAnimationPatch.MarkPendingAbandonDeath();
            EWDeathAnimationPatch.TryStartDeathAnimationFromCurrentScene(
                $"run abandon via {__originalMethod.DeclaringType?.Name}.{__originalMethod.Name}");
        }

        private static bool IsAbandonMethod(MethodBase method)
        {
            return !method.IsSpecialName
                   && method.DeclaringType != null
                   && !method.DeclaringType.IsInterface
                   && !method.IsAbstract
                   && AbandonMethodNameParts.Any(part => method.Name.Contains(part, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(type => type != null)!;
            }
        }
    }
}
